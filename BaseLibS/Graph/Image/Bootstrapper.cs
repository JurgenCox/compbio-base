using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BaseLibS.Graph.Image.Formats;
using BaseLibS.Graph.Image.Formats.Bmp;
using BaseLibS.Graph.Image.Formats.Gif;
using BaseLibS.Graph.Image.Formats.Jpg;
using BaseLibS.Graph.Image.Formats.Png;

namespace BaseLibS.Graph.Image{
	public class Bootstrapper{
		private static readonly Lazy<Bootstrapper> lazy = new Lazy<Bootstrapper>(() => new Bootstrapper());
		private readonly List<IImageFormat> imageFormats;
		private readonly Dictionary<Type, Func<IImageBase, IPixelAccessor>> pixelAccessors;
		private Bootstrapper(){
			imageFormats = new List<IImageFormat>{new BmpFormat(), new JpegFormat(), new PngFormat(), new GifFormat()};
			pixelAccessors = new Dictionary<Type, Func<IImageBase, IPixelAccessor>>{
				{typeof (Color2), i => new ColorPixelAccessor(i)}
			};
		}
		public static Bootstrapper instance = lazy.Value;
		public ICollection<IImageFormat> ImageFormats => new ReadOnlyCollection<IImageFormat>(imageFormats);
		public IDictionary<Type, Func<IImageBase, IPixelAccessor>> PixelAccessors
			=> new Dictionary<Type, Func<IImageBase, IPixelAccessor>>(pixelAccessors);
		public ParallelOptions ParallelOptions { get; set; } = new ParallelOptions{
			MaxDegreeOfParallelism = Environment.ProcessorCount
		};
		public void AddImageFormat(IImageFormat format){
			imageFormats.Add(format);
		}
		public void AddPixelAccessor(Type packedType, Func<IImageBase, IPixelAccessor> initializer){
			if (!typeof (Color2).IsAssignableFrom(packedType)){
				throw new ArgumentException($"Type {packedType} must implement {nameof(Color2)}");
			}
			pixelAccessors.Add(packedType, initializer);
		}
		public IPixelAccessor GetPixelAccessor(IImageBase image) {
			Type packed = typeof (Color2);
			if (pixelAccessors.ContainsKey(packed)){
				return pixelAccessors[packed].Invoke(image);
			}
			throw new NotSupportedException($"PixelAccessor cannot be loaded for {packed}:");
		}
	}
}