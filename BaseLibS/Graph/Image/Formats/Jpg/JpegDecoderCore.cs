using System;
using System.IO;
using System.Threading.Tasks;

namespace BaseLibS.Graph.Image.Formats.Jpg{
	internal class JpegDecoderCore{
		private const int maxCodeLength = 16;
		private const int maxNCodes = 256;
		private const int lutSize = 8;
		private const int maxComponents = 4;
		private const int maxTc = 1;
		private const int maxTh = 3;
		private const int maxTq = 3;
		private const int dcTable = 0;
		private const int acTable = 1;
		private const int adobeTransformUnknown = 0;
		private const int adobeTransformYCbCr = 1;
		private const int adobeTransformYCbCrK = 2;

		private static readonly int[] unzig ={
			0, 1, 8, 16, 9, 2, 3, 10, 17, 24, 32, 25, 18, 11, 4, 5, 12, 19, 26, 33, 40, 48,
			41, 34, 27, 20, 13, 6, 7, 14, 21, 28, 35, 42, 49, 56, 57, 50, 43, 36, 29, 22, 15, 23, 30, 37, 44, 51, 58, 59, 52, 45,
			38, 31, 39, 46, 53, 60, 61, 54, 47, 55, 62, 63,
		};

		public int width;
		public int height;
		public int nComp;
		public GrayImage grayImage; // grayscale
		private YCbCrImage ycbcrImage; // YCrCb
		private Stream inputStream;
		private BitsClass bits;
		private readonly BytesClass bytes;
		private byte[] blackPix;
		private int blackStride;
		private int ri; // Restart Interval.
		private bool progressive;
		private bool jfif;
		private bool adobeTransformValid;
		private byte adobeTransform;
		private ushort eobRun; // End-of-Band run, specified in section G.1.2.2.
		private readonly Component[] comp;
		private readonly Block[][] progCoeffs; // Saved state between progressive-mode scans.
		private readonly Huffman[,] huff;
		private readonly Block[] quant; // Quantization tables, in zig-zag order.
		private readonly byte[] tmp;
		private short horizontalResolution;
		private short verticalResolution;

		public JpegDecoderCore(){
			huff = new Huffman[maxTc + 1, maxTh + 1];
			quant = new Block[maxTq + 1];
			tmp = new byte[2*Block.blockSize];
			comp = new Component[maxComponents];
			progCoeffs = new Block[maxComponents][];
			bits = new BitsClass();
			bytes = new BytesClass();
			for (int i = 0; i < maxTc + 1; i++){
				for (int j = 0; j < maxTh + 1; j++){
					huff[i, j] = new Huffman();
				}
			}
			for (int i = 0; i < quant.Length; i++){
				quant[i] = new Block();
			}
			for (int i = 0; i < comp.Length; i++){
				comp[i] = new Component();
			}
		}

		private void EnsureNBits(int n){
			while (true){
				byte c = ReadByteStuffedByte();
				bits.a = (bits.a << 8) | c;
				bits.n += 8;
				if (bits.m == 0){
					bits.m = 1 << 7;
				} else{
					bits.m <<= 8;
				}
				if (bits.n >= n){
					break;
				}
			}
		}

		private int ReceiveExtend(byte t){
			if (bits.n < t){
				EnsureNBits(t);
			}
			bits.n -= t;
			bits.m >>= t;
			int s = 1 << t;
			int x = (int) ((bits.a >> bits.n) & (s - 1));
			if (x < (s >> 1)){
				x += ((-1) << t) + 1;
			}
			return x;
		}

		private void ProcessDht(int n){
			while (n > 0){
				if (n < 17){
					throw new Exception("DHT has wrong length");
				}
				ReadFull(tmp, 0, 17);
				int tc = tmp[0] >> 4;
				if (tc > maxTc){
					throw new Exception("bad Tc value");
				}
				int th = tmp[0] & 0x0f;
				if (th > maxTh || !progressive && th > 1){
					throw new Exception("bad Th value");
				}
				Huffman h = huff[tc, th];
				h.nCodes = 0;
				int[] ncodes = new int[maxCodeLength];
				for (int i = 0; i < ncodes.Length; i++){
					ncodes[i] = tmp[i + 1];
					h.nCodes += ncodes[i];
				}
				if (h.nCodes == 0){
					throw new Exception("Huffman table has zero length");
				}
				if (h.nCodes > maxNCodes){
					throw new Exception("Huffman table has excessive length");
				}
				n -= h.nCodes + 17;
				if (n < 0){
					throw new Exception("DHT has wrong length");
				}
				ReadFull(h.vals, 0, h.nCodes);
				for (int i = 0; i < h.lut.Length; i++){
					h.lut[i] = 0;
				}
				uint x = 0, code = 0;
				for (int i = 0; i < lutSize; i++){
					code <<= 1;
					for (int j = 0; j < ncodes[i]; j++){
						byte base2 = (byte) (code << (7 - i));
						ushort lutValue = (ushort) ((h.vals[x] << 8) | (2 + i));
						for (int k = 0; k < 1 << (7 - i); k++){
							h.lut[base2 | k] = lutValue;
						}
						code++;
						x++;
					}
				}
				int c = 0, index = 0;
				for (int i = 0; i < ncodes.Length; i++){
					int nc = ncodes[i];
					if (nc == 0){
						h.minCodes[i] = -1;
						h.maxCodes[i] = -1;
						h.valsIndices[i] = -1;
					} else{
						h.minCodes[i] = c;
						h.maxCodes[i] = c + nc - 1;
						h.valsIndices[i] = index;
						c += nc;
						index += nc;
					}
					c <<= 1;
				}
			}
		}

