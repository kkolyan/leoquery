using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal class EntitySet<T1, T2> : IEntitySet<T1, T2>, IEntitySet<T1>
        where T1 : struct
        where T2 : struct
    {
        private readonly EcsFilter.Mask _mask;
        private readonly LeoLiteStorage _storage;
        internal EcsFilter filter;
        private Dictionary<Type, object> _excFilters = new Dictionary<Type, object>();
        private Pool<EntityEnumerator<T1, T2>> _enumeratorPool;

        public EntitySet(EcsFilter.Mask mask, LeoLiteStorage storage)
        {
            _storage = storage;
            _mask = mask;
            _enumeratorPool = new Pool<EntityEnumerator<T1, T2>>(pool =>
            {
                if (pool.BorrowedCount > 3)
                {
                    throw new Exception("enumerator leak detected");
                }

                return new EntityEnumerator<T1, T2>(_storage, filter, pool.Return);
            });
        }

        public EntityEnumerator<T1, T2> GetEnumerator()
        {
            if (filter == null)
            {
                filter = _mask.End();
            }

            EntityEnumerator<T1, T2> enumerator = _enumeratorPool.Borrow();
            enumerator.Reset();
            return enumerator;
        }

        IEnumerator<Entity<T1, T2>> IEntitySet<T1, T2>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<Entity<T1>> IEntitySet<T1>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEntitySet<T1> IEntitySet<T1>.Excluding<TExc>()
        {
            Type type = typeof(TExc);
            if (!_excFilters.TryGetValue(type, out var excFilter))
            {
                excFilter = new EntitySet<T1, GenericVoid>(
                    _storage.world
                        .Filter<T1>()
                        .Exc<TExc>()
                    , _storage
                );
                _excFilters[type] = excFilter;
            }

            return (IEntitySet<T1>)excFilter;
        }

        IEntitySet<T1, T2> IEntitySet<T1, T2>.Excluding<TExc>()
        {
            Type type = typeof(TExc);
            if (!_excFilters.TryGetValue(type, out var excFilter))
            {
                excFilter = new EntitySet<T1, T2>(
                    _storage.world
                        .Filter<T1>()
                        .Inc<T2>()
                        .Exc<TExc>()
                    , _storage
                );
                _excFilters[type] = excFilter;
            }

            return (IEntitySet<T1, T2>)excFilter;
        }
    }
}