using BaseLibS.Util;
using NUnit.Framework;

namespace BaseLibS.Test.Util {
	[TestFixture]
	public class FileUtilsTest {
		[Test]
		public void TestAnnot()
		{
		    var executablePath = FileUtils.executablePath;
            Assert.IsNotNull(executablePath);

		}
	}
}