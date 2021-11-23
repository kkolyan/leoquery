using System;
using System.Collections.Generic;
using Kk.LeoQuery;
using Leopotam.EcsLite;
using NUnit.Framework;

namespace leoquery.test
{
    [TestFixture]
    public class CompInitTest
    {
        
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
        public void ScalarsNotClearedWithAutoReset()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C2>().scalar = 42;
            a.Destroy();

            Entity b = storage.NewEntity();
            Assert.AreEqual(42, b.Add<C2>().scalar);
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