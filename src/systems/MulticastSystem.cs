using System;

namespace Kk.LeoQuery
{
    public class MulticastSystem : ISystem
    {
        private ISystem[] _systems;
        private int _systemCount;

        public MulticastSystem(int initialCapacity = 16)
        {
            _systems = new ISystem[initialCapacity];
        }

        public MulticastSystem Add(ISystem system)
        {
            if (_systemCount >= _systems.Length)
            {
                Array.Resize(ref _systems, _systems.Length * 2);
            }

            _systems[_systemCount++] = system;
            
            return this;
        }

        public MulticastSystem ForEach(Action<ISystem> action)
        {
            for (int i = 0; i < _systemCount; i++)
            {
                action(_systems[i]);
            }

            return this;
        }

        public void Act(IEntityStorage storage)
        {
            for (int i = 0; i < _systemCount; i++)
            {
                _systems[i].Act(storage);
            }
        }
    }
}