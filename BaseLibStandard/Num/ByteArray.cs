using System;

namespace BaseLibS.Num{
	public static class ByteArray{
		/// <summary>The data is stored in big endian mode.</summary>
		public static readonly int endianBig = 0;
		/// <summary>The data is stored in little endian mode.</summary>
		public static readonly int endianLittle = 1;

		/// <summary>
		/// Converts a byte array containing double-values into a double array. The
		/// given value for endiannes (either {@link ByteArray#EndianBig} or
		/// {@link ByteArray#EndianLittle}) indicates the byte-order the data has been
		/// stored in. The precision indicates the number of bits for each element
		/// in the double array.
		/// </summary>
		/// <param name="array">The byte-array to be converted.</param>
		/// <param name="endiannes">The byte-order in which the data has been stored (either <see cref="endianLittle"/> or <see cref="endianBig"/>).</param>
		/// <param name="precision">The number of bits for each element in the double-array.</param>
		/// <returns>The array with the double-values.</returns>
		public static double[] ToDoubleArray(byte[] array, int endiannes, int precision){
			if (precision != 32 && precision != 64){
				throw new Exception("only 32 and 64 bits precision supported");
			}
			if (endiannes != endianBig && endiannes != endianLittle){
				throw new Exception("unknown endiannes set: '" + endiannes + "'");
			}
			int length = array.Length/(precision/8);
			double[] doublearray = new double[length];
			if (precision == 32 && endiannes == endianBig){
				int pos = 0;
				byte[] value = new byte[4];
				for (int i = 0; i < length; ++i){
					value[0] = array[pos + 3];
					value[1] = array[pos + 2];
					value[2] = array[pos + 1];
					value[3] = array[pos + 0];
					doublearray[i] = BitConverter.ToSingle(value, 0);
					pos += 4;
				}
			} else if (precision == 32 && endiannes == endianLittle){
				int pos = 0;
				for (int i = 0; i < length; ++i){
					doublearray[i] = BitConverter.ToSingle(array, pos);
					pos += 4;
				}
			} else if (precision == 64 && endiannes == endianBig){
				int pos = 0;
				byte[] value = new byte[8];
				for (int i = 0; i < length; ++i){
					value[0] = array[pos + 3];
					value[1] = array[pos + 2];
					value[2] = array[pos + 1];
					value[3] = array[pos + 0];
					value[4] = array[pos + 7];
					value[5] = array[pos + 6];
					value[6] = array[pos + 5];
					value[7] = array[pos + 4];
					doublearray[i] = BitConverter.ToDouble(value, 0);
					pos += 8;
				}
			} else if (precision == 64 && endiannes == endianLittle){
				int pos = 0;
				for (int i = 0; i < length; ++i){
					doublearray[i] = BitConverter.ToDouble(array, pos);
					pos += 8;
				}
			}
			return doublearray;
		}
	}
}