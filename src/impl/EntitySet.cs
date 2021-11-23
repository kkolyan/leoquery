using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal class EntitySet<T1, T2, T3> : IEntitySet<T1, T2, T3>, IEntitySet<T1, T2>, IEntitySet<T1>
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        private readonly EcsFilter.Mask _mask;
        private readonly World _world;
        internal EcsFilter filter;
        private Dictionary<Type, object> _excFilters = new Dictionary<Type, object>();
        private Pool<EntityEnumerator<T1, T2, T3>> _enumeratorPool;

        public EntitySet(EcsFilter.Mask mask, World world)
        {
            _world = world;
            _mask = mask;
            _enumeratorPool = new Pool<EntityEnumerator<T1, T2, T3>>(pool =>
            {
                if (pool.BorrowedCount > 3)
                {
                    throw new Exception("enumerator leak detected");
                }

                return new EntityEnumerator<T1, T2, T3>(_world, filter, pool.Return);
            });
        }

        private EntityEnumerator<T1, T2, T3> GetEnumerator()
        {
            if (filter == null)
            {
                filter = _mask.End();
            }

            EntityEnumerator<T1, T2, T3> enumerator = _enumeratorPool.Borrow();
            enumerator.Reset();
            return enumerator;
        }

        IEnumerator<Entity<T1, T2, T3>> IEntitySet<T1, T2, T3>.GetEnumerator()
        {
            return GetEnumerator();
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
                excFilter = new EntitySet<T1, GenericVoid, GenericVoid>(
                    _world.raw
                        .Filter<T1>()
                        .Exc<TExc>()
                    , _world
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
                excFilter = new EntitySet<T1, T2, GenericVoid>(
                    _world.raw
                        .Filter<T1>()
                        .Inc<T2>()
                        .Exc<TExc>()
                    , _world
                );
                _excFilters[type] = excFilter;
            }

            return (IEntitySet<T1, T2>)excFilter;
        }

        IEntitySet<T1, T2, T3> IEntitySet<T1, T2, T3>.Excluding<TExc>()
        {
            Type type = typeof(TExc);
            if (!_excFilters.TryGetValue(type, out var excFilter))
            {
                excFilter = new EntitySet<T1, T2, T3>(
                    _world.raw
                        .Filter<T1>()
                        .Inc<T2>()
                        .Inc<T3>()
                        .Exc<TExc>()
                    , _world
                );
                _excFilters[type] = excFilter;
            }

            return (IEntitySet<T1, T2, T3>)excFilter;
        }
    }
}