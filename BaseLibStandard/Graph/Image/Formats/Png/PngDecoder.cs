using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Png{
	public class PngDecoder : IImageDecoder{
		public int HeaderSize => 8;
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			extension = extension.StartsWith(".") ? extension.Substring(1) : extension;
			return extension.Equals("Png", StringComparison.OrdinalIgnoreCase);
		}
		public bool IsSupportedFileFormat(byte[] header){
			return header.Length >= 8 && header[0] == 0x89 && header[1] == 0x50 && // P
					header[2] == 0x4E && // N
					header[3] == 0x47 && // G
					header[4] == 0x0D && // CR
					header[5] == 0x0A && // LF
					header[6] == 0x1A && // EOF
					header[7] == 0x0A; // LF
		}
		public void Decode(Image2 image, Stream stream) {
			new PngDecoderCore().Decode(image, stream);
		}
	}
}