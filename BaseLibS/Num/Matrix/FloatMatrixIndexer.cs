using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Matrix{
	[Serializable]
    public class FloatMatrixIndexer : MatrixIndexer
    {
        private float[,] vals;
		public FloatMatrixIndexer(){}

		public FloatMatrixIndexer(float[,] vals){
			this.vals = vals;
		}

		public override void Init(int nrows, int ncols){
			vals = new float[nrows, ncols];
		}

		public void TransposeInPlace(){
			if (vals != null){
				vals = ArrayUtils.Transpose(vals);
			}
		}

		public override MatrixIndexer Transpose(){
			return vals == null ? new FloatMatrixIndexer() : new FloatMatrixIndexer(ArrayUtils.Transpose(vals));
		}

		public override void Set(double[,] value) {
			vals = new float[value.GetLength(0),value.GetLength(1)];
			for (int i = 0; i < value.GetLength(0); i++) {
				for (int j = 0; j < value.GetLength(1); j++) {
					vals[i, j] = (float)value[i, j];
				}
			}
		}

		public override BaseVector GetRow(int row){
			float[] result = new float[ColumnCount];
			for (int i = 0; i < result.Length; i++){
				result[i] = vals[row, i];
			}
			return new FloatArrayVector(result);
		}

		public override BaseVector GetColumn(int col){
			float[] result = new float[RowCount];
			for (int i = 0; i < result.Length; i++){
				result[i] = vals[i, col];
			}
			return new FloatArrayVector(result);
		}

		public override bool IsInitialized(){
			return vals != null;
		}

		public override MatrixIndexer ExtractRows(IList<int> rows){
			return new FloatMatrixIndexer(ArrayUtils.ExtractRows(vals, rows));
		}

		public override MatrixIndexer ExtractColumns(IList<int> columns){
			return new FloatMatrixIndexer(ArrayUtils.ExtractColumns(vals, columns));
		}

		public override void ExtractRowsInPlace(IList<int> rows){
			if (vals != null){
				vals = ArrayUtils.ExtractRows(vals, rows);
			}
		}

		public override void ExtractColumnsInPlace(IList<int> columns){
			if (vals != null){
				vals = ArrayUtils.ExtractColumns(vals, columns);
			}
		}

		public override bool ContainsNaNOrInf(){
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
			for (int i = 0; i < ColumnCount; i++){
				float v = vals[row, i];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override bool IsNanOrInfColumn(int column){
			for (int i = 0; i < RowCount; i++){
				float v = vals[i, column];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override int RowCount => vals?.GetLength(0) ?? 0;
		public override int ColumnCount => vals?.GetLength(1) ?? 0;

		public override double this[int i, int j]{
			get => vals[i, j];
			set => vals[i, j] = (float)value;
		}

		public override double Get(int i, int j){
			return !IsInitialized() ? float.NaN : vals[i, j];
		}

		public override void Set(int i, int j, double value){
			if (!IsInitialized()){
				return;
			}
			vals[i, j] = (float)value;
		}

		public override void Dispose(){
			vals = null;
		}

		public override object Clone(){
			return vals == null ? new FloatMatrixIndexer(null) : new FloatMatrixIndexer((float[,]) vals.Clone());
		}

    }
}