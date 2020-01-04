using System;
using System.IO;
using System.Text;

namespace BaseLibS.Parse.Endian{
	public class EndianBinaryWriter : IDisposable{
		private readonly byte[] buffer = new byte[16];
		private readonly char[] charBuffer = new char[1];
		private bool disposed;
		public EndianBinaryWriter(EndianBitConverter bitConverter, Stream stream) : this(bitConverter, stream, Encoding.UTF8){}
		public EndianBinaryWriter(EndianBitConverter bitConverter, Stream stream, Encoding encoding){
			if (bitConverter == null){
				throw new ArgumentNullException(nameof(bitConverter));
			}
			if (stream == null){
				throw new ArgumentNullException(nameof(stream));
			}
			if (encoding == null){
				throw new ArgumentNullException(nameof(encoding));
			}
			if (!stream.CanWrite){
				throw new ArgumentException("Cannot write to stream", nameof(stream));
			}
			BaseStream = stream;
			BitConverter = bitConverter;
			Encoding = encoding;
		}
		public EndianBitConverter BitConverter { get; }
		public Encoding Encoding { get; }
		public Stream BaseStream { get; }
		public void Close(){
			Dispose();
		}
		public void Flush(){
			CheckDisposed();
			BaseStream.Flush();
		}
		public void Seek(int offset, SeekOrigin origin){
			CheckDisposed();
			BaseStream.Seek(offset, origin);
		}
		public void Write(bool value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 1);
		}
		public void Write(short value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 2);
		}
		public void Write(int value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 4);
		}
		public void Write(long value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 8);
		}
		public void Write(ushort value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 2);
		}
		public void Write(uint value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 4);
		}
		public void Write(ulong value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 8);
		}
		public void Write(float value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 4);
		}
		public void Write(double value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 8);
		}
		public void Write(decimal value){
			BitConverter.CopyBytes(value, buffer, 0);
			WriteInternal(buffer, 16);
		}
		public void Write(byte value){
			buffer[0] = value;
			WriteInternal(buffer, 1);
		}
		public void Write(sbyte value){
			buffer[0] = unchecked((byte) value);
			WriteInternal(buffer, 1);
		}
		public void Write(byte[] value){
			if (value == null){
				throw new ArgumentNullException(nameof(value));
			}
			WriteInternal(value, value.Length);
		}
		public void Write(byte[] value, int offset, int count){
			CheckDisposed();
			BaseStream.Write(value, offset, count);
		}
		public void Write(char value){
			charBuffer[0] = value;
			Write(charBuffer);
		}
		public void Write(char[] value){
			if (value == null){
				throw new ArgumentNullException(nameof(value));
			}
			CheckDisposed();
			byte[] data = Encoding.GetBytes(value, 0, value.Length);
			WriteInternal(data, data.Length);
		}
		public void Write(string value){
			if (value == null){
				throw new ArgumentNullException(nameof(value));
			}
			CheckDisposed();
			byte[] data = Encoding.GetBytes(value);
			Write7BitEncodedInt(data.Length);
			WriteInternal(data, data.Length);
		}
		public void Write7BitEncodedInt(int value){
			CheckDisposed();
			if (value < 0){
				throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater equal 0.");
			}
			int index = 0;
			while (value >= 128){
				buffer[index++] = (byte) ((value & 0x7f) | 0x80);
				value = value >> 7;
				index++;
			}
			buffer[index++] = (byte) value;
			BaseStream.Write(buffer, 0, index);
		}
		private void CheckDisposed(){
			if (disposed){
				throw new ObjectDisposedException("EndianBinaryWriter");
			}
		}
		private void WriteInternal(byte[] bytes, int length){
			CheckDisposed();
			BaseStream.Write(bytes, 0, length);
		}
		public void Dispose(){
			if (!disposed){
				Flush();
				disposed = true;
				((IDisposable) BaseStream).Dispose();
			}
		}
	}
}