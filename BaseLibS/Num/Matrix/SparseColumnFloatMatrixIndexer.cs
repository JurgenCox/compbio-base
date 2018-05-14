using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Matrix{
	[Serializable]
	public class SparseColumnFloatMatrixIndexer : MatrixIndexer{
		private SparseFloatVector[] vals;
		private int nrows;

		public SparseColumnFloatMatrixIndexer(SparseFloatVector[] vals, int nrows){
			this.vals = vals;
			this.nrows = nrows;
		}

		private SparseColumnFloatMatrixIndexer(){}

		public override void Init(int nrows1, int ncols){
			nrows = nrows1;
			vals = new SparseFloatVector[ncols];
			for (int i = 0; i < ncols; i++){
				vals[i] = new SparseFloatVector(new int[0], new float[0], nrows);
			}
		}

		public override void Set(double[,] value) {
			nrows = value.GetLength(0);
			vals = new SparseFloatVector[value.GetLength(1)];
			for (int i = 0; i < vals.Length; i++) {
				List<int> v = new List<int>();
				for (int j = 0; j < nrows; j++) {
					if (value[j, i] == 0) {
						continue;
					}
					v.Add(j);
				}
				int[] v1 = v.ToArray();
				float[] x = new float[v1.Length];
				for (int j = 0; j < v1.Length; j++) {
					x[j] = (float)value[v1[j], i];
				}
				vals[i] = new SparseFloatVector(v1, x, nrows);
			}
		}

		public override BaseVector GetRow(int row){
			List<int> inds = new List<int>();
			List<float> x = new List<float>();
			for (int i = 0; i < vals.Length; i++){
				float w = (float) vals[i][row];
				if (w == 0){
					continue;
				}
				inds.Add(i);
				x.Add(w);
			}
			return new SparseFloatVector(inds.ToArray(), x.ToArray(), vals.Length);
		}

		public override BaseVector GetColumn(int col){
			return vals[col];
		}

		public override bool IsInitialized(){
			return vals != null;
		}

		public override MatrixIndexer ExtractRows(IList<int> rows){
			SparseFloatVector[] r = new SparseFloatVector[vals.Length];
			for (int i = 0; i < vals.Length; i++){
				r[i] = (SparseFloatVector) vals[i].SubArray(rows);
			}
			return new SparseColumnFloatMatrixIndexer{vals = r, nrows = rows.Count};
		}

		public override void ExtractRowsInPlace(IList<int> rows){
			for (int i = 0; i < vals.Length; i++){
				vals[i] = (SparseFloatVector) vals[i].SubArray(rows);
			}
			nrows = rows.Count;
		}

		public override MatrixIndexer ExtractColumns(IList<int> columns){
			return new SparseColumnFloatMatrixIndexer{vals = ArrayUtils.SubArray(vals, columns), nrows = nrows};
		}

		public override void ExtractColumnsInPlace(IList<int> columns){
			vals = ArrayUtils.SubArray(vals, columns);
		}

		public override MatrixIndexer Transpose(){
			return new SparseRowFloatMatrixIndexer(vals, nrows);
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
			for (int i = 0; i < ColumnCount; i++){
				float v = (float) vals[i][row];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override bool IsNanOrInfColumn(int column){
			return vals[column].IsNaNOrInf();
		}

		public override int RowCount => nrows;
		public override int ColumnCount => vals.Length;

		public override double this[int i, int j]{
			get => vals[j][i];
			set => vals[j][i] = value;
		}

		public override double Get(int i, int j){
			return vals[j][i];
		}

		public override void Set(int i, int j, double value){
			vals[j][i] = value;
		}

		public override void Dispose(){
			foreach (SparseFloatVector val in vals){
				val.Dispose();
			}
			vals = null;
		}

		public override object Clone(){
			if (vals == null){
				return new SparseColumnFloatMatrixIndexer();
			}
			SparseFloatVector[] v = new SparseFloatVector[vals.Length];
			for (int i = 0; i < v.Length; i++){
				v[i] = (SparseFloatVector) vals[i].Clone();
			}
			return new SparseColumnFloatMatrixIndexer{vals = v, nrows = nrows};
		}
	}
}