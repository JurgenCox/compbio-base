using System.Drawing;
using System.Drawing.Imaging;

namespace BaseLib.Graphic{
	public sealed class BitmapGraphics : WindowsBasedGraphics{
		public Bitmap Bitmap { get; }
		private readonly string filename;
		private readonly ImageFormat imageFormat;
		public BitmapGraphics(int width, int height) : this(null, width, height, null){}

		public BitmapGraphics(string filename, int width, int height, ImageFormat imageFormat) : base(null){
			Bitmap = new Bitmap(width, height);
			gc = Graphics.FromImage(Bitmap);
			this.filename = filename;
			this.imageFormat = imageFormat;
		}

		public override void Close(){
			//TODO: this seems to write EMF or WMF as PNG. (Issue Perseus-104)
			Bitmap.Save(filename, imageFormat);
		}
	}
}