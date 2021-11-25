using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public class EntityEnumerator<T1, T2, T3> : IEnumerator<Entity<T1, T2, T3>>, IEnumerator<Entity<T1, T2>>, IEnumerator<Entity<T1>>
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        private World _world;
        private EcsFilter _filter;
        private EcsFilter.Enumerator _enumerator;
        private Action<EntityEnumerator<T1, T2, T3>> _release;

        internal EntityEnumerator(World world, EcsFilter filter, Action<EntityEnumerator<T1, T2, T3>> release)
        {
            _world = world;
            _filter = filter;
            _release = release;
        }

        public void Reset()
        {
            _enumerator = _filter.GetEnumerator();
        }

        object IEnumerator.Current => 
            throw new NotSupportedException();

        Entity<T1> IEnumerator<Entity<T1>>.Current =>
            new Entity<T1>(_filter.GetWorld().PackEntityWithWorld(_enumerator.Current));

        Entity<T1, T2> IEnumerator<Entity<T1, T2>>.Current =>
            new Entity<T1, T2>(_filter.GetWorld().PackEntityWithWorld(_enumerator.Current));

        Entity<T1, T2, T3> IEnumerator<Entity<T1, T2, T3>>.Current =>
            new Entity<T1, T2, T3>(_filter.GetWorld().PackEntityWithWorld(_enumerator.Current));

        bool IEnumerator.MoveNext()
        {
            return _enumerator.MoveNext();
        }

        void IDisposable.Dispose()
        {
            _enumerator.Dispose();
            _release(this);
        }
    }
}