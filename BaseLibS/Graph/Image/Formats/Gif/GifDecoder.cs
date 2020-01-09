using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Gif{
	public class GifDecoder : IImageDecoder{
		public int HeaderSize => 6;
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			extension = extension.StartsWith(".") ? extension.Substring(1) : extension;
			return extension.Equals("GIF", StringComparison.OrdinalIgnoreCase);
		}
		public bool IsSupportedFileFormat(byte[] header){
			return header.Length >= 6 && header[0] == 0x47 && // G
					header[1] == 0x49 && // I
					header[2] == 0x46 && // F
					header[3] == 0x38 && // 8
					(header[4] == 0x39 || header[4] == 0x37) && // 9 or 7
					header[5] == 0x61; // a
		}
		public void Decode(Image2 image, Stream stream){
			new GifDecoderCore().Decode(image, stream);
		}
	}
}