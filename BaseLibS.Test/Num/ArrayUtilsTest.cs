using System.Linq;
using BaseLibS.Num;
using NUnit.Framework;

namespace BaseLibS.Test.Num
{
    [TestFixture]
    public class ArrayUtilsTest
    {
        [Test]
        public void TestPackAndUnpackArrayOfArrays()
        {
            var arr = new[]
            {
                new[] {1, 2, 3, 4},
                new[] {5, 6, 7, 8},
            };
            ArrayUtils.PackArrayOfArrays(arr, out var value, out var ind);
            var arr2 = ArrayUtils.UnpackArrayOfArrays(value, ind);
            Assert.AreEqual(arr.Length, arr2.Length);
            CollectionAssert.AreEqual(arr[0], arr2[0]);
            CollectionAssert.AreEqual(arr[1], arr2[1]);
        }

        [Test]
        public void TestPackAndUnpackArrayOfStrings()
        {
            var arr = new[] { "1", "2", "3", "4"};
            ArrayUtils.PackArrayOfStrings(arr, out var value, out var ind);
            var arr2 = ArrayUtils.UnpackArrayOfStrings(value, ind);
            CollectionAssert.AreEqual(arr, arr2);
        }

        [Test]
        public void TestPackAndUnpackEmptyArrayOfStrings()
        {
            var arr = new string[0];
            ArrayUtils.PackArrayOfStrings(arr, out var value, out var ind);
            var arr2 = ArrayUtils.UnpackArrayOfStrings(value, ind);
            CollectionAssert.AreEqual(arr, arr2);
        }

        [Test]
        public void TestHistogram()
        {
            // create some fake data
            // run the histogram function
            // assert correctness
            // CollectionAssert.AreEqual();
            // Assert.AreEqual();
        }

        [Test]
        // Tested against Python 'from sklearn.metrics.pairwise import cosine_similarity'
        public void TestCosineSimilarity()
        {
            var example1 = ArrayUtils.Cosine(new[] {1, 2, 3.0}, new[] {4, 5, 6.0});
            Assert.AreEqual(0.9746, example1, 0.0001);
            var example2 = ArrayUtils.Cosine(new[] {0, 1, 2, 3.0}, new[] {4, 5, 6.0, 0});
            Assert.AreEqual(0.5177, example2, 0.0001);

            var lengthTwo = ArrayUtils.Cosine(new[] {0.0, 1}, new[] {4.0, 5});
            Assert.AreEqual(0.7808, lengthTwo, 0.0001);

            // lengthOne
            var bothValueNonZero = ArrayUtils.Cosine(new[] {1.0}, new[] {4.0});
            Assert.AreEqual(1, bothValueNonZero, 0.0001);
            var oneValueNonZero = ArrayUtils.Cosine(new[] {0.0}, new[] {4.0});
            Assert.AreEqual(0, oneValueNonZero, 0.0001);
            var oneValueZero = ArrayUtils.Cosine(new[] {0.0}, new[] {0.0});
            Assert.AreEqual(0, oneValueZero, 0.0001);

            // zero padding
            var zeroPadding = ArrayUtils.Cosine(new[] {0.0, 1, 2, 3.0, 0.0}, new[] {0.0, 4, 5, 6.0, 0.0});
            Assert.AreEqual(0.9746, zeroPadding, 0.0001);
            var zerosInTheMiddle = ArrayUtils.Cosine(new[] {1, 0.0, 2, 0.0, 3.0}, new[] {4, 0.0, 5, 0.0, 6.0});
            Assert.AreEqual(0.9746, zerosInTheMiddle, 0.0001);
        }

        [Test]
        // Tested against Python 'from sklearn.metrics.pairwise import cosine_similarity'
        public void TestCosineSimilarityFloat()
        {
            var example1 = ArrayUtils.Cosine(new[] {1, 2, 3.0f}, new[] {4, 5, 6.0f});
            Assert.AreEqual(0.9746, example1, 0.0001);
            var example2 = ArrayUtils.Cosine(new[] {0, 1, 2, 3.0f}, new[] {4, 5, 6.0f, 0});
            Assert.AreEqual(0.5177, example2, 0.0001);

            var lengthTwo = ArrayUtils.Cosine(new[] {0.0f, 1}, new[] {4.0f, 5});
            Assert.AreEqual(0.7808, lengthTwo, 0.0001);

            // lengthOne
            var bothValueNonZero = ArrayUtils.Cosine(new[] {1.0f}, new[] {4.0f});
            Assert.AreEqual(1, bothValueNonZero, 0.0001);
            var oneValueNonZero = ArrayUtils.Cosine(new[] {0.0f}, new[] {4.0f});
            Assert.AreEqual(0, oneValueNonZero, 0.0001);
            var oneValueZero = ArrayUtils.Cosine(new[] {0.0f}, new[] {0.0f});
            Assert.AreEqual(0, oneValueZero, 0.0001);

            // zero padding
            var zeroPadding = ArrayUtils.Cosine(new[] {0.0f, 1, 2, 3.0f, 0.0f}, new[] {0.0f, 4, 5, 6.0f, 0.0f});
            Assert.AreEqual(0.9746, zeroPadding, 0.0001);
            var zerosInTheMiddle = ArrayUtils.Cosine(new[] {1, 0.0f, 2, 0.0f, 3.0f}, new[] {4, 0.0f, 5, 0.0f, 6.0f});
            Assert.AreEqual(0.9746, zerosInTheMiddle, 0.0001);
        }

        [Test]
        public void TestComplement()
        {
            var arr = new[] {1, 2, 3};
            var complement = ArrayUtils.Complement(arr, 7);
            CollectionAssert.AreEqual(new [] {0, 4, 5, 6}, complement);
            CollectionAssert.AreEqual(new [] {0, 4, 5, 6}, Enumerable.Range(0, 7).Except(arr));
        }

        [Test]
        public void TestCeilIndexWithSimpleList()
        {
            var arr = new[] {1, 2, 3};
            var index = ArrayUtils.CeilIndex(arr, 2);
            Assert.AreEqual(1, index);
        }

        [Test]
        public void TestCeilIndexWithDuplicateList()
        {
            var arr = new[] {0, 1, 1, 1, 3, 3, 3, 3, 5};
            var index = ArrayUtils.CeilIndex(arr, 2);
            Assert.AreEqual(4, index);
        }

        [Test]
        public void TestCeilIndexWithDuplicateList2()
        {
            var arr = new[] {0, 1, 1, 1, 3, 3, 3, 3, 5};
            var index = ArrayUtils.CeilIndex(arr, 3);
            Assert.AreEqual(4, index);
        }

        [Test]
        public void TestFloorIndexWithDuplicateList()
        {
            var arr = new[] {0, 1, 1, 1, 3, 3, 3, 3, 5};
            var index = ArrayUtils.FloorIndex(arr, 2);
            Assert.AreEqual(3, index);
        }

        [Test]
        public void TestFloorIndexWithDuplicateList2()
        {
            var arr = new[] {0, 1, 1, 1, 3, 3, 3, 3, 5};
            var index = ArrayUtils.FloorIndex(arr, 3);
            Assert.AreEqual(7, index);
        }
    }
}
