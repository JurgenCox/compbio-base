using BaseLibS.Num.Matrix;
using NumPluginBase.Distance;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace NumPluginBase.Test
{
    [TestFixture]
    public class DistanceMatrixTest
    {
        [Test]
        public void TestGeneralDistanceMatrix()
        {
            EuclideanDistance distance = new EuclideanDistance();
            GenericDistanceMatrix distanceMatrix = new GenericDistanceMatrix(new FloatMatrixIndexer(new float[,] { {0,0}, {1,0}, {3,0} }), distance);
            Assert.AreEqual(0, distanceMatrix[0,0]);
            Assert.AreEqual(0, distanceMatrix[2,2]);
            Assert.AreEqual(distanceMatrix[1,2], distanceMatrix[2,1]);
            Assert.AreEqual(1, distanceMatrix[1,0]);
            Assert.AreEqual(2, distanceMatrix[1,2]);

            distanceMatrix[0, 1] = -1;
            Assert.AreEqual(-1, distanceMatrix[0, 1]);
        }
    }
}
