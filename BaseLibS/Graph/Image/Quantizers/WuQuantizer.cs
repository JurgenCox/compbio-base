using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLibS.Num;

namespace BaseLibS.Graph.Image.Quantizers{
	public sealed class WuQuantizer : IQuantizer{
		private const float Epsilon = 0.001f;
		private const int IndexBits = 6;
		private const int IndexAlphaBits = 3;
		private const int IndexCount = (1 << IndexBits) + 1;
		private const int IndexAlphaCount = (1 << IndexAlphaBits) + 1;
		private const int TableLength = IndexCount*IndexCount*IndexCount*IndexAlphaCount;
		private readonly long[] vwt;
		private readonly long[] vmr;
		private readonly long[] vmg;
		private readonly long[] vmb;
		private readonly long[] vma;
		private readonly double[] m2;
		private readonly byte[] tag;
		public WuQuantizer(){
			vwt = new long[TableLength];
			vmr = new long[TableLength];
			vmg = new long[TableLength];
			vmb = new long[TableLength];
			vma = new long[TableLength];
			m2 = new double[TableLength];
			tag = new byte[TableLength];
		}
		public byte Threshold { get; set; }
		public QuantizedImage Quantize(ImageBase image, int maxColors){
			if (image == null){
				throw new ArgumentNullException();
			}
			int colorCount = NumUtils.Clamp(maxColors,1, 256);
			Clear();
			using (IPixelAccessor imagePixels = image.Lock()){
				Build3DHistogram(imagePixels);
				Get3DMoments();
				Box[] cube;
				BuildCube(out cube, ref colorCount);
				return GenerateResult(imagePixels, colorCount, cube);
			}
		}
		private static int GetPaletteIndex(int r, int g, int b, int a){
			return (r << ((IndexBits*2) + IndexAlphaBits)) + (r << (IndexBits + IndexAlphaBits + 1)) +
					(g << (IndexBits + IndexAlphaBits)) + (r << (IndexBits*2)) + (r << (IndexBits + 1)) + (g << IndexBits) +
					((r + g + b) << IndexAlphaBits) + r + g + b + a;
		}
		private static double Volume(Box cube, long[] moment){
			return moment[GetPaletteIndex(cube.R1, cube.G1, cube.B1, cube.A1)] -
					moment[GetPaletteIndex(cube.R1, cube.G1, cube.B1, cube.A0)] -
					moment[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A1)] +
					moment[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A0)] -
					moment[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A1)] +
					moment[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A0)] +
					moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A1)] -
					moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A0)] -
					moment[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A1)] +
					moment[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A0)] +
					moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A1)] -
					moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A0)] +
					moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A1)] -
					moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A0)] -
					moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A1)] +
					moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A0)];
		}
		private static long Bottom(Box cube, int direction, long[] moment){
			switch (direction){
				case 0:
					return -moment[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A1)] +
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A1)] -
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A1)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A1)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A0)];
				case 1:
					return -moment[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A1)] +
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A0)] +
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A1)] -
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A1)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A1)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A0)];
				case 2:
					return -moment[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A1)] +
							moment[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A0)] +
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A1)] -
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A1)] -
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A1)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A0)];
				case 3:
					return -moment[GetPaletteIndex(cube.R1, cube.G1, cube.B1, cube.A0)] +
							moment[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A0)] +
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A0)] -
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A0)];
				default:
					throw new ArgumentOutOfRangeException(nameof(direction));
			}
		}
		private static long Top(Box cube, int direction, int position, long[] moment){
			switch (direction){
				case 0:
					return moment[GetPaletteIndex(position, cube.G1, cube.B1, cube.A1)] -
							moment[GetPaletteIndex(position, cube.G1, cube.B1, cube.A0)] -
							moment[GetPaletteIndex(position, cube.G1, cube.B0, cube.A1)] +
							moment[GetPaletteIndex(position, cube.G1, cube.B0, cube.A0)] -
							moment[GetPaletteIndex(position, cube.G0, cube.B1, cube.A1)] +
							moment[GetPaletteIndex(position, cube.G0, cube.B1, cube.A0)] +
							moment[GetPaletteIndex(position, cube.G0, cube.B0, cube.A1)] -
							moment[GetPaletteIndex(position, cube.G0, cube.B0, cube.A0)];
				case 1:
					return moment[GetPaletteIndex(cube.R1, position, cube.B1, cube.A1)] -
							moment[GetPaletteIndex(cube.R1, position, cube.B1, cube.A0)] -
							moment[GetPaletteIndex(cube.R1, position, cube.B0, cube.A1)] +
							moment[GetPaletteIndex(cube.R1, position, cube.B0, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, position, cube.B1, cube.A1)] +
							moment[GetPaletteIndex(cube.R0, position, cube.B1, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, position, cube.B0, cube.A1)] -
							moment[GetPaletteIndex(cube.R0, position, cube.B0, cube.A0)];
				case 2:
					return moment[GetPaletteIndex(cube.R1, cube.G1, position, cube.A1)] -
							moment[GetPaletteIndex(cube.R1, cube.G1, position, cube.A0)] -
							moment[GetPaletteIndex(cube.R1, cube.G0, position, cube.A1)] +
							moment[GetPaletteIndex(cube.R1, cube.G0, position, cube.A0)] -
							moment[GetPaletteIndex(cube.R0, cube.G1, position, cube.A1)] +
							moment[GetPaletteIndex(cube.R0, cube.G1, position, cube.A0)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, position, cube.A1)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, position, cube.A0)];
				case 3:
					return moment[GetPaletteIndex(cube.R1, cube.G1, cube.B1, position)] -
							moment[GetPaletteIndex(cube.R1, cube.G1, cube.B0, position)] -
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B1, position)] +
							moment[GetPaletteIndex(cube.R1, cube.G0, cube.B0, position)] -
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B1, position)] +
							moment[GetPaletteIndex(cube.R0, cube.G1, cube.B0, position)] +
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B1, position)] -
							moment[GetPaletteIndex(cube.R0, cube.G0, cube.B0, position)];
				default:
					throw new ArgumentOutOfRangeException(nameof(direction));
			}
		}
		private void Clear(){
			Array.Clear(vwt, 0, TableLength);
			Array.Clear(vmr, 0, TableLength);
			Array.Clear(vmg, 0, TableLength);
			Array.Clear(vmb, 0, TableLength);
			Array.Clear(vma, 0, TableLength);
			Array.Clear(m2, 0, TableLength);
			Array.Clear(tag, 0, TableLength);
		}
		private void Build3DHistogram(IPixelAccessor pixels){
			for (int y = 0; y < pixels.Height; y++){
				for (int x = 0; x < pixels.Width; x++){
					byte[] color = pixels[x, y].ToBytes();
					byte r = color[0];
					byte g = color[1];
					byte b = color[2];
					byte a = color[3];
					int inr = r >> (8 - IndexBits);
					int ing = g >> (8 - IndexBits);
					int inb = b >> (8 - IndexBits);
					int ina = a >> (8 - IndexAlphaBits);
					int ind = GetPaletteIndex(inr + 1, ing + 1, inb + 1, ina + 1);
					vwt[ind]++;
					vmr[ind] += r;
					vmg[ind] += g;
					vmb[ind] += b;
					vma[ind] += a;
					m2[ind] += (r*r) + (g*g) + (b*b) + (a*a);
				}
			}
		}
		private void Get3DMoments(){
			long[] volume = new long[IndexCount*IndexAlphaCount];
			long[] volumeR = new long[IndexCount*IndexAlphaCount];
			long[] volumeG = new long[IndexCount*IndexAlphaCount];
			long[] volumeB = new long[IndexCount*IndexAlphaCount];
			long[] volumeA = new long[IndexCount*IndexAlphaCount];
			double[] volume2 = new double[IndexCount*IndexAlphaCount];
			long[] area = new long[IndexAlphaCount];
			long[] areaR = new long[IndexAlphaCount];
			long[] areaG = new long[IndexAlphaCount];
			long[] areaB = new long[IndexAlphaCount];
			long[] areaA = new long[IndexAlphaCount];
			double[] area2 = new double[IndexAlphaCount];
			for (int r = 1; r < IndexCount; r++){
				Array.Clear(volume, 0, IndexCount*IndexAlphaCount);
				Array.Clear(volumeR, 0, IndexCount*IndexAlphaCount);
				Array.Clear(volumeG, 0, IndexCount*IndexAlphaCount);
				Array.Clear(volumeB, 0, IndexCount*IndexAlphaCount);
				Array.Clear(volumeA, 0, IndexCount*IndexAlphaCount);
				Array.Clear(volume2, 0, IndexCount*IndexAlphaCount);
				for (int g = 1; g < IndexCount; g++){
					Array.Clear(area, 0, IndexAlphaCount);
					Array.Clear(areaR, 0, IndexAlphaCount);
					Array.Clear(areaG, 0, IndexAlphaCount);
					Array.Clear(areaB, 0, IndexAlphaCount);
					Array.Clear(areaA, 0, IndexAlphaCount);
					Array.Clear(area2, 0, IndexAlphaCount);
					for (int b = 1; b < IndexCount; b++){
						long line = 0;
						long lineR = 0;
						long lineG = 0;
						long lineB = 0;
						long lineA = 0;
						double line2 = 0;
						for (int a = 1; a < IndexAlphaCount; a++){
							int ind1 = GetPaletteIndex(r, g, b, a);
							line += vwt[ind1];
							lineR += vmr[ind1];
							lineG += vmg[ind1];
							lineB += vmb[ind1];
							lineA += vma[ind1];
							line2 += m2[ind1];
							area[a] += line;
							areaR[a] += lineR;
							areaG[a] += lineG;
							areaB[a] += lineB;
							areaA[a] += lineA;
							area2[a] += line2;
							int inv = (b*IndexAlphaCount) + a;
							volume[inv] += area[a];
							volumeR[inv] += areaR[a];
							volumeG[inv] += areaG[a];
							volumeB[inv] += areaB[a];
							volumeA[inv] += areaA[a];
							volume2[inv] += area2[a];
							int ind2 = ind1 - GetPaletteIndex(1, 0, 0, 0);
							vwt[ind1] = vwt[ind2] + volume[inv];
							vmr[ind1] = vmr[ind2] + volumeR[inv];
							vmg[ind1] = vmg[ind2] + volumeG[inv];
							vmb[ind1] = vmb[ind2] + volumeB[inv];
							vma[ind1] = vma[ind2] + volumeA[inv];
							m2[ind1] = m2[ind2] + volume2[inv];
						}
					}
				}
			}
		}
		private double Variance(Box cube){
			double dr = Volume(cube, vmr);
			double dg = Volume(cube, vmg);
			double db = Volume(cube, vmb);
			double da = Volume(cube, vma);
			double xx = m2[GetPaletteIndex(cube.R1, cube.G1, cube.B1, cube.A1)] -
						m2[GetPaletteIndex(cube.R1, cube.G1, cube.B1, cube.A0)] - m2[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A1)] +
						m2[GetPaletteIndex(cube.R1, cube.G1, cube.B0, cube.A0)] - m2[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A1)] +
						m2[GetPaletteIndex(cube.R1, cube.G0, cube.B1, cube.A0)] + m2[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A1)] -
						m2[GetPaletteIndex(cube.R1, cube.G0, cube.B0, cube.A0)] - m2[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A1)] +
						m2[GetPaletteIndex(cube.R0, cube.G1, cube.B1, cube.A0)] + m2[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A1)] -
						m2[GetPaletteIndex(cube.R0, cube.G1, cube.B0, cube.A0)] + m2[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A1)] -
						m2[GetPaletteIndex(cube.R0, cube.G0, cube.B1, cube.A0)] - m2[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A1)] +
						m2[GetPaletteIndex(cube.R0, cube.G0, cube.B0, cube.A0)];
			return xx - (((dr*dr) + (dg*dg) + (db*db) + (da*da))/Volume(cube, vwt));
		}
		private double Maximize(Box cube, int direction, int first, int last, out int cut, double wholeR, double wholeG,
			double wholeB, double wholeA, double wholeW){
			long baseR = Bottom(cube, direction, vmr);
			long baseG = Bottom(cube, direction, vmg);
			long baseB = Bottom(cube, direction, vmb);
			long baseA = Bottom(cube, direction, vma);
			long baseW = Bottom(cube, direction, vwt);
			double max = 0.0;
			cut = -1;
			for (int i = first; i < last; i++){
				double halfR = baseR + Top(cube, direction, i, vmr);
				double halfG = baseG + Top(cube, direction, i, vmg);
				double halfB = baseB + Top(cube, direction, i, vmb);
				double halfA = baseA + Top(cube, direction, i, vma);
				double halfW = baseW + Top(cube, direction, i, vwt);
				double temp;
				if (Math.Abs(halfW) < Epsilon){
					continue;
				}
				temp = ((halfR*halfR) + (halfG*halfG) + (halfB*halfB) + (halfA*halfA))/halfW;
				halfR = wholeR - halfR;
				halfG = wholeG - halfG;
				halfB = wholeB - halfB;
				halfA = wholeA - halfA;
				halfW = wholeW - halfW;
				if (Math.Abs(halfW) < Epsilon){
					continue;
				}
				temp += ((halfR*halfR) + (halfG*halfG) + (halfB*halfB) + (halfA*halfA))/halfW;
				if (temp > max){
					max = temp;
					cut = i;
				}
			}
			return max;
		}
		private bool Cut(Box set1, Box set2){
			double wholeR = Volume(set1, vmr);
			double wholeG = Volume(set1, vmg);
			double wholeB = Volume(set1, vmb);
			double wholeA = Volume(set1, vma);
			double wholeW = Volume(set1, vwt);
			int cutr;
			int cutg;
			int cutb;
			int cuta;
			double maxr = Maximize(set1, 0, set1.R0 + 1, set1.R1, out cutr, wholeR, wholeG, wholeB, wholeA, wholeW);
			double maxg = Maximize(set1, 1, set1.G0 + 1, set1.G1, out cutg, wholeR, wholeG, wholeB, wholeA, wholeW);
			double maxb = Maximize(set1, 2, set1.B0 + 1, set1.B1, out cutb, wholeR, wholeG, wholeB, wholeA, wholeW);
			double maxa = Maximize(set1, 3, set1.A0 + 1, set1.A1, out cuta, wholeR, wholeG, wholeB, wholeA, wholeW);
			int dir;
			if ((maxr >= maxg) && (maxr >= maxb) && (maxr >= maxa)){
				dir = 0;
				if (cutr < 0){
					return false;
				}
			} else if ((maxg >= maxr) && (maxg >= maxb) && (maxg >= maxa)){
				dir = 1;
			} else if ((maxb >= maxr) && (maxb >= maxg) && (maxb >= maxa)){
				dir = 2;
			} else{
				dir = 3;
			}
			set2.R1 = set1.R1;
			set2.G1 = set1.G1;
			set2.B1 = set1.B1;
			set2.A1 = set1.A1;
			switch (dir){
				case 0:
					set2.R0 = set1.R1 = cutr;
					set2.G0 = set1.G0;
					set2.B0 = set1.B0;
					set2.A0 = set1.A0;
					break;
				case 1:
					set2.G0 = set1.G1 = cutg;
					set2.R0 = set1.R0;
					set2.B0 = set1.B0;
					set2.A0 = set1.A0;
					break;
				case 2:
					set2.B0 = set1.B1 = cutb;
					set2.R0 = set1.R0;
					set2.G0 = set1.G0;
					set2.A0 = set1.A0;
					break;
				case 3:
					set2.A0 = set1.A1 = cuta;
					set2.R0 = set1.R0;
					set2.G0 = set1.G0;
					set2.B0 = set1.B0;
					break;
			}
			set1.Volume = (set1.R1 - set1.R0)*(set1.G1 - set1.G0)*(set1.B1 - set1.B0)*(set1.A1 - set1.A0);
			set2.Volume = (set2.R1 - set2.R0)*(set2.G1 - set2.G0)*(set2.B1 - set2.B0)*(set2.A1 - set2.A0);
			return true;
		}
		private void Mark(Box cube, byte label){
			for (int r = cube.R0 + 1; r <= cube.R1; r++){
				for (int g = cube.G0 + 1; g <= cube.G1; g++){
					for (int b = cube.B0 + 1; b <= cube.B1; b++){
						for (int a = cube.A0 + 1; a <= cube.A1; a++){
							tag[GetPaletteIndex(r, g, b, a)] = label;
						}
					}
				}
			}
		}
		private void BuildCube(out Box[] cube, ref int colorCount){
			cube = new Box[colorCount];
			double[] vv = new double[colorCount];
			for (int i = 0; i < colorCount; i++){
				cube[i] = new Box();
			}
			cube[0].R0 = cube[0].G0 = cube[0].B0 = cube[0].A0 = 0;
			cube[0].R1 = cube[0].G1 = cube[0].B1 = IndexCount - 1;
			cube[0].A1 = IndexAlphaCount - 1;
			int next = 0;
			for (int i = 1; i < colorCount; i++){
				if (Cut(cube[next], cube[i])){
					vv[next] = cube[next].Volume > 1 ? Variance(cube[next]) : 0.0;
					vv[i] = cube[i].Volume > 1 ? Variance(cube[i]) : 0.0;
				} else{
					vv[next] = 0.0;
					i--;
				}
				next = 0;
				double temp = vv[0];
				for (int k = 1; k <= i; k++){
					if (vv[k] > temp){
						temp = vv[k];
						next = k;
					}
				}
				if (temp <= 0.0){
					colorCount = i + 1;
					break;
				}
			}
		}
		private QuantizedImage GenerateResult(IPixelAccessor imagePixels, int colorCount, Box[] cube){
			List<Color2> pallette = new List<Color2>();
			byte[] pixels = new byte[imagePixels.Width*imagePixels.Height];
			int transparentIndex = -1;
			int width = imagePixels.Width;
			int height = imagePixels.Height;
			for (int k = 0; k < colorCount; k++){
				Mark(cube[k], (byte) k);
				double weight = Volume(cube[k], vwt);
				if (Math.Abs(weight) > Epsilon){
					byte r = (byte) (Volume(cube[k], vmr)/weight);
					byte g = (byte) (Volume(cube[k], vmg)/weight);
					byte b = (byte) (Volume(cube[k], vmb)/weight);
					byte a = (byte) (Volume(cube[k], vma)/weight);
					Color2 color = Color2.FromArgb(a,r, g, b);
					if (color.Equals(default(Color2))){
						transparentIndex = k;
					}
					pallette.Add(color);
				} else{
					pallette.Add(default(Color2));
					transparentIndex = k;
				}
			}
			Parallel.For(0, height, Bootstrapper.instance.ParallelOptions, y =>{
				for (int x = 0; x < width; x++){
					// Expected order r->g->b->a
					byte[] color = imagePixels[x, y].ToBytes();
					int r = color[0] >> (8 - IndexBits);
					int g = color[1] >> (8 - IndexBits);
					int b = color[2] >> (8 - IndexBits);
					int a = color[3] >> (8 - IndexAlphaBits);
					if (transparentIndex > -1 && color[3] <= Threshold){
						pixels[(y*width) + x] = (byte) transparentIndex;
						continue;
					}
					int ind = GetPaletteIndex(r + 1, g + 1, b + 1, a + 1);
					pixels[(y*width) + x] = tag[ind];
				}
			});
			return new QuantizedImage(width, height, pallette.ToArray(), pixels, transparentIndex);
		}
	}
}