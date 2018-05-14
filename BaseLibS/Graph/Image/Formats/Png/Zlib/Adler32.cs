using System;

namespace BaseLibS.Graph.Image.Formats.Png.Zlib{
	internal sealed class Adler32 : IChecksum{
		private const uint Base = 65521;
		private uint checksum;

		public Adler32(){
			Reset();
		}

		public long Value => checksum;

		public void Reset(){
			checksum = 1;
		}

		public void Update(int value){
			uint s1 = checksum & 0xFFFF;
			uint s2 = checksum >> 16;
			s1 = (s1 + ((uint) value & 0xFF))%Base;
			s2 = (s1 + s2)%Base;
			checksum = (s2 << 16) + s1;
		}

		public void Update(byte[] buffer){
			if (buffer == null){
				throw new ArgumentNullException(nameof(buffer));
			}
			Update(buffer, 0, buffer.Length);
		}

		public void Update(byte[] buffer, int offset, int count){
			if (buffer == null){
				throw new ArgumentNullException(nameof(buffer));
			}
			if (offset < 0){
				throw new ArgumentOutOfRangeException(nameof(offset), "cannot be negative");
			}
			if (count < 0){
				throw new ArgumentOutOfRangeException(nameof(count), "cannot be negative");
			}
			if (offset >= buffer.Length){
				throw new ArgumentOutOfRangeException(nameof(offset), "not a valid index into buffer");
			}
			if (offset + count > buffer.Length){
				throw new ArgumentOutOfRangeException(nameof(count), "exceeds buffer size");
			}
			uint s1 = checksum & 0xFFFF;
			uint s2 = checksum >> 16;
			while (count > 0){
				int n = 3800;
				if (n > count){
					n = count;
				}
				count -= n;
				while (--n >= 0){
					s1 = s1 + (uint) (buffer[offset++] & 0xff);
					s2 = s2 + s1;
				}
				s1 %= Base;
				s2 %= Base;
			}
			checksum = (s2 << 16) | s1;
		}
	}
}