using System.Linq;
using BaseLibS.Util;
using NUnit.Framework;

namespace BaseLibS.Test.Util {
	[TestFixture]
	public class StringUtilsTest {
		[Test]
		public void TestConcat() {
			string[] strings = {"", "b", "c"};
			string concatList = StringUtils.Concat(",", strings);
			Assert.AreEqual(",b,c", concatList);
			string concatEnumerable = StringUtils.Concat(",", strings.AsEnumerable());
			Assert.AreEqual(",b,c", concatEnumerable);
		}
	}
}