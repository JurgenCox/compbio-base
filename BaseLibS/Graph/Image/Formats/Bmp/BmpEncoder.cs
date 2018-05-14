using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Bmp{
	public class BmpEncoder : IImageEncoder{
		public int Quality { get; set; }
		public string MimeType => "image/bmp";
		public string Extension => "bmp";
		public BmpBitsPerPixel BitsPerPixel { get; set; } = BmpBitsPerPixel.Pixel24;
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			extension = extension.StartsWith(".") ? extension.Substring(1) : extension;
			return extension.Equals(Extension, StringComparison.OrdinalIgnoreCase) ||
					extension.Equals("dip", StringComparison.OrdinalIgnoreCase);
		}
		public void Encode(ImageBase image, Stream stream){
			BmpEncoderCore encoder = new BmpEncoderCore();
			encoder.Encode(image, stream, BitsPerPixel);
		}
	}
}