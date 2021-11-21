using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kk.LeoQuery;
using NUnit.Framework;

namespace leoquery.test
{
    [TestFixture]
    public class CompInitTest
    {
        [Test]
        public void ListInitialized()
        {
            IEntityStorage storage = new LeoLiteStorage();
            C1 a = storage.NewEntity().Add<C1>();
            Assert.IsNotNull(a.list);
            Assert.AreEqual(0, a.list.Count);
        }
        
        [Test]
        public void ValueStoredWell()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity e1 = storage.NewEntity();
            C1 a = e1.Add<C1>();
            a.list.Add(42);

            C1 a1 = e1.Get<C1>();
            Assert.AreEqual(42, a1.list.Single());
        }
        
        [Test]
        public void ClearedAfter()
        {
            IEntityStorage storage = new LeoLiteStorage();
            WeakReference weakList;
            {
                Entity e1 = storage.NewEntity();
                C1 a = e1.Add<C1>();
                a.list.Add(42);
                weakList = new WeakReference(a.list);
                e1.Destroy();
                
                // for some reason scope end doesn't free it, so we need to nullify it manually
                a = default;
                e1 = default;
            }
            
            GC.Collect();
            
            Assert.IsFalse(weakList.IsAlive);
        }

        public struct C1 : IComponentInit<C1>
        {
            public List<int> list;

            public void Init(ref C1 c)
            {
                c.list = new List<int>();
            }
        }
    }
}