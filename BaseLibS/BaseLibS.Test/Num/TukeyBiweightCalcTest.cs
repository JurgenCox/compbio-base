using System;
using System.Collections.Generic;
using BaseLibS.Num;
using NUnit.Framework;

namespace BaseLibS.Test.Num
{
	[TestFixture]
	public class TukeyBiweightCalcTest
	{
		[Test]
		public void TestOnEmptyArray()
		{
			var nan = TukeyBiweightCalc.TukeyBiweight(new List<double> { });
			Assert.AreEqual(double.NaN, nan);
		}
	}
}
