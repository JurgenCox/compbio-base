using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BaseLibS.Graph.Image.Formats;
using BaseLibS.Graph.Image.Formats.Png;
using BaseLibS.Util;

namespace BaseLibS.Graph.Image{
	public class Image2 : ImageBase{
		public const double defaultHorizontalResolution = 96;
		public const double defaultVerticalResolution = 96;
		public Image2(){
			CurrentImageFormat = Bootstrapper.instance.ImageFormats.First(f => f.GetType() == typeof (PngFormat));
		}
		public Image2(int width, int height) : base(width, height){
			CurrentImageFormat = Bootstrapper.instance.ImageFormats.First(f => f.GetType() == typeof (PngFormat));
		}
		public Image2(Stream stream){
			if (stream == null){
				throw new ArgumentNullException();
			}
			Load(stream);
		}
		public Image2(Image2 other) : base(other){
			foreach (ImageFrame frame in other.Frames){
				if (frame != null){
					Frames.Add(new ImageFrame(frame));
				}
			}
			RepeatCount = other.RepeatCount;
			HorizontalResolution = other.HorizontalResolution;
			VerticalResolution = other.VerticalResolution;
			CurrentImageFormat = other.CurrentImageFormat;
		}
		public ICollection<IImageFormat> Formats { get; } = Bootstrapper.instance.ImageFormats;
		public double HorizontalResolution { get; set; } = defaultHorizontalResolution;
		public double VerticalResolution { get; set; } = defaultVerticalResolution;
		public double InchWidth{
			get{
				double resolution = HorizontalResolution;
				if (resolution <= 0){
					resolution = defaultHorizontalResolution;
				}
				return Width/resolution;
			}
		}
		public double InchHeight{
			get{
				double resolution = VerticalResolution;
				if (resolution <= 0){
					resolution = defaultVerticalResolution;
				}
				return Height/resolution;
			}
		}
		public bool IsAnimated => Frames.Count > 0;
		public ushort RepeatCount { get; set; }
		public IList<ImageFrame> Frames { get; } = new List<ImageFrame>();
		public IList<ImageProperty> Properties { get; } = new List<ImageProperty>();
		public IImageFormat CurrentImageFormat { get; set; }
		public override IPixelAccessor Lock(){
			return Bootstrapper.instance.GetPixelAccessor(this);
		}
		public void Save(Stream stream){
			if (stream == null){
				throw new ArgumentNullException();
			}
			CurrentImageFormat.Encoder.Encode(this, stream);
		}
		public void Save(Stream stream, IImageFormat format){
			if (stream == null){
				throw new ArgumentNullException();
			}
			format.Encoder.Encode(this, stream);
		}
		public void Save(Stream stream, IImageEncoder encoder){
			if (stream == null){
				throw new ArgumentNullException();
			}
			encoder.Encode(this, stream);
		}
		public override string ToString(){
			using (MemoryStream stream = new MemoryStream()){
				Save(stream);
				stream.Flush();
				return $"data:{CurrentImageFormat.Encoder.MimeType};base64,{Convert.ToBase64String(stream.ToArray())}";
			}
		}
		private void Load(Stream stream){
			if (!Formats.Any()){
				return;
			}
			if (!stream.CanRead){
				throw new NotSupportedException("Cannot read from the stream.");
			}
			if (!stream.CanSeek){
				throw new NotSupportedException("The stream does not support seeking.");
			}
			int maxHeaderSize = Formats.Max(x => x.Decoder.HeaderSize);
			if (maxHeaderSize > 0){
				byte[] header = new byte[maxHeaderSize];
				stream.Position = 0;
				stream.Read(header, 0, maxHeaderSize);
				stream.Position = 0;
				IImageFormat format = Formats.FirstOrDefault(x => x.Decoder.IsSupportedFileFormat(header));
				if (format != null){
					format.Decoder.Decode(this, stream);
					CurrentImageFormat = format;
					return;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Image cannot be loaded. Available formats:");
			foreach (IImageFormat format in Formats){
				stringBuilder.AppendLine("-" + format);
			}
			throw new NotSupportedException(stringBuilder.ToString());
		}
		public static Bitmap2 ReadImage(string filePath){
			BinaryReader reader = FileUtils.GetBinaryReader(filePath);
			Bitmap2 bm = ReadImage(reader.BaseStream);
			reader.Close();
			return bm;
		}
		public static Bitmap2 ReadImage(Stream stream){
			return stream == null ? null : new Bitmap2(stream);
		}
	}
}