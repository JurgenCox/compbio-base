using System.IO;
using System.Windows.Forms;
using BaseLibS.Graph;

namespace BaseLib.Forms.Base{
	public static class Printing{
		public static void Print(IPrintable printable, string filename, int width, int height){
			filename = ShowDialog(filename);
			if (filename == null){
				return;
			}
			string extension = Path.GetExtension(filename).ToLower();
			if (File.Exists(filename)){
				File.Delete(filename);
			}
			BasicImageFormat format = BasicImageFormat.GetFromExtension(extension);
			if (format == null){
				MessageBox.Show("Could not find the specified file format: " + extension);
			}
			IGraphics graphics = format.CreateGraphics(filename, width, height);
			printable.Print(graphics, width, height);
			graphics.Close();
			graphics.Dispose();
		}

		public static void PrintFullSize(IScrollableControl c, string name) {
			int x = c.VisibleX;
			int y = c.VisibleY;
			c.VisibleX = 0;
			c.VisibleY = 0;
			Print(c, name, c.TotalClientWidth, c.TotalClientHeight);
			c.VisibleX = x;
			c.VisibleY = y;
		}

		public static void PrintVisibleSize(IScrollableControl c, string name) {
			Print(c, name, c.Width1, c.Height1);
		}

		private static string ShowDialog(string filename) {
			SaveFileDialog dialog = new SaveFileDialog{Filter = BasicImageFormat.GetFilter(), FileName = filename};
			return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
		}
	}
}