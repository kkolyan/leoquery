using System.Collections.Generic;

namespace Kk.LeoQuery
{
    internal class Pool<T>
    {
        public delegate T CreateNewItem(Pool<T> pool);
            
        private readonly CreateNewItem _factory;
        private readonly Stack<T> _readyToBorrow = new Stack<T>();
        private int _borrowedCount;

        public int BorrowedCount => _borrowedCount;

        public Pool(CreateNewItem factory)
        {
            _factory = factory;
        }

        public T Borrow()
        {
            if (_readyToBorrow.Count <= 0)
            {
                _readyToBorrow.Push(_factory(this));
            }

            _borrowedCount++;
            return _readyToBorrow.Pop();
        }

        public void Return(T obj)
        {
            _borrowedCount--;
            _readyToBorrow.Push(obj);
        }
    }
}