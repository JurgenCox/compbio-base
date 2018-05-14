using System;
using BaseLibS.Api;
using BaseLibS.Num.Matrix;
using BaseLibS.Num.Vector;

namespace NumPluginBase.Distance {
	/// <summary>
	/// Allows for the conversion of any <see cref="IDistance"/> into a distance matrix.
	/// Implementation stores only upper triangular matrix due to symmetry in the distance.
	/// This improves memory footprint by factor 2 at the cost of slightly reduced performance
	/// due to index comparison and calculation.
	/// </summary>
	public class GenericDistanceMatrix : IDistanceMatrix {
		private readonly double[] distances;
		public int N { get; }

		public double[] AsCondensed() {
			return distances;
		}

		public double[,] AsQuadratic() {
			double[,] distances1 = new double[N, N];
			for (int i = 0; i < N; i++) {
				for (int j = 0; j < N; j++) {
					distances1[i, j] = this[i, j];
				}
			}
			return distances1;
		}

		/// <summary>
		/// Create distance matrix from <see cref="IDistance"/>.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="distance"></param>
		public GenericDistanceMatrix(MatrixIndexer data, IDistance distance) {
			N = data.RowCount;
			distances = new double[N * (N - 1) / 2];
			int k = 0;
			for (int i = 0; i < N; i++) {
				BaseVector xi = data.GetRow(i);
				for (int j = i + 1; j < N; j++) {
					distances[k++] = distance.Get(xi, data.GetRow(j));
				}
			}
		}

		/// <summary>
		/// Create distance matrix from a condensed distances array.
		/// </summary>
		/// <param name="distances1"></param>
		public GenericDistanceMatrix(double[] distances1) {
			distances = distances1;
			N = Convert.ToInt32(1.0 / 2.0 * (Math.Sqrt(8 * distances.Length + 1) + 1));
		}

		/// <summary>
		/// Create distance matrix from a condensed distances array.
		/// </summary>
		/// <param name="distances1"></param>
		public GenericDistanceMatrix(double[,] distances1) {
			N = distances.GetLength(0);
			distances = new double[N];
			int k = 0;
			for (int i = 1; i < N; i++) {
				for (int j = i + 1; j < N; j++) {
					distances[k++] = distances1[i, j];
				}
			}
		}

		public double this[int i, int j] {
			get {
				int comp = i.CompareTo(j);
				if (comp == 0) {
					return 0.0;
				}
				if (comp > 0) {
					int tmp = i;
					i = j;
					j = tmp;
				}
				int k = N * (N - 1) / 2 - (N - i) * (N - i - 1) / 2 + j - i - 1;
				return distances[k];
			}
			set {
				int k = N * (N - 1) / 2 - (N - i) * (N - i - 1) / 2 + j - i - 1;
				distances[k] = value;
			}
		}
	}
}