using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using BaseLibS.Parse;
using BaseLibS.Util;
using NUnit.Framework;

namespace BaseLibS.Test.Parse
{
	[TestFixture]
	public class TabSepTest
	{
		[Test]
		public void TestGetColumnNamesFromGzippedFile()
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
			var columnNames = TabSep.GetColumnNames(tmpFile, ' ');
			CollectionAssert.AreEqual(new [] {"Col_1", "Col_2"}, columnNames);
		}
	}
}
