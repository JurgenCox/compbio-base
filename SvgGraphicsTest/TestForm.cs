using System.IO;
using System.Windows.Forms;
using BaseLib.Graphic;
using BaseLibS.Graph;
using Path = System.IO.Path;

namespace SvgGraphicsTest
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
			var tmpFile = Path.GetTempFileName();
			var graphics = new SvgGraphics2(tmpFile);
			graphics.FillEllipse(Brushes2.Red, 0, 0, 20, 20);
			graphics.TranslateTransform(50, 50);
			graphics.FillEllipse(Brushes2.Green, 0, 0, 20, 20);
			graphics.DrawString("Write horizontal text", new Font2("Arial", 12), Brushes2.Black, 30, 30);
			graphics.RotateTransform(90);
			graphics.DrawString("Write vertical text", new Font2("Arial", 12), Brushes2.Black, 30, 30);
			graphics.Close();
			var content = File.ReadAllText(tmpFile);
			webBrowser.DocumentText = content;
		}
	}
}
