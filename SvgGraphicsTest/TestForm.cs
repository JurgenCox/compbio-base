using System.IO;
using System.Windows.Forms;

namespace SvgGraphicsTest
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
			var tmpFile = Path.GetTempFileName();
			using (var writer = new StreamWriter(tmpFile))
			{
				writer.Write("<!DOCTYPE html>\r\n<html>\r\n<head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=9\"/></head><body>\r\n\r\n<svg height=\"100\" width=\"100\">\r\n  <circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"black\" stroke-width=\"3\" fill=\"red\" />\r\n  Sorry, your browser does not support inline SVG.  \r\n</svg> \r\n \r\n</body>\r\n</html>");
			}
			var content = File.ReadAllText(tmpFile);
			webBrowser.DocumentText = content;
		}
	}
}
