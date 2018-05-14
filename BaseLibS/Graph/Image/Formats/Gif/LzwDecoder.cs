using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Gif{
	internal sealed class LzwDecoder{
		private const int maxStackSize = 4096;
		private const int nullCode = -1;
		private readonly Stream stream;

		public LzwDecoder(Stream stream){
			if (stream == null){
				throw new ArgumentNullException();
			}
			this.stream = stream;
		}

		public byte[] DecodePixels(int width, int height, int dataSize){
			if (dataSize >= int.MaxValue){
				throw new ArgumentOutOfRangeException();
			}
			byte[] pixels = new byte[width*height];
			int clearCode = 1 << dataSize;
			int codeSize = dataSize + 1;
			int endCode = clearCode + 1;
			int availableCode = clearCode + 2;
			int code;
			int oldCode = nullCode;
			int codeMask = (1 << codeSize) - 1;
			int bits = 0;
			int[] prefix = new int[maxStackSize];
			int[] suffix = new int[maxStackSize];
			int[] pixelStatck = new int[maxStackSize + 1];
			int top = 0;
			int count = 0;
			int bi = 0;
			int xyz = 0;
			int data = 0;
			int first = 0;
			for (code = 0; code < clearCode; code++){
				prefix[code] = 0;
				suffix[code] = (byte) code;
			}
			byte[] buffer = null;
			while (xyz < pixels.Length){
				if (top == 0){
					if (bits < codeSize){
						// Load bytes until there are enough bits for a code.
						if (count == 0){
							// Read a new data block.
							buffer = ReadBlock();
							count = buffer.Length;
							if (count == 0){
								break;
							}
							bi = 0;
						}
						if (buffer != null){
							data += buffer[bi] << bits;
						}
						bits += 8;
						bi++;
						count--;
						continue;
					}
					// Get the next code
					code = data & codeMask;
					data >>= codeSize;
					bits -= codeSize;
					// Interpret the code
					if (code > availableCode || code == endCode){
						break;
					}
					if (code == clearCode){
						// Reset the decoder
						codeSize = dataSize + 1;
						codeMask = (1 << codeSize) - 1;
						availableCode = clearCode + 2;
						oldCode = nullCode;
						continue;
					}
					if (oldCode == nullCode){
						pixelStatck[top++] = suffix[code];
						oldCode = code;
						first = code;
						continue;
					}
					int inCode = code;
					if (code == availableCode){
						pixelStatck[top++] = (byte) first;
						code = oldCode;
					}
					while (code > clearCode){
						pixelStatck[top++] = suffix[code];
						code = prefix[code];
					}
					first = suffix[code];
					pixelStatck[top++] = suffix[code];
					if (availableCode < maxStackSize){
						prefix[availableCode] = oldCode;
						suffix[availableCode] = first;
						availableCode++;
						if (availableCode == codeMask + 1 && availableCode < maxStackSize){
							codeSize++;
							codeMask = (1 << codeSize) - 1;
						}
					}
					oldCode = inCode;
				}
				// Pop a pixel off the pixel stack.
				top--;
				// Clear missing pixels
				pixels[xyz++] = (byte) pixelStatck[top];
			}
			return pixels;
		}

		private byte[] ReadBlock(){
			int blockSize = stream.ReadByte();
			return ReadBytes(blockSize);
		}

		private byte[] ReadBytes(int length){
			byte[] buffer = new byte[length];
			stream.Read(buffer, 0, length);
			return buffer;
		}
	}
}