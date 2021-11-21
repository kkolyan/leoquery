using System;
using System.Collections.Generic;
using System.Linq;
using Kk.LeoQuery;
using Leopotam.EcsLite;
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
        
        [Test]
        public void ClearedAfterWithAutoReset()
        {
            IEntityStorage storage = new LeoLiteStorage();
            WeakReference<List<int>> weakList;
            {
                Entity e1 = storage.NewEntity();
                C2 a = e1.Add<C2>();
                a.list.Add(42);
                weakList = new WeakReference<List<int>>(a.list);
                e1.Destroy();
                
                // for some reason scope end doesn't free it, so we need to nullify it manually
                a = default;
                e1 = default;
            }
            
            GC.Collect();
            
            Assert.IsTrue(weakList.TryGetTarget(out var list));
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void ScalarsClearedWithAutoInit()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C1>().scalar = 42;
            a.Destroy();

            Entity b = storage.NewEntity();
            Assert.AreEqual(0, b.Add<C1>().scalar);
        }

        [Test]
        public void ScalarsNotClearedWithAutoReset()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C2>().scalar = 42;
            a.Destroy();

            Entity b = storage.NewEntity();
            Assert.AreEqual(42, b.Add<C2>().scalar);
        }

        public struct C1 : IComponentInit<C1>
        {
            public List<int> list;
            public int scalar;

            public void Init(ref C1 c)
            {
                c.list = new List<int>();
            }
        }

        public struct C2 : IEcsAutoReset<C2>
        {
            public List<int> list;
            public int scalar;

            public void AutoReset(ref C2 c)
            {
                if (c.list == null)
                {
                    c.list = new List<int>();
                }

                c.list.Clear();
            }
        }
    }
}