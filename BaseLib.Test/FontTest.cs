using System.Drawing;
using BaseLib.Graphic;
using BaseLibS.Drawing;
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
            Font2 font2 = new Font2("Courier New", 9);
            Font convertedFont = GraphUtils.ToFont(font2);
            Font nativeFont = new Font(FontFamily.GenericMonospace, 9);
            Assert.AreEqual(nativeFont.Name, convertedFont.Name);
            Assert.AreEqual(nativeFont.Size, convertedFont.Size);
        }

        [Test]
        public void TestArialUnicode()
        {
            Font2 font2 = new Font2("Arial Unicode MS", 9, FontStyle2.Regular);
            iTextSharp.text.Font font = PdfGraphicUtils.GetFont(font2);
            Assert.IsNotNull(font.BaseFont);
            Font convertedFont = GraphUtils.ToFont(font2);
            Assert.IsNotNull(convertedFont);
        }
    }
}