		private byte DecodeHuffman(Huffman h){
			if (h.nCodes == 0){
				throw new Exception("uninitialized Huffman table");
			}
			if (bits.n < 8){
				try{
					EnsureNBits(8);
				} catch (MissingFF00Exception){
					if (bytes.nUnreadable != 0){
						UnreadByteStuffedByte();
					}
					goto slowPath;
				} catch (ShortHuffmanDataException){
					if (bytes.nUnreadable != 0){
						UnreadByteStuffedByte();
					}
					goto slowPath;
				}
			}
			ushort v = h.lut[(bits.a >> (bits.n - lutSize)) & 0xff];
			if (v != 0){
				byte n = (byte) ((v & 0xff) - 1);
				bits.n -= n;
				bits.m >>= n;
				return (byte) (v >> 8);
			}
			slowPath:
			int code = 0;
			for (int i = 0; i < maxCodeLength; i++){
				if (bits.n == 0){
					EnsureNBits(1);
				}
				if ((bits.a & bits.m) != 0){
					code |= 1;
				}
				bits.n--;
				bits.m >>= 1;
				if (code <= h.maxCodes[i]){
					return h.vals[h.valsIndices[i] + code - h.minCodes[i]];
				}
				code <<= 1;
			}
			throw new Exception("bad Huffman code");
		}

		private bool DecodeBit(){
			if (bits.n == 0){
				EnsureNBits(1);
			}
			bool ret = (bits.a & bits.m) != 0;
			bits.n--;
			bits.m >>= 1;
			return ret;
		}

		private uint DecodeBits(int n){
			if (bits.n < n){
				EnsureNBits(n);
			}
			uint ret = bits.a >> (bits.n - n);
			ret = (uint) (ret & ((1 << n) - 1));
			bits.n -= n;
			bits.m >>= n;
			return ret;
		}

		private void Fill(){
			if (bytes.i != bytes.j){
				throw new Exception("jpeg: Fill called when unread bytes exist");
			}
			if (bytes.j > 2){
				bytes.buf[0] = bytes.buf[bytes.j - 2];
				bytes.buf[1] = bytes.buf[bytes.j - 1];
				bytes.i = 2;
				bytes.j = 2;
			}
			int n = inputStream.Read(bytes.buf, bytes.j, bytes.buf.Length - bytes.j);
			if (n == 0){
				throw new EOFException();
			}
			bytes.j += n;
		}

		private void UnreadByteStuffedByte(){
			bytes.i -= bytes.nUnreadable;
			bytes.nUnreadable = 0;
			if (bits.n >= 8){
				bits.a >>= 8;
				bits.n -= 8;
				bits.m >>= 8;
			}
		}

		private byte ReadByte(){
			while (bytes.i == bytes.j){
				Fill();
			}
			byte x = bytes.buf[bytes.i];
			bytes.i++;
			bytes.nUnreadable = 0;
			return x;
		}

		private byte ReadByteStuffedByte(){
			byte x;
			if (bytes.i + 2 <= bytes.j){
				x = bytes.buf[bytes.i];
				bytes.i++;
				bytes.nUnreadable = 1;
				if (x != JpegConstants.Markers.XFF){
					return x;
				}
				if (bytes.buf[bytes.i] != 0x00){
					throw new MissingFF00Exception();
				}
				bytes.i++;
				bytes.nUnreadable = 2;
				return 0xff;
			}
			bytes.nUnreadable = 0;
			x = ReadByte();
			bytes.nUnreadable = 1;
			if (x != 0xff){
				return x;
			}
			x = ReadByte();
			bytes.nUnreadable = 2;
			if (x != 0x00){
				throw new MissingFF00Exception();
			}
			return 0xff;
		}

