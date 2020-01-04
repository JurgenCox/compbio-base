using System;
using System.IO;
using BaseLibS.Graph.Image.Quantizers;

namespace BaseLibS.Graph.Image.Formats.Gif{
	public class GifEncoder : IImageEncoder{
		public int Quality { get; set; }
		public byte Threshold { get; set; } = 128;
		public IQuantizer Quantizer { get; set; }
		public string Extension => "gif";
		public string MimeType => "image/gif";
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			extension = extension.StartsWith(".") ? extension.Substring(1) : extension;
			return extension.Equals(Extension, StringComparison.OrdinalIgnoreCase);
		}
		public void Encode(ImageBase image, Stream stream) {
			GifEncoderCore encoder = new GifEncoderCore{Quality = Quality, Quantizer = Quantizer, Threshold = Threshold};
			encoder.Encode(image, stream);
		}
	}
}