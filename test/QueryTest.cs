using System.Collections.Generic;
using System.Text;
using Kk.LeoQuery;
using NUnit.Framework;

namespace leoquery.test
{
    [TestFixture]
    public class QueryTest
    {

        [Test]
        public void QuerySingle()
        {
            LeoLiteStorage storage = new LeoLiteStorage();
            Entity a = storage.NewEntity()
                .Add(new C1(42));

            StringBuilder s = new StringBuilder();
            foreach (Entity<C1> entity in storage.Query<C1>())
            {
                s.Append(entity.Get1().value).Append(";");
            }
            
            Assert.AreEqual("42;", s.ToString());
        }
        
        [Test]
        public void QueryTwo()
        {
            LeoLiteStorage storage = new LeoLiteStorage();
            storage.NewEntity()
                .Add(new C1(42));
            storage.NewEntity()
                .Add(new C1(17));

            StringBuilder s = new StringBuilder();
            foreach (Entity<C1> entity in storage.Query<C1>())
            {
                s.Append(entity.Get1().value).Append(";");
            }
            
            Assert.AreEqual("42;17;", s.ToString());
        }
        
        [Test]
        public void QueryTwoTwice()
        {
            LeoLiteStorage storage = new LeoLiteStorage();
            storage.NewEntity()
                .Add(new C1(42));
            storage.NewEntity()
                .Add(new C1(17));

            StringBuilder s = new StringBuilder();
            foreach (Entity<C1> entity in storage.Query<C1>())
            {
                s.Append(entity.Get1().value).Append(";");
            }
            foreach (Entity<C1> entity in storage.Query<C1>())
            {
                s.Append(entity.Get1().value).Append(";");
            }
            
            Assert.AreEqual("42;17;42;17;", s.ToString());
        }

        public struct C1
        {
            public int value;

            public C1(int value)
            {
                this.value = value;
            }
        }

        public struct C2
        {
            public int value;

            public C2(int value)
            {
                this.value = value;
            }
        }

        public struct C3
        {
            public int value;

            public C3(int value)
            {
                this.value = value;
            }
        }
    }
}