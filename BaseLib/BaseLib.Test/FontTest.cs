using System.Drawing;
using BaseLib.Graphic;
using BaseLibS.Graph;
using NUnit.Framework;

namespace BaseLib.Test
{
    [TestFixture]
    public class FontTest
    {
        [Test]
        public void TestMonospaceFont()
        {
            var font2 = new Font2("Courier New", 9);
            var convertedFont = GraphUtils.ToFont(font2);
            var nativeFont = new Font(FontFamily.GenericMonospace, 9);
            Assert.AreEqual(nativeFont.Name, convertedFont.Name);
            Assert.AreEqual(nativeFont.Size, convertedFont.Size);
        }

        [Test]
        public void TestArialUnicode()
        {
            var font2 = new Font2("Arial Unicode MS", 9, FontStyle2.Regular);
            var font = PdfGraphicUtils.GetFont(font2);
            Assert.IsNotNull(font.BaseFont);
            var convertedFont = GraphUtils.ToFont(font2);
            Assert.IsNotNull(convertedFont);
        }
    }
}
