using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Matrix{
	[Serializable]
	public class SparseRowFloatMatrixIndexer : MatrixIndexer{
		private SparseFloatVector[] vals;
		private int ncolumns;

		public SparseRowFloatMatrixIndexer(SparseFloatVector[] vals, int ncolumns){
			this.vals = vals;
			this.ncolumns = ncolumns;
		}

		private SparseRowFloatMatrixIndexer(){}

		public override void Init(int nrows, int ncolumns1){
			ncolumns = ncolumns1;
			vals = new SparseFloatVector[nrows];
			for (int i = 0; i < nrows; i++){
				vals[i] = new SparseFloatVector(new int[0], new float[0], ncolumns);
			}
		}

		public override void Set(double[,] value) {
			ncolumns = value.GetLength(1);
			vals = new SparseFloatVector[value.GetLength(0)];
			for (int i = 0; i < vals.Length; i++) {
				List<int> v = new List<int>();
				for (int j = 0; j < ncolumns; j++) {
					if (value[i, j] == 0) {
						continue;
					}
					v.Add(j);
				}
				int[] v1 = v.ToArray();
				float[] x = new float[v1.Length];
				for (int j = 0; j < v1.Length; j++) {
					x[j] = (float)value[i, v1[j]];
				}
				vals[i] = new SparseFloatVector(v1, x, ncolumns);
			}
		}

		public override BaseVector GetRow(int row){
			return vals[row];
		}

		public override BaseVector GetColumn(int col){
			List<int> inds = new List<int>();
			List<float> x = new List<float>();
			for (int i = 0; i < vals.Length; i++){
				float w = (float) vals[i][col];
				if (w == 0){
					continue;
				}
				inds.Add(i);
				x.Add(w);
			}
			return new SparseFloatVector(inds.ToArray(), x.ToArray(), vals.Length);
		}

		public override bool IsInitialized(){
			return vals != null;
		}

		public override MatrixIndexer ExtractRows(IList<int> rows){
			return new SparseRowFloatMatrixIndexer{vals = ArrayUtils.SubArray(vals, rows), ncolumns = ncolumns};
		}

		public override void ExtractRowsInPlace(IList<int> rows){
			vals = ArrayUtils.SubArray(vals, rows);
		}

		public override MatrixIndexer ExtractColumns(IList<int> columns){
			SparseFloatVector[] r = new SparseFloatVector[vals.Length];
			for (int i = 0; i < vals.Length; i++){
				r[i] = (SparseFloatVector) vals[i].SubArray(columns);
			}
			return new SparseRowFloatMatrixIndexer{vals = r, ncolumns = columns.Count};
		}

		public override void ExtractColumnsInPlace(IList<int> columns){
			for (int i = 0; i < vals.Length; i++){
				vals[i] = (SparseFloatVector) vals[i].SubArray(columns);
			}
			ncolumns = columns.Count;
		}

		public override MatrixIndexer Transpose(){
			return new SparseColumnFloatMatrixIndexer(vals, ncolumns);
		}

		public override bool ContainsNaNOrInf(){
			foreach (SparseFloatVector val in vals){
				if (val.ContainsNaNOrInf()){
					return true;
				}
			}
			return false;
		}

		public override bool IsNanOrInfRow(int row){
			return vals[row].IsNaNOrInf();
		}

		public override bool IsNanOrInfColumn(int column){
			for (int i = 0; i < RowCount; i++){
				float v = (float) vals[i][column];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override int RowCount => vals.Length;
		public override int ColumnCount => ncolumns;

		public override double this[int i, int j]{
			get => vals[i][j];
			set => vals[i][j] = value;
		}

		public override double Get(int i, int j){
			return vals[i][j];
		}

		public override void Set(int i, int j, double value){
			vals[i][j] = value;
		}

		public override void Dispose(){
			foreach (SparseFloatVector val in vals){
				val.Dispose();
			}
			vals = null;
		}

		public override object Clone(){
			if (vals == null){
				return new SparseRowFloatMatrixIndexer();
			}
			SparseFloatVector[] v = new SparseFloatVector[vals.Length];
			for (int i = 0; i < v.Length; i++){
				v[i] = (SparseFloatVector) vals[i].Clone();
			}
			return new SparseRowFloatMatrixIndexer{vals = v, ncolumns = ncolumns};
		}
	}
}