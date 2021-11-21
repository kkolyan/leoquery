using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public class LeoLiteStorage : IEntityStorage, ISafeEntityOps
    {
        internal EcsWorld world;
        private Dictionary<Type, object> _filters = new Dictionary<Type, object>();

        public LeoLiteStorage(EcsWorld world = null)
        {
            this.world = world ?? new EcsWorld();
        }

        public IEntitySet<T> Query<T>()
            where T : struct
        {
            Type type = typeof(IEntitySet<T>);
            if (!_filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T>(world.Filter<T>(), this);
                _filters[type] = filterRaw;
            }

            return (IEntitySet<T>)filterRaw;
        }

        public IEntitySet<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct
        {
            Type type = typeof(IEntitySet<T1, T2>);
            if (!_filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T1, T2>(world.Filter<T1>().Inc<T2>(), this);
                _filters[type] = filterRaw;
            }

            return (IEntitySet<T1, T2>)filterRaw;
        }

        public bool TrySingle<T>(out Entity<T> entity) where T : struct
        {
            EntitySet<T> entitySet = (EntitySet<T>)Query<T>();

            foreach (Entity<T> candidate in entitySet)
            {
                int entitiesCount = entitySet.filter.GetEntitiesCount();
                if (entitiesCount > 1)
                {
                    throw new Exception($"cannot resolve unique {typeof(T).FullName}. count: {entitiesCount}");
                }

                entity = candidate;
                return true;
            }

            entity = default;
            return false;
        }

        public Entity NewEntity()
        {
            return new Entity(this, new SafeEntityId
            {
                value = world.PackEntity(world.NewEntity())
            });
        }

        public ref T Get<T>(SafeEntityId id) where T : struct
        {
            return ref world.GetPool<T>().Get(Unpack(id));
        }

        public bool Has<T>(SafeEntityId id) where T : struct
        {
            return world.GetPool<T>().Has(Unpack(id));
        }

        public ref T Add<T>(SafeEntityId id) where T : struct
        {
            int entity = Unpack(id);
            ref T comp = ref AddInternal(world.GetPool<T>(), entity, world);

            return ref comp;
        }

        public void Add<T>(SafeEntityId id, T initialState) where T : struct
        {
            if (ComponentInit<T>.Instance != null || ComponentAutoReset<T>.AutoReset)
            {
                throw new Exception($"passing initial state is disabled (as error prone) for components with either of {nameof(IEcsAutoReset<T>)} nor {nameof(IComponentInit<T>)}");
            }

            Add<T>(id) = initialState;
        }

        public void Del<T>(SafeEntityId id) where T : struct
        {
            int entity = Unpack(id);
            DeleteInternal(world.GetPool<T>(), entity, world);
        }

        public void Destroy(SafeEntityId id)
        {
            world.DelEntity(Unpack(id));
        }

        public object[] GetComponents(SafeEntityId id)
        {
            int entity = Unpack(id);
            object[] result = new object[world.GetComponentsCount(entity)];
            world.GetComponents(entity, ref result);
            return result;
        }

        public bool IsAlive(SafeEntityId id)
        {
            return id.value.Unpack(world, out int _);
        }

        private int Unpack(SafeEntityId id)
        {
            return Unpack(world, id);
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
            if (ComponentInit<T>.Instance != null)
            {
                ComponentInit<T>.Instance.Init(ref comp);
            }

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
            internal static readonly RelationsManager Manager;

            static ComponentRelations()
            {
                if (Activator.CreateInstance<T>() is IRelationsOwner g)
                {
                    Manager = new RelationsManager();
                    g.DescribeRelations(Manager);
                }
            }
        }

        private static class ComponentInit<T>
        {
            internal static readonly IComponentInit<T> Instance = Activator.CreateInstance<T>() as IComponentInit<T>;
        }

        private static class ComponentAutoReset<T> where T : struct
        {
            internal static readonly bool AutoReset = Activator.CreateInstance<T>() is IEcsAutoReset<T>;
        }
    }
}