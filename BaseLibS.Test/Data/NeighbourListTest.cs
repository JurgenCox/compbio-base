using System;
using BaseLibS.Data;
using NUnit.Framework;

namespace BaseLibS.Test.Data
{
    [TestFixture]
    public class NeighbourListTest
    {
        [Test]
        public void TestGetClusters()
        {
            var neighbours = new NeighbourList();
            neighbours.Add(0, 1);
            neighbours.Add(2, 3);
            var clusters = neighbours.GetAllClusters();
            Assert.AreEqual(2, clusters.Length);
            CollectionAssert.AreEqual(new [] {0, 1}, clusters[0]);
            CollectionAssert.AreEqual(new [] {2, 3}, clusters[1]);
        }
    }
}
