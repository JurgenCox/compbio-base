using System;
using System.IO;
using System.IO.Compression;

namespace BaseLibS.Graph.Image.Formats.Png.Zlib{
	internal sealed class ZlibDeflateStream : Stream{
		private readonly Stream rawStream;
		private readonly Adler32 adler32 = new Adler32();
		private bool isDisposed;
		private DeflateStream deflateStream;

		public ZlibDeflateStream(Stream stream, int compressionLevel){
			rawStream = stream;
			int cmf = 0x78;
			int flg = 218;
			if (compressionLevel >= 5 && compressionLevel <= 6){
				flg = 156;
			} else if (compressionLevel >= 3 && compressionLevel <= 4){
				flg = 94;
			} else if (compressionLevel <= 2){
				flg = 1;
			}
			flg -= (cmf*256 + flg)%31;
			if (flg < 0){
				flg += 31;
			}
			rawStream.WriteByte((byte) cmf);
			rawStream.WriteByte((byte) flg);
			//CompressionLevel level = CompressionLevel.Optimal;
			//if (compressionLevel >= 1 && compressionLevel <= 5){
			//	level = CompressionLevel.Fastest;
			//} else if (compressionLevel == 0){
			//	level = CompressionLevel.NoCompression;
			//}
			deflateStream = new DeflateStream(rawStream, CompressionMode.Compress, true);
		}

		public override bool CanRead => false;
		public override bool CanSeek => false;
		public override bool CanWrite => true;

		public override long Length => throw new NotSupportedException();

		public override long Position{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}

		public override void Flush(){
			deflateStream?.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count){
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin){
			throw new NotSupportedException();
		}

		public override void SetLength(long value){
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count){
			deflateStream.Write(buffer, offset, count);
			adler32.Update(buffer, offset, count);
		}

		protected override void Dispose(bool disposing){
			if (isDisposed){
				return;
			}
			if (disposing){
				if (deflateStream != null){
					deflateStream.Dispose();
					deflateStream = null;
				} else{
					rawStream.WriteByte(3);
					rawStream.WriteByte(0);
				}
				uint crc = (uint) adler32.Value;
				rawStream.WriteByte((byte) ((crc >> 24) & 0xFF));
				rawStream.WriteByte((byte) ((crc >> 16) & 0xFF));
				rawStream.WriteByte((byte) ((crc >> 8) & 0xFF));
				rawStream.WriteByte((byte) ((crc) & 0xFF));
			}
			base.Dispose(disposing);
			isDisposed = true;
		}
	}
}