		private void ReadFull(byte[] data, int offset, int len){
			if (bytes.nUnreadable != 0){
				if (bits.n >= 8){
					UnreadByteStuffedByte();
				}
				bytes.nUnreadable = 0;
			}
			while (len > 0){
				if (bytes.j - bytes.i >= len){
					Array.Copy(bytes.buf, bytes.i, data, offset, len);
					bytes.i += len;
					len -= len;
				} else{
					Array.Copy(bytes.buf, bytes.i, data, offset, bytes.j - bytes.i);
					offset += bytes.j - bytes.i;
					len -= bytes.j - bytes.i;
					bytes.i += bytes.j - bytes.i;
					Fill();
				}
			}
		}

		private void Ignore(int n){
			if (bytes.nUnreadable != 0){
				if (bits.n >= 8){
					UnreadByteStuffedByte();
				}
				bytes.nUnreadable = 0;
			}
			while (true){
				int m = bytes.j - bytes.i;
				if (m > n){
					m = n;
				}
				bytes.i += m;
				n -= m;
				if (n == 0){
					break;
				} else{
					Fill();
				}
			}
		}

		private void ProcessSof(int n){
			if (nComp != 0){
				throw new Exception("multiple SOF markers");
			}
			switch (n){
				case 6 + (3*1): // Grayscale image.
					nComp = 1;
					break;
				case 6 + (3*3): // YCbCr or Rgb image.
					nComp = 3;
					break;
				case 6 + (3*4): // YCbCrK or CMYK image.
					nComp = 4;
					break;
				default:
					throw new Exception("Incorrect number of components");
			}
			ReadFull(tmp, 0, n);
			if (tmp[0] != 8){
				throw new Exception("Only 8-Bit precision supported.");
			}
			height = (tmp[1] << 8) + tmp[2];
			width = (tmp[3] << 8) + tmp[4];
			if (tmp[5] != nComp){
				throw new Exception("SOF has wrong length");
			}
			for (int i = 0; i < nComp; i++){
				comp[i].c = tmp[6 + (3*i)];
				for (int j = 0; j < i; j++){
					if (comp[i].c == comp[j].c){
						throw new Exception("Repeated component identifier");
					}
				}
				comp[i].tq = tmp[8 + (3*i)];
				if (comp[i].tq > maxTq){
					throw new Exception("Bad Tq value");
				}
				byte hv = tmp[7 + (3*i)];
				int h = hv >> 4;
				int v = hv & 0x0f;
				if (h < 1 || 4 < h || v < 1 || 4 < v){
					throw new Exception("Unsupported Luma/chroma subsampling ratio");
				}
				if (h == 3 || v == 3){
					throw new Exception("Lnsupported subsampling ratio");
				}
				switch (nComp){
					case 1:
						h = 1;
						v = 1;
						break;
					case 3:
						switch (i){
							case 0:{
								if (v == 4){
									throw new Exception("unsupported subsampling ratio");
								}
								break;
							}
							case 1:{
								if (comp[0].h%h != 0 || comp[0].v%v != 0){
									throw new Exception("unsupported subsampling ratio");
								}
								break;
							}
							case 2:{
								if (comp[1].h != h || comp[1].v != v){
									throw new Exception("unsupported subsampling ratio");
								}
								break;
							}
						}
						break;
					case 4:
						switch (i){
							case 0:
								if (hv != 0x11 && hv != 0x22){
									throw new Exception("unsupported subsampling ratio");
								}
								break;
							case 1:
							case 2:
								if (hv != 0x11){
									throw new Exception("unsupported subsampling ratio");
								}
								break;
							case 3:
								if (comp[0].h != h || comp[0].v != v){
									throw new Exception("unsupported subsampling ratio");
								}
								break;
						}
						break;
				}
				comp[i].h = h;
				comp[i].v = v;
			}
		}

		private void ProcessDqt(int n){
			while (n > 0){
				bool done = false;
				n--;
				byte x = ReadByte();
				byte tq = (byte) (x & 0x0f);
				if (tq > maxTq){
					throw new Exception("bad Tq value");
				}
				switch (x >> 4){
					case 0:
						if (n < Block.blockSize){
							done = true;
							break;
						}
						n -= Block.blockSize;
						ReadFull(tmp, 0, Block.blockSize);
						for (int i = 0; i < Block.blockSize; i++){
							quant[tq][i] = tmp[i];
						}
						break;
					case 1:
						if (n < 2*Block.blockSize){
							done = true;
							break;
						}
						n -= 2*Block.blockSize;
						ReadFull(tmp, 0, 2*Block.blockSize);
						for (int i = 0; i < Block.blockSize; i++){
							quant[tq][i] = (tmp[2*i] << 8) | tmp[2*i + 1];
						}
						break;
					default:
						throw new Exception("bad Pq value");
				}
				if (done){
					break;
				}
			}
			if (n != 0){
				throw new Exception("DQT has wrong length");
			}
		}

