using Kk.LeoQuery;
using NUnit.Framework;

namespace leoquery.test
{
    [TestFixture]
    public class ComponentRelationsTest
    {
        [Test]
        public void Adds()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C1>();

            Assert.IsTrue(a.Has<C2>());
            Assert.IsTrue(a.Has<C3>());
        }
        
        [Test]
        public void TolerateExisting()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C2>();
            a.Add<C1>();

            Assert.IsTrue(a.Has<C2>());
            Assert.IsTrue(a.Has<C3>());
        }

        [Test]
        public void Deletes()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C2>();
            a.Add<C3>();
            a.Add<C1>();
            a.Add<C4>();

            a.Del<C1>();

            Assert.IsFalse(a.Has<C1>());
            Assert.IsFalse(a.Has<C2>());
            Assert.IsFalse(a.Has<C3>());
            Assert.IsTrue(a.Has<C4>());
        }

        [Test]
        public void TolerateAbsent()
        {
            IEntityStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity();
            a.Add<C2>();
            a.Add<C3>();
            a.Add<C1>();
            a.Add<C4>();

            a.Del<C2>();
            a.Del<C1>();

            Assert.IsFalse(a.Has<C1>());
            Assert.IsFalse(a.Has<C2>());
            Assert.IsFalse(a.Has<C3>());
            Assert.IsTrue(a.Has<C4>());
        }

        public struct C1 : IRelationsOwner
        {
            public void DescribeRelations(IRelationsBuilder b)
            {
                b.Satellite<C2>();
                b.Satellite<C3>();
            }
        }

        public struct C2 { }

        public struct C3 { }

        public struct C4 { }
    }
}