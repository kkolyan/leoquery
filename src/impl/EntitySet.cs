using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal class EntitySet<T> : IEntitySet<T>
        where T : struct
    {
        private readonly EcsFilter.Mask _mask;
        private readonly LeoLiteStorage _storage;
        internal EcsFilter filter;
        private Dictionary<Type, object> _excFilters = new Dictionary<Type, object>();

        public EntitySet(EcsFilter.Mask mask, LeoLiteStorage storage)
        {
            _storage = storage;
            _mask = mask;
        }

        public EntityEnumerator<T> GetEnumerator()
        {
            if (filter == null)
            {
                filter = _mask.End();
            }

            return new EntityEnumerator<T>(_storage, filter);
        }

        public IEntitySet<T> Excluding<T0>() where T0 : struct
        {
            Type type = typeof(T0);
            if (!_excFilters.TryGetValue(type, out var excFilter))
            {
                excFilter = new EntitySet<T>(_storage.world.Filter<T>().Exc<T0>(), _storage);
                _excFilters[type] = excFilter;
            }

            return (IEntitySet<T>)excFilter;
        }
    }

    internal class EntitySet<T1, T2> : IEntitySet<T1, T2>
        where T1 : struct
        where T2 : struct
    {
        private readonly EcsFilter.Mask _mask;
        private readonly LeoLiteStorage _storage;
        private EcsFilter _filter;
        private Dictionary<Type, object> _excFilters = new Dictionary<Type, object>();

        public EntitySet(EcsFilter.Mask mask, LeoLiteStorage storage)
        {
            _storage = storage;
            _mask = mask;
        }

        public EntityEnumerator<T1, T2> GetEnumerator()
        {
            if (_filter == null)
            {
                _filter = _mask.End();
            }

            return new EntityEnumerator<T1, T2>(_storage, _filter);
        }

        public IEntitySet<T1, T2> Excluding<T0>() where T0 : struct
        {
            Type type = typeof(T0);
            if (!_excFilters.TryGetValue(type, out var filter))
            {
                filter = new EntitySet<T1, T2>(_storage.world.Filter<T1>().Inc<T2>().Exc<T0>(), _storage);
                _excFilters[type] = filter;
            }

            return (IEntitySet<T1, T2>)filter;
        }
    }
}