using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Api;
using BaseLibS.Num.Cluster;
using BaseLibS.Num.Matrix;
using NumPluginBase.Distance;
using NUnit.Framework;

namespace BaseLibS.Test
{
    [TestFixture]
    public class ClusteringTest
    {

        [Test]
        public void TestClusterNodeFormat()
        {
            HierarchicalClustering hclust = new HierarchicalClustering();
            FloatMatrixIndexer data = new FloatMatrixIndexer(new float[,] {{1, 2, 3}, {2, 3, 4}});
            EuclideanDistance distance = new EuclideanDistance();
            HierarchicalClusterNode[] rowTree = hclust.TreeCluster(data, MatrixAccess.Rows, distance,
                HierarchicalClusterLinkage.Maximum, false,
                false, 1, i => { });
            HierarchicalClusterNode[] rowTreeR =
                HierarchicalClusterNode.FromRFormat(new[] {-1}, new[] {-2}, new[] {1.732051});
            Assert.AreEqual(rowTreeR[0], rowTree[0]);

            HierarchicalClusterNode[] colTree = hclust.TreeCluster(data, MatrixAccess.Columns, distance,
                HierarchicalClusterLinkage.Maximum, false,
                false, 1, i => { });
            HierarchicalClusterNode[] colTreeR =
                HierarchicalClusterNode.FromRFormat(new[] {-1, -3}, new[] {-2, 1}, new[] {1.414214, 2.828428});
            CollectionAssert.AreEqual(colTree, colTreeR);
        }

        [Test]
        public void TestKmedoidClustering()
        {
            FloatMatrixIndexer data = new FloatMatrixIndexer(new float[,]
            {
                {2, 6}, {3, 4}, {3, 8}, {4, 7}, {6, 2}, {6, 4}, {7, 3}, {7, 4}, {8, 5}, {7, 6}
            });
            GenericDistanceMatrix distance = new GenericDistanceMatrix(data, new L1Distance());
            int[] clustering = KmedoidClustering.GenerateClusters(data, distance, 2);
            Assert.IsTrue(new[] {0, 0, 0, 0, 7, 7, 7, 7, 7, 7}.SequenceEqual(clustering));
        }

        private static HierarchicalClusterLinkage[] _linkages =
            {HierarchicalClusterLinkage.Average, HierarchicalClusterLinkage.Maximum, HierarchicalClusterLinkage.Single};

        private static IDistance[] _distances =
        {
            new EuclideanDistance(), new CanberraDistance(), new CosineDistance(), new L1Distance(), new LpDistance(3),
            new MaximumDistance(), new PearsonCorrelationDistance(), new SpearmanCorrelationDistance()
        };

        [Test]
        public void TestKmeansClusteringPreculusteringWithDuplicateRows(
            [ValueSource(nameof(_data))] float[,] values,
            [ValueSource(nameof(_linkages))] HierarchicalClusterLinkage linkage,
            [ValueSource(nameof(_distances))] IDistance distance)
        {
            HierarchicalClustering hclust = new HierarchicalClustering();
            FloatMatrixIndexer data = new FloatMatrixIndexer(values);
            var clusterNodes = hclust.TreeClusterKmeans(data, MatrixAccess.Columns, distance, linkage, false, false, 1,
                2, 1, 1000, i => { });
            Assert.AreEqual(3, clusterNodes.Length);
        }
        [Test]
        public void TestKmeansClusteringPreculusteringWithManyDuplicateRows()
        {
            HierarchicalClustering hclust = new HierarchicalClustering();
            var values = new float[,] {{1, 2, 3, 1}, {2, 3, 4, 2}, {2, 3, 4, 2}, {2, 3, 4, 2}, {2, 3, 4, 2}, {3, 4, 5, 2}};
            FloatMatrixIndexer data = new FloatMatrixIndexer(values);
            var clusterNodes = hclust.TreeClusterKmeans(data, MatrixAccess.Rows, new EuclideanDistance(), HierarchicalClusterLinkage.Average, false, false, 1,
                5, 1, 1000, i => { });
            Assert.AreEqual(5, clusterNodes.Length);
        }

        private static List<float[,]> _data = new List<float[,]>
        {
            new float[,] {{1, 2, 3, 1}},
            new float[,] {{1, 2, 3, 1}, {2, 3, 4, 2}},
            new float[,] {{1, 2, 3, 1}, {2, 3, 4, 2}, {3, 4, 5, 2}}
        };

        [Test]
        public void TestKmeans()
        {
            FloatMatrixIndexer data = new FloatMatrixIndexer(new float[,] {{1, 2, 3, 1}, {2, 3, 4, 2}});
            KmeansClustering.GenerateClusters(data.Transpose(), 2, 100, 1, i => { }, out var clusterCenters, out var clusterIndices);
            Assert.AreEqual(2, clusterCenters.GetLength(0));
        }


    }
}
