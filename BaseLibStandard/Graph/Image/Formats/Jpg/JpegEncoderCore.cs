using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Jpg{
	internal class JpegEncoderCore{
		private static readonly int[] unzig ={
			0, 1, 8, 16, 9, 2, 3, 10, 17, 24, 32, 25, 18, 11, 4, 5, 12, 19, 26, 33, 40, 48,
			41, 34, 27, 20, 13, 6, 7, 14, 21, 28, 35, 42, 49, 56, 57, 50, 43, 36, 29, 22, 15, 23, 30, 37, 44, 51, 58, 59, 52, 45,
			38, 31, 39, 46, 53, 60, 61, 54, 47, 55, 62, 63,
		};

		private const int nQuantIndex = 2;

		private readonly byte[] bitCount ={
			0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
			8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
			8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
			8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		};

		private readonly byte[,] unscaledQuant ={
			{ // Luminance.
				16, 11, 12, 14, 12, 10, 16, 14, 13, 14, 18, 17, 16, 19, 24, 40, 26, 24, 22, 22, 24, 49, 35, 37, 29, 40, 58, 51, 61,
				60, 57, 51, 56, 55, 64, 72, 92, 78, 64, 68, 87, 69, 55, 56, 80, 109, 81, 87, 95, 98, 103, 104, 103, 62, 77, 113, 121,
				112, 100, 120, 92, 101, 103, 99,
			},{ // Chrominance.
				17, 18, 18, 24, 21, 24, 47, 26, 26, 47, 99, 66, 56, 66, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
				99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
				99, 99, 99, 99, 99, 99,
			}
		};

		private readonly HuffmanSpec[] theHuffmanSpec ={ // Luminance DC.
			new HuffmanSpec(new byte[]{0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
				new byte[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11}),
			new HuffmanSpec(new byte[]{0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 125},
				new byte[]{
					0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12, 0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07, 0x22, 0x71, 0x14,
					0x32, 0x81, 0x91, 0xa1, 0x08, 0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0, 0x24, 0x33, 0x62, 0x72, 0x82, 0x09,
					0x0a, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a,
					0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x63, 0x64, 0x65,
					0x66, 0x67, 0x68, 0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88,
					0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9,
					0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca,
					0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
					0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa
				}),
			new HuffmanSpec(new byte[]{0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
				new byte[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11}),
			new HuffmanSpec(new byte[]{0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 119},
				new byte[]{
					0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21, 0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71, 0x13, 0x22, 0x32,
					0x81, 0x08, 0x14, 0x42, 0x91, 0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0, 0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16,
					0x24, 0x34, 0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38, 0x39,
					0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x63, 0x64,
					0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86,
					0x87, 0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
					0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8,
					0xc9, 0xca, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
					0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa,
				})
		};

		private class HuffmanLut{
			public readonly uint[] values;

			public HuffmanLut(HuffmanSpec s){
				int maxValue = 0;
				foreach (byte v in s.values){
					if (v > maxValue){
						maxValue = v;
					}
				}
				values = new uint[maxValue + 1];
				int code = 0;
				int k = 0;
				for (int i = 0; i < s.count.Length; i++){
					int nBits = (i + 1) << 24;
					for (int j = 0; j < s.count[i]; j++){
						values[s.values[k]] = (uint) (nBits | code);
						code++;
						k++;
					}
					code <<= 1;
				}
			}
		}

		private Stream outputStream;
		private readonly byte[] buffer = new byte[16];
		private uint bits;
		private uint nBits;
		private readonly byte[][] quant = new byte[nQuantIndex][]; // [Block.blockSize];
		private readonly HuffmanLut[] theHuffmanLut = new HuffmanLut[4];
		private JpegSubsample subsample;

		private void WriteByte(byte b){
			byte[] data = new byte[1];
			data[0] = b;
			outputStream.Write(data, 0, 1);
		}

		private void Emit(uint bits, uint nBits){
			nBits += this.nBits;
			bits <<= (int) (32 - nBits);
			bits |= this.bits;
			while (nBits >= 8){
				byte b = (byte) (bits >> 24);
				WriteByte(b);
				if (b == 0xff){
					WriteByte(0x00);
				}
				bits <<= 8;
				nBits -= 8;
			}
			this.bits = bits;
			this.nBits = nBits;
		}

		private void EmitHuff(HuffIndex index, int value){
			uint x = theHuffmanLut[(int) index].values[value];
			Emit(x & ((1 << 24) - 1), x >> 24);
		}

		private void EmitHuffRle(HuffIndex index, int runLength, int value){
			int a = value;
			int b = value;
			if (a < 0){
				a = -value;
				b = value - 1;
			}
			uint bt;
			if (a < 0x100){
				bt = bitCount[a];
			} else{
				bt = 8 + (uint) bitCount[a >> 8];
			}
			EmitHuff(index, (int) ((uint) (runLength << 4) | bt));
			if (bt > 0){
				Emit((uint) b & (uint) ((1 << ((int) bt)) - 1), bt);
			}
		}

		private int WriteBlock(Block block, QuantIndex index, int prevDc){
			FDCT.Transform(block);
			int dc = Round(block[0], 8*quant[(int) index][0]);
			EmitHuffRle((HuffIndex) (2*(int) index + 0), 0, dc - prevDc);
			HuffIndex h = (HuffIndex) (2*(int) index + 1);
			int runLength = 0;
			for (int zig = 1; zig < Block.blockSize; zig++){
				int ac = Round(block[unzig[zig]], 8*quant[(int) index][zig]);
				if (ac == 0){
					runLength++;
				} else{
					while (runLength > 15){
						EmitHuff(h, 0xf0);
						runLength -= 16;
					}
					EmitHuffRle(h, runLength, ac);
					runLength = 0;
				}
			}
			if (runLength > 0){
				EmitHuff(h, 0x00);
			}
			return dc;
		}

		private static void ToYCbCr(IPixelAccessor pixels, int x, int y, Block yBlock, Block cbBlock, Block crBlock){
			int xmax = pixels.Width - 1;
			int ymax = pixels.Height - 1;
			for (int j = 0; j < 8; j++){
				for (int i = 0; i < 8; i++){
					byte[] pixel = pixels[Math.Min(x + i, xmax), Math.Min(y + j, ymax)].ToBytes();
					YCbCr2 color = Color2.FromArgb(pixel[3], pixel[0], pixel[1], pixel[2]);
					int index = 8*j + i;
					yBlock[index] = (int) color.Y;
					cbBlock[index] = (int) color.Cb;
					crBlock[index] = (int) color.Cr;
				}
			}
		}

		private static void Scale16X16_8X8(Block destination, Block[] source){
			for (int i = 0; i < 4; i++){
				int dstOff = ((i & 2) << 4) | ((i & 1) << 2);
				for (int y = 0; y < 4; y++){
					for (int x = 0; x < 4; x++){
						int j = 16*y + 2*x;
						int sum = source[i][j] + source[i][j + 1] + source[i][j + 8] + source[i][j + 9];
						destination[8*y + x + dstOff] = (sum + 2)/4;
					}
				}
			}
		}

		private readonly byte[] sosHeaderY ={
			JpegConstants.Markers.XFF, JpegConstants.Markers.SOS, 0x00, 0x08, 0x01,
			// Number of components in a scan, 1
			0x01, // Component Id Y
			0x00, // DC/AC Huffman table 
			0x00, // Ss - Start of spectral selection.
			0x3f, // Se - End of spectral selection.
			0x00 // Ah + Ah (Successive approximation bit position high + low)
		};

		private readonly byte[] sosHeaderYCbCr ={
			JpegConstants.Markers.XFF, JpegConstants.Markers.SOS, 0x00, 0x0c, 0x03,
			// Number of components in a scan, 3
			0x01, // Component Id Y
			0x00, // DC/AC Huffman table 
			0x02, // Component Id Cb
			0x11, // DC/AC Huffman table 
			0x03, // Component Id Cr
			0x11, // DC/AC Huffman table 
			0x00, // Ss - Start of spectral selection.
			0x3f, // Se - End of spectral selection.
			0x00 // Ah + Ah (Successive approximation bit position high + low)
		};

		public void Encode(ImageBase image, Stream stream, int quality, JpegSubsample sample){
			if (image == null || stream == null){
				throw new ArgumentNullException();
			}
			ushort max = JpegConstants.MaxLength;
			if (image.Width >= max || image.Height >= max){
				throw new Exception($"Image is too large to encode at {image.Width}x{image.Height}.");
			}
			outputStream = stream;
			subsample = sample;
			for (int i = 0; i < theHuffmanSpec.Length; i++){
				theHuffmanLut[i] = new HuffmanLut(theHuffmanSpec[i]);
			}
			for (int i = 0; i < nQuantIndex; i++){
				quant[i] = new byte[Block.blockSize];
			}
			if (quality < 1){
				quality = 1;
			}
			if (quality > 100){
				quality = 100;
			}
			int scale;
			if (quality < 50){
				scale = 5000/quality;
			} else{
				scale = 200 - quality*2;
			}
			for (int i = 0; i < nQuantIndex; i++){
				for (int j = 0; j < Block.blockSize; j++){
					int x = unscaledQuant[i, j];
					x = (x*scale + 50)/100;
					if (x < 1){
						x = 1;
					}
					if (x > 255){
						x = 255;
					}
					quant[i][j] = (byte) x;
				}
			}
			int componentCount = 3;
			double densityX = ((Image2) image).HorizontalResolution;
			double densityY = ((Image2) image).VerticalResolution;
			WriteApplicationHeader((short) densityX, (short) densityY);
			WriteDqt();
			WriteSof0(image.Width, image.Height, componentCount);
			WriteDht(componentCount);
			using (IPixelAccessor pixels = image.Lock()){
				WriteSos(pixels);
			}
			buffer[0] = 0xff;
			buffer[1] = 0xd9;
			stream.Write(buffer, 0, 2);
			stream.Flush();
		}

		private static int Round(int dividend, int divisor){
			if (dividend >= 0){
				return (dividend + (divisor >> 1))/divisor;
			}
			return -((-dividend + (divisor >> 1))/divisor);
		}

		private void WriteApplicationHeader(short horizontalResolution, short verticalResolution){
			buffer[0] = JpegConstants.Markers.XFF;
			buffer[1] = JpegConstants.Markers.SOI;
			buffer[2] = JpegConstants.Markers.XFF;
			buffer[3] = JpegConstants.Markers.APP0; // Application Marker
			buffer[4] = 0x00;
			buffer[5] = 0x10;
			buffer[6] = 0x4a; // J
			buffer[7] = 0x46; // F
			buffer[8] = 0x49; // I
			buffer[9] = 0x46; // F
			buffer[10] = 0x00; // = "JFIF",'\0'
			buffer[11] = 0x01; // versionhi
			buffer[12] = 0x01; // versionlo
			buffer[13] = 0x01; // xyunits as dpi
			buffer[14] = 0x00; // Thumbnail width
			buffer[15] = 0x00; // Thumbnail height
			outputStream.Write(buffer, 0, 16);
			buffer[0] = (byte) (horizontalResolution >> 8);
			buffer[1] = (byte) horizontalResolution;
			buffer[2] = (byte) (verticalResolution >> 8);
			buffer[3] = (byte) verticalResolution;
			outputStream.Write(buffer, 0, 4);
		}

		private void WriteDqt(){
			int markerlen = 2 + nQuantIndex*(1 + Block.blockSize);
			WriteMarkerHeader(JpegConstants.Markers.DQT, markerlen);
			for (int i = 0; i < nQuantIndex; i++){
				WriteByte((byte) i);
				outputStream.Write(quant[i], 0, quant[i].Length);
			}
		}

		private void WriteSof0(int width, int height, int componentCount){
			byte[] subsamples = {0x22, 0x11, 0x11};
			byte[] chroma = {0x00, 0x01, 0x01};
			switch (subsample){
				case JpegSubsample.Ratio444:
					subsamples = new byte[]{0x11, 0x11, 0x11};
					break;
				case JpegSubsample.Ratio420:
					subsamples = new byte[]{0x22, 0x11, 0x11};
					break;
			}
			int markerlen = 8 + 3*componentCount;
			WriteMarkerHeader(JpegConstants.Markers.SOF0, markerlen);
			buffer[0] = 8; // Data Precision. 8 for now, 12 and 16 bit jpegs not supported
			buffer[1] = (byte) (height >> 8);
			buffer[2] = (byte) (height & 0xff); // (2 bytes, Hi-Lo), must be > 0 if DNL not supported
			buffer[3] = (byte) (width >> 8);
			buffer[4] = (byte) (width & 0xff); // (2 bytes, Hi-Lo), must be > 0 if DNL not supported
			buffer[5] = (byte) componentCount;
			if (componentCount == 1){
				buffer[6] = 1;
				buffer[7] = 0x11;
				buffer[8] = 0x00;
			} else{
				for (int i = 0; i < componentCount; i++){
					buffer[3*i + 6] = (byte) (i + 1);
					buffer[3*i + 7] = subsamples[i];
					buffer[3*i + 8] = chroma[i];
				}
			}
			outputStream.Write(buffer, 0, 3*(componentCount - 1) + 9);
		}

		private void WriteDht(int nComponent){
			byte[] headers = {0x00, 0x10, 0x01, 0x11};
			int markerlen = 2;
			HuffmanSpec[] specs = theHuffmanSpec;
			if (nComponent == 1){
				specs = new[]{theHuffmanSpec[0], theHuffmanSpec[1]};
			}
			foreach (HuffmanSpec s in specs){
				markerlen += 1 + 16 + s.values.Length;
			}
			WriteMarkerHeader(JpegConstants.Markers.DHT, markerlen);
			for (int i = 0; i < specs.Length; i++){
				HuffmanSpec spec = specs[i];
				WriteByte(headers[i]);
				outputStream.Write(spec.count, 0, spec.count.Length);
				outputStream.Write(spec.values, 0, spec.values.Length);
			}
		}

		private void WriteSos(IPixelAccessor pixels){
			// TODO: We should allow grayscale writing.
			outputStream.Write(sosHeaderYCbCr, 0, sosHeaderYCbCr.Length);
			switch (subsample){
				case JpegSubsample.Ratio444:
					Encode444(pixels);
					break;
				case JpegSubsample.Ratio420:
					Encode420(pixels);
					break;
			}
			Emit(0x7f, 7);
		}

		private void Encode444(IPixelAccessor pixels){
			Block b = new Block();
			Block cb = new Block();
			Block cr = new Block();
			int prevDcy = 0, prevDcCb = 0, prevDcCr = 0;
			for (int y = 0; y < pixels.Height; y += 8){
				for (int x = 0; x < pixels.Width; x += 8){
					ToYCbCr(pixels, x, y, b, cb, cr);
					prevDcy = WriteBlock(b, QuantIndex.Luminance, prevDcy);
					prevDcCb = WriteBlock(cb, QuantIndex.Chrominance, prevDcCb);
					prevDcCr = WriteBlock(cr, QuantIndex.Chrominance, prevDcCr);
				}
			}
		}

		private void Encode420(IPixelAccessor pixels){
			Block b = new Block();
			Block[] cb = new Block[4];
			Block[] cr = new Block[4];
			int prevDcy = 0, prevDcCb = 0, prevDcCr = 0;
			for (int i = 0; i < 4; i++){
				cb[i] = new Block();
			}
			for (int i = 0; i < 4; i++){
				cr[i] = new Block();
			}
			for (int y = 0; y < pixels.Height; y += 16){
				for (int x = 0; x < pixels.Width; x += 16){
					for (int i = 0; i < 4; i++){
						int xOff = (i & 1)*8;
						int yOff = (i & 2)*4;
						ToYCbCr(pixels, x + xOff, y + yOff, b, cb[i], cr[i]);
						prevDcy = WriteBlock(b, QuantIndex.Luminance, prevDcy);
					}
					Scale16X16_8X8(b, cb);
					prevDcCb = WriteBlock(b, QuantIndex.Chrominance, prevDcCb);
					Scale16X16_8X8(b, cr);
					prevDcCr = WriteBlock(b, QuantIndex.Chrominance, prevDcCr);
				}
			}
		}

		private void WriteMarkerHeader(byte marker, int length){
			buffer[0] = JpegConstants.Markers.XFF;
			buffer[1] = marker;
			buffer[2] = (byte) (length >> 8);
			buffer[3] = (byte) (length & 0xff);
			outputStream.Write(buffer, 0, 4);
		}

		private enum HuffIndex{
			LuminanceDC = 0,
			LuminanceAC = 1,
			ChrominanceDC = 2,
			ChrominanceAc = 3,
		}

		private enum QuantIndex{
			Luminance = 0,
			Chrominance = 1,
		}

		private struct HuffmanSpec{
			public HuffmanSpec(byte[] count, byte[] values){
				this.count = count;
				this.values = values;
			}

			public readonly byte[] count;
			public readonly byte[] values;
		}
	}
}