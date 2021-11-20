using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public struct EntityEnumerator<T> : IDisposable
        where T : struct
    {
        private ISafeEntityOps _ops;
        private EcsFilter _filter;
        private EcsFilter.Enumerator _enumerator;

        internal EntityEnumerator(ISafeEntityOps ops, EcsFilter filter) : this()
        {
            _ops = ops;
            _filter = filter;
            _enumerator = _filter.GetEnumerator();
        }

        public Entity<T> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                new Entity<T>
                {
                    id = new SafeEntityId
                    {
                        value = _filter.GetWorld().PackEntity(_enumerator.Current)
                    },
                    ops = _ops
                };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }

    public struct EntityEnumerator<T1, T2> : IDisposable
        where T1 : struct
        where T2 : struct
    {
        private ISafeEntityOps _ops;
        private EcsFilter _filter;
        private EcsFilter.Enumerator _enumerator;

        internal EntityEnumerator(ISafeEntityOps ops, EcsFilter filter) : this()
        {
            _ops = ops;
            _filter = filter;
            _enumerator = _filter.GetEnumerator();
        }

        public Entity<T1, T2> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                new Entity<T1, T2>
                {
                    id = new SafeEntityId
                    {
                        value = _filter.GetWorld().PackEntity(_enumerator.Current)
                    },
                    ops = _ops
                };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }
}