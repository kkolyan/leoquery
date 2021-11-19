using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public struct EntityEnumerator<T> 
        where T : struct
    {
        private EcsFilter _filter;
        private EcsFilter.Enumerator _enumerator;

        public EntityEnumerator(EcsFilter filter) : this()
        {
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
                    }
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
    
    public struct EntityEnumerator<T1, T2> 
        where T1 : struct
        where T2 : struct
    {
        private EcsFilter _filter;
        private EcsFilter.Enumerator _enumerator;

        public EntityEnumerator(EcsFilter filter) : this()
        {
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
                    }
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