		private void ProcessDri(int n){
			if (n != 2){
				throw new Exception("DRI has wrong length");
			}
			ReadFull(tmp, 0, 2);
			ri = (tmp[0] << 8) + tmp[1];
		}

		private void ProcessApp0Marker(int n){
			if (n < 5){
				Ignore(n);
				return;
			}
			ReadFull(tmp, 0, 13);
			n -= 13;
			jfif = tmp[0] == 'J' && tmp[1] == 'F' && tmp[2] == 'I' && tmp[3] == 'F' && tmp[4] == '\x00';
			if (jfif){
				horizontalResolution = (short) (tmp[9] + (tmp[10] << 8));
				verticalResolution = (short) (tmp[11] + (tmp[12] << 8));
			}
			if (n > 0){
				Ignore(n);
			}
		}

		private void ProcessApp14Marker(int n){
			if (n < 12){
				Ignore(n);
				return;
			}
			ReadFull(tmp, 0, 12);
			n -= 12;
			if (tmp[0] == 'A' && tmp[1] == 'd' && tmp[2] == 'o' && tmp[3] == 'b' && tmp[4] == 'e'){
				adobeTransformValid = true;
				adobeTransform = tmp[11];
			}
			if (n > 0){
				Ignore(n);
			}
		}

		public void Decode(Image2 image, Stream stream, bool configOnly){
			inputStream = stream;
			ReadFull(tmp, 0, 2);
			if (tmp[0] != JpegConstants.Markers.XFF || tmp[1] != JpegConstants.Markers.SOI){
				throw new Exception("Missing SOI marker.");
			}
			while (true){
				ReadFull(tmp, 0, 2);
				while (tmp[0] != 0xff){
					tmp[0] = tmp[1];
					tmp[1] = ReadByte();
				}
				byte marker = tmp[1];
				if (marker == 0){
					continue;
				}
				while (marker == 0xff){
					marker = ReadByte();
				}
				if (marker == JpegConstants.Markers.EOI){
					break;
				}
				if (JpegConstants.Markers.RST0 <= marker && marker <= JpegConstants.Markers.RST7){
					continue;
				}
				ReadFull(tmp, 0, 2);
				int n = (tmp[0] << 8) + tmp[1] - 2;
				if (n < 0){
					throw new Exception("Short segment length.");
				}
				switch (marker){
					case JpegConstants.Markers.SOF0:
					case JpegConstants.Markers.SOF1:
					case JpegConstants.Markers.SOF2:
						progressive = marker == JpegConstants.Markers.SOF2;
						ProcessSof(n);
						if (configOnly && jfif){
							return;
						}
						break;
					case JpegConstants.Markers.DHT:
						if (configOnly){
							Ignore(n);
						} else{
							ProcessDht(n);
						}
						break;
					case JpegConstants.Markers.DQT:
						if (configOnly){
							Ignore(n);
						} else{
							ProcessDqt(n);
						}
						break;
					case JpegConstants.Markers.SOS:
						if (configOnly){
							return;
						}
						ProcessSos(n);
						break;
					case JpegConstants.Markers.DRI:
						if (configOnly){
							Ignore(n);
						} else{
							ProcessDri(n);
						}
						break;
					case JpegConstants.Markers.APP0:
						ProcessApp0Marker(n);
						break;
					case JpegConstants.Markers.APP14:
						ProcessApp14Marker(n);
						break;
					default:
						if (JpegConstants.Markers.APP0 <= marker && marker <= JpegConstants.Markers.APP15 ||
							marker == JpegConstants.Markers.COM){
							Ignore(n);
						} else if (marker < JpegConstants.Markers.SOF0){
							throw new Exception("Unknown marker");
						} else{
							throw new Exception("Unknown marker");
						}
						break;
				}
			}
			if (grayImage != null){
				ConvertFromGrayScale(width, height, image);
			} else if (ycbcrImage != null){
				if (IsRgb()){
					ConvertFromRgb(width, height, image);
				} else{
					ConvertFromYCbCr(width, height, image);
				}
			} else{
				throw new Exception("Missing SOS marker.");
			}
		}

