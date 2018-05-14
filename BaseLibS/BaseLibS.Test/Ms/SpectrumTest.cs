using System;
using BaseLibS.Ms;
using NUnit.Framework;

namespace BaseLibS.Test.Ms
{
    [TestFixture]
    public class SpectrumTest
    {
        [Test]
        public void TestCalcMinPeakIndex()
        {
            var spectrum = new Spectrum(new[] {1, 2, 3, 4, 5, 6, 7.0}, new[] {2.0, 1, 2, 5, 2, 3, 6});
            var expected = new[] {0, 1, 1, 1, 4, 4, 4, 4};
            for (int i = 0; i < spectrum.Count; i++)
            {
                var actual = spectrum.CalcMinPeakIndex(i);
                Assert.AreEqual(expected[i], actual);

            }
        }
        [Test]
        public void TestCalcMaxPeakIndex()
        {
            var spectrum = new Spectrum(new[] {1, 2, 3, 4, 5, 6, 7.0}, new[] {2.0, 1, 2, 5, 2, 3, 6});
            var expected = new[] {1, 1, 2, 4, 4, 5, 6, 6};
            for (int i = 0; i < spectrum.Count; i++)
            {
                var actual = spectrum.CalcMaxPeakIndex(i);
                Assert.AreEqual(expected[i], actual);

            }
        }
    }
}
 