using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BaseLibS.Util{
	/// <summary>
	/// Reads the stream backwards one character at a time.
	/// </summary>
	public class StreamBackwardReader : IDisposable
	{
		/// <summary>
		/// Create <see cref="StreamBackwardReader"/> from file name.
		/// </summary>
		public StreamBackwardReader(string filename) : this(new FileStream(filename, FileMode.Open, FileAccess.Read))
		{
		}

		/// <summary>
		/// Create <see cref="StreamBackwardReader"/> from a <see cref="Stream"/>.
		/// </summary>
		public StreamBackwardReader(Stream stream)
		{
			if (!stream.CanSeek)
			{
				throw new ArgumentException($"{nameof(StreamBackwardReader)} requires a seekable stream.");
			}
			_stream = stream;
			_stream.Seek(0, SeekOrigin.End);
		}

		private readonly Stream _stream;

		/// <summary>
		/// Read a line from the back of the stream
		/// </summary>
		public string ReadLine()
		{
			StringBuilder str = new StringBuilder();
			int ch;
			do{
				if (_stream.Position == 0){
					return str.Length > 0 ? str.ToString() : null;
				}
				_stream.Seek(-1, SeekOrigin.Current);
				ch = _stream.ReadByte();
				_stream.Seek(-1, SeekOrigin.Current);
				if (ch != '\n'){
					str.Insert(0, (char) ch);
				}
			} while (ch != '\n');
			return str.ToString();
		}

		/// <summary>
		/// Lazily iterate over all lines in reverse order.
		/// </summary>
		public IEnumerable<string> ReadLines()
		{
			string line;
			while ((line = ReadLine()) != null)
			{
				yield return line;
			}
		}

		public void Dispose()
		{
			_stream?.Dispose();
		}
	}
}