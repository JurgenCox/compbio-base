using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLibS.Num.Matrix;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num {
	public static class ArrayUtils {
		/// <summary>
		///     Determines the smallest number in the input array.
		/// </summary>
		/// <param name="x">The input array. It may contain NaN and infinity values.</param>
		/// <returns>The minimum.</returns>
		public static double Min(IList<double> x) {
			if (x == null || x.Count == 0) {
				return double.NaN;
			}
			double min = double.MaxValue;
			foreach (double val in x) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		/// <summary>
		///     Determines the smallest number in the input array.
		/// </summary>
		/// <param name="x">The input array. It may contain NaN and infinity values.</param>
		/// <returns>The minimum.</returns>
		public static float Min(IList<float> x) {
			if (x == null || x.Count == 0) {
				return float.NaN;
			}
			float min = float.MaxValue;
			foreach (float val in x) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		/// <summary>
		///     Determines the smallest number in the input array.
		/// </summary>
		/// <param name="x">The input array.</param>
		/// <returns>The minimum.</returns>
		public static byte Min(IList<byte> x) {
			if (x == null || x.Count == 0) {
				return byte.MaxValue;
			}
			byte min = byte.MaxValue;
			foreach (byte val in x) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		public static int MinInd(IList<decimal> x) {
			int n = x.Count;
			decimal min = decimal.MaxValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				decimal val = x[i];
				if (val <= min) {
					min = val;
					ind = i;
				}
			}
			return ind;
		}

		public static int MinInd(IList<double> x) {
			int n = x.Count;
			double min = double.MaxValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				double val = x[i];
				if (val <= min) {
					min = val;
					ind = i;
				}
			}
			return ind;
		}

		public static int MinInd(IList<float> x) {
			int n = x.Count;
			float min = float.MaxValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				float val = x[i];
				if (val <= min) {
					min = val;
					ind = i;
				}
			}
			return ind;
		}

		public static int MinInd(IList<int> x) {
			int n = x.Count;
			int min = int.MaxValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				int val = x[i];
				if (val <= min) {
					min = val;
					ind = i;
				}
			}
			return ind;
		}

		public static int[] MinInds(IList<int> x) {
			int min = Min(x);
			List<int> result = new List<int>();
			for (int i = 0; i < x.Count; i++) {
				if (x[i] == min) {
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public static double Range(IList<double> x) {
			return Max(x) - Min(x);
		}

		/// <summary>
		///     Determines the biggest number in the input array.
		/// </summary>
		/// <param name="x">The input array. It may contain NaN and infinity values.</param>
		/// <returns>The maximum.</returns>
		public static double Max(IList<double> x) {
			if (x == null || x.Count == 0) {
				return double.NaN;
			}
			double max = double.MinValue;
			foreach (double val in x) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		/// <summary>
		///     Determines the biggest number in the input array.
		/// </summary>
		/// <param name="x">The input array. It may contain NaN and infinity values.</param>
		/// <returns>The maximum.</returns>
		public static float Max(IList<float> x) {
			if (x == null || x.Count == 0) {
				return float.NaN;
			}
			float max = float.MinValue;
			foreach (float val in x) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		/// <summary>
		///     Determines the biggest number in the input array.
		/// </summary>
		/// <param name="x">The input array.</param>
		/// <returns>The maximum.</returns>
		public static byte Max(IList<byte> x) {
			if (x == null || x.Count == 0) {
				return byte.MinValue;
			}
			byte max = byte.MinValue;
			foreach (byte val in x) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		/// <summary>
		///     Determines the biggest number in the input array.
		/// </summary>
		/// <param name="x">The input array.</param>
		/// <returns>The maximum.</returns>
		public static long Max(IList<long> x) {
			if (x == null || x.Count == 0) {
				return long.MinValue;
			}
			long max = long.MinValue;
			foreach (long val in x) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		/// <summary>
		///     Determines the biggest number in the input array.
		/// </summary>
		/// <param name="x">The input array.</param>
		/// <returns>The maximum.</returns>
		public static short Max(IList<short> x) {
			if (x == null || x.Count == 0) {
				return short.MinValue;
			}
			short max = short.MinValue;
			foreach (short val in x) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

		public static int MaxInd(IList<float> x) {
			int n = x.Count;
			float max = float.MinValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				float val = x[i];
				if (val >= max) {
					max = val;
					ind = i;
				}
			}
			return ind;
		}

		public static int MaxInd(IList<int> x) {
			int n = x.Count;
			int max = int.MinValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				int val = x[i];
				if (val >= max) {
					max = val;
					ind = i;
				}
			}
			return ind;
		}

		public static int[] MaxInds(IList<int> x) {
			int maxVal = Max(x);
			List<int> inds = new List<int>();
			for (int i = 0; i < x.Count; i++) {
				if (x[i] == maxVal) {
					inds.Add(i);
				}
			}
			return inds.ToArray();
		}

		public static double Mean(IList<int> x) {
			int n = x.Count;
			if (n == 0) {
				return double.NaN;
			}
			double sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum / n;
		}

		public static double Mean(IList<float> x) {
			int n = x.Count;
			if (n == 0) {
				return double.NaN;
			}
			double sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum / n;
		}

		public static double Mean(float[,] x) {
			int n0 = x.GetLength(0);
			int n1 = x.GetLength(1);
			double sum = 0;
			long n = 0;
			for (int i = 0; i < n0; i++) {
				for (int j = 0; j < n1; j++) {
					float v = x[i, j];
					if (!float.IsNaN(v) && !float.IsInfinity(v)) {
						sum += v;
						n++;
					}
				}
			}
			if (n == 0) {
				return double.NaN;
			}
			return sum / n;
		}

		public static double Mean(MatrixIndexer x) {
			int n0 = x.RowCount;
			int n1 = x.ColumnCount;
			double sum = 0;
			long n = 0;
			for (int i = 0; i < n0; i++) {
				for (int j = 0; j < n1; j++) {
					double v = x[i, j];
					if (!double.IsNaN(v) && !double.IsInfinity(v)) {
						sum += v;
						n++;
					}
				}
			}
			if (n == 0) {
				return double.NaN;
			}
			return sum / n;
		}

		public static double Mean(IList<double> x) {
			int n = x.Count;
			if (n == 0) {
				return double.NaN;
			}
			double sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum / n;
		}

		public static double GeometricMean(IList<double> x) {
			int n = x.Count;
			if (n == 0) {
				return double.NaN;
			}
			double prod = 1;
			for (int i = 0; i < n; i++) {
				prod *= x[i];
			}
			return Math.Pow(prod, 1.0 / n);
		}

		public static double Median(IList<double> x) {
			int n = x.Count;
			if (n == 0) {
				return double.NaN;
			}
			int[] o = Order(x);
			if (n % 2 == 1) {
				return x[o[n / 2]];
			}
			return 0.5 * (x[o[n / 2 - 1]] + x[o[n / 2]]);
		}

		public static float Median(IList<float> x) {
			int n = x.Count;
			if (n == 0) {
				return float.NaN;
			}
			int[] o = Order(x);
			if (n % 2 == 1) {
				return x[o[n / 2]];
			}
			return 0.5f * (x[o[n / 2 - 1]] + x[o[n / 2]]);
		}

		public static float Median(IList<int> x) {
			int n = x.Count;
			if (n == 0) {
				return int.MinValue;
			}
			int[] o = Order(x);
			if (n % 2 == 1) {
				return x[o[n / 2]];
			}
			return 0.5f * (x[o[n / 2 - 1]] + x[o[n / 2]]);
		}

		public static double TukeyBiweight(IList<double> x) {
			return TukeyBiweightCalc.TukeyBiweight(x);
		}

		public static double TukeyBiweightSe(IList<double> x) {
			double result = TukeyBiweightCalc.TukeyBiweight(x);
			return TukeyBiweightCalc.TukeyBiweightSe(x, result);
		}

		public static double MostFrequentValue(IList<double> data) {
			int n = data.Count;
			if (n <= 3) {
				return Median(data);
			}
			double[] x;
			double[] y;
			Histogram(data, out x, out y, false, false);
			int ind = MaxInd(y);
			return x[ind];
		}

		public static double MostFrequentValue(IList<float> data) {
			int n = data.Count;
			if (n <= 3) {
				return Median(data);
			}
			double[] x;
			double[] y;
			Histogram(data, out x, out y, false, false);
			int ind = MaxInd(y);
			return x[ind];
		}

		public static int MaxInd(IList<double> x) {
			int n = x.Count;
			double max = double.MinValue;
			int ind = -1;
			for (int i = 0; i < n; i++) {
				double val = x[i];
				if (val > max) {
					max = val;
					ind = i;
				}
			}
			return ind;
		}

		public static double[] ToDoubles(IList<float> floats) {
			double[] result = new double[floats.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = floats[i];
			}
			return result;
		}

		public static double[,] ToDoubles(float[,] floats) {
			double[,] result = new double[floats.GetLength(0), floats.GetLength(1)];
			for (int i = 0; i < result.GetLength(0); i++) {
				for (int j = 0; j < result.GetLength(1); j++) {
					result[i, j] = floats[i, j];
				}
			}
			return result;
		}

		public static float[] ToFloats(BaseVector floats) {
			float[] result = new float[floats.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = (float) floats[i];
			}
			return result;
		}

		public static float[,] ToFloats(MatrixIndexer floats) {
			float[,] result = new float[floats.RowCount, floats.ColumnCount];
			for (int i = 0; i < result.GetLength(0); i++) {
				for (int j = 0; j < result.GetLength(1); j++) {
					result[i, j] = (float) floats[i, j];
				}
			}
			return result;
		}

		public static double[] ToDoubles(BaseVector floats) {
			double[] result = new double[floats.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = floats[i];
			}
			return result;
		}

		public static string[] ToStrings(IList<object> x) {
			string[] result = new string[x.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = x[i].ToString();
			}
			return result;
		}

		public static string[] ToStrings(IList<char> x) {
			string[] result = new string[x.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = "" + x[i];
			}
			return result;
		}

        /// <summary>
        /// Set of all ints in [0, length] not contained in present
        /// </summary>
		public static int[] Complement(IList<int> present, int length) {
			return Complement(new HashSet<int>(present), length);
		}

        /// <summary>
        /// Set of all ints in [0, length] not contained in present
        /// </summary>
		public static int[] Complement(HashSet<int> present, int length) {
			List<int> result = new List<int>();
			for (int i = 0; i < length; i++) {
				if (!present.Contains(i)) {
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public static T[] Concat<T>(IList<T> first, IList<T> second) {
			if (first == null && second == null) {
				return null;
			}
			if (first == null || first.Count == 0) {
				return second?.ToArray();
			}
			if (second == null || second.Count == 0) {
				return first.ToArray();
			}
			T[] result = new T[first.Count + second.Count];
			Array.Copy(first.ToArray(), 0, result, 0, first.Count);
			Array.Copy(second.ToArray(), 0, result, first.Count, second.Count);
			return result;
		}

		public static T[] Concat<T>(IList<T> a, T b) {
			if (a == null) {
				return new[] {b};
			}
			T[] result = new T[a.Count + 1];
			Array.Copy(a.ToArray(), 0, result, 0, a.Count);
			result[a.Count] = b;
			return result;
		}

		public static T[] Concat<T>(IList<T[]> x) {
			int len = 0;
			foreach (T[] t in x) {
				if (t != null) {
					len += t.Length;
				}
			}
			T[] result = new T[len];
			int c = 0;
			foreach (T t1 in x.Where(t => t != null).SelectMany(t => t)) {
				result[c++] = t1;
			}
			return result;
		}

		public static T[] UniqueValuesPreserveOrder<T>(IList<T> array) {
			HashSet<T> taken = new HashSet<T>();
			List<T> result = new List<T>();
			foreach (T ty in array) {
				if (!taken.Contains(ty)) {
					taken.Add(ty);
					result.Add(ty);
				}
			}
			return result.ToArray();
		}

		public static T[] UniqueValuesPreserveOrder<T>(IList<T[]> array) {
			HashSet<T> taken = new HashSet<T>();
			List<T> result = new List<T>();
			foreach (T[] tx in array) {
				foreach (T ty in tx) {
					if (!taken.Contains(ty)) {
						taken.Add(ty);
						result.Add(ty);
					}
				}
			}
			return result.ToArray();
		}

		public static int Min(IList<int> x) {
			if (x == null || x.Count == 0) {
				return int.MaxValue;
			}
			int min = int.MaxValue;
			foreach (int val in x) {
				if (val < min) {
					min = val;
				}
			}
			return min;
		}

		public static int Max(IList<int> x) {
			if (x == null || x.Count == 0) {
				return int.MinValue;
			}
			int max = int.MinValue;
			foreach (int val in x) {
				if (val > max) {
					max = val;
				}
			}
			return max;
		}

        /// <summary>
        /// Create a sublist by indexing with an indicator. If the indicator is longer than the values,
        /// superfluous entries will be ignored.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
	    public static List<T> SubList<T>(this IEnumerable<T> values, IEnumerable<bool> indicator)
	    {
	        var result = new List<T>();
	        using (var valueIter = values.GetEnumerator())
	        using (var indIter = indicator.GetEnumerator())
	        {
	            while (valueIter.MoveNext() && indIter.MoveNext())
	            {
	                if (indIter.Current)
	                {
	                    result.Add(valueIter.Current);
	                }
	            }
                if (valueIter.MoveNext())
                {
                    throw new ArgumentException($"{nameof(indicator)} was exhausted before all values were enumerated.");
                }
	        }
	        return result;
	    }

        /// <summary>
        /// Create a sublist by extracting all provided indices.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
		public static List<T> SubList<T>(this IList<T> list, int[] indices) {
			List<T> result = new List<T>();
			foreach (int index in indices) {
				result.Add(list[index]);
			}
			return result;
		}

		public static int[] ConsecutiveInts(int to) {
			return ConsecutiveInts(0, to);
		}

		public static short[] ConsecutiveShorts(int to) {
			return ConsecutiveShorts(0, to);
		}

		/// <summary>
		///     Create a list of consecutive integers.
		/// </summary>
		/// <param name="from">Start index.</param>
		/// <param name="to">End (exclusive).</param>
		/// <returns>The list of consecutive integers.</returns>
		public static int[] ConsecutiveInts(int from, int to) {
			int len = to - from;
			int[] result = new int[len];
			for (int i = 0; i < len; i++) {
				result[i] = from + i;
			}
			return result;
		}

		/// <summary>
		///     Create a list of consecutive shorts.
		/// </summary>
		/// <param name="from">Start index.</param>
		/// <param name="to">End (exclusive).</param>
		/// <returns>The list of consecutive integers.</returns>
		public static short[] ConsecutiveShorts(int from, int to) {
			int len = to - from;
			short[] result = new short[len];
			for (int i = 0; i < len; i++) {
				result[i] = (short) (from + i);
			}
			return result;
		}

		/// <summary>
		///     Create a list of consecutive longs.
		/// </summary>
		/// <param name="from">Start index.</param>
		/// <param name="to">End (exclusive).</param>
		/// <returns>The list of consecutive longs.</returns>
		public static long[] ConsecutiveLongs(long from, long to) {
			long len = to - from;
			long[] result = new long[len];
			for (int i = 0; i < len; i++) {
				result[i] = from + i;
			}
			return result;
		}

		/// <summary>
		///     Extracts the indexed elements from the given array.
		/// </summary>
		/// <typeparam name="T">Arbitrary type of the array elements.</typeparam>
		/// <param name="array">The input array.</param>
		/// <param name="indices">Indices of the elements to be extracted.</param>
		/// <returns>
		///     An array containing the elements of the input array indexed by the <code>indices</code> array.
		/// </returns>
		public static T[] SubArray<T>(IList<T> array, IList<int> indices) {
			T[] result = new T[indices.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = array[indices[i]];
			}
			return result;
		}

		/// <summary>
		///     Extracts the first <code>len</code> elements from the input array.
		/// </summary>
		/// <typeparam name="T">Arbitrary type of the array elements.</typeparam>
		/// <param name="array">The input array.</param>
		/// <param name="len">Length of the output array.</param>
		/// <returns>
		///     The first <code>len</code> elements of the input array.
		/// </returns>
		public static T[] SubArray<T>(T[] array, int len) {
			if (array.Length <= len) {
				return (T[]) array.Clone();
			}
			T[] result = new T[len];
			Array.Copy(array, 0, result, 0, len);
			return result;
		}

		/// <summary>
		///     Extracts the subarrry from the position <code>startPos</code> on.
		/// </summary>
		/// <typeparam name="T">Arbitrary type of the array elements.</typeparam>
		/// <param name="array">The input array.</param>
		/// <param name="startPos">Start position of the output array.</param>
		/// <returns>
		///     The subarrry from the position <code>startPos</code> on.
		/// </returns>
		public static T[] SubArrayFrom<T>(T[] array, int startPos) {
			if (startPos < 0 || startPos > array.Length) {
				return null;
			}
			int len = array.Length - startPos;
			T[] result = new T[len];
			Array.Copy(array, startPos, result, 0, len);
			return result;
		}

        /// <summary>
        /// Extracts the subarray from the position <code>startIndex</code> to the position <code>stopIndex</code> (exclusive).
        /// </summary>
        /// <typeparam name="T">Arbitrary type of the array elements.</typeparam>
        /// <param name="array">The input array.</param>
        /// <param name="startIndex">Start position of the output array.</param>
        /// <param name="stopIndex">Exclusive stop position of the output array.</param>
        /// <returns>The subarrry.</returns>
        public static T[] SubArray<T>(IList<T> array, int startIndex, int stopIndex) {
			int len = stopIndex - startIndex;
			T[] result = new T[len];
			for (int i = 0; i < len; i++) {
				result[i] = array[startIndex + i];
			}
			return result;
		}

		public static void Histogram(IList<double> data, out double[] x, out double[] y, bool normalized, bool cumulative) {
			data = Remove(data, double.NaN);
			int n = data.Count;
			if (n == 0) {
				x = new double[0];
				y = new double[0];
				return;
			}
			if (n == 1) {
				x = new[] {data[0]};
				y = new double[] {1};
				return;
			}
			double sdev = StandardDeviation(data);
			double iqr = InterQuartileRange(data);
			double h = 1.06 * Math.Min(sdev, iqr / 1.34) / Math.Pow(n, 0.2);
			if (h == 0) {
				h = 1;
			}
			Histogram(data, out x, out y, normalized, cumulative, h);
		}

		public static void Histogram(IList<float> data, out double[] x, out double[] y, bool normalized, bool cumulative) {
			data = Remove(data, float.NaN);
			int n = data.Count;
			if (n == 0) {
				x = new double[0];
				y = new double[0];
				return;
			}
			if (n == 1) {
				x = new double[] {data[0]};
				y = new double[] {1};
				return;
			}
			double sdev = StandardDeviation(data);
			double iqr = InterQuartileRange(data);
			double h = 1.06 * Math.Min(sdev, iqr / 1.34) / Math.Pow(n, 0.2);
			if (h == 0) {
				h = 1;
			}
			Histogram(data, out x, out y, normalized, cumulative, h);
		}

		public static double StandardDeviation(IList<double> x) {
			return Math.Sqrt(Variance(x));
		}

		public static double StandardDeviation(IList<float> x) {
			return Math.Sqrt(Variance(x));
		}

		public static double StandardDeviation(IList<int> x) {
			return Math.Sqrt(Variance(x));
		}

		public static double StandardDeviation(float[,] x) {
			return Math.Sqrt(Variance(x));
		}

		public static double StandardDeviation(MatrixIndexer x) {
			return Math.Sqrt(Variance(x));
		}

		public static double Variance(IList<double> x) {
			if (x.Count < 2) {
				return double.NaN;
			}
			int n = x.Count;
			double mean = Mean(x);
			double var = 0;
			for (int i = 0; i < n; i++) {
				double w = x[i] - mean;
				var += w * w;
			}
			var /= n - 1;
			return var;
		}

		public static double Variance(IList<float> x) {
			if (x.Count < 2) {
				return double.NaN;
			}
			int n = x.Count;
			double mean = Mean(x);
			double var = 0;
			for (int i = 0; i < n; i++) {
				double w = x[i] - mean;
				var += w * w;
			}
			var /= (n - 1);
			return var;
		}

		public static double Variance(float[,] x) {
			int n0 = x.GetLength(0);
			int n1 = x.GetLength(1);
			double mean = Mean(x);
			double var = 0;
			long n = 0;
			for (int i = 0; i < n0; i++) {
				for (int j = 0; j < n1; j++) {
					double w = x[i, j] - mean;
					if (!double.IsNaN(w) && !double.IsInfinity(w)) {
						var += w * w;
						n++;
					}
				}
			}
			if (n < 2) {
				return double.NaN;
			}
			var /= n - 1;
			return var;
		}

		public static double Variance(MatrixIndexer x) {
			int n0 = x.RowCount;
			int n1 = x.ColumnCount;
			double mean = Mean(x);
			double var = 0;
			long n = 0;
			for (int i = 0; i < n0; i++) {
				for (int j = 0; j < n1; j++) {
					double w = x[i, j] - mean;
					if (!double.IsNaN(w) && !double.IsInfinity(w)) {
						var += w * w;
						n++;
					}
				}
			}
			if (n < 2) {
				return double.NaN;
			}
			var /= n - 1;
			return var;
		}

		public static double Variance(IList<int> x) {
			if (x.Count < 2) {
				return double.NaN;
			}
			int n = x.Count;
			double mean = Mean(x);
			double var = 0;
			for (int i = 0; i < n; i++) {
				double w = x[i] - mean;
				var += w * w;
			}
			var /= n - 1;
			return var;
		}

		public static T[] Remove<T>(IList<T> x, T elem) {
			List<T> result = new List<T>();
			foreach (T t in x) {
				if (!elem.Equals(t)) {
					result.Add(t);
				}
			}
			return result.ToArray();
		}

		public static void Histogram(IList<double> data, out double[] x, out double[] y, bool normalized, bool cumulative,
			double h, double min, double max) {
			int n = data.Count;
			double span = max - min;
			int nbins = (int) Math.Max(Math.Round(span / h), 1);
			x = new double[nbins];
			double binsize = span / nbins;
			for (int i = 0; i < nbins; i++) {
				x[i] = min + binsize * (i + 0.5);
			}
			y = new double[nbins];
			try {
				foreach (int index in data.Select(d => (int) Math.Floor((d - min) / binsize))) {
					if (index < 0 || index >= y.Length) {
						continue;
					}
					if (normalized) {
						y[index] += 1.0 / binsize / n;
					} else {
						y[index]++;
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
			}
			if (cumulative) {
				for (int i = 1; i < y.Length; i++) {
					y[i] += y[i - 1];
				}
				for (int i = 0; i < y.Length; i++) {
					y[i] *= binsize;
				}
			}
		}

		public static void Histogram(IList<float> data, out double[] x, out double[] y, bool normalized, bool cumulative,
			double h, double min, double max) {
			int n = data.Count;
			double span = max - min;
			int nbins = (int) Math.Max(Math.Round(span / h), 1);
			x = new double[nbins];
			double binsize = span / nbins;
			for (int i = 0; i < nbins; i++) {
				x[i] = min + binsize * (i + 0.5);
			}
			y = new double[nbins];
			try {
				foreach (int index in data.Select(d => (int) Math.Floor((d - min) / binsize))) {
					if (index < 0 || index >= y.Length) {
						continue;
					}
					if (normalized) {
						y[index] += 1.0 / binsize / n;
					} else {
						y[index]++;
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
			}
			if (cumulative) {
				for (int i = 1; i < y.Length; i++) {
					y[i] += y[i - 1];
				}
				for (int i = 0; i < y.Length; i++) {
					y[i] *= binsize;
				}
			}
		}

		public static void Histogram(IList<double> data, out double[] x, out double[] y, bool normalized, bool cumulative,
			double h) {
			double min;
			double max;
			MinMax(data, out min, out max);
			if (min == max) {
				Histogram(data, out x, out y, normalized, cumulative, 0.1, min - 0.05, max + 0.05);
			}
			double span = max - min;
			int nbins = (int) Math.Max(Math.Round(span / h), 1);
			min -= span / 2.0 / nbins;
			max += span / 2.0 / nbins;
			Histogram(data, out x, out y, normalized, cumulative, h, min, max);
		}

		public static void Histogram(IList<float> data, out double[] x, out double[] y, bool normalized, bool cumulative,
			double h) {
			float min;
			float max;
			MinMax(data, out min, out max);
			if (min == max) {
				Histogram(data, out x, out y, normalized, cumulative, 0.1, min - 0.05, max + 0.05);
			}
			float span = max - min;
			int nbins = (int) Math.Max(Math.Round(span / h), 1);
			min -= span / 2.0f / nbins;
			max += span / 2.0f / nbins;
			Histogram(data, out x, out y, normalized, cumulative, h, min, max);
		}

		public static void MinMax(IList<double> x, out double min, out double max) {
			int n = x.Count;
			if (n == 0) {
				min = double.NaN;
				max = double.NaN;
				return;
			}
			min = double.MaxValue;
			max = double.MinValue;
			for (int i = 0; i < n; i++) {
				double val = x[i];
				if (double.IsInfinity(val)) {
					continue;
				}
				if (val < min) {
					min = val;
				}
				if (val > max) {
					max = val;
				}
			}
			if (min == double.MaxValue) {
				min = double.NaN;
				max = double.NaN;
			}
		}

		public static void MinMax(IList<long> x, out long min, out long max) {
			int n = x.Count;
			min = long.MaxValue;
			max = long.MinValue;
			for (int i = 0; i < n; i++) {
				long val = x[i];
				if (val < min) {
					min = val;
				}
				if (val > max) {
					max = val;
				}
			}
		}

		public static void MinMax(IList<float> x, out float min, out float max) {
			int n = x.Count;
			if (n == 0) {
				min = float.NaN;
				max = float.NaN;
				return;
			}
			min = float.MaxValue;
			max = float.MinValue;
			for (int i = 0; i < n; i++) {
				float val = x[i];
				if (val < min) {
					min = val;
				}
				if (val > max) {
					max = val;
				}
			}
			if (min == float.MaxValue) {
				min = float.NaN;
				max = float.NaN;
			}
		}

		public static void MinMax(IList<int> x, out int min, out int max) {
			int n = x.Count;
			min = int.MaxValue;
			max = int.MinValue;
			for (int i = 0; i < n; i++) {
				int val = x[i];
				if (val < min) {
					min = val;
				}
				if (val > max) {
					max = val;
				}
			}
		}

		/// <summary>
		///     For the sake of simplicity do all sorting tasks always and ever with these method.
		/// </summary>
		/// <typeparam name="T">
		///     The array type has to inherit IComparable in order to have a
		///     criterion to sort on.
		/// </typeparam>
		/// <param name="x">The input data to be sorted.</param>
		/// <returns>
		///     An array of indices such that if x is accessed with those indices the values are in
		///     ascending (or to be more precise, non-decending) order.
		/// </returns>
		public static int[] Order<T>(IList<T> x) where T : IComparable<T> {
			if (x == null) {
				return null;
			}
			int[] order = ConsecutiveInts(x.Count);
			const int low = 0;
			int high = order.Length - 1;
			int[] dummy = new int[order.Length];
			Array.Copy(order, dummy, order.Length);
			SortImpl(x, order, dummy, low, high);
			return order;
		}

		/// <summary>
		///     For the sake of simplicity do all sorting tasks always and ever with these method.
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T0"></typeparam>
		/// <param name="x">The input data to be sorted.</param>
		/// <param name="y">If the same, use this data to sort.</param>
		/// <returns>
		///     An array of indices such that if x is accessed with those indices the values are in
		///     ascending (or to be more precise, non-decending) order.
		/// </returns>
		public static int[] Order<T0, T1>(IList<T0> x, IList<T1> y)
			where T0 : IComparable<T0>
			where T1 : IComparable<T1> {
			if (x == null || y == null || x.Count != y.Count) {
				return null;
			}
			int[] order = ConsecutiveInts(x.Count);
			const int low = 0;
			int high = order.Length - 1;
			int[] dummy = new int[order.Length];
			Array.Copy(order, dummy, order.Length);
			SortImpl(x, y, order, dummy, low, high);
			return order;
		}

		/// <summary>
		///     For the sake of simplicity do all sorting tasks always and ever with these method.
		/// </summary>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T0"></typeparam>
		/// <param name="x">The input data to be sorted.</param>
		/// <param name="y">If the same, use this data to sort.</param>
		/// <returns>
		///     An array of indices such that if x is accessed with those indices the values are in
		///     ascending (or to be more precise, non-decending) order.
		/// </returns>
		public static int[] Order<T0, T1, T2>(IList<T0> x, IList<T1> y, IList<T2> z)
			where T0 : IComparable<T0>
			where T1 : IComparable<T1>
			where T2 : IComparable<T2> {
			if (x == null || y == null || z == null || x.Count != y.Count || x.Count != z.Count) {
				return null;
			}
			int[] order = ConsecutiveInts(x.Count);
			const int low = 0;
			int high = order.Length - 1;
			int[] dummy = new int[order.Length];
			Array.Copy(order, dummy, order.Length);
			SortImpl(x, y, z, order, dummy, low, high);
			return order;
		}

		/// <summary>
		///     For the sake of simplicity do all sorting tasks always and ever with these method.
		/// </summary>
		/// <param name="x">The input data to be sorted.</param>
		/// <returns>
		///     An array of indices such that if x is accessed with those indices the values are in
		///     ascending (or to be more precise, non-decending) order.
		/// </returns>
		public static int[] Order(BaseVector x) {
			if (x == null) {
				return null;
			}
			int[] order = ConsecutiveInts(x.Length);
			const int low = 0;
			int high = order.Length - 1;
			int[] dummy = new int[order.Length];
			Array.Copy(order, dummy, order.Length);
			SortImpl(x, order, dummy, low, high);
			return order;
		}

		/// <summary>
		///     Private class that implements the sorting algorithm.
		/// </summary>
		private static void SortImpl<T>(IList<T> data, int[] orderDest, int[] orderSrc, int low, int high)
			where T : IComparable<T> {
			if (low >= high) {
				return;
			}
			int mid = low + ((high - low) >> 1);
			SortImpl(data, orderSrc, orderDest, low, mid);
			SortImpl(data, orderSrc, orderDest, mid + 1, high);
			if (data[orderSrc[mid]].CompareTo(data[orderSrc[mid + 1]]) <= 0) {
				Array.Copy(orderSrc, low, orderDest, low, high - low + 1);
				return;
			}
			if (data[orderSrc[low]].CompareTo(data[orderSrc[high]]) > 0) {
				int m = (high - low) % 2 == 0 ? mid : mid + 1;
				Array.Copy(orderSrc, low, orderDest, m, mid - low + 1);
				Array.Copy(orderSrc, mid + 1, orderDest, low, high - mid);
				return;
			}
			int tLow = low;
			int tHigh = mid + 1;
			for (int i = low; i <= high; i++) {
				if ((tLow <= mid) && ((tHigh > high) || (data[orderSrc[tLow]]).CompareTo(data[orderSrc[tHigh]]) <= 0)) {
					orderDest[i] = orderSrc[tLow++];
				} else {
					orderDest[i] = orderSrc[tHigh++];
				}
			}
		}

		/// <summary>
		///     Private class that implements the sorting algorithm.
		/// </summary>
		private static void SortImpl<T0, T1>(IList<T0> first, IList<T1> second, int[] orderDest, int[] orderSrc, int low, int high)
			where T0 : IComparable<T0>
			where T1 : IComparable<T1> {
			if (low >= high) {
				return;
			}
			int mid = low + ((high - low) >> 1);
			SortImpl(first, second, orderSrc, orderDest, low, mid);
			SortImpl(first, second, orderSrc, orderDest, mid + 1, high);
			if (first[orderSrc[mid]].CompareTo(first[orderSrc[mid + 1]]) < 0) {
				Array.Copy(orderSrc, low, orderDest, low, high - low + 1);
				return;
			}
			if (first[orderSrc[low]].CompareTo(first[orderSrc[high]]) > 0) {
				int m = (high - low) % 2 == 0 ? mid : mid + 1;
				Array.Copy(orderSrc, low, orderDest, m, mid - low + 1);
				Array.Copy(orderSrc, mid + 1, orderDest, low, high - mid);
				return;
			}
			int tLow = low;
			int tHigh = mid + 1;
			for (int i = low; i <= high; i++) {
				if (tLow > mid) {
					orderDest[i] = orderSrc[tHigh++];
				} else if (tHigh > high) {
					orderDest[i] = orderSrc[tLow++];
				} else {
					switch (first[orderSrc[tLow]].CompareTo(first[orderSrc[tHigh]])) {
						case -1:
						orderDest[i] = orderSrc[tLow++];
						break;
						case 0:
						orderDest[i] = second[orderSrc[tLow]].CompareTo(second[orderSrc[tHigh]]) <= 0 ?
							orderSrc[tLow++] :
							orderSrc[tHigh++];
						break;
						case 1:
						orderDest[i] = orderSrc[tHigh++];
						break;
					}
				}
			}
		}

		/// <summary>
		///     Private class that implements the sorting algorithm.
		/// </summary>
		private static void SortImpl<T0, T1, T2>(IList<T0> first, IList<T1> second, IList<T2> third, int[] orderDest, int[] orderSrc, int low, int high)
			where T0 : IComparable<T0>
			where T1 : IComparable<T1>
			where T2 : IComparable<T2> {
			if (low >= high) {
				return;
			}
			int mid = low + ((high - low) >> 1);
			SortImpl(first, second, third, orderSrc, orderDest, low, mid);
			SortImpl(first, second, third, orderSrc, orderDest, mid + 1, high);
			if (first[orderSrc[mid]].CompareTo(first[orderSrc[mid + 1]]) < 0) {
				Array.Copy(orderSrc, low, orderDest, low, high - low + 1);
				return;
			}
			if (first[orderSrc[low]].CompareTo(first[orderSrc[high]]) > 0) {
				int m = (high - low) % 2 == 0 ? mid : mid + 1;
				Array.Copy(orderSrc, low, orderDest, m, mid - low + 1);
				Array.Copy(orderSrc, mid + 1, orderDest, low, high - mid);
				return;
			}
			int tLow = low;
			int tHigh = mid + 1;
			for (int i = low; i <= high; i++) {
				if (tLow > mid) {
					orderDest[i] = orderSrc[tHigh++];
				} else if (tHigh > high) {
					orderDest[i] = orderSrc[tLow++];
				} else {
					switch (first[orderSrc[tLow]].CompareTo(first[orderSrc[tHigh]])) {
						case -1:
						orderDest[i] = orderSrc[tLow++];
						break;
						case 0:
						switch (second[orderSrc[tLow]].CompareTo(second[orderSrc[tHigh]])) {
							case -1:
							orderDest[i] = orderSrc[tLow++];
							break;
							case 0:
							orderDest[i] = third[orderSrc[tLow]].CompareTo(third[orderSrc[tHigh]]) <= 0 ?
								orderSrc[tLow++] :
								orderSrc[tHigh++];
							break;
							case 1:
							orderDest[i] = orderSrc[tHigh++];
							break;
						}
						break;
						case 1:
						orderDest[i] = orderSrc[tHigh++];
						break;
					}
				}
			}
		}

		/// <summary>
		///     Private class that implements the sorting algorithm.
		/// </summary>
		private static void SortImpl(BaseVector data, int[] orderDest, int[] orderSrc, int low, int high) {
			if (low >= high) {
				return;
			}
			int mid = low + ((high - low) >> 1);
			SortImpl(data, orderSrc, orderDest, low, mid);
			SortImpl(data, orderSrc, orderDest, mid + 1, high);
			if (data[orderSrc[mid]].CompareTo(data[orderSrc[mid + 1]]) <= 0) {
				Array.Copy(orderSrc, low, orderDest, low, high - low + 1);
				return;
			}
			if (data[orderSrc[low]].CompareTo(data[orderSrc[high]]) > 0) {
				int m = (high - low) % 2 == 0 ? mid : mid + 1;
				Array.Copy(orderSrc, low, orderDest, m, mid - low + 1);
				Array.Copy(orderSrc, mid + 1, orderDest, low, high - mid);
				return;
			}
			int tLow = low;
			int tHigh = mid + 1;
			for (int i = low; i <= high; i++) {
				if ((tLow <= mid) && ((tHigh > high) || (data[orderSrc[tLow]]).CompareTo(data[orderSrc[tHigh]]) <= 0)) {
					orderDest[i] = orderSrc[tLow++];
				} else {
					orderDest[i] = orderSrc[tHigh++];
				}
			}
		}

		public static float[] Quantiles(IList<float> x, double[] qs) {
			int n = x.Count;
			float[] result = new float[qs.Length];
			if (n == 0) {
				for (int i = 0; i < result.Length; i++) {
					result[i] = float.NaN;
				}
				return result;
			}
			int[] o = Order(x);
			for (int i = 0; i < result.Length; i++) {
				double indD = (n - 1) * qs[i];
				indD = Math.Max(0, indD);
				indD = Math.Min(indD, n - 1);
				int rind = (int) Math.Round(indD);
				if (Math.Abs(indD - rind) < 1e-6 || rind == 0 || rind == n - 1) {
					result[i] = x[o[rind]];
				} else {
					int floor = (int) Math.Floor(indD);
					int ceil = floor + 1;
					float x1 = x[o[floor]];
					float x2 = x[o[ceil]];
					double w1 = ceil - indD;
					double w2 = indD - floor;
					result[i] = (float) (w1 * x1 + w2 * x2);
				}
			}
			return result;
		}

		public static double[] Quantiles(IList<double> x, double[] qs) {
			int n = x.Count;
			double[] result = new double[qs.Length];
			if (n == 0) {
				for (int i = 0; i < result.Length; i++) {
					result[i] = double.NaN;
				}
				return result;
			}
			int[] o = Order(x);
			for (int i = 0; i < result.Length; i++) {
				double indD = (n - 1) * qs[i];
				indD = Math.Max(0, indD);
				indD = Math.Min(indD, n - 1);
				int rind = (int) Math.Round(indD);
				if (Math.Abs(indD - rind) < 1e-6 || rind == 0 || rind == n - 1) {
					result[i] = x[o[rind]];
				} else {
					int floor = (int) Math.Floor(indD);
					int ceil = floor + 1;
					double x1 = x[o[floor]];
					double x2 = x[o[ceil]];
					double w1 = ceil - indD;
					double w2 = indD - floor;
					result[i] = (w1 * x1 + w2 * x2);
				}
			}
			return result;
		}

		public static double[] Rank<T>(IList<T> data) where T : IComparable<T> {
			return Rank(data, true);
		}

		/// <summary>
		///     Calculates the rank of the given data. The lowest rank value is 0.
		///     The input array type must inherit IComparable.
		/// </summary>
		public static double[] Rank<T>(IList<T> data, bool tieCorrection) where T : IComparable<T> {
			int n = data.Count;
			double[] rank = new double[n];
			int[] index = Order(data);
			for (int j = 0; j < n; j++) {
				rank[index[j]] = j;
			}
			/* Fix for equal ranks */
			if (tieCorrection) {
				int i = 0;
				while (i < n) {
					T value = data[index[i]];
					int j = i + 1;
					while (j < n && data[index[j]].Equals(value)) {
						j++;
					}
					int m = j - i;
					double v1 = rank[index[i]] + (m - 1) / 2.0;
					for (j = i; j < i + m; j++) {
						rank[index[j]] = v1;
					}
					i += m;
				}
			}
			return rank;
		}

		public static double[] Rank(BaseVector data) {
			return Rank(data, true);
		}

		/// <summary>
		///     Calculates the rank of the given data. The lowest rank value is 0.
		///     The input array type must inherit IComparable.
		/// </summary>
		public static double[] Rank(BaseVector data, bool tieCorrection) {
			int n = data.Length;
			double[] rank = new double[n];
			int[] index = Order(data);
			for (int j = 0; j < n; j++) {
				rank[index[j]] = j;
			}
			/* Fix for equal ranks */
			if (tieCorrection) {
				int i = 0;
				while (i < n) {
					double value = data[index[i]];
					int j = i + 1;
					while (j < n && data[index[j]] == value) {
						j++;
					}
					int m = j - i;
					double v1 = rank[index[i]] + (m - 1) / 2.0;
					for (j = i; j < i + m; j++) {
						rank[index[j]] = v1;
					}
					i += m;
				}
			}
			return rank;
		}

		public static float[] RankF<T>(IList<T> data) where T : IComparable<T> {
			return RankF(data, true);
		}

		/// <summary>
		///     Calculates the rank of the given data. The lowest rank value is 0.
		///     The input array type must inherit IComparable.
		/// </summary>
		public static float[] RankF<T>(IList<T> data, bool tieCorrection) where T : IComparable<T> {
			int n = data.Count;
			float[] rank = new float[n];
			int[] index = Order(data);
			for (int j = 0; j < n; j++) {
				rank[index[j]] = j;
			}
			/* Fix for equal ranks */
			if (tieCorrection) {
				int i = 0;
				while (i < n) {
					T value = data[index[i]];
					int j = i + 1;
					while (j < n && data[index[j]].Equals(value)) {
						j++;
					}
					int m = j - i;
					double v1 = rank[index[i]] + (m - 1) / 2.0;
					for (j = i; j < i + m; j++) {
						rank[index[j]] = (float) v1;
					}
					i += m;
				}
			}
			return rank;
		}

		public static T[] RemoveAtIndex<T>(IList<T> x, int index) {
			T[] result = new T[x.Count - 1];
			for (int i = 0; i < index; i++) {
				result[i] = x[i];
			}
			for (int i = index + 1; i < x.Count; i++) {
				result[i - 1] = x[i];
			}
			return result;
		}

		public static T[] Remove<T>(IList<T> x, IList<int> indices) {
			List<T> result = new List<T>();
			for (int i = 0; i < x.Count; i++) {
				if (!indices.Contains(i)) {
					result.Add(x[i]);
				}
			}
			return result.ToArray();
		}

		public static T[] Insert<T>(IList<T> x, T value, int index) {
			T[] result = new T[x.Count + 1];
			for (int i = 0; i < index; i++) {
				result[i] = x[i];
			}
			result[index] = value;
			for (int i = index; i < x.Count; i++) {
				result[i + 1] = x[i];
			}
			return result;
		}

		public static double MeanAndStddev(IList<double> vals, out double stddev) {
			return MeanAndStddev(vals, out stddev, false);
		}

		public static double MeanAndStddev(IList<double> vals, out double stddev, bool useMedian) {
			double mean = 0;
			int validCount = 0;
			if (useMedian) {
				int[] valids = GetValidInds(vals);
				validCount = valids.Length;
				mean = Median(SubArray(vals, valids));
			} else {
				foreach (double t in vals.Where(t => !double.IsNaN(t) && !double.IsInfinity(t))) {
					mean += t;
					validCount++;
				}
				if (validCount == 0) {
					stddev = double.NaN;
					return double.NaN;
				}
				if (validCount == 1) {
					stddev = double.NaN;
					return mean;
				}
				mean /= validCount;
			}
			stddev = 0;
			foreach (double v in vals) {
				if (!double.IsNaN(v) && !double.IsInfinity(v)) {
					double x = v - mean;
					stddev += x * x;
				}
			}
			stddev /= (validCount - 1.0);
			stddev = Math.Sqrt(stddev);
			return mean;
		}

		public static double MeanAndStddev(IList<float> vals, out double stddev) {
			return MeanAndStddev(vals, out stddev, false);
		}

		public static double MeanAndStddev(IList<float> vals, out double stddev, bool useMedian) {
			int validCount;
			return MeanAndStddev(vals, out stddev, out validCount, useMedian);
		}

		public static double MeanAndStddev(IList<float> vals, out double stddev, out int validCount) {
			return MeanAndStddev(vals, out stddev, out validCount, false);
		}

		public static double MeanAndStddev(IList<float> vals, out double stddev, out int validCount, bool useMedian) {
			double mean = 0;
			validCount = 0;
			if (useMedian) {
				int[] valids = GetValidInds(vals);
				validCount = valids.Length;
				mean = Median(SubArray(vals, valids));
			} else {
				foreach (float t in vals.Where(t => !float.IsNaN(t) && !float.IsInfinity(t))) {
					mean += t;
					validCount++;
				}
				if (validCount == 0) {
					stddev = double.NaN;
					return double.NaN;
				}
				if (validCount == 1) {
					stddev = double.NaN;
					return mean;
				}
				mean /= validCount;
			}
			stddev = 0;
			foreach (float v in vals) {
				if (!float.IsNaN(v) && !float.IsInfinity(v)) {
					double x = v - mean;
					stddev += x * x;
				}
			}
			stddev /= (validCount - 1.0);
			stddev = Math.Sqrt(stddev);
			return mean;
		}

		/// <summary>
		///     Returns an array containing all values from the given dictionary for which there is a key present in the key array.
		/// </summary>
		public static V[] GetValues<K, V>(IEnumerable<K> keys, IDictionary<K, V> dict) {
			List<V> result = new List<V>();
			foreach (K key in keys) {
				if (dict.ContainsKey(key)) {
					result.Add(dict[key]);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		///     Returns the index of the first element in an array that equals a given object.
		/// </summary>
		/// <param name="p">Array to be searched.</param>
		/// <param name="q">Element to be found.</param>
		/// <returns>Index of first occurence. -1 otherwise.</returns>
		public static int IndexOf<T>(IList<T> p, T q) {
			for (int i = 0; i < p.Count; i++) {
				if (p[i].Equals(q)) {
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		///     Returns all indices of the array elements that equal a given object.
		/// </summary>
		/// <param name="p">Array to be searched.</param>
		/// <param name="q">Element to be found.</param>
		/// <returns>All indices of occurence.</returns>
		public static int[] IndicesOf<T>(IList<T> p, T q) {
			List<int> result = new List<int>();
			for (int i = 0; i < p.Count; i++) {
				if (p[i] != null && p[i].Equals(q)) {
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public static T[] ToArray<T>(HashSet<T> set) {
			T[] array = new T[set.Count];
			set.CopyTo(array);
			return array;
		}

		public static double[] ToDoubles(IList<int> ints) {
			double[] result = new double[ints.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = ints[i];
			}
			return result;
		}

		public static float[] ToFloats(IList<double> doubles) {
			float[] result = new float[doubles.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = (float) doubles[i];
			}
			return result;
		}

		public static int[] ToInts(IList<float> doubles) {
			int[] result = new int[doubles.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = (int) Math.Round(doubles[i]);
			}
			return result;
		}

		public static int[] ToInts(IList<double> doubles) {
			int[] result = new int[doubles.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = (int) Math.Round(doubles[i]);
			}
			return result;
		}

		public static int[] ToInts(IList<ushort> doubles) {
			int[] result = new int[doubles.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = doubles[i];
			}
			return result;
		}

		public static float[] ToFloats(IList<int> doubles) {
			float[] result = new float[doubles.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = doubles[i];
			}
			return result;
		}

		public static float[] ToFloats(IList<bool> doubles) {
			float[] result = new float[doubles.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = doubles[i] ? 1 : 0;
			}
			return result;
		}

		/// <summary>
		///     In case the item is not in the array or occurs only once the conventional binary search output index
		///     is returned in <code>minInd</code> and <code>maxInd</code>. If the item occurs more than once,
		///     <code>minInd</code> and <code>maxInd</code> indicate the
		///     inclusive index interval for which the item occurs in the <code>array</code>.
		/// </summary>
		/// <typeparam name="T">
		///     Needs to be <code>IComparable</code>
		/// </typeparam>
		/// <param name="array">Sorted array, potentially with ties</param>
		/// <param name="value">Value to be searched for.</param>
		/// <param name="minInd">
		///     Outputs the minimum array index where <code>value</code> is found.
		/// </param>
		/// <param name="maxInd">
		///     Outputs the inclusive maximum array index where <code>value</code> is found.
		/// </param>
		public static void BinarySearchWithTies<T>(T[] array, T value, out int minInd, out int maxInd)
			where T : IComparable<T> {
			int ind = Array.BinarySearch(array, value);
			if (ind < 0) {
				minInd = ind;
				maxInd = ind;
				return;
			}
			minInd = ind;
			while (minInd > 0 && array[minInd - 1].Equals(array[ind])) {
				minInd--;
			}
			maxInd = ind;
			while (maxInd < array.Length - 1 && array[maxInd + 1].Equals(array[ind])) {
				maxInd++;
			}
		}

        /// <summary>
        /// Index of last element &gt; value in sorted array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int FloorIndex<T>(T[] array, T value) where T : IComparable<T> {
			int n = array.Length;
			if (n == 0) {
				return -1;
			}
			if (value.CompareTo(array[n - 1]) > 0) {
				return n - 1;
			}
			if (value.CompareTo(array[0]) < 0) {
				return -1;
			}
			int a = Array.BinarySearch(array, value);
			if (a >= 0) {
				while (a < array.Length - 1 && array[a + 1].Equals(array[a])) {
					a++;
				}
				return a;
			}
			return -2 - a;
		}

        /// <summary>
        /// Index of first element &lt; value in sorted array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int CeilIndex<T>(T[] array, T value) where T : IComparable<T> {
			int n = array.Length;
			if (n == 0) {
				return -1;
			}
			if (value.CompareTo(array[n - 1]) > 0) {
				return -1;
			}
			if (value.CompareTo(array[0]) < 0) {
				return 0;
			}
			int a = Array.BinarySearch(array, value);
			if (a >= 0) {
				while (a > 0 && array[a - 1].Equals(array[a])) {
					a--;
				}
				return a;
			}
			return -1 - a;
		}

        /// <summary>
        /// Index of last element &gt; value in sorted array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int FloorIndex<T>(List<T> array, T value) where T : IComparable<T> {
			int n = array.Count;
			if (n == 0) {
				return -1;
			}
			if (value.CompareTo(array[n - 1]) > 0) {
				return n - 1;
			}
			if (value.CompareTo(array[0]) < 0) {
				return -1;
			}
			int a = array.BinarySearch(value);
			if (a >= 0) {
				while (a < array.Count - 1 && array[a + 1].Equals(array[a])) {
					a++;
				}
				return a;
			}
			return -2 - a;
		}

        /// <summary>
        /// Index of first element &lt; value in sorted array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int CeilIndex<T>(List<T> array, T value) where T : IComparable<T> {
			int n = array.Count;
			if (n == 0) {
				return -1;
			}
			if (value.CompareTo(array[n - 1]) > 0) {
				return -1;
			}
			if (value.CompareTo(array[0]) < 0) {
				return 0;
			}
			int a = array.BinarySearch(value);
			if (a >= 0) {
				while (a > 0 && array[a - 1].Equals(array[a])) {
					a--;
				}
				return a;
			}
			return -1 - a;
		}

        /// <summary>
        /// Index of last element &gt; value in sorted array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ilist"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int FloorIndex<T>(IList<T> ilist, T value) where T : IComparable<T> {
            if (ilist is T[] array)
            {
                return FloorIndex(array, value);
            }
            if (ilist is List<T> list)
            {
                return FloorIndex(list, value);
            }
            return FloorIndex(ilist.ToArray(), value);
        }

        /// <summary>
        /// Index of first element &lt; value in sorted array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ilist"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public static int CeilIndex<T>(IList<T> ilist, T value) where T : IComparable<T> {
            if (ilist is T[] array)
            {
                return CeilIndex(array, value);
            }
            if (ilist is List<T> list)
            {
                return CeilIndex(list, value);
            }
            return CeilIndex(ilist.ToArray(), value);
		}

		/// <summary>
		/// Reverses the order of the values in the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="x"></param>
		public static void Revert<T>(IList<T> x) {
			int n = x.Count;
			for (int i = 0; i < n / 2; i++) {
				T tmp = x[i];
				x[i] = x[n - i - 1];
				x[n - i - 1] = tmp;
			}
		}

		/// <summary>
		///     Compares the two given arrays and returns the equality of their contents. First the lengths of the
		///     given arrays are compared and then their contents.
		/// </summary>
		/// <param name="a">The first array.</param>
		/// <param name="b">The second array.</param>
		/// <returns>True when their contents are equal, false otherwise.</returns>
		public static bool EqualArrays<T>(IList<T> a, IList<T> b) {
			if (a.Count != b.Count) {
				return false;
			}
			for (int i = 0; i < a.Count; i++) {
				if (!a[i].Equals(b[i])) {
					return false;
				}
			}
			return true;
		}

		public static bool EqualArraysOfArrays<T>(IList<T[]> a, IList<T[]> b) {
			if (a.Count != b.Count) {
				return false;
			}
			for (int i = 0; i < a.Count; i++) {
				if (!EqualArrays(a[i], b[i])) {
					return false;
				}
			}
			return true;
		}

		public static T[] UniqueValues<T>(IList<T> array) where T : IComparable<T> {
			if (array.Count < 1) {
				return new T[0];
			}
			T[] tmp = new T[array.Count];
			for (int i = 0; i < tmp.Length; i++) {
				tmp[i] = array[i];
			}
			Array.Sort(tmp);
			int counter = 1;
			T lastVal = tmp[0];
			for (int i = 1; i < tmp.Length; i++) {
				if (!lastVal.Equals(tmp[i])) {
					lastVal = tmp[i];
					tmp[counter++] = lastVal;
				}
			}
			Array.Resize(ref tmp, counter);
			return tmp;
		}

		public static T[] UniqueValuesSorted<T>(IList<T[]> array) {
			HashSet<T> hs = new HashSet<T>();
			foreach (T ty in from tx in array from ty in tx where !hs.Contains(ty) select ty) {
				hs.Add(ty);
			}
			T[] x = ToArray(hs);
			Array.Sort(x);
			return x;
		}

		public static T[] UniqueValuesSorted<T>(IList<T> array) {
			HashSet<T> hs = new HashSet<T>();
			foreach (T ty in from ty in array where !hs.Contains(ty) select ty) {
				hs.Add(ty);
			}
			T[] x = ToArray(hs);
			Array.Sort(x);
			return x;
		}

		public static IList<T> UniqueValuesAndCounts<T>(IList<T> array, out IList<int> counters) where T : IComparable<T> {
			T[] unique = UniqueValues(array);
			counters = new List<int>();
			foreach (T u in unique) {
				counters.Add(array.Count(x => x.Equals(u)));
			}
			return unique;
		}

		public static T[] UniqueValuesAndCounts<T>(T[] array, out int[] counters) where T : IComparable<T> {
			if (array.Length == 0) {
				counters = new int[0];
				return new T[0];
			}
			if (array.Length == 1) {
				counters = new[] {1};
				return array;
			}
			T[] sorted = (T[]) array.Clone();
			counters = new int[array.Length];
			Array.Sort(sorted);
			int counter = 1;
			T lastVal = sorted[0];
			counters[0] = 1;
			for (int i = 1; i < sorted.Length; i++) {
				if (!lastVal.Equals(sorted[i])) {
					lastVal = sorted[i];
					sorted[counter++] = lastVal;
				}
				counters[counter - 1]++;
			}
			Array.Resize(ref counters, counter);
			Array.Resize(ref sorted, counter);
			return sorted;
		}

		public static double Sum(IList<double> x) {
			int n = x.Count;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum;
		}

		public static double Product(IList<double> x) {
			int n = x.Count;
			double prod = 1;
			for (int i = 0; i < n; i++) {
				prod *= x[i];
			}
			return prod;
		}

		public static double Sum(IList<float> x) {
			int n = x.Count;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum;
		}

		public static long Sum(IList<int> x) {
			int n = x.Count;
			long sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum;
		}

		public static long Sum(IList<long> x) {
			int n = x.Count;
			long sum = 0;
			for (int i = 0; i < n; i++) {
				sum += x[i];
			}
			return sum;
		}

		public static int Sum(IList<bool> x) {
			int n = x.Count;
			int sum = 0;
			for (int i = 0; i < n; i++) {
				if (x[i]) {
					sum++;
				}
			}
			return sum;
		}

		public static double Cosine(IList<double> x, IList<double> y) {
			double xx = 0;
			double yy = 0;
			double xy = 0;
			for (int i = 0; i < x.Count; i++) {
				double wx = x[i];
				double wy = y[i];
				xx += wx * wx;
				yy += wy * wy;
				xy += wx * wy;
			}
			double denom = xx * yy;
			if (denom > 0.0) {
				return xy / Math.Sqrt(denom);
			}
			return 0f;
		}

		public static float Cosine(IList<float> x, IList<float> y) {
			float xx = 0;
			float yy = 0;
			float xy = 0;
			for (int i = 0; i < x.Count; i++) {
				float wx = x[i];
				float wy = y[i];
				xx += wx * wx;
				yy += wy * wy;
				xy += wx * wy;
			}
			double denom = xx * yy;
			if (denom > 0.0) {
				return (float) (xy / Math.Sqrt(denom));
			}
			return 0f;
		}

		public static T[] FillArray<T>(Func<int, T> map, int len) {
			T[] array = new T[len];
			for (int i = 0; i < len; i++) {
				array[i] = map(i);
			}
			return array;
		}

		public static T[] FillArray<T>(T item, int len) {
			T[] array = new T[len];
			for (int i = 0; i < len; i++) {
				array[i] = item;
			}
			return array;
		}

		public static int ClosestIndex(double[] array, double value) {
			if (double.IsNaN(value)) {
				return -1;
			}
			int n = array.Length;
			if (n == 0) {
				return -1;
			}
			if (n == 1) {
				return 0;
			}
			if (value > array[n - 1]) {
				return n - 1;
			}
			if (value < array[0]) {
				return 0;
			}
			int a = Array.BinarySearch(array, value);
			if (a >= 0) {
				return a;
			}
			int b = -1 - a;
			if (b == 0) {
				return b;
			}
			if (b >= n) { //can only happen if the array contains NaNs
				return -1;
			}
			if (array[b] < 2 * value - array[b - 1]) {
				return b;
			}
			return b - 1;
		}

		public static int ClosestIndex(float[] array, float value) {
			if (float.IsNaN(value)) {
				return -1;
			}
			int n = array.Length;
			if (n == 0) {
				return -1;
			}
			if (n == 1) {
				return 0;
			}
			if (value > array[n - 1]) {
				return n - 1;
			}
			if (value < array[0]) {
				return 0;
			}
			int a = Array.BinarySearch(array, value);
			if (a >= 0) {
				return a;
			}
			int b = -1 - a;
			if (b == 0) {
				return b;
			}
			if (b >= n) { //can only happen if the array contains NaNs
				return -1;
			}
			if (array[b] < 2 * value - array[b - 1]) {
				return b;
			}
			return b - 1;
		}

		public static int ClosestIndex(int[] array, int value) {
			int n = array.Length;
			if (n == 0) {
				return -1;
			}
			if (n == 1) {
				return 0;
			}
			if (value > array[n - 1]) {
				return n - 1;
			}
			if (value < array[0]) {
				return 0;
			}
			int a = Array.BinarySearch(array, value);
			if (a >= 0) {
				return a;
			}
			int b = -1 - a;
			if (b == 0) {
				return b;
			}
			if (array[b] < 2 * value - array[b - 1]) {
				return b;
			}
			return b - 1;
		}

		public static int GetArrayOfArrayHashCode<T>(T[][] array) {
			int hash = 397;
			foreach (T[] elem in array) {
				hash = (hash * 397) ^ GetArrayHashCode(elem);
			}
			return hash;
		}

		public static int GetArrayHashCode<T>(T[] array) {
			int hash = 397;
			foreach (T elem in array) {
				hash = (hash * 397) ^ elem.GetHashCode();
			}
			return hash;
		}

		public static float[,] ToFloats(double[,] x) {
			float[,] result = new float[x.GetLength(0), x.GetLength(1)];
			for (int i = 0; i < x.GetLength(0); i++) {
				for (int j = 0; j < x.GetLength(1); j++) {
					result[i, j] = (float) x[i, j];
				}
			}
			return result;
		}

		public static double[] ExtractValidValues(IList<double> x) {
			List<double> result = new List<double>();
			foreach (double y in x) {
				if (!double.IsNaN(y) && !double.IsInfinity(y)) {
					result.Add(y);
				}
			}
			return result.ToArray();
		}

		public static int[] GetValidInds(IList<double> x) {
			List<int> result = new List<int>();
			for (int i = 0; i < x.Count; i++) {
				double y = x[i];
				if (!double.IsNaN(y) && !double.IsInfinity(y)) {
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public static int[] GetValidInds(IList<float> x) {
			List<int> result = new List<int>();
			for (int i = 0; i < x.Count; i++) {
				float y = x[i];
				if (!float.IsNaN(y) && !float.IsInfinity(y)) {
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public static float[] ExtractValidValues(IList<float> x) {
			List<float> result = new List<float>();
			foreach (float y in x) {
				if (!float.IsNaN(y) && !float.IsInfinity(y)) {
					result.Add(y);
				}
			}
			return result.ToArray();
		}

		public static float[] SmoothMedian(IList<float> m, int width) {
			float[] result = new float[m.Count];
			for (int i = 0; i < result.Length; i++) {
				int min = Math.Max(0, i - width);
				int max = Math.Min(result.Length - 1, i + width);
				float[] q = SubArray(m, min, max + 1);
				if (q.Length < 2 * width + 1) {
					q = Concat(q, new float[2 * width + 1 - q.Length]);
				}
				result[i] = Median(q);
			}
			return result;
		}

		public static float[] SmoothMean(IList<float> m, int width) {
			float[] result = new float[m.Count];
			for (int i = 0; i < result.Length; i++) {
				int min = Math.Max(0, i - width);
				int max = Math.Min(result.Length - 1, i + width);
				result[i] = Average(m, min, max, 2 * width + 1);
			}
			return result;
		}

		public static float Average(IList<float> m, int min, int max, int len) {
			float sum = 0;
			for (int i = min; i <= max; i++) {
				sum += m[i];
			}
			return sum / len;
		}

		public static T[,] Transpose<T>(T[,] x) {
			T[,] result = new T[x.GetLength(1), x.GetLength(0)];
			for (int i = 0; i < x.GetLength(0); i++) {
				for (int j = 0; j < x.GetLength(1); j++) {
					result[j, i] = x[i, j];
				}
			}
			return result;
		}

		public static T[][] Transpose<T>(T[][] x) {
			if (x.Length == 0) {
				return new T[0][];
			}
			T[][] result = new T[x[0].Length][];
			for (int i = 0; i < result.Length; i++) {
				result[i] = new T[x.Length];
			}
			for (int i = 0; i < x.Length; i++) {
				for (int j = 0; j < x[0].Length; j++) {
					result[j][i] = x[i][j];
				}
			}
			return result;
		}

		public static double Correlation(IList<double> x, IList<double> y) {
			if (x.Count < 3) {
				return 0;
			}
			double mx = Mean(x);
			double my = Mean(y);
			double xx = 0;
			double yy = 0;
			double xy = 0;
			for (int i = 0; i < x.Count; i++) {
				double wx = x[i] - mx;
				double wy = y[i] - my;
				xx += wx * wx;
				yy += wy * wy;
				xy += wx * wy;
			}
			double denom = xx * yy;
			if (denom > 0.0) {
				return xy / Math.Sqrt(denom);
			}
			return 0f;
		}

		public static double Correlation(IList<float> x, IList<float> y) {
			if (x.Count < 3) {
				return 0;
			}
			double mx = Mean(x);
			double my = Mean(y);
			double xx = 0;
			double yy = 0;
			double xy = 0;
			for (int i = 0; i < x.Count; i++) {
				double wx = x[i] - mx;
				double wy = y[i] - my;
				xx += wx * wx;
				yy += wy * wy;
				xy += wx * wy;
			}
			double denom = xx * yy;
			if (denom > 0.0) {
				return xy / Math.Sqrt(denom);
			}
			return 0f;
		}

		public static double[] SmoothMean(IList<double> m, int width) {
			double[] result = new double[m.Count];
			for (int i = 0; i < result.Length; i++) {
				int min = Math.Max(0, i - width);
				int max = Math.Min(result.Length - 1, i + width);
				result[i] = Average(m, min, max);
			}
			return result;
		}

		public static double[] SmoothMedian(IList<double> m, int width) {
			double[] result = new double[m.Count];
			for (int i = 0; i < result.Length; i++) {
				int min = Math.Max(0, i - width);
				int max = Math.Min(result.Length - 1, i + width);
				result[i] = Median(m, min, max);
			}
			return result;
		}

		private static double Median(IList<double> m, int min, int max) {
			int len = max - min + 1;
			if (len == 1) {
				return m[min];
			}
			if (len == 2) {
				return 0.5f * (m[min] + m[max]);
			}
			if (len == 3) {
				double m1 = m[min];
				double m2 = m[min + 1];
				double m3 = m[min + 2];
				if (m1 <= m2 && m2 <= m3) {
					return m2;
				}
				if (m2 <= m3 && m3 <= m1) {
					return m3;
				}
				if (m3 <= m1 && m1 <= m2) {
					return m1;
				}
				if (m3 <= m2 && m2 <= m1) {
					return m2;
				}
				if (m2 <= m1 && m1 <= m3) {
					return m1;
				}
				if (m1 <= m3 && m3 <= m2) {
					return m3;
				}
			}
			double[] x = new double[len];
			for (int i = 0; i < len; i++) {
				x[i] = m[min + i];
			}
			Array.Sort(x);
			if (len % 2 == 0) {
				int w = len / 2;
				return 0.5f * (x[w - 1] + x[w]);
			} else {
				int w = len / 2;
				return x[w];
			}
		}

		private static double Average(IList<double> m, int min, int max) {
			double sum = 0;
			for (int i = min; i <= max; i++) {
				sum += m[i];
			}
			return sum / (max - min + 1);
		}

		public static double Quantile(IList<double> x, double q) {
			int n = x.Count;
			if (n == 0) {
				return double.NaN;
			}
			int ind = (int) Math.Round((n - 1) * q);
			int[] o = Order(x);
			return x[o[ind]];
		}

		public static float Quantile(IList<float> x, float q) {
			int n = x.Count;
			if (n == 0) {
				return float.NaN;
			}
			int ind = (int) Math.Round((n - 1) * q);
			int[] o = Order(x);
			return x[o[ind]];
		}

		public static T[,] ExtractRows<T>(T[,] values, IList<int> rows) {
			T[,] result = new T[rows.Count, values.GetLength(1)];
			for (int i = 0; i < rows.Count; i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					result[i, j] = values[rows[i], j];
				}
			}
			return result;
		}

		public static T[,] ExtractColumns<T>(T[,] values, IList<int> cols) {
			T[,] result = new T[values.GetLength(0), cols.Count];
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < cols.Count; j++) {
					result[i, j] = values[i, cols[j]];
				}
			}
			return result;
		}

		public static T[,,] ExtractDim0<T>(T[,,] values, IList<int> inds) {
			T[,,] result = new T[inds.Count, values.GetLength(1), values.GetLength(2)];
			for (int i = 0; i < inds.Count; i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					for (int k = 0; k < values.GetLength(2); k++) {
						result[i, j, k] = values[inds[i], j, k];
					}
				}
			}
			return result;
		}

		public static T[,,] ExtractDim1<T>(T[,,] values, IList<int> inds) {
			T[,,] result = new T[values.GetLength(0), inds.Count, values.GetLength(2)];
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < inds.Count; j++) {
					for (int k = 0; k < values.GetLength(2); k++) {
						result[i, j, k] = values[i, inds[j], k];
					}
				}
			}
			return result;
		}

		public static T[,,] ExtractDim2<T>(T[,,] values, IList<int> inds) {
			T[,,] result = new T[values.GetLength(0), values.GetLength(1), inds.Count];
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					for (int k = 0; k < inds.Count; k++) {
						result[i, j, k] = values[i, j, inds[k]];
					}
				}
			}
			return result;
		}

		public static T[,,,] ExtractDim0<T>(T[,,,] values, IList<int> inds) {
			T[,,,] result = new T[inds.Count, values.GetLength(1), values.GetLength(2), values.GetLength(3)];
			for (int i = 0; i < inds.Count; i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					for (int k = 0; k < values.GetLength(2); k++) {
						for (int l = 0; l < values.GetLength(3); l++) {
							result[i, j, k, l] = values[inds[i], j, k, l];
						}
					}
				}
			}
			return result;
		}

		public static T[,,,] ExtractDim1<T>(T[,,,] values, IList<int> inds) {
			T[,,,] result = new T[values.GetLength(0), inds.Count, values.GetLength(2), values.GetLength(3)];
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < inds.Count; j++) {
					for (int k = 0; k < values.GetLength(2); k++) {
						for (int l = 0; l < values.GetLength(3); l++) {
							result[i, j, k, l] = values[i, inds[j], k, l];
						}
					}
				}
			}
			return result;
		}

		public static T[,,,] ExtractDim2<T>(T[,,,] values, IList<int> inds) {
			T[,,,] result = new T[values.GetLength(0), values.GetLength(1), inds.Count, values.GetLength(3)];
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					for (int k = 0; k < inds.Count; k++) {
						for (int l = 0; l < values.GetLength(3); l++) {
							result[i, j, k, l] = values[i, j, inds[k], l];
						}
					}
				}
			}
			return result;
		}

		public static T[,,,] ExtractDim3<T>(T[,,,] values, IList<int> inds) {
			T[,,,] result = new T[values.GetLength(0), values.GetLength(1), values.GetLength(2), inds.Count];
			for (int i = 0; i < values.GetLength(0); i++) {
				for (int j = 0; j < values.GetLength(1); j++) {
					for (int k = 0; k < values.GetLength(2); k++) {
						for (int l = 0; l < inds.Count; l++) {
							result[i, j, k, l] = values[i, j, k, inds[l]];
						}
					}
				}
			}
			return result;
		}

		public static float[] Zscore(IList<float> vals) {
			return Zscore(vals, false);
		}

		public static float[] Zscore(IList<float> vals, bool useMedian) {
			int validCount;
			double stddev;
			double mean = MeanAndStddev(vals, out stddev, out validCount, useMedian);
			float[] result = new float[vals.Count];
			if (validCount < 3) {
				for (int i = 0; i < result.Length; i++) {
					result[i] = float.NaN;
				}
				return result;
			}
			for (int i = 0; i < result.Length; i++) {
				result[i] = (float) ((vals[i] - mean) / stddev);
			}
			return result;
		}

		public static double InterQuartileRange(IList<double> vals) {
			return Quantile(vals, 0.75) - Quantile(vals, 0.25);
		}

		public static double InterQuartileRange(IList<float> vals) {
			return Quantile(vals, 0.75f) - Quantile(vals, 0.25f);
		}

		public static double FirstQuartile(IList<double> vals) {
			return Quantile(vals, 0.25);
		}

		public static double ThirdQuartile(IList<double> vals) {
			return Quantile(vals, 0.75);
		}

		public static double Skewness(IList<double> vals) {
			int n = vals.Count;
			double mean = Mean(vals);
			double m2 = 0;
			double m3 = 0;
			foreach (double c in vals.Select(val => val - mean)) {
				m3 += c * c * c;
				m2 += c * c;
			}
			m2 /= n;
			m3 /= n;
			double g1 = m3 / Math.Pow(m2, 1.5);
			return g1 * Math.Sqrt(n * (n - 1)) / (n - 2);
		}

		public static double Kurtosis(IList<double> vals) {
			int n = vals.Count;
			double mean = Mean(vals);
			double m2 = 0;
			double m4 = 0;
			foreach (double c in vals.Select(val => val - mean)) {
				m4 += c * c * c * c;
				m2 += c * c;
			}
			m2 /= n;
			m4 /= n;
			double num = n * n * ((n + 1) * m4 - 3 * (n - 1) * m2 * m2) * (n - 1) * (n - 1);
			double denom = (n - 1) * (n - 2) * (n - 3) * n * n * m2 * m2;
			return num / denom;
		}

		public static double CoefficientOfVariation(IList<double> x) {
			return StandardDeviation(x) / Mean(x);
		}

		public static double MedianAbsoluteDeviation(IList<double> x) {
			double median = Median(x);
			List<double> w = new List<double>();
			foreach (double t in x) {
				w.Add(Math.Abs(median - t));
			}
			return Median(w.ToArray());
		}

		public static double FullWidthHalfMaximum(IList<double> data) {
			if (data.Count == 0) {
				return double.NaN;
			}
			if (data.Count == 1) {
				return 0;
			}
			double q1 = FirstQuartile(data);
			double q3 = ThirdQuartile(data);
			double d = q3 - q1;
			double min = q1 - d;
			double max = q3 + d;
			List<double> data1 = new List<double>();
			foreach (double w in data) {
				if (w >= min && w <= max) {
					data1.Add(w);
				}
			}
			Histogram(data1, out double[] x, out double[] y, false, false);
			int maxInd = MaxInd(y);
			double max2 = y[maxInd]*0.5;
			int leftInd = maxInd;
			while (y[leftInd] > max2) {
				if (leftInd == 0) {
					return double.NaN;
				}
				leftInd--;
			}
			int rightInd = maxInd;
			while (y[rightInd] > max2) {
				if (rightInd == y.Length-1) {
					return double.NaN;
				}
				rightInd++;
			}
			double leftX = GetXval(x[leftInd], y[leftInd], x[leftInd+1], y[leftInd+1], max2);
			double rightX = GetXval(x[rightInd - 1], y[rightInd - 1], x[rightInd], y[rightInd], max2);
			return rightX - leftX;

		}

		private static double GetXval(double x1, double y1, double x2, double y2, double y) {
			return x1 +(x2-x1)/(y2-y1)* (y - y1);
		}

		public static HashSet<T> ToHashSet<T>(IEnumerable<T> x) {
			HashSet<T> result = new HashSet<T>();
			foreach (T t in x) {
				result.Add(t);
			}
			return result;
		}

		private static int[] SplitIndices(int n, int k) {
			int[] result = new int[k + 1];
			for (int i = 0; i < k + 1; i++) {
				result[i] = (int) Math.Round(i / (double) k * n);
			}
			return result;
		}

		/// <summary>
		///     Split the array x into n pieces.
		/// </summary>
		/// <typeparam name="T">No element type restriction.</typeparam>
		/// <param name="x">Array to be split into pieces.</param>
		/// <param name="n">Number of arrays the input array is going to be split into.</param>
		/// <returns>Array of arrays with the split result.</returns>
		public static T[][] SplitArray<T>(T[] x, int n) {
			n = Math.Min(n, x.Length);
			int[] indices = SplitIndices(x.Length, n);
			T[][] result = new T[n][];
			for (int i = 0; i < n; i++) {
				result[i] = SubArray(x, indices[i], indices[i + 1]);
			}
			return result;
		}

		/// <summary>
		///     Split the array x into n pieces.
		/// </summary>
		/// <typeparam name="T">No element type restriction.</typeparam>
		/// <param name="x">Array to be split into pieces.</param>
		/// <param name="size">Maximal size of the chunks.</param>
		/// <returns>Array of arrays with the split result.</returns>
		public static T[][] SplitArrayBySize<T>(T[] x, int size) {
			if (x.Length <= size) {
				return new[] {x};
			}
			int n = (x.Length - 1) / size + 1;
			int[] indices = SplitIndices(x.Length, n);
			T[][] result = new T[n][];
			for (int i = 0; i < n; i++) {
				result[i] = SubArray(x, indices[i], indices[i + 1]);
			}
			return result;
		}

		public static Dictionary<T, int> InverseMap<T>(IList<T> list) {
			Dictionary<T, int> result = new Dictionary<T, int>();
			for (int i = 0; i < list.Count; i++) {
				if (!result.ContainsKey(list[i])) {
					result.Add(list[i], i);
				}
			}
			return result;
		}

		public static T[] EveryNth<T>(T[] array, int n) {
			List<T> result = new List<T>();
			for (int i = 0; i < array.Length; i += n) {
				result.Add(array[i]);
			}
			return result.ToArray();
		}

		public static double CalcCovariance(IList<double> data) {
			int n = data.Count;
			double means = 0;
			for (int j = 0; j < n; j++) {
				means += data[j];
			}
			means /= n;
			double cov = 0;
			for (int k = 0; k < n; k++) {
				cov += (data[k] - means) * (data[k] - means);
			}
			cov /= n;
			return cov;
		}

		public static int[] GetTopN<T>(IList<T> vals, int n) where T : IComparable<T> {
			if (vals.Count <= n) {
				return ConsecutiveInts(vals.Count);
			}
			int[] o = Order(vals);
			int[] result = new int[n];
			for (int i = 0; i < n; i++) {
				result[i] = o[vals.Count - 1 - i];
			}
			return result;
		}

		/// <summary>
		/// Split the array into pieces not larger than <code>size</code>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static T[][] Split<T>(IList<T> array, int size) {
			if (array.Count <= size) {
				return new[] {array.ToArray()};
			}
			int n = (array.Count - 1) / size + 1;
			T[][] result = new T[n][];
			for (int i = 0; i < n - 1; i++) {
				result[i] = SubArray(array, i * size, (i + 1) * size);
			}
			result[n - 1] = SubArray(array, (n - 1) * size, array.Count);
			return result;
		}

		/// <summary>
		///     Checks whether the given value is in the array.
		/// </summary>
		/// <typeparam name="T">The type of the array and value.</typeparam>
		/// <param name="array">The array to look for the value.</param>
		/// <param name="value">The value to look for.</param>
		/// <returns></returns>
		public static bool Contains<T>(IList<T> array, T value) {
			foreach (T t in array) {
				if (t.Equals(value)) {
					return true;
				}
			}
			return false;
		}

		public static int TriangleToLinearIndex(int i, int j) {
			int b = i * (i - 1) / 2;
			return b + j;
		}

		public static bool Or(IList<bool> x) {
			foreach (bool b in x) {
				if (b) {
					return true;
				}
			}
			return false;
		}

		public static bool And(IList<bool> x) {
			foreach (bool b in x) {
				if (!b) {
					return false;
				}
			}
			return true;
		}

		public static void PackArrayOfArrays<T>(T[][] data, out T[] value, out int[] ind) {
			List<T> values = new List<T>();
			List<int> inds = new List<int>();
			foreach (T[] d in data) {
				inds.Add(values.Count);
				if (d != null) {
					foreach (T t in d) {
						values.Add(t);
					}
				}
			}
			value = values.ToArray();
			ind = inds.ToArray();
		}

		public static T[][] UnpackArrayOfArrays<T>(T[] values, int[] inds) {
			T[][] data = new T[inds.Length][];
			for (int i = 0; i < inds.Length - 1; i++) {
				int len = inds[i + 1] - inds[i];
				if (len > 0) {
					T[] x = new T[len];
					for (int j = 0; j < len; j++) {
						x[j] = values[inds[i] + j];
					}
					data[i] = x;
				}
			}
			{
				int len = values.Length - inds[inds.Length - 1];
				if (len > 0) {
					T[] x = new T[len];
					for (int j = 0; j < len; j++) {
						x[j] = values[inds[inds.Length - 1] + j];
					}
					data[inds.Length - 1] = x;
				}
			}
			return data;
		}

		public static void PackArrayOfStrings(string[] data, out string value, out int[] ind) {
			StringBuilder values = new StringBuilder();
			List<int> inds = new List<int>();
			foreach (string d in data) {
				inds.Add(values.Length);
				if (d != null) {
					values.Append(d);
				}
			}
			value = values.ToString();
			ind = inds.ToArray();
		}

		public static string[] UnpackArrayOfStrings(string values, int[] inds) {
			string[] data = new string[inds.Length];
		    if (string.IsNullOrEmpty(values))
		    {
		        return data;
		    }
			for (int i = 0; i < inds.Length - 1; i++) {
				int len = inds[i + 1] - inds[i];
				if (len > 0) {
					data[i] = values.Substring(inds[i], len);
				} else {
					data[i] = "";
				}
			}
			{
				int len = values.Length - inds[inds.Length - 1];
				if (len > 0) {
					data[inds.Length - 1] = values.Substring(inds[inds.Length - 1], len);
				} else {
					data[inds.Length - 1] = "";
				}
			}
			return data;
		}
	}
}