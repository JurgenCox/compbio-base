using System;
using System.IO;
using System.IO.Compression;

namespace BaseLibS.Graph.Image.Formats.Png.Zlib{
	internal sealed class ZlibInflateStream : Stream{
		private bool isDisposed;
		private readonly Stream rawStream;
		private byte[] crcread;
		private DeflateStream deflateStream;

		public ZlibInflateStream(Stream stream){
			rawStream = stream;
			int cmf = rawStream.ReadByte();
			int flag = rawStream.ReadByte();
			if (cmf == -1 || flag == -1){
				return;
			}
			if ((cmf & 0x0f) != 8){
				throw new Exception($"Bad compression method for ZLIB header: cmf={cmf}");
			}
			bool fdict = (flag & 32) != 0;
			if (fdict){
				byte[] dictId = new byte[4];
				for (int i = 0; i < 4; i++){
					dictId[i] = (byte) rawStream.ReadByte();
				}
			}
			deflateStream = new DeflateStream(rawStream, CompressionMode.Decompress, true);
		}

		public override bool CanRead => true;
		public override bool CanSeek => false;
		public override bool CanWrite => false;

		public override long Length => throw new NotSupportedException();

		public override long Position{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}

		public override void Flush(){
			deflateStream?.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count){
			int read = deflateStream.Read(buffer, offset, count);
			if (read < 1 && crcread == null){
				crcread = new byte[4];
				for (int i = 0; i < 4; i++){
					crcread[i] = (byte) rawStream.ReadByte();
				}
			}
			return read;
		}

		public override long Seek(long offset, SeekOrigin origin){
			throw new NotSupportedException();
		}

		public override void SetLength(long value){
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count){
			throw new NotSupportedException();
		}

		protected override void Dispose(bool disposing){
			if (isDisposed){
				return;
			}
			if (disposing){
				if (deflateStream != null){
					deflateStream.Dispose();
					deflateStream = null;
					if (crcread == null){
						crcread = new byte[4];
						for (int i = 0; i < 4; i++){
							crcread[i] = (byte) rawStream.ReadByte();
						}
					}
				}
			}
			base.Dispose(disposing);
			isDisposed = true;
		}
	}
}