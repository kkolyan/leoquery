using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public class LeoLiteStorage : IEntityStorage
    {
        private World[] _worlds;
        private List<int> _worldIndexes = new List<int>();

        public LeoLiteStorage(in EcsWorld.Config config = default, int initialWorldCount = 1)
        {
            _worlds = new World[initialWorldCount];
            World(0, config);
        }

        public int GetWorlds(ref EcsWorld[] results)
        {
            if (results == null)
            {
                results = new EcsWorld[_worldIndexes.Count];
            }
            else if (results.Length <= _worldIndexes.Count)
            {
                Array.Resize(ref results, _worldIndexes.Count);
            }

            for (var i = 0; i < _worldIndexes.Count; i++)
            {
                results[i] = _worlds[_worldIndexes[i]].raw;
            }

            return _worldIndexes.Count;
        }

        public LeoLiteStorage World(int index, in EcsWorld.Config config = default)
        {
            if (_worlds.Length <= index)
            {
                Array.Resize(ref _worlds, Math.Max(_worlds.Length * 2, index * 2));
            }

            _worlds[index] = new World(new EcsWorld(config));
            if (!_worldIndexes.Contains(index))
            {
                _worldIndexes.Add(index);
            }

            return this;
        }

        public IEntitySet<T> Query<T>() where T : struct
        {
            return QueryInternal<T>(0);
        }

        public IEntitySet<T> Query<T>(int worldIndex) where T : struct
        {
            return QueryInternal<T>(worldIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private EntitySet<T, GenericVoid, GenericVoid> QueryInternal<T>(int liteWorldIndex) where T : struct
        {
            World ops = _worlds[liteWorldIndex];
            Type type = typeof(IEntitySet<T>);
            if (!ops.filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T, GenericVoid, GenericVoid>(ops.raw.Filter<T>(), ops);
                ops.filters[type] = filterRaw;
            }

            return (EntitySet<T, GenericVoid, GenericVoid>)filterRaw;
        }

        public IEntitySet<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct
        {
            return Query<T1, T2>(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEntitySet<T1, T2> Query<T1, T2>(int worldIndex)
            where T1 : struct
            where T2 : struct
        {
            World ops = _worlds[worldIndex];
            Type type = typeof(IEntitySet<T1, T2>);
            if (!ops.filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T1, T2, GenericVoid>(ops.raw.Filter<T1>().Inc<T2>(), ops);
                ops.filters[type] = filterRaw;
            }

            return (IEntitySet<T1, T2>)filterRaw;
        }

        public IEntitySet<T1, T2, T3> Query<T1, T2, T3>()
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            return Query<T1, T2, T3>(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEntitySet<T1, T2, T3> Query<T1, T2, T3>(int worldIndex)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            World ops = _worlds[worldIndex];
            Type type = typeof(IEntitySet<T1, T2>);
            if (!ops.filters.TryGetValue(type, out object filterRaw))
            {
                filterRaw = new EntitySet<T1, T2, T3>(ops.raw.Filter<T1>().Inc<T2>().Inc<T3>(), ops);
                ops.filters[type] = filterRaw;
            }

            return (IEntitySet<T1, T2, T3>)filterRaw;
        }

        public bool TrySingle<T>(out Entity<T> entity) where T : struct
        {
            return TrySingle(0, out entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySingle<T>(int worldIndex, out Entity<T> entity) where T : struct
        {
            EntitySet<T, GenericVoid, GenericVoid> query = QueryInternal<T>(worldIndex);

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

        public Entity NewEntity()
        {
            return NewEntity(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity NewEntity(int worldIndex)
        {
            World ops = _worlds[worldIndex];
            return new Entity(ops, new SafeEntityId(ops.raw.PackEntity(ops.raw.NewEntity())));
        }
    }
}