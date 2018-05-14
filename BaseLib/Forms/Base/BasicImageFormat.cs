using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using BaseLib.Graphic;
using BaseLibS.Graph;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLib.Forms.Base{
	public sealed class BasicImageFormat{
		public static readonly BasicImageFormat pdf = new BasicImageFormat(new[]{".pdf"}, "PDF Portable Document Format",
			(filename, width, height) => new PdfGraphics(filename, width, height));
		public static readonly BasicImageFormat svg = new BasicImageFormat(new[]{".svg"}, "SVG Scalable Vector Graphics",
			(filename, width, height) => new SvgGraphics(filename, width, height));
		public static readonly BasicImageFormat bmp = new BasicImageFormat(new[]{".bmp"}, "BMP Windows Bitmap",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Bmp));
		public static readonly BasicImageFormat emf = new BasicImageFormat(new[]{".emf"}, "EMF Windows Enhanced Meta File",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Emf));
		public static readonly BasicImageFormat gif = new BasicImageFormat(new[]{".gif"}, "GIF Graphics Interchange Format",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Gif));
		public static readonly BasicImageFormat jpeg = new BasicImageFormat(new[]{".jpg", ".jif", ".jpe", ".jpeg"}, "JPG JPEG",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Jpeg));
		public static readonly BasicImageFormat png = new BasicImageFormat(new[]{".png"}, "PNG Portable Network Graphics",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Png));
		public static readonly BasicImageFormat tiff = new BasicImageFormat(new[]{".tif", ".tiff"},
			"TIF Tagged Image File Format",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Tiff));
		public static readonly BasicImageFormat wmf = new BasicImageFormat(new[]{".wmf"}, "WMF Windows Meta File",
			(filename, width, height) => new BitmapGraphics(filename, width, height, ImageFormat.Wmf));
		public static readonly BasicImageFormat[] allFormats = { png, pdf, gif, jpeg, tiff, wmf, bmp, emf };// svg
		private static readonly Dictionary<string, BasicImageFormat> map = new Dictionary<string, BasicImageFormat>();

		static BasicImageFormat(){
			foreach (BasicImageFormat format in allFormats){
				foreach (string ex in format.extensions){
					map.Add(ex, format);
				}
			}
		}

		private readonly string[] extensions;
		private readonly string description;
		private readonly Func<string, int, int, IGraphics> create;

		public static BasicImageFormat GetFromExtension(string ex){
			return map.ContainsKey(ex) ? map[ex] : null;
		}

		public static string GetFilter(){
			return StringUtils.Concat("|", ArrayUtils.FillArray(i => allFormats[i].GetFilterImpl(), allFormats.Length));
		}

		private BasicImageFormat(string[] extensions, string description, Func<string, int, int, IGraphics> create){
			this.extensions = extensions;
			this.description = description;
			this.create = create;
		}

		public IGraphics CreateGraphics(string filename, int width, int height){
			return create(filename, width, height);
		}

		internal string GetFilterImpl(){
			string ext = StringUtils.Concat(";", extensions);
			return description + "|" + ext;
		}
	}
}