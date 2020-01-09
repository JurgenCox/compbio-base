using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Bmp{
	public class BmpDecoder : IImageDecoder{
		public int HeaderSize => 2;
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			extension = extension.StartsWith(".") ? extension.Substring(1) : extension;
			return extension.Equals("BMP", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals("DIP", StringComparison.OrdinalIgnoreCase);
		}
		public bool IsSupportedFileFormat(byte[] header){
			bool isBmp = false;
			if (header.Length >= 2){
				isBmp = header[0] == 0x42 && // B
						header[1] == 0x4D; // M
			}
			return isBmp;
		}
		public void Decode(Image2 image, Stream stream) {
			if (image == null || stream == null){
				throw new ArgumentNullException();
			}
			new BmpDecoderCore().Decode(image, stream);
		}
	}
}