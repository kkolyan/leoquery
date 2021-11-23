using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal readonly struct World
    {
        internal readonly EcsWorld raw;
        internal readonly Dictionary<Type, object> filters;

        public World(EcsWorld raw)
        {
            this.raw = raw;
            filters = new Dictionary<Type, object>();
        }

        public ref T Get<T>(SafeEntityId id) where T : struct
        {
            return ref raw.GetPool<T>().Get(Unpack(id));
        }

        public bool Has<T>(SafeEntityId id) where T : struct
        {
            return raw.GetPool<T>().Has(Unpack(id));
        }

        public ref T Add<T>(SafeEntityId id) where T : struct
        {
            int entity = Unpack(id);
            ref T comp = ref AddInternal(raw.GetPool<T>(), entity, raw);

            return ref comp;
        }

        public void Add<T>(SafeEntityId id, T initialState) where T : struct
        {
            if (ComponentAutoReset<T>.AutoReset)
            {
                throw new Exception($"passing initial state is disabled (as error prone) for components with {nameof(IEcsAutoReset<T>)}");
            }

            Add<T>(id) = initialState;
        }

        public void Del<T>(SafeEntityId id) where T : struct
        {
            int entity = Unpack(id);
            DeleteInternal(raw.GetPool<T>(), entity, raw);
        }

        public void Destroy(SafeEntityId id)
        {
            raw.DelEntity(Unpack(id));
        }

        public object[] GetComponents(SafeEntityId id)
        {
            int entity = Unpack(id);
            object[] result = new object[raw.GetComponentsCount(entity)];
            raw.GetComponents(entity, ref result);
            return result;
        }

        public bool IsAlive(SafeEntityId id)
        {
            return id.value.Unpack(raw, out int _);
        }

        private int Unpack(SafeEntityId id)
        {
            return Unpack(raw, id);
        }

        private static int Unpack(EcsWorld world, SafeEntityId id)
        {
            if (!id.value.Unpack(world, out var idx))
            {
                throw new Exception($"invalid entity: {id}");
            }
            
            

            return idx;
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref T AddInternal<T>(EcsPool<T> pool, int entity, EcsWorld world) where T : struct
        {
            ref T comp = ref pool.Add(entity);

            if (ComponentRelations<T>.Manager != null)
            {
                ComponentRelations<T>.Manager.add(world, entity);
            }

            return ref comp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DeleteInternal<T>(EcsPool<T> pool, int entity, EcsWorld world) where T : struct
        {
            pool.Del(entity);

            if (ComponentRelations<T>.Manager != null)
            {
                ComponentRelations<T>.Manager.delete(world, entity);
            }
        }

        private class RelationsManager : IRelationsBuilder
        {
            internal Action<EcsWorld,int> add;
            internal Action<EcsWorld,int> delete;
            
            public void Satellite<T>() where T : struct
            {
                add += (world, entity) =>
                {
                    EcsPool<T> pool = world.GetPool<T>();
                    if (!pool.Has(entity))
                    {
                        AddInternal(pool, entity, world);
                    }
                };
                delete += (world, entity) =>
                {
                    EcsPool<T> pool = world.GetPool<T>();
                    if (pool.Has(entity))
                    {
                        DeleteInternal(pool, entity, world);
                    }
                };
            }
        }

        private static class ComponentRelations<T>
        {
            // intended
            // ReSharper disable once StaticMemberInGenericType
            internal static RelationsManager Manager;

            static ComponentRelations()
            {
                if (Activator.CreateInstance<T>() is IRelationsOwner g)
                {
                    Manager = ApplyRelationsConfig(g.DescribeRelations);
                }
                else if (RelationAttributes.TryGet(typeof(T), out DescribeRelations c))
                {
                    Manager = ApplyRelationsConfig(c);
                }
            }

            private static RelationsManager ApplyRelationsConfig(DescribeRelations g)
            {
                RelationsManager relationsManager = new RelationsManager();
                g(relationsManager);
                return relationsManager;
            }
        }

        private static class ComponentAutoReset<T> where T : struct
        {
            internal static readonly bool AutoReset = Activator.CreateInstance<T>() is IEcsAutoReset<T>;
        }
    }
}