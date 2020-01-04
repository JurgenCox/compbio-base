using System;
using System.IO;
using System.Text;

namespace BaseLibS.Parse.Endian{
	public class EndianBinaryReader : IDisposable{
		private readonly Decoder decoder;
		private readonly byte[] buffer = new byte[16];
		private readonly char[] charBuffer = new char[1];
		private readonly int minBytesPerChar;
		private bool disposed;
		public EndianBinaryReader(EndianBitConverter bitConverter, Stream stream) : this(bitConverter, stream, Encoding.UTF8){}
		public EndianBinaryReader(EndianBitConverter bitConverter, Stream stream, Encoding encoding){
			if (bitConverter == null){
				throw new ArgumentNullException(nameof(bitConverter));
			}
			if (stream == null){
				throw new ArgumentNullException(nameof(stream));
			}
			if (encoding == null){
				throw new ArgumentNullException(nameof(encoding));
			}
			if (!stream.CanRead){
				throw new ArgumentException("Stream is not writable", nameof(stream));
			}
			BaseStream = stream;
			BitConverter = bitConverter;
			Encoding = encoding;
			decoder = encoding.GetDecoder();
			minBytesPerChar = 1;
			if (encoding is UnicodeEncoding){
				minBytesPerChar = 2;
			}
		}
		public EndianBitConverter BitConverter { get; }
		public Encoding Encoding { get; }
		public Stream BaseStream { get; }
		public void Close(){
			Dispose();
		}
		public void Seek(int offset, SeekOrigin origin){
			CheckDisposed();
			BaseStream.Seek(offset, origin);
		}
		public byte ReadByte(){
			ReadInternal(buffer, 1);
			return buffer[0];
		}
		public sbyte ReadSByte(){
			ReadInternal(buffer, 1);
			return unchecked((sbyte) buffer[0]);
		}
		public bool ReadBoolean(){
			ReadInternal(buffer, 1);
			return BitConverter.ToBoolean(buffer, 0);
		}
		public short ReadInt16(){
			ReadInternal(buffer, 2);
			return BitConverter.ToInt16(buffer, 0);
		}
		public int ReadInt32(){
			ReadInternal(buffer, 4);
			return BitConverter.ToInt32(buffer, 0);
		}
		public long ReadInt64(){
			ReadInternal(buffer, 8);
			return BitConverter.ToInt64(buffer, 0);
		}
		public ushort ReadUInt16(){
			ReadInternal(buffer, 2);
			return BitConverter.ToUInt16(buffer, 0);
		}
		public uint ReadUInt32(){
			ReadInternal(buffer, 4);
			return BitConverter.ToUInt32(buffer, 0);
		}
		public ulong ReadUInt64(){
			ReadInternal(buffer, 8);
			return BitConverter.ToUInt64(buffer, 0);
		}
		public float ReadSingle(){
			ReadInternal(buffer, 4);
			return BitConverter.ToSingle(buffer, 0);
		}
		public double ReadDouble(){
			ReadInternal(buffer, 8);
			return BitConverter.ToDouble(buffer, 0);
		}
		public decimal ReadDecimal(){
			ReadInternal(buffer, 16);
			return BitConverter.ToDecimal(buffer, 0);
		}
		public int Read(){
			int charsRead = Read(charBuffer, 0, 1);
			if (charsRead == 0){
				return -1;
			}
			return charBuffer[0];
		}
		public int Read(char[] data, int index, int count){
			CheckDisposed();
			if (buffer == null){
				throw new Exception("buffer");
			}
			if (index < 0){
				throw new ArgumentOutOfRangeException(nameof(index));
			}
			if (count < 0){
				throw new ArgumentOutOfRangeException(nameof(index));
			}
			if (count + index > data.Length){
				throw new ArgumentException(
					"Not enough space in buffer for specified number of characters starting at specified index");
			}
			int read = 0;
			bool firstTime = true;

			// Use the normal buffer if we're only reading a small amount, otherwise
			// use at most 4K at a time.
			byte[] byteBuffer = buffer;
			if (byteBuffer.Length < count*minBytesPerChar){
				byteBuffer = new byte[4096];
			}
			while (read < count){
				int amountToRead;
				// First time through we know we haven't previously read any data
				if (firstTime){
					amountToRead = count*minBytesPerChar;
					firstTime = false;
				}
				// After that we can only assume we need to fully read 'chars left -1' characters
				// and a single byte of the character we may be in the middle of
				else{
					amountToRead = (count - read - 1)*minBytesPerChar + 1;
				}
				if (amountToRead > byteBuffer.Length){
					amountToRead = byteBuffer.Length;
				}
				int bytesRead = TryReadInternal(byteBuffer, amountToRead);
				if (bytesRead == 0){
					return read;
				}
				int decoded = decoder.GetChars(byteBuffer, 0, bytesRead, data, index);
				read += decoded;
				index += decoded;
			}
			return read;
		}
		public int Read(byte[] buffer1, int index, int count){
			CheckDisposed();
			if (buffer1 == null){
				throw new ArgumentNullException(nameof(buffer1));
			}
			if (index < 0){
				throw new ArgumentOutOfRangeException(nameof(index));
			}
			if (count < 0){
				throw new ArgumentOutOfRangeException(nameof(index));
			}
			if (count + index > buffer1.Length){
				throw new ArgumentException("Not enough space in buffer for specified number of bytes starting at specified index");
			}
			int read = 0;
			while (count > 0){
				int block = BaseStream.Read(buffer1, index, count);
				if (block == 0){
					return read;
				}
				index += block;
				read += block;
				count -= block;
			}
			return read;
		}
		public byte[] ReadBytes(int count){
			CheckDisposed();
			if (count < 0){
				throw new ArgumentOutOfRangeException(nameof(count));
			}
			byte[] ret = new byte[count];
			int index = 0;
			while (index < count){
				int read = BaseStream.Read(ret, index, count - index);

				// Stream has finished half way through. That's fine, return what we've got.
				if (read == 0){
					byte[] copy = new byte[index];
					Buffer.BlockCopy(ret, 0, copy, 0, index);
					return copy;
				}
				index += read;
			}
			return ret;
		}
		public byte[] ReadBytesOrThrow(int count){
			byte[] ret = new byte[count];
			ReadInternal(ret, count);
			return ret;
		}
		public int Read7BitEncodedInt(){
			CheckDisposed();
			int ret = 0;
			for (int shift = 0; shift < 35; shift += 7){
				int b = BaseStream.ReadByte();
				if (b == -1){
					throw new EndOfStreamException();
				}
				ret = ret | ((b & 0x7f) << shift);
				if ((b & 0x80) == 0){
					return ret;
				}
			}

			// Still haven't seen a byte with the high bit unset? Dodgy data.
			throw new IOException("Invalid 7-bit encoded integer in stream.");
		}
		public int ReadBigEndian7BitEncodedInt(){
			CheckDisposed();
			int ret = 0;
			for (int i = 0; i < 5; i++){
				int b = BaseStream.ReadByte();
				if (b == -1){
					throw new EndOfStreamException();
				}
				ret = (ret << 7) | (b & 0x7f);
				if ((b & 0x80) == 0){
					return ret;
				}
			}

			// Still haven't seen a byte with the high bit unset? Dodgy data.
			throw new IOException("Invalid 7-bit encoded integer in stream.");
		}
		public string ReadString(){
			int bytesToRead = Read7BitEncodedInt();
			byte[] data = new byte[bytesToRead];
			ReadInternal(data, bytesToRead);
			return Encoding.GetString(data, 0, data.Length);
		}
		public void Dispose(){
			if (!disposed){
				disposed = true;
				((IDisposable) BaseStream).Dispose();
			}
		}
		private void CheckDisposed(){
			if (disposed){
				throw new ObjectDisposedException("EndianBinaryReader");
			}
		}
		private void ReadInternal(byte[] data, int size){
			CheckDisposed();
			int index = 0;
			while (index < size){
				int read = BaseStream.Read(data, index, size - index);
				if (read == 0){
					throw new EndOfStreamException(
						$"End of stream reached with {size - index} byte{(size - index == 1 ? "s" : string.Empty)} left to read.");
				}
				index += read;
			}
		}
		private int TryReadInternal(byte[] data, int size){
			CheckDisposed();
			int index = 0;
			while (index < size){
				int read = BaseStream.Read(data, index, size - index);
				if (read == 0){
					return index;
				}
				index += read;
			}
			return index;
		}
	}
}