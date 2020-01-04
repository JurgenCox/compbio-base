using System;
using System.IO;
using BaseLibS.Graph.Image.Quantizers;

namespace BaseLibS.Graph.Image.Formats.Png{
	public class PngEncoder : IImageEncoder{
		public int Quality { get; set; }
		public string MimeType => "image/png";
		public string Extension => "png";
		public int CompressionLevel { get; set; } = 6;
		public float Gamma { get; set; } = 2.2F;
		public IQuantizer Quantizer { get; set; }
		public byte Threshold { get; set; } = 128;
		public bool WriteGamma { get; set; }
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			extension = extension.StartsWith(".") ? extension.Substring(1) : extension;
			return extension.Equals(Extension, StringComparison.OrdinalIgnoreCase);
		}
		public void Encode(ImageBase image, Stream stream) {
			PngEncoderCore encoder = new PngEncoderCore{
				CompressionLevel = CompressionLevel,
				Gamma = Gamma,
				Quality = Quality,
				Quantizer = Quantizer,
				WriteGamma = WriteGamma,
				Threshold = Threshold
			};
			encoder.Encode(image, stream);
		}
	}
}