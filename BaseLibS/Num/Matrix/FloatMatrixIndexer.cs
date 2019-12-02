using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Matrix{
	[Serializable]
	public class FloatMatrixIndexer : MatrixIndexer{
		private float[,] vals;
		private bool isConstant;
		private readonly float constVal;
		private int nrows;
		private int ncols;
		public FloatMatrixIndexer(){ }

		public FloatMatrixIndexer(float[,] vals){
			this.vals = vals;
		}

		public FloatMatrixIndexer(float val, int nrows, int ncols){
			isConstant = true;
			constVal = val;
			this.nrows = nrows;
			this.ncols = ncols;
		}

		public override void Init(int nrows, int ncols){
			vals = new float[nrows, ncols];
			isConstant = false;
		}

		public void TransposeInPlace(){
			if (isConstant){
				int tmp = nrows;
				nrows = ncols;
				ncols = tmp;
				return;
			}
			if (vals != null){
				vals = ArrayUtils.Transpose(vals);
			}
		}

		public override MatrixIndexer Transpose(){
			if (isConstant){
				return new FloatMatrixIndexer(constVal, ncols, nrows);
			}
			return vals == null ? new FloatMatrixIndexer() : new FloatMatrixIndexer(ArrayUtils.Transpose(vals));
		}

		public override void Set(double[,] value){
			isConstant = false;
			vals = new float[value.GetLength(0), value.GetLength(1)];
			for (int i = 0; i < value.GetLength(0); i++){
				for (int j = 0; j < value.GetLength(1); j++){
					vals[i, j] = (float) value[i, j];
				}
			}
		}

		public override BaseVector GetRow(int row){
			float[] result = new float[ColumnCount];
			for (int i = 0; i < result.Length; i++){
				result[i] = isConstant ? constVal : vals[row, i];
			}
			return new FloatArrayVector(result);
		}

		public override BaseVector GetColumn(int col){
			float[] result = new float[RowCount];
			for (int i = 0; i < result.Length; i++){
				result[i] = isConstant ? constVal : vals[i, col];
			}
			return new FloatArrayVector(result);
		}

		public override bool IsInitialized(){
			return vals != null || isConstant;
		}

		public override MatrixIndexer ExtractRows(IList<int> rows){
			if (isConstant){
				return new FloatMatrixIndexer(constVal, rows.Count, ncols);
			}
			return new FloatMatrixIndexer(ArrayUtils.ExtractRows(vals, rows));
		}

		public override MatrixIndexer ExtractColumns(IList<int> columns){
			if (isConstant){
				return new FloatMatrixIndexer(constVal, nrows, columns.Count);
			}
			return new FloatMatrixIndexer(ArrayUtils.ExtractColumns(vals, columns));
		}

		public override void ExtractRowsInPlace(IList<int> rows){
			if (isConstant){
				nrows = rows.Count;
				return;
			}
			if (vals != null){
				vals = ArrayUtils.ExtractRows(vals, rows);
			}
		}

		public override void ExtractColumnsInPlace(IList<int> columns){
			if (isConstant){
				ncols = columns.Count;
				return;
			}
			if (vals != null){
				vals = ArrayUtils.ExtractColumns(vals, columns);
			}
		}

		public override bool ContainsNaNOrInf(){
			if (isConstant){
				return float.IsInfinity(constVal) || float.IsNaN(constVal);
			}
			for (int i = 0; i < vals.GetLength(0); i++){
				for (int j = 0; j < vals.GetLength(1); j++){
					if (float.IsNaN(vals[i, j]) || float.IsInfinity(vals[i, j])){
						return true;
					}
				}
			}
			return false;
		}

		public override bool IsNanOrInfRow(int row){
			if (isConstant){
				return float.IsInfinity(constVal) || float.IsNaN(constVal);
			}
			for (int i = 0; i < ColumnCount; i++){
				float v = vals[row, i];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override bool IsNanOrInfColumn(int column){
			if (isConstant){
				return float.IsInfinity(constVal) || float.IsNaN(constVal);
			}
			for (int i = 0; i < RowCount; i++){
				float v = vals[i, column];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override int RowCount => isConstant ? nrows : vals?.GetLength(0) ?? 0;
		public override int ColumnCount => isConstant ? ncols : vals?.GetLength(1) ?? 0;

		public override double this[int i, int j]{
			get => isConstant ? constVal : vals[i, j];
			set => vals[i, j] = (float) value;
		}

		public override double Get(int i, int j){
			if (isConstant){
				return constVal;
			}
			return !IsInitialized() ? float.NaN : vals[i, j];
		}

		public override void Set(int i, int j, double value){
			if (isConstant){
				throw new Exception("Setting value in constant matrix.");
			}
			if (!IsInitialized()){
				return;
			}
			vals[i, j] = (float) value;
		}

		public override void Dispose(){
			vals = null;
		}

		public override object Clone(){
			if (isConstant){
				return new FloatMatrixIndexer(constVal, nrows, ncols);
			}
			return vals == null ? new FloatMatrixIndexer(null) : new FloatMatrixIndexer((float[,]) vals.Clone());
		}
	}
}