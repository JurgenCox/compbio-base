using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Jpg{
	public class JpegDecoder : IImageDecoder{
		public int HeaderSize => 11;

		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			if (extension.StartsWith(".")){
				extension = extension.Substring(1);
			}
			return extension.Equals("JPG", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals("Jpeg", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals("JFIF", StringComparison.OrdinalIgnoreCase);
		}

		public bool IsSupportedFileFormat(byte[] header){
			if (header == null){
				throw new ArgumentNullException();
			}
			bool isSupported = false;
			if (header.Length >= 11){
				bool isJfif = IsJfif(header);
				bool isExif = IsExif(header);
				bool isJpeg = IsJpeg(header);
				isSupported = isJfif || isExif || isJpeg;
			}
			return isSupported;
		}

		public void Decode(Image2 image, Stream stream){
			if (image == null || stream == null){
				throw new ArgumentNullException();
			}
			JpegDecoderCore decoder = new JpegDecoderCore();
			decoder.Decode(image, stream, false);
		}

		private static bool IsJfif(byte[] header){
			bool isJfif = header[6] == 0x4A && // J
						header[7] == 0x46 && // F
						header[8] == 0x49 && // I
						header[9] == 0x46 && // F
						header[10] == 0x00;
			return isJfif;
		}

		private static bool IsExif(byte[] header){
			bool isExif = header[6] == 0x45 && // E
						header[7] == 0x78 && // X
						header[8] == 0x69 && // I
						header[9] == 0x66 && // F
						header[10] == 0x00;
			return isExif;
		}

		private static bool IsJpeg(byte[] header){
			bool isJpg = header[0] == 0xFF && // 255
						header[1] == 0xD8; // 216
			return isJpg;
		}
	}
}