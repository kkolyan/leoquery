using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public class EntityEnumerator<T1, T2> : IEnumerator<Entity<T1, T2>>, IEnumerator<Entity<T1>>
        where T1 : struct
        where T2 : struct
    {
        private ISafeEntityOps _ops;
        private EcsFilter _filter;
        private EcsFilter.Enumerator _enumerator;
        private Action<EntityEnumerator<T1, T2>> _release;

        internal EntityEnumerator(ISafeEntityOps ops, EcsFilter filter, Action<EntityEnumerator<T1, T2>> release)
        {
            _ops = ops;
            _filter = filter;
            _release = release;
        }

        public void Reset()
        {
            _enumerator = _filter.GetEnumerator();
        }

        Entity<T1> IEnumerator<Entity<T1>>.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                new Entity<T1>
                {
                    id = new SafeEntityId
                    {
                        value = _filter.GetWorld().PackEntity(_enumerator.Current)
                    },
                    ops = _ops
                };
        }

        object IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => throw new NotSupportedException();
        }

        Entity<T1, T2> IEnumerator<Entity<T1, T2>>.Current
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
            _release(this);
        }
    }
}