		private void ConvertFromGrayScale(int imageWidth, int imageHeight, ImageBase image){
			Color2[] pixels = new Color2[imageWidth*imageHeight];
			Parallel.For(0, imageHeight, Bootstrapper.instance.ParallelOptions, y =>{
				int yoff = grayImage.GetRowOffset(y);
				for (int x = 0; x < imageWidth; x++){
					int offset = (y*imageWidth) + x;
					byte rgb = grayImage.pixels[yoff + x];
					Color2 packed = Color2.FromArgb(rgb, rgb, rgb);
					pixels[offset] = packed;
				}
			});
			image.SetPixels(imageWidth, imageHeight, pixels);
			AssignResolution(image);
		}

		private void ConvertFromYCbCr(int imageWidth, int imageHeight, ImageBase image){
			int scale = comp[0].h/comp[1].h;
			Color2[] pixels = new Color2[imageWidth*imageHeight];
			Parallel.For(0, imageHeight, Bootstrapper.instance.ParallelOptions, y =>{
				int yo = ycbcrImage.get_row_y_offset(y);
				int co = ycbcrImage.get_row_c_offset(y);
				for (int x = 0; x < imageWidth; x++){
					byte yy = ycbcrImage.pix_y[yo + x];
					byte cb = ycbcrImage.pix_cb[co + (x/scale)];
					byte cr = ycbcrImage.pix_cr[co + (x/scale)];
					int index = y*imageWidth + x;
					Color2 color = new YCbCr2(yy, cb, cr);
					Color2 packed = Color2.FromArgb(color.A, color.R, color.G, color.B);
					pixels[index] = packed;
				}
			});
			image.SetPixels(imageWidth, imageHeight, pixels);
			AssignResolution(image);
		}

		private void ConvertFromRgb(int imageWidth, int imageHeight, ImageBase image){
			int scale = comp[0].h/comp[1].h;
			Color2[] pixels = new Color2[imageWidth*imageHeight];
			Parallel.For(0, imageHeight, Bootstrapper.instance.ParallelOptions, y =>{
				int yo = ycbcrImage.get_row_y_offset(y);
				int co = ycbcrImage.get_row_c_offset(y);
				for (int x = 0; x < imageWidth; x++){
					byte red = ycbcrImage.pix_y[yo + x];
					byte green = ycbcrImage.pix_cb[co + (x/scale)];
					byte blue = ycbcrImage.pix_cr[co + (x/scale)];
					int index = (y*imageWidth) + x;
					Color2 packed = Color2.FromArgb(red, green, blue);
					pixels[index] = packed;
				}
			});
			image.SetPixels(imageWidth, imageHeight, pixels);
			AssignResolution(image);
		}

		private void AssignResolution(ImageBase image){
			if (jfif && horizontalResolution > 0 && verticalResolution > 0){
				((Image2) image).HorizontalResolution = horizontalResolution;
				((Image2) image).VerticalResolution = verticalResolution;
			}
		}

