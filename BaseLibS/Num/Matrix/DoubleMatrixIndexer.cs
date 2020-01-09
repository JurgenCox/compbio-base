using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Matrix {
	[Serializable]
	public class DoubleMatrixIndexer : MatrixIndexer {
		private double[,] vals;
		public DoubleMatrixIndexer() { }

		public DoubleMatrixIndexer(double[,] vals) {
			this.vals = vals;
		}

		public override void Init(int nrows, int ncols) {
			vals = new double[nrows, ncols];
		}

		public void TransposeInPlace() {
			if (vals != null) {
				vals = ArrayUtils.Transpose(vals);
			}
		}

		public override MatrixIndexer Transpose() {
			return vals == null ? new DoubleMatrixIndexer() : new DoubleMatrixIndexer(ArrayUtils.Transpose(vals));
		}

		public override void Set(double[,] value) {
			vals = value;
		}

		public override BaseVector GetRow(int row) {
			double[] result = new double[ColumnCount];
			for (int i = 0; i < result.Length; i++) {
				result[i] = vals[row, i];
			}
			return new DoubleArrayVector(result);
		}

		public override BaseVector GetColumn(int col) {
			double[] result = new double[RowCount];
			for (int i = 0; i < result.Length; i++) {
				result[i] = vals[i, col];
			}
			return new DoubleArrayVector(result);
		}

		public override bool IsInitialized() {
			return vals != null;
		}

		public override MatrixIndexer ExtractRows(IList<int> rows) {
			return new DoubleMatrixIndexer(ArrayUtils.ExtractRows(vals, rows));
		}

		public override MatrixIndexer ExtractColumns(IList<int> columns) {
			return new DoubleMatrixIndexer(ArrayUtils.ExtractColumns(vals, columns));
		}

		public override void ExtractRowsInPlace(IList<int> rows) {
			if (vals != null) {
				vals = ArrayUtils.ExtractRows(vals, rows);
			}
		}

		public override void ExtractColumnsInPlace(IList<int> columns) {
			if (vals != null) {
				vals = ArrayUtils.ExtractColumns(vals, columns);
			}
		}

		public override bool ContainsNaNOrInf() {
			for (int i = 0; i < vals.GetLength(0); i++) {
				for (int j = 0; j < vals.GetLength(1); j++) {
					if (double.IsNaN(vals[i, j]) || double.IsInfinity(vals[i, j])) {
						return true;
					}
				}
			}
			return false;
		}

		public override bool IsNanOrInfRow(int row) {
			for (int i = 0; i < ColumnCount; i++) {
				double v = vals[row, i];
				if (!double.IsNaN(v) && !double.IsInfinity(v)) {
					return false;
				}
			}
			return true;
		}

		public override bool IsNanOrInfColumn(int column) {
			for (int i = 0; i < RowCount; i++) {
				double v = vals[i, column];
				if (!double.IsNaN(v) && !double.IsInfinity(v)) {
					return false;
				}
			}
			return true;
		}

		public override int RowCount => vals.GetLength(0);
		public override int ColumnCount => vals.GetLength(1);

		public override double this[int i, int j] {
			get => vals[i, j];
			set => vals[i, j] = value;
		}

		public override double Get(int i, int j) {
			return !IsInitialized() ? double.NaN : vals[i, j];
		}

		public override void Set(int i, int j, double value) {
			if (!IsInitialized()) {
				return;
			}
			vals[i, j] = value;
		}

		public override void Dispose() {
			vals = null;
		}

		public override object Clone() {
			return vals == null ? new DoubleMatrixIndexer(null) : new DoubleMatrixIndexer((double[,])vals.Clone());
		}
	}
}
