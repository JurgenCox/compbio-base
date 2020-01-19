using System;

namespace PluginRawMzMl{
	public class MsNumpress{
		public const string accNumpressLinear = "MS:1002312";

		public const string accNumpressPic = "MS:1002313";

		public const string accNumpressSlof = "MS:1002314";

		public static double[] Decode(string cvAccession, byte[] data, int dataSize){
			if (cvAccession == accNumpressLinear){
				if (dataSize < 8 || data.Length < 8)
					throw new ArgumentException(
						"Cannot decode numLin data, need at least 8 initial bytes for fixed point.");
				double[] buffer = new double[dataSize * 2];
				int nbrOfDoubles = MsNumpress.DecodeLinear(data, dataSize, buffer);
				if (nbrOfDoubles < 0)
					throw new ArgumentException("Corrupt numLin data!");
				double[] result = new double[nbrOfDoubles];
				Array.Copy(buffer, 0, result, 0, nbrOfDoubles);
				return result;
			}
			if (cvAccession == accNumpressSlof){
				double[] result = new double[(dataSize - 8) / 2];
				MsNumpress.DecodeSlof(data, dataSize, result);
				return result;
			}
			if (cvAccession == accNumpressPic){
				if (dataSize < 8 || data.Length < 8)
					throw new ArgumentException(
						"Cannot decode numPic data, need at least 8 initial bytes for fixed point.");
				double[] buffer = new double[dataSize * 2];
				int nbrOfDoubles = MsNumpress.DecodePic(data, dataSize, buffer);
				if (nbrOfDoubles < 0)
					throw new ArgumentException("Corrupt numPic data!");
				double[] result = new double[nbrOfDoubles];
				Array.Copy(buffer, 0, result, 0, nbrOfDoubles);
				return result;
			}
			throw new ArgumentException("'" + cvAccession + "' is not a numpress compression term");
		}


		public static double DecodeFixedPoint(byte[] data){
			long fp = 0;
			for (int i = 0; i < 8; i++){
				fp = fp | ((0xFFL & data[7 - i]) << (8 * i));
			}
			return BitConverter.Int64BitsToDouble(fp);
		}

		public static int DecodeLinear(byte[] data, int dataSize, double[] result){
			int ri = 2;
			long[] ints = new long[3];
			IntDecoder dec = new IntDecoder(data, 16);
			if (dataSize == 8) return 0;
			if (dataSize < 8) return -1;
			double fixedPoint = DecodeFixedPoint(data);
			if (dataSize < 12) return -1;
			ints[1] = 0;
			for (int i = 0; i < 4; i++){
				ints[1] = ints[1] | ((0xFFL & data[8 + i]) << (i * 8));
			}
			result[0] = ints[1] / fixedPoint;
			if (dataSize == 12) return 1;
			if (dataSize < 16) return -1;
			ints[2] = 0;
			for (int i = 0; i < 4; i++){
				ints[2] = ints[2] | ((0xFFL & data[12 + i]) << (i * 8));
			}
			result[1] = ints[2] / fixedPoint;
			while (dec.pos < dataSize){
				if (dec.pos == (dataSize - 1) && dec.half)
					if ((data[dec.pos] & 0xf) != 0x8)
						break;
				ints[0] = ints[1];
				ints[1] = ints[2];
				ints[2] = dec.Next();
				long extrapol = ints[1] + (ints[1] - ints[0]);
				long y = extrapol + ints[2];
				result[ri++] = y / fixedPoint;
				ints[2] = y;
			}
			return ri;
		}

		public static int DecodePic(byte[] data, int dataSize, double[] result){
			int ri = 0;
			IntDecoder dec = new IntDecoder(data, 0);
			while (dec.pos < dataSize){
				if (dec.pos == (dataSize - 1) && dec.half)
					if ((data[dec.pos] & 0xf) != 0x8)
						break;
				long count = dec.Next();
				result[ri++] = count;
			}
			return ri;
		}

		public static int DecodeSlof(byte[] data, int dataSize, double[] result){
			int ri = 0;
			if (dataSize < 8) return -1;
			double fixedPoint = DecodeFixedPoint(data);
			if (dataSize % 2 != 0) return -1;
			for (int i = 8; i < dataSize; i += 2){
				int x = (0xff & data[i]) | ((0xff & data[i + 1]) << 8);
				result[ri++] = Math.Exp((0xffff & x) / fixedPoint) - 1;
			}
			return ri;
		}

		public class IntDecoder{
			public int pos;
			public bool half;
			public byte[] bytes;

			public IntDecoder(byte[] bytes, int pos){
				this.bytes = bytes;
				this.pos = pos;
			}

			public long Next(){
				int head;
				int i, n;
				long res = 0;
				if (!half)
					head = (0xff & bytes[pos]) >> 4;
				else
					head = 0xf & bytes[pos++];
				half = !half;
				if (head <= 8)
					n = head;
				else{
					// leading ones, fill in res
					n = head - 8;
					long mask = unchecked((int) 0xF0000000);
					for (i = 0; i < n; i++){
						long m = mask >> (4 * i);
						res = res | m;
					}
				}
				if (n == 8) return 0;
				for (i = n; i < 8; i++){
					int hb;
					if (!half)
						hb = (0xff & bytes[pos]) >> 4;
					else
						hb = 0xf & bytes[pos++];
					res = (int) res | (hb << ((i - n) * 4));
					half = !half;
				}
				return res;
			}
		}
	}
}