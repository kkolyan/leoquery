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

        IEntitySet<T> IEntityStorage.Query<T>()
        {
            return QueryInternal<T>();
        }

        private EntitySet<T, GenericVoid> QueryInternal<T>() where T : struct
        {
            Type type = typeof(IEntitySet<T>);
            if (!_filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T, GenericVoid>(world.Filter<T>(), this);
                _filters[type] = filterRaw;
            }

            return (EntitySet<T, GenericVoid>)filterRaw;
        }

        IEntitySet<T1, T2> IEntityStorage.Query<T1, T2>()
        {
            Type type = typeof(IEntitySet<T1, T2>);
            if (!_filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T1, T2>(world.Filter<T1>().Inc<T2>(), this);
                _filters[type] = filterRaw;
            }

            return (IEntitySet<T1, T2>)filterRaw;
        }

        bool IEntityStorage.TrySingle<T>(out Entity<T> entity)
        {
            EntitySet<T, GenericVoid> query = QueryInternal<T>();

            foreach (Entity<T> candidate in (IEntitySet<T>)query)
            {
                // filter field is initialized on GetEnumerator
                int entitiesCount = query.filter.GetEntitiesCount();
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

        Entity IEntityStorage.NewEntity()
        {
            return new Entity(this, new SafeEntityId
            {
                value = world.PackEntity(world.NewEntity())
            });
        }

        ref T ISafeEntityOps.Get<T>(SafeEntityId id)
        {
            return ref world.GetPool<T>().Get(Unpack(id));
        }

        bool ISafeEntityOps.Has<T>(SafeEntityId id)
        {
            return world.GetPool<T>().Has(Unpack(id));
        }

        ref T ISafeEntityOps.Add<T>(SafeEntityId id)
        {
            int entity = Unpack(id);
            ref T comp = ref AddInternal(world.GetPool<T>(), entity, world);

            return ref comp;
        }

        void ISafeEntityOps.Add<T>(SafeEntityId id, T initialState)
        {
            if (ComponentInit<T>.Instance != null || ComponentAutoReset<T>.AutoReset)
            {
                throw new Exception($"passing initial state is disabled (as error prone) for components with either of {nameof(IEcsAutoReset<T>)} nor {nameof(IComponentInit<T>)}");
            }

            ((ISafeEntityOps)this).Add<T>(id) = initialState;
        }

        void ISafeEntityOps.Del<T>(SafeEntityId id)
        {
            int entity = Unpack(id);
            DeleteInternal(world.GetPool<T>(), entity, world);
        }

        void ISafeEntityOps.Destroy(SafeEntityId id)
        {
            world.DelEntity(Unpack(id));
        }

        object[] ISafeEntityOps.GetComponents(SafeEntityId id)
        {
            int entity = Unpack(id);
            object[] result = new object[world.GetComponentsCount(entity)];
            world.GetComponents(entity, ref result);
            return result;
        }

        bool ISafeEntityOps.IsAlive(SafeEntityId id)
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