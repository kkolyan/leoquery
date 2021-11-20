using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public class LeoLiteStorage : IEntityStorage, ISafeEntityOps
    {
        internal EcsWorld world = new EcsWorld();
        private Dictionary<Type, object> _filters = new Dictionary<Type, object>();

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

        public void Add<T>(SafeEntityId id, T state) where T : struct
        {
            world.GetPool<T>().Add(Unpack(id)) = state;
        }

        public void Del<T>(SafeEntityId id) where T : struct
        {
            world.GetPool<T>().Del(Unpack(id));
        }

        public void Destroy(SafeEntityId id)
        {
            world.DelEntity(Unpack(id));
        }

        private int Unpack(SafeEntityId id)
        {
            if (!id.value.Unpack(world, out var idx))
            {
                throw new Exception("state entity");
            }

            return idx;
        }
    }
}