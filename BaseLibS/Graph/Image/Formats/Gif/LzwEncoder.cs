using System;
using System.IO;

namespace BaseLibS.Graph.Image.Formats.Gif{
	internal sealed class LzwEncoder{
		private const int eof = -1;
		private const int bits = 12;
		private const int hashSize = 5003; // 80% occupancy
		private readonly byte[] pixelArray;
		private readonly int initialCodeSize;
		private int curPixel;
		private int bitCount;
		private int maxbits = bits;
		private int maxcode; // maximum code, given bitCount
		private int maxmaxcode = 1 << bits; // should NEVER generate this code
		private readonly int[] hashTable = new int[hashSize];
		private readonly int[] codeTable = new int[hashSize];
		private int hsize = hashSize;
		private int freeEntry;
		private bool clearFlag;
		private int globalInitialBits;
		private int clearCode;
		private int eofCode;
		private int currentAccumulator;
		private int currentBits;
		private readonly int[] masks ={
			0x0000, 0x0001, 0x0003, 0x0007, 0x000F, 0x001F, 0x003F, 0x007F, 0x00FF, 0x01FF, 0x03FF,
			0x07FF, 0x0FFF, 0x1FFF, 0x3FFF, 0x7FFF, 0xFFFF
		};
		private int accumulatorCount;
		private readonly byte[] accumulators = new byte[256];
		public LzwEncoder(byte[] indexedPixels, int colorDepth){
			pixelArray = indexedPixels;
			initialCodeSize = Math.Max(2, colorDepth);
		}
		public void Encode(Stream stream){
			stream.WriteByte((byte) initialCodeSize);
			curPixel = 0;
			Compress(initialCodeSize + 1, stream);
			stream.WriteByte(GifConstants.terminator);
		}
		private static int GetMaxcode(int bitCount){
			return (1 << bitCount) - 1;
		}
		private void AddCharacter(byte c, Stream stream){
			accumulators[accumulatorCount++] = c;
			if (accumulatorCount >= 254){
				FlushPacket(stream);
			}
		}
		private void ClearBlock(Stream stream){
			ResetCodeTable(hsize);
			freeEntry = clearCode + 2;
			clearFlag = true;
			Output(clearCode, stream);
		}
		private void ResetCodeTable(int size){
			for (int i = 0; i < size; ++i){
				hashTable[i] = -1;
			}
		}
		private void Compress(int intialBits, Stream stream){
			int fcode;
			int c;
			globalInitialBits = intialBits;
			clearFlag = false;
			bitCount = globalInitialBits;
			maxcode = GetMaxcode(bitCount);
			clearCode = 1 << (intialBits - 1);
			eofCode = clearCode + 1;
			freeEntry = clearCode + 2;
			accumulatorCount = 0; // clear packet
			int ent = NextPixel();
			int hshift = 0;
			for (fcode = hsize; fcode < 65536; fcode *= 2){
				++hshift;
			}
			hshift = 8 - hshift; // set hash code range bound
			int hsizeReg = hsize;
			ResetCodeTable(hsizeReg); // clear hash table
			Output(clearCode, stream);
			while ((c = NextPixel()) != eof){
				fcode = (c << maxbits) + ent;
				int i = (c << hshift) ^ ent /* = 0 */;
				if (hashTable[i] == fcode){
					ent = codeTable[i];
					continue;
				}

				// Non-empty slot
				if (hashTable[i] >= 0){
					int disp = hsizeReg - i;
					if (i == 0){
						disp = 1;
					}
					do{
						if ((i -= disp) < 0){
							i += hsizeReg;
						}
						if (hashTable[i] == fcode){
							ent = codeTable[i];
							break;
						}
					} while (hashTable[i] >= 0);
					if (hashTable[i] == fcode){
						continue;
					}
				}
				Output(ent, stream);
				ent = c;
				if (freeEntry < maxmaxcode){
					codeTable[i] = freeEntry++; // code -> hashtable
					hashTable[i] = fcode;
				} else{
					ClearBlock(stream);
				}
			}
			Output(ent, stream);
			Output(eofCode, stream);
		}
		private void FlushPacket(Stream outs){
			if (accumulatorCount > 0){
				outs.WriteByte((byte) accumulatorCount);
				outs.Write(accumulators, 0, accumulatorCount);
				accumulatorCount = 0;
			}
		}
		private int NextPixel(){
			if (curPixel == pixelArray.Length){
				return eof;
			}
			if (curPixel == pixelArray.Length){
				return eof;
			}
			curPixel++;
			return pixelArray[curPixel - 1] & 0xff;
		}
		private void Output(int code, Stream outs){
			currentAccumulator &= masks[currentBits];
			if (currentBits > 0){
				currentAccumulator |= (code << currentBits);
			} else{
				currentAccumulator = code;
			}
			currentBits += bitCount;
			while (currentBits >= 8){
				AddCharacter((byte) (currentAccumulator & 0xff), outs);
				currentAccumulator >>= 8;
				currentBits -= 8;
			}
			if (freeEntry > maxcode || clearFlag){
				if (clearFlag){
					maxcode = GetMaxcode(bitCount = globalInitialBits);
					clearFlag = false;
				} else{
					++bitCount;
					maxcode = bitCount == maxbits ? maxmaxcode : GetMaxcode(bitCount);
				}
			}
			if (code == eofCode){
				while (currentBits > 0){
					AddCharacter((byte) (currentAccumulator & 0xff), outs);
					currentAccumulator >>= 8;
					currentBits -= 8;
				}
				FlushPacket(outs);
			}
		}
	}
}