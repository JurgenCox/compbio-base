using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Num;

namespace BaseLibS.Graph.Image.Quantizers{
	public sealed class OctreeQuantizer : Quantizer{
		private Octree octree;
		private int colors;
		public OctreeQuantizer() : base(false){}
		public override QuantizedImage Quantize(ImageBase image, int maxColors){
			colors = NumUtils.Clamp(maxColors, 1, 255);
			if (octree == null){
				octree = new Octree(GetBitsNeededForColorDepth(maxColors));
			}
			return base.Quantize(image, maxColors);
		}
		protected override void InitialQuantizePixel(Color2 pixel){
			octree.AddColor(pixel);
		}
		protected override byte QuantizePixel(Color2 pixel){
			byte paletteIndex = (byte) colors;
			if (pixel.ToBytes()[3] > Threshold){
				paletteIndex = (byte) octree.GetPaletteIndex(pixel);
			}
			return paletteIndex;
		}
		protected override List<Color2> GetPalette(){
			List<Color2> palette = octree.Palletize(Math.Max(colors, 1));
			int diff = colors - palette.Count;
			if (diff > 0){
				palette.AddRange(Enumerable.Repeat(default(Color2), diff));
			}
			TransparentIndex = colors;
			return palette;
		}
		private int GetBitsNeededForColorDepth(int colorCount){
			return (int) Math.Ceiling(Math.Log(colorCount, 2));
		}
		private class Octree{
			private static readonly int[] Mask = {0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01};
			private readonly OctreeNode root;
			private readonly OctreeNode[] reducibleNodes;
			private readonly int maxColorBits;
			private OctreeNode previousNode;
			private int previousColor;
			public Octree(int maxColorBits){
				this.maxColorBits = maxColorBits;
				Leaves = 0;
				reducibleNodes = new OctreeNode[9];
				root = new OctreeNode(0, this.maxColorBits, this);
				previousColor = default(int);
				previousNode = null;
			}
			private int Leaves { get; set; }
			private OctreeNode[] ReducibleNodes => reducibleNodes;
			public void AddColor(Color2 pixel){
				int packed = pixel.GetPackedValue();
				if (previousColor.Equals(packed)){
					if (previousNode == null){
						previousColor = packed;
						root.AddColor(pixel, maxColorBits, 0, this);
					} else{
						previousNode.Increment(pixel);
					}
				} else{
					previousColor = packed;
					root.AddColor(pixel, maxColorBits, 0, this);
				}
			}
			public List<Color2> Palletize(int colorCount){
				while (Leaves > colorCount){
					Reduce();
				}
				List<Color2> palette = new List<Color2>(Leaves);
				int paletteIndex = 0;
				root.ConstructPalette(palette, ref paletteIndex);
				return palette;
			}
			public int GetPaletteIndex(Color2 pixel){
				return root.GetPaletteIndex(pixel, 0);
			}
			protected void TrackPrevious(OctreeNode node){
				previousNode = node;
			}
			private void Reduce(){
				int index = maxColorBits - 1;
				while ((index > 0) && (reducibleNodes[index] == null)){
					index--;
				}
				OctreeNode node = reducibleNodes[index];
				reducibleNodes[index] = node.NextReducible;
				Leaves -= node.Reduce();
				previousNode = null;
			}
			protected class OctreeNode{
				private readonly OctreeNode[] children;
				private bool leaf;
				private int pixelCount;
				private int red;
				private int green;
				private int blue;
				private int paletteIndex;
				public OctreeNode(int level, int colorBits, Octree octree){
					leaf = level == colorBits;
					red = green = blue = 0;
					pixelCount = 0;
					if (leaf){
						octree.Leaves++;
						NextReducible = null;
						children = null;
					} else{
						NextReducible = octree.ReducibleNodes[level];
						octree.ReducibleNodes[level] = this;
						children = new OctreeNode[8];
					}
				}
				public OctreeNode NextReducible { get; }
				public void AddColor(Color2 pixel, int colorBits, int level, Octree octree){
					if (leaf){
						Increment(pixel);
						octree.TrackPrevious(this);
					} else{
						int shift = 7 - level;
						byte[] components = pixel.ToBytes();
						int index = ((components[2] & Mask[level]) >> (shift - 2)) | ((components[1] & Mask[level]) >> (shift - 1)) |
									((components[0] & Mask[level]) >> shift);
						OctreeNode child = children[index];
						if (child == null){
							child = new OctreeNode(level + 1, colorBits, octree);
							children[index] = child;
						}
						child.AddColor(pixel, colorBits, level + 1, octree);
					}
				}
				public int Reduce(){
					red = green = blue = 0;
					int childNodes = 0;
					for (int index = 0; index < 8; index++){
						if (children[index] != null){
							red += children[index].red;
							green += children[index].green;
							blue += children[index].blue;
							pixelCount += children[index].pixelCount;
							++childNodes;
							children[index] = null;
						}
					}
					leaf = true;
					return childNodes - 1;
				}
				public void ConstructPalette(List<Color2> palette, ref int index){
					if (leaf){
						paletteIndex = index++;
						byte r = ToByte(red/pixelCount);
						byte g = ToByte(green/pixelCount);
						byte b = ToByte(blue/pixelCount);
						Color2 pixel = Color2.FromArgb(255, r, g, b);
						palette.Add(pixel);
					} else{
						for (int i = 0; i < 8; i++){
							if (children[i] != null){
								children[i].ConstructPalette(palette, ref index);
							}
						}
					}
				}
				private static byte ToByte(int value){
					return (byte) NumUtils.Clamp(value, 0, 255);
				}
				public int GetPaletteIndex(Color2 pixel, int level){
					int index = paletteIndex;
					if (!leaf){
						int shift = 7 - level;
						byte[] components = pixel.ToBytes();
						int pixelIndex = ((components[2] & Mask[level]) >> (shift - 2)) | ((components[1] & Mask[level]) >> (shift - 1)) |
										((components[0] & Mask[level]) >> shift);
						if (children[pixelIndex] != null){
							index = children[pixelIndex].GetPaletteIndex(pixel, level + 1);
						} else{
							throw new Exception($"Cannot retrive a pixel at the given index {pixelIndex}.");
						}
					}
					return index;
				}
				public void Increment(Color2 pixel){
					pixelCount++;
					byte[] components = pixel.ToBytes();
					red += components[0];
					green += components[1];
					blue += components[2];
				}
			}
		}
	}
}