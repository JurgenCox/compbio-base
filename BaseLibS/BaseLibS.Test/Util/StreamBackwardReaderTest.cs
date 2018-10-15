using System.Collections.Generic;
using System.IO;
using System.Text;
using BaseLibS.Util;
using NUnit.Framework;

namespace BaseLibS.Test.Util
{
	[TestFixture]
	public class StreamBackwardReaderTest
	{
		[Test]
		public void TestReadingBackwards()
		{
			var text = "these\nare\nsome\nlines";
			var linesBackwards = new List<string>();
			using (var memory = new MemoryStream(Encoding.UTF8.GetBytes(text)))
			using (var reader = new StreamBackwardReader(memory))
			{
				linesBackwards.AddRange(reader.ReadLines());	
			}
			linesBackwards.Reverse();
			var actual = string.Join("\n", linesBackwards);
			Assert.AreEqual(text, actual);
		}
			
	}
}