		private void ProcessSos(int n){
			if (nComp == 0){
				throw new Exception("missing SOF marker");
			}
			if (n < 6 || 4 + (2*nComp) < n || n%2 != 0){
				throw new Exception("SOS has wrong length");
			}
			ReadFull(tmp, 0, n);
			byte lnComp = tmp[0];
			if (n != 4 + (2*lnComp)){
				throw new Exception("SOS length inconsistent with number of components");
			}
			Scan[] scan = new Scan[maxComponents];
			int totalHv = 0;
			for (int i = 0; i < lnComp; i++){
				int cs = tmp[1 + (2*i)];
				int compIndex = -1;
				for (int j = 0; j < nComp; j++){
					Component compv = comp[j];
					if (cs == compv.c){
						compIndex = j;
					}
				}
				if (compIndex < 0){
					throw new Exception("Unknown component selector");
				}
				scan[i].compIndex = (byte) compIndex;
				for (int j = 0; j < i; j++){
					if (scan[i].compIndex == scan[j].compIndex){
						throw new Exception("Repeated component selector");
					}
				}
				totalHv += comp[compIndex].h*comp[compIndex].v;
				scan[i].td = (byte) (tmp[2 + (2*i)] >> 4);
				if (scan[i].td > maxTh){
					throw new Exception("bad Td value");
				}
				scan[i].ta = (byte) (tmp[2 + (2*i)] & 0x0f);
				if (scan[i].ta > maxTh){
					throw new Exception("bad Ta value");
				}
			}
			if (nComp > 1 && totalHv > 10){
				throw new Exception("Total sampling factors too large.");
			}
			int zigStart = 0;
			int zigEnd = Block.blockSize - 1;
			int ah = 0;
			int al = 0;
			if (progressive){
				zigStart = tmp[1 + (2*lnComp)];
				zigEnd = tmp[2 + (2*lnComp)];
				ah = tmp[3 + (2*lnComp)] >> 4;
				al = tmp[3 + (2*lnComp)] & 0x0f;
				if ((zigStart == 0 && zigEnd != 0) || zigStart > zigEnd || Block.blockSize <= zigEnd){
					throw new Exception("Bad spectral selection bounds");
				}
				if (zigStart != 0 && lnComp != 1){
					throw new Exception("Progressive AC coefficients for more than one component");
				}
				if (ah != 0 && ah != al + 1){
					throw new Exception("Bad successive approximation values");
				}
			}
			int h0 = comp[0].h;
			int v0 = comp[0].v;
			int mxx = (width + (8*h0) - 1)/(8*h0);
			int myy = (height + (8*v0) - 1)/(8*v0);
			if (grayImage == null && ycbcrImage == null){
				MakeImg(mxx, myy);
			}
			if (progressive){
				for (int i = 0; i < lnComp; i++){
					int compIndex = scan[i].compIndex;
					if (progCoeffs[compIndex] == null){
						progCoeffs[compIndex] = new Block[mxx*myy*comp[compIndex].h*comp[compIndex].v];
						for (int j = 0; j < progCoeffs[compIndex].Length; j++){
							progCoeffs[compIndex][j] = new Block();
						}
					}
				}
			}
			bits = new BitsClass();
			int mcu = 0;
			byte expectedRst = JpegConstants.Markers.RST0;
			Block b = new Block();
			int[] dc = new int[maxComponents];
			int blockCount = 0;
			for (int my = 0; my < myy; my++){
				for (int mx = 0; mx < mxx; mx++){
					for (int i = 0; i < lnComp; i++){
						int compIndex = scan[i].compIndex;
						int hi = comp[compIndex].h;
						int vi = comp[compIndex].v;
						Block qt = quant[comp[compIndex].tq];
						for (int j = 0; j < hi*vi; j++){
							int bx;
							int by;
							if (lnComp != 1){
								bx = hi*mx + j%hi;
								by = vi*my + j/hi;
							} else{
								int q = mxx*hi;
								bx = blockCount%q;
								by = blockCount/q;
								blockCount++;
								if (bx*8 >= width || by*8 >= height){
									continue;
								}
							}
							b = progressive ? progCoeffs[compIndex][@by*mxx*hi + bx] : new Block();
							if (ah != 0){
								Refine(b, huff[acTable, scan[i].ta], zigStart, zigEnd, 1 << al);
							} else{
								int zig = zigStart;
								if (zig == 0){
									zig++;
									byte value = DecodeHuffman(huff[dcTable, scan[i].td]);
									if (value > 16){
										throw new Exception("Excessive DC component");
									}
									int dcDelta = ReceiveExtend(value);
									dc[compIndex] += dcDelta;
									b[0] = dc[compIndex] << al;
								}
								if (zig <= zigEnd && eobRun > 0){
									eobRun--;
								} else{
									Huffman huffv = huff[acTable, scan[i].ta];
									for (; zig <= zigEnd; zig++){
										byte value = DecodeHuffman(huffv);
										byte val0 = (byte) (value >> 4);
										byte val1 = (byte) (value & 0x0f);
										if (val1 != 0){
											zig += val0;
											if (zig > zigEnd){
												break;
											}
											int ac = ReceiveExtend(val1);
											b[unzig[zig]] = ac << al;
										} else{
											if (val0 != 0x0f){
												eobRun = (ushort) (1 << val0);
												if (val0 != 0){
													eobRun |= (ushort) DecodeBits(val0);
												}
												eobRun--;
												break;
											}
											zig += 0x0f;
										}
									}
								}
							}
							if (progressive){
								if (zigEnd != Block.blockSize - 1 || al != 0){
									progCoeffs[compIndex][by*mxx*hi + bx] = b;
									continue;
								}
							}
							for (int zig = 0; zig < Block.blockSize; zig++){
								b[unzig[zig]] *= qt[zig];
							}
							IDCT.Transform(b);
							byte[] dst;
							int offset;
							int stride;
							if (nComp == 1){
								dst = grayImage.pixels;
								stride = grayImage.stride;
								offset = grayImage.offset + 8*(by*grayImage.stride + bx);
							} else{
								switch (compIndex){
									case 0:
										dst = ycbcrImage.pix_y;
										stride = ycbcrImage.y_stride;
										offset = ycbcrImage.y_offset + 8*(by*ycbcrImage.y_stride + bx);
										break;
									case 1:
										dst = ycbcrImage.pix_cb;
										stride = ycbcrImage.c_stride;
										offset = ycbcrImage.c_offset + 8*(by*ycbcrImage.c_stride + bx);
										break;
									case 2:
										dst = ycbcrImage.pix_cr;
										stride = ycbcrImage.c_stride;
										offset = ycbcrImage.c_offset + 8*(by*ycbcrImage.c_stride + bx);
										break;
									case 3:
										throw new Exception("Too many components");
									default:
										throw new Exception("Too many components");
								}
							}
							for (int y = 0; y < 8; y++){
								int y8 = y*8;
								int yStride = y*stride;
								for (int x = 0; x < 8; x++){
									int c = b[y8 + x];
									if (c < -128){
										c = 0;
									} else if (c > 127){
										c = 255;
									} else{
										c += 128;
									}
									dst[yStride + x + offset] = (byte) c;
								}
							}
						} // for j
					} // for i
					mcu++;
					if (ri > 0 && mcu%ri == 0 && mcu < mxx*myy){
						ReadFull(tmp, 0, 2);
						if (tmp[0] != 0xff || tmp[1] != expectedRst){
							throw new Exception("Bad RST marker");
						}
						expectedRst++;
						if (expectedRst == JpegConstants.Markers.RST7 + 1){
							expectedRst = JpegConstants.Markers.RST0;
						}
						bits = new BitsClass();
						dc = new int[maxComponents];
						eobRun = 0;
					}
				} // for mx
			} // for my
		}

