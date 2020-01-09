using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Num;

namespace BaseLibS.Graph.Image.Quantizers{
	public class PaletteQuantizer : Quantizer{
		private readonly ConcurrentDictionary<string, byte> colorMap = new ConcurrentDictionary<string, byte>();
		private Color2[] colors;
		public PaletteQuantizer(Color2[] palette = null) : base(true){
			if (palette == null){
				Color2[] constants = Color2.WebSafeColors;
				List<Color2> safe = new List<Color2> {default(Color2) };
				foreach (Color2 c in constants){
					Color2 packed = Color2.FromVector4(c.ToVector4());
					safe.Add(packed);
				}
				colors = safe.ToArray();
			} else{
				colors = palette;
			}
		}
		public override QuantizedImage Quantize(ImageBase image, int maxColors){
			Array.Resize(ref colors, NumUtils.Clamp(maxColors,1, 256));
			return base.Quantize(image, maxColors);
		}
		protected override byte QuantizePixel(Color2 pixel){
			byte colorIndex = 0;
			string colorHash = pixel.ToString();
			if (colorMap.ContainsKey(colorHash)){
				colorIndex = colorMap[colorHash];
			} else{
				byte[] bytes = pixel.ToBytes();
				if (!(bytes[3] > Threshold)){
					for (int index = 0; index < colors.Length; index++){
						if (colors[index].ToBytes()[3] == 0){
							colorIndex = (byte) index;
							TransparentIndex = colorIndex;
							break;
						}
					}
				} else{
					int leastDistance = int.MaxValue;
					int red = bytes[0];
					int green = bytes[1];
					int blue = bytes[2];
					for (int index = 0; index < colors.Length; index++){
						byte[] paletteColor = colors[index].ToBytes();
						int redDistance = paletteColor[0] - red;
						int greenDistance = paletteColor[1] - green;
						int blueDistance = paletteColor[2] - blue;
						int distance = (redDistance*redDistance) + (greenDistance*greenDistance) + (blueDistance*blueDistance);
						if (distance < leastDistance){
							colorIndex = (byte) index;
							leastDistance = distance;
							if (distance == 0){
								break;
							}
						}
					}
				}
				colorMap.TryAdd(colorHash, colorIndex);
			}
			return colorIndex;
		}
		/// <inheritdoc/>
		protected override List<Color2> GetPalette(){
			return colors.ToList();
		}
	}
}