using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal static class PackedEntityExtensions
    {
        public static ref T Get<T>(this EcsPackedEntityWithWorld id) where T : struct
        {
            id.UnpackReq(out EcsWorld raw, out var entity);
            return ref raw.GetPool<T>().Get(entity);
        }

        public static bool Has<T>(this EcsPackedEntityWithWorld id) where T : struct
        {
            id.UnpackReq(out EcsWorld raw, out var entity);
            return raw.GetPool<T>().Has(entity);
        }

        public static ref T Add<T>(this EcsPackedEntityWithWorld id) where T : struct
        {
            id.UnpackReq(out EcsWorld raw, out var entity);
            ref T comp = ref AddInternal(raw.GetPool<T>(), entity, raw);

            return ref comp;
        }

        public static void Add<T>(this EcsPackedEntityWithWorld id, T initialState) where T : struct
        {
            if (ComponentAutoReset<T>.AutoReset)
            {
                throw new Exception($"passing initial state is disabled (as error prone) for components with {nameof(IEcsAutoReset<T>)}");
            }

            Add<T>(id) = initialState;
        }

        public static void Del<T>(this EcsPackedEntityWithWorld id) where T : struct
        {
            id.UnpackReq(out EcsWorld raw, out var entity);
            DeleteInternal(raw.GetPool<T>(), entity, raw);
        }

        public static void Destroy(this EcsPackedEntityWithWorld id)
        {
            id.UnpackReq(out EcsWorld raw, out var entity);
            raw.DelEntity(entity);
        }

        public static object[] GetComponents(this EcsPackedEntityWithWorld id)
        {
            id.UnpackReq(out EcsWorld raw, out var entity);
            object[] result = new object[raw.GetComponentsCount(entity)];
            raw.GetComponents(entity, ref result);
            return result;
        }

        public static bool IsAlive(this EcsPackedEntityWithWorld id)
        {
            return id.Unpack(out _, out _);
        }

        private static void UnpackReq(this EcsPackedEntityWithWorld id, out EcsWorld world, out int entity)
        {
            if (!id.Unpack(out world, out entity))
            {
                throw new Exception($"invalid entity: {id}");
            }
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