		private void Refine(Block b, Huffman h, int zigStart, int zigEnd, int delta){
			if (zigStart == 0){
				if (zigEnd != 0){
					throw new Exception("Invalid state for zig DC component");
				}
				bool bit = DecodeBit();
				if (bit){
					b[0] |= delta;
				}
				return;
			}
			int zig = zigStart;
			if (eobRun == 0){
				for (; zig <= zigEnd; zig++){
					bool done = false;
					int z = 0;
					byte val = DecodeHuffman(h);
					int val0 = val >> 4;
					int val1 = val & 0x0f;
					switch (val1){
						case 0:
							if (val0 != 0x0f){
								eobRun = (ushort) (1 << val0);
								if (val0 != 0){
									uint bits = DecodeBits(val0);
									eobRun |= (ushort) bits;
								}
								done = true;
							}
							break;
						case 1:
							z = delta;
							bool bit = DecodeBit();
							if (!bit){
								z = -z;
							}
							break;
						default:
							throw new Exception("unexpected Huffman code");
					}
					if (done){
						break;
					}
					zig = RefineNonZeroes(b, zig, zigEnd, val0, delta);
					if (zig > zigEnd){
						throw new Exception($"too many coefficients {zig} > {zigEnd}");
					}
					if (z != 0){
						b[unzig[zig]] = z;
					}
				}
			}
			if (eobRun > 0){
				eobRun--;
				RefineNonZeroes(b, zig, zigEnd, -1, delta);
			}
		}

		public int RefineNonZeroes(Block b, int zig, int zigEnd, int nz, int delta){
			for (; zig <= zigEnd; zig++){
				int u = unzig[zig];
				if (b[u] == 0){
					if (nz == 0){
						break;
					}
					nz--;
					continue;
				}
				bool bit = DecodeBit();
				if (!bit){
					continue;
				}
				if (b[u] >= 0){
					b[u] += delta;
				} else{
					b[u] -= delta;
				}
			}
			return zig;
		}

		private void MakeImg(int mxx, int myy){
			if (nComp == 1){
				GrayImage m = new GrayImage(8*mxx, 8*myy);
				grayImage = m.subimage(0, 0, width, height);
			} else{
				int h0 = comp[0].h;
				int v0 = comp[0].v;
				int hRatio = h0/comp[1].h;
				int vRatio = v0/comp[1].v;
				YCbCrImage.YCbCrSubsampleRatio ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio444;
				switch ((hRatio << 4) | vRatio){
					case 0x11:
						ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio444;
						break;
					case 0x12:
						ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio440;
						break;
					case 0x21:
						ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio422;
						break;
					case 0x22:
						ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio420;
						break;
					case 0x41:
						ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio411;
						break;
					case 0x42:
						ratio = YCbCrImage.YCbCrSubsampleRatio.YCbCrSubsampleRatio410;
						break;
				}
				YCbCrImage m = new YCbCrImage(8*h0*mxx, 8*v0*myy, ratio);
				ycbcrImage = m.Subimage(0, 0, width, height);
			}
		}

		private bool IsRgb(){
			if (jfif){
				return false;
			}
			if (adobeTransformValid && adobeTransform == adobeTransformUnknown){
				return true;
			}
			return comp[0].c == 'R' && comp[1].c == 'G' && comp[2].c == 'B';
		}

		private class Component{
			public int h; // Horizontal sampling factor.
			public int v; // Vertical sampling factor.
			public byte c; // Component identifier.
			public byte tq; // Quantization table destination selector.
		}

		private class YCbCrImage{
			public enum YCbCrSubsampleRatio{
				YCbCrSubsampleRatio444,
				YCbCrSubsampleRatio422,
				YCbCrSubsampleRatio420,
				YCbCrSubsampleRatio440,
				YCbCrSubsampleRatio411,
				YCbCrSubsampleRatio410,
			}

			private static void yCbCrSize(int w, int h, YCbCrSubsampleRatio ratio, out int cw, out int ch){
				switch (ratio){
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio422:
						cw = (w + 1)/2;
						ch = h;
						break;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio420:
						cw = (w + 1)/2;
						ch = (h + 1)/2;
						break;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio440:
						cw = w;
						ch = (h + 1)/2;
						break;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio411:
						cw = (w + 3)/4;
						ch = h;
						break;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio410:
						cw = (w + 3)/4;
						ch = (h + 1)/2;
						break;
					default:

						// Default to 4:4:4 subsampling.
						cw = w;
						ch = h;
						break;
				}
			}

			private YCbCrImage(){}
			public byte[] pix_y;
			public byte[] pix_cb;
			public byte[] pix_cr;
			public int y_stride;
			public int c_stride;
			public int y_offset;
			public int c_offset;
			private int x;
			private int y;
			private int w;
			private int h;
			private YCbCrSubsampleRatio ratio;

			public YCbCrImage(int w, int h, YCbCrSubsampleRatio ratio){
				int cw, ch;
				yCbCrSize(w, h, ratio, out cw, out ch);
				pix_y = new byte[w*h];
				pix_cb = new byte[cw*ch];
				pix_cr = new byte[cw*ch];
				this.ratio = ratio;
				y_stride = w;
				c_stride = cw;
				x = 0;
				y = 0;
				this.w = w;
				this.h = h;
			}

			public YCbCrImage Subimage(int x, int y, int w, int h){
				YCbCrImage ret = new YCbCrImage{
					w = w,
					h = h,
					pix_y = pix_y,
					pix_cb = pix_cb,
					pix_cr = pix_cr,
					ratio = ratio,
					y_stride = y_stride,
					c_stride = c_stride,
					y_offset = y*y_stride + x,
					c_offset = y*c_stride + x
				};
				return ret;
			}

			public int get_row_y_offset(int y){
				return y*y_stride;
			}

			public int get_row_c_offset(int y){
				switch (ratio){
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio422:
						return y*c_stride;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio420:
						return (y/2)*c_stride;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio440:
						return (y/2)*c_stride;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio411:
						return y*c_stride;
					case YCbCrSubsampleRatio.YCbCrSubsampleRatio410:
						return (y/2)*c_stride;
					default:
						return y*c_stride;
				}
			}
		}

		public class GrayImage{
			public byte[] pixels;
			public int stride;
			public int x;
			public int y;
			public int w;
			public int h;
			public int offset;
			private GrayImage(){}

			public GrayImage(int w, int h){
				this.w = w;
				this.h = h;
				pixels = new byte[w*h];
				stride = w;
				offset = 0;
			}

			public GrayImage subimage(int x, int y, int w, int h){
				GrayImage ret = new GrayImage{w = w, h = h, pixels = pixels, stride = stride, offset = y*stride + x};
				return ret;
			}

			public int GetRowOffset(int y){
				return offset + y*stride;
			}
		}

		private class Huffman{
			public Huffman(){
				lut = new ushort[1 << lutSize];
				vals = new byte[maxNCodes];
				minCodes = new int[maxCodeLength];
				maxCodes = new int[maxCodeLength];
				valsIndices = new int[maxCodeLength];
				nCodes = 0;
			}

			public int nCodes;
			public readonly ushort[] lut;
			public readonly byte[] vals;
			public readonly int[] minCodes;
			public readonly int[] maxCodes;
			public readonly int[] valsIndices;
		}

		private class BytesClass{
			public BytesClass(){
				buf = new byte[4096];
				i = 0;
				j = 0;
				nUnreadable = 0;
			}

			public readonly byte[] buf;
			public int i;
			public int j;
			public int nUnreadable;
		}

		private class BitsClass{
			public uint a; // accumulator.
			public uint m; // mask. m==1<<(n-1) when n>0, with m==0 when n==0.
			public int n; // the number of unread bits in a.
		}

		private class MissingFF00Exception : Exception{}

		private class ShortHuffmanDataException : Exception{}

		private class EOFException : Exception{}

		private struct Scan{
			public byte compIndex;
			public byte td;
			public byte ta;
		}
	}
}