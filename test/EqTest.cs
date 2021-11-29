using Kk.LeoQuery;
using NUnit.Framework;

namespace leoquery.test
{
    public class EqTest
    {
        [Test]
        public void Test1()
        {
            Entity a = default;
            Entity<int> b = default;

            Assert.True(a == b);
        }
    }
}