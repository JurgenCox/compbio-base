using System;
using System.IO;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
namespace PluginRawMzMl{
	public class MzMlReader{
		public static float[] ReadBinaryArray(byte[] bytes, bool useZlibDecompression, int precision) {
			if (bytes.Length == 0) {
				return new float[0];
			}
			if (useZlibDecompression) {
				using (var output = new MemoryStream())
				using (var compressed = new MemoryStream(bytes, false))
				using (var inflator = new InflaterInputStream(compressed, new Inflater(false))) {
					inflator.CopyTo(output);
					bytes = output.ToArray();
				}
			}
			if (precision != 64 && precision != 32) {
				throw new InvalidOperationException("Invalid precision value found");
			}
			return precision == 64
				? ConvertArray(bytes, (b, i) => (float)BitConverter.ToDouble(b, i), 8)
				: ConvertArray(bytes, BitConverter.ToSingle, 4);
		}
		public static float[] ReadBinaryArray(XElement binaryNode, bool useZlibDecompression, int precision) {
			byte[] bytes = Convert.FromBase64String(binaryNode.Value);
			return ReadBinaryArray(bytes, useZlibDecompression, precision);
		}
		private static float[] ConvertArray(byte[] data, Func<byte[], int, float> convert, int bytesPerElement){
			var arraySize = data.Length / bytesPerElement;
			var array = new float[arraySize];
			for (int i = 0; i < array.Length; ++i){
				array[i] = convert(data, i * bytesPerElement);
			}
			return array;
		}
	}
}