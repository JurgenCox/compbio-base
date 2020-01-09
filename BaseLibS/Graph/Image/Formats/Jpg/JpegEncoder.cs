using System;
using System.IO;
using BaseLibS.Num;

namespace BaseLibS.Graph.Image.Formats.Jpg{
	public class JpegEncoder : IImageEncoder{
		private int quality = 75;
		private JpegSubsample subsample = JpegSubsample.Ratio420;
		private bool subsampleSet;
		public int Quality{
			get => quality;
			set => quality = NumUtils.Clamp(value,1, 100);
		}
		public JpegSubsample Subsample{
			get => subsample;
			set{
				subsample = value;
				subsampleSet = true;
			}
		}
		public string MimeType => "image/jpeg";
		public string Extension => "jpg";
		public bool IsSupportedFileExtension(string extension){
			if (string.IsNullOrEmpty(extension)){
				throw new ArgumentNullException();
			}
			if (extension.StartsWith(".")){
				extension = extension.Substring(1);
			}
			return extension.Equals(Extension, StringComparison.OrdinalIgnoreCase) ||
					extension.Equals("jpeg", StringComparison.OrdinalIgnoreCase) ||
					extension.Equals("jfif", StringComparison.OrdinalIgnoreCase);
		}
		public void Encode(ImageBase image, Stream stream) {
			JpegEncoderCore encode = new JpegEncoderCore();
			if (subsampleSet){
				encode.Encode(image, stream, Quality, Subsample);
			} else{
				encode.Encode(image, stream, Quality, Quality >= 80 ? JpegSubsample.Ratio444 : JpegSubsample.Ratio420);
			}
		}
	}
}