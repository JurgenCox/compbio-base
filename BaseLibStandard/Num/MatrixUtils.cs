using System;
using BaseLibS.Api;
using BaseLibS.Num.Matrix;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num{
	/// <summary>
	/// A collection of primitive operation on matrices.
	/// </summary>
	public static class MatrixUtils{
		public static bool HasNanOrInf(MatrixIndexer m, MatrixAccess access){
			switch (access){
				case MatrixAccess.Columns:
					return HasNaNOrInfColumns(m);
				case MatrixAccess.Rows:
					return HasNaNOrInfRows(m);
				default:
					throw new NotImplementedException($"Not implemented for access {access}");
			}
		}

		public static bool HasNaNOrInfRows(MatrixIndexer m){
			for (int i = 0; i < m.RowCount; i++){
				if (m.IsNanOrInfRow(i)){
					return true;
				}
			}
			return false;
		}

		public static bool HasNaNOrInfColumns(MatrixIndexer m){
			for (int i = 0; i < m.ColumnCount; i++){
				if (m.IsNanOrInfColumn(i)){
					return true;
				}
			}
			return false;
		}

		public static T[,] Transpose<T>(T[,] x){
			int nrows = x.GetLength(0);
			int ncols = x.GetLength(1);
			T[,] result = new T[ncols, nrows];
			for (int i = 0; i < ncols; i++){
				for (int j = 0; j < nrows; j++){
					result[i, j] = x[j, i];
				}
			}
			return result;
		}

		public static double[,] ExtractRows(double[,] x, int[] indices){
			double[,] result = new double[indices.Length, x.GetLength(1)];
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < result.GetLength(1); j++){
					result[i, j] = x[indices[i], j];
				}
			}
			return result;
		}

		public static float[,] ExtractRows(float[,] x, int[] indices){
			float[,] result = new float[indices.Length, x.GetLength(1)];
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < result.GetLength(1); j++){
					result[i, j] = x[indices[i], j];
				}
			}
			return result;
		}

		public static double[] ExtractColumn(double[,] a, int index){
			int n = a.GetLength(0);
			double[] result = new double[n];
			for (int i = 0; i < n; i++){
				result[i] = a[i, index];
			}
			return result;
		}

		public static double[] ExtractRow(double[,] a, int index){
			int n = a.GetLength(1);
			double[] result = new double[n];
			for (int i = 0; i < n; i++){
				result[i] = a[index, i];
			}
			return result;
		}

		public static float[] ExtractRow(float[,] a, int index){
			int n = a.GetLength(1);
			float[] result = new float[n];
			for (int i = 0; i < n; i++){
				result[i] = a[index, i];
			}
			return result;
		}

		public static double[,] ExtractColumns(double[,] x, int[] indices){
			int n = x.GetLength(0);
			int ncol = indices.Length;
			double[,] result = new double[n, ncol];
			for (int i = 0; i < n; i++){
				for (int j = 0; j < ncol; j++){
					result[i, j] = x[i, indices[j]];
				}
			}
			return result;
		}

		public static double[,] MatrixTimesMatrix(double[,] a, double[,] b){
			double[,] result = new double[a.GetLength(0), b.GetLength(1)];
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < result.GetLength(1); j++){
					for (int k = 0; k < a.GetLength(1); k++){
						result[i, j] += a[i, k]*b[k, j];
					}
				}
			}
			return result;
		}

		public static float[,] MatrixTimesMatrix(float[,] a, float[,] b){
			float[,] result = new float[a.GetLength(0), b.GetLength(1)];
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < result.GetLength(1); j++){
					for (int k = 0; k < a.GetLength(1); k++){
						result[i, j] += a[i, k]*b[k, j];
					}
				}
			}
			return result;
		}

		public static double[] MatrixTimesVector(double[,] a, double[] b){
			double[] result = new double[a.GetLength(0)];
			for (int i = 0; i < result.GetLength(0); i++){
				for (int k = 0; k < a.GetLength(1); k++){
					result[i] += a[i, k]*b[k];
				}
			}
			return result;
		}

		public static double[,] MatrixTimesMatrixTransposed(double[,] a, double[,] b){
			double[,] result = new double[a.GetLength(0), b.GetLength(0)];
			for (int i = 0; i < result.GetLength(0); i++){
				for (int j = 0; j < result.GetLength(1); j++){
					for (int k = 0; k < a.GetLength(1); k++){
						result[i, j] += a[i, k]*b[j, k];
					}
				}
			}
			return result;
		}

		public static double[] VectorTimesMatrix(double[] a, double[,] b){
			double[] result = new double[b.GetLength(1)];
			for (int i = 0; i < result.Length; i++){
				for (int k = 0; k < a.Length; k++){
					result[i] += a[k]*b[k, i];
				}
			}
			return result;
		}

		public static double[] VectorTimesMatrix(BaseVector a, double[,] b){
			double[] result = new double[b.GetLength(1)];
			for (int i = 0; i < result.Length; i++){
				for (int k = 0; k < a.Length; k++){
					result[i] += a[k]*b[k, i];
				}
			}
			return result;
		}

		public static float[] VectorTimesMatrix(float[] a, float[,] b){
			float[] result = new float[b.GetLength(1)];
			for (int i = 0; i < result.Length; i++){
				for (int k = 0; k < a.Length; k++){
					result[i] += a[k]*b[k, i];
				}
			}
			return result;
		}

		public static double RowSum(double[,] a, int index){
			double result = 0;
			for (int i = 0; i < a.GetLength(1); i++){
				result += a[index, i];
			}
			return result;
		}

		public static bool IsSquare(double[,] matrix){
			return matrix.GetLength(0) == matrix.GetLength(1);
		}

		public static void SetColumn(double[,] matrix, double[] values, int column){
			int n = matrix.GetLength(0);
			for (int i = 0; i < n; i++){
				matrix[i, column] = values[i];
			}
		}

		public static void SetRow(double[,] matrix, double[] values, int row){
			int n = matrix.GetLength(1);
			for (int i = 0; i < n; i++){
				matrix[row, i] = values[i];
			}
		}

		public static double[,] Identity(int nrows, int ncols){
			double[,] identity = new double[nrows, ncols]; // assumes all 0
			for (int i = 0; i < Math.Min(nrows, ncols); ++i){
				identity[i, i] = 1;
			}
			return identity;
		}

		public class Lu{
			public double[,] l;
			public Lu llu;
			public double[,] u;
			public Lu ulu;
			public int[] pi;
			public double determinantOfPi;
		}

		public static Lu LuDecomposition(double[,] matrix){
			if (!IsSquare(matrix)){
				throw new Exception("LU decomposition requires a square matrix");
			}
			int nrows = matrix.GetLength(0);
			int ncols = matrix.GetLength(1);
			Lu lu = new Lu{l = Identity(nrows, ncols), u = (double[,]) matrix.Clone(), pi = new int[nrows]};
			for (int i = 0; i < nrows; i++){
				lu.pi[i] = i;
			}
			int k0 = 0;
			lu.determinantOfPi = 1;
			for (int k = 0; k < ncols - 1; k++){
				double p = 0;
				// find the row with the biggest pivot
				for (int i = k; i < nrows; i++){
					if (Math.Abs(lu.u[i, k]) > p){
						p = Math.Abs(lu.u[i, k]);
						k0 = i;
					}
				}
				if (p == 0){
					throw new Exception("The matrix is singular!");
				}
				// switch two rows in permutation matrix
				int pom1 = lu.pi[k];
				lu.pi[k] = lu.pi[k0];
				lu.pi[k0] = pom1;
				for (int i = 0; i < k; i++){
					double pom2 = lu.l[k, i];
					lu.l[k, i] = lu.l[k0, i];
					lu.l[k0, i] = pom2;
				}
				if (k != k0){
					lu.determinantOfPi *= -1;
				}
				// Switch rows in U
				for (int i = 0; i < ncols; i++){
					double pom2 = lu.u[k, i];
					lu.u[k, i] = lu.u[k0, i];
					lu.u[k0, i] = pom2;
				}
				for (int i = k + 1; i < nrows; i++){
					lu.l[i, k] = lu.u[i, k]/lu.u[k, k];
					for (int j = k; j < ncols; j++){
						lu.u[i, j] = lu.u[i, j] - lu.l[i, k]*lu.u[k, j];
					}
				}
			}
			return lu;
		}

		public static double[,] Invert(double[,] matrix, Lu lu){
			int nrows = matrix.GetLength(0);
			int ncols = matrix.GetLength(1);
			double[,] inv = new double[nrows, ncols];
			for (int i = 0; i < nrows; i++){
				double[] ei = new double[nrows];
				ei[lu.pi[lu.pi[i]]] = 1;
				double[,] col = SolveWith(matrix, ei, lu);
				SetColumn(inv, ExtractColumn(col, 0), i);
			}
			return inv;
		}

		public static double[,] SolveWith(double[,] matrix, double[] v, Lu lu){
			int nrows = matrix.GetLength(0);
			int ncols = matrix.GetLength(1);
			if (!IsSquare(matrix)){
				throw new Exception("SolveWith requires a square matrix.");
			}
			if (nrows != v.Length){
				throw new Exception("Wrong number of results in solution vector.");
			}
			double[,] b = new double[nrows, ncols];
			for (int i = 0; i < nrows; i++){
				b[lu.pi[i], 0] = v[i];
			}
			if (lu.llu == null){
				lu.llu = LuDecomposition(lu.l);
			}
			double[,] z = ForwardSubstitution(lu.l, lu.llu, b);
			if (lu.ulu == null){
				lu.ulu = LuDecomposition(lu.u);
			}
			double[,] x = BackwardSubstitution(lu.u, lu.ulu, z);
			return x;
		}

		public static double[,] ForwardSubstitution(double[,] a, Lu alu, double[,] b){
			int n = a.GetLength(0);
			double[,] x = new double[n, 1];
			for (int i = 0; i < n; i++){
				x[i, 0] = b[i, 0];
				for (int j = 0; j < i; j++){
					x[i, 0] -= a[i, j]*x[j, 0];
				}
				x[i, 0] = x[i, 0]/a[i, i];
			}
			return x;
		}

		public static double[,] BackwardSubstitution(double[,] a, Lu alu, double[,] b){
			int n = a.GetLength(0);
			double[,] x = new double[n, 1];
			for (int i = n - 1; i > -1; i--){
				x[i, 0] = b[i, 0];
				for (int j = n - 1; j > i; j--){
					x[i, 0] -= a[i, j]*x[j, 0];
				}
				x[i, 0] = x[i, 0]/a[i, i];
			}
			return x;
		}

		public static bool HasNanOrInf(float[,] x){
			for (int i = 0; i < x.GetLength(0); i++){
				for (int j = 0; j < x.GetLength(1); j++){
					if (float.IsNaN(x[i, j]) || float.IsInfinity(x[i, j])){
						return true;
					}
				}
			}
			return false;
		}

		public static double ColumnNorm2(double[,] x, int colInd){
			int nrows = x.GetLength(0);
			double sum = 0;
			for (int i = 0; i < nrows; i++){
				sum += x[i, colInd]*x[i, colInd];
			}
			return Math.Sqrt(sum);
		}

		public static double[,] NewDoubleMatix(int nrows, int ncolumns, double value){
			double[,] result = new double[nrows, ncolumns];
			if (value != 0){
				for (int i = 0; i < nrows; i++){
					for (int j = 0; j < ncolumns; j++){
						result[i, j] = value;
					}
				}
			}
			return result;
		}
	}
}