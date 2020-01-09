using System.IO;
using System.IO.Compression;
using System.Text;
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
		[Test]
		public void TestGetSeekableGzipStream()
		{
			var lines = new[]
			{
				"Col_1 Col_2",
				"a b",
			};
			var tmpFile = Path.GetTempFileName() + ".gz";
			using (var memory = new MemoryStream(Encoding.UTF8.GetBytes(string.Join("\n", lines))))
			using (var outFile = File.Create(tmpFile))
			using (var gzip = new GZipStream(outFile, CompressionMode.Compress))
			{
				memory.CopyTo(gzip);
			}
			using (var reader = FileUtils.GetReader(tmpFile, true))
			{
				Assert.IsTrue(reader.BaseStream.CanSeek);
			}
		}
	}
}