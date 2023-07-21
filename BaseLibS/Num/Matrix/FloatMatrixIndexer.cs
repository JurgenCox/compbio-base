using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Num.Vector;
using BaseLibS.Util;
namespace BaseLibS.Num.Matrix{
	[Serializable]
	public class FloatMatrixIndexer : MatrixIndexer{
		private float[][,] vals;
		private bool isConstant;
		private float constVal;
		private int nrows;
		private int ncols;
		public FloatMatrixIndexer(){ }

		public FloatMatrixIndexer(float[,] vals){
			this.vals = new[]{vals};
		}

		public FloatMatrixIndexer(float[][,] vals){
			this.vals = vals;
		}

		public FloatMatrixIndexer(float val, int nrows, int ncols){
			isConstant = true;
			constVal = val;
			this.nrows = nrows;
			this.ncols = ncols;
		}

		public FloatMatrixIndexer(BinaryReader reader){
			vals = FileUtils.Read3DFloatArray2(reader);
			isConstant = reader.ReadBoolean();
			constVal = reader.ReadSingle();
			nrows = reader.ReadInt32();
			ncols = reader.ReadInt32();
		}
		public override void Write(BinaryWriter writer){
			FileUtils.Write(vals, writer);
			writer.Write(isConstant);
			writer.Write(constVal);
			writer.Write(nrows);
			writer.Write(ncols);
		}
		internal const int maxArraySize = int.MaxValue / 4;

		public override void Init(int nrows1, int ncols1){
			long ncells = (long) nrows1 * ncols1;
			int nRowBlocks = (int) (ncells == 0 ? 1 : (ncells - 1) / maxArraySize + 1);
			int rowBlockSize = nrows1 == 0 ? 0 : (nrows1 - 1) / nRowBlocks + 1;
			int lastRowBlockSize = nrows1 - rowBlockSize * (nRowBlocks - 1);
			vals = new float[nRowBlocks][,];
			for (int i = 0; i < nRowBlocks - 1; i++){
				vals[i] = new float[rowBlockSize, ncols1];
			}
			vals[nRowBlocks - 1] = new float[lastRowBlockSize, ncols1];
			isConstant = false;
		}

		public void TransposeInPlace(){
			if (isConstant){
				(nrows, ncols) = (ncols, nrows);
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
			float[,] vals1 = new float[value.GetLength(0), value.GetLength(1)];
			for (int i = 0; i < value.GetLength(0); i++){
				for (int j = 0; j < value.GetLength(1); j++){
					vals1[i, j] = (float) value[i, j];
				}
			}
			this.vals = new[]{vals1};
		}

		public override BaseVector GetRow(int row){
			float[] result = new float[ColumnCount];
			if (isConstant){
				for (int i = 0; i < result.Length; i++){
					result[i] = constVal;
				}
			} else{
				int q = vals[0].GetLength(0);
				int row0 = row / q;
				int row1 = row % q;
				for (int i = 0; i < result.Length; i++){
					result[i] = vals[row0][row1, i];
				}
			}
			return new FloatArrayVector(result);
		}

		public override BaseVector GetColumn(int col){
			float[] result = new float[RowCount];
			if (isConstant){
				for (int i = 0; i < result.Length; i++){
					result[i] = constVal;
				}
			} else{
				for (int i = 0; i < result.Length; i++){
					int q = vals[0].GetLength(0);
					int row0 = i / q;
					int row1 = i % q;
					result[i] = vals[row0][row1, col];
				}
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
			foreach (float[,] t in vals){
				for (int j = 0; j < t.GetLength(0); j++){
					for (int k = 0; k < t.GetLength(1); k++){
						if (float.IsNaN(t[j, k]) || float.IsInfinity(t[j, k])){
							return true;
						}
					}
				}
			}
			return false;
		}

		public override bool IsNanOrInfRow(int row){
			if (isConstant){
				return float.IsInfinity(constVal) || float.IsNaN(constVal);
			}
			int q = vals[0].GetLength(0);
			int row0 = row / q;
			int row1 = row % q;
			for (int i = 0; i < ColumnCount; i++){
				float v = vals[row0][row1, i];
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
				int q = vals[0].GetLength(0);
				int row0 = i / q;
				int row1 = i % q;
				float v = vals[row0][row1, column];
				if (!float.IsNaN(v) && !float.IsInfinity(v)){
					return false;
				}
			}
			return true;
		}

		public override int RowCount{
			get{
				if (isConstant){
					return nrows;
				}
				if (vals == null){
					return 0;
				}
				int rows = 0;
				foreach (float[,] f in vals){
					rows += f.GetLength(0);
				}
				return rows;
			}
		}

		public override int ColumnCount{
			get{
				if (isConstant){
					return ncols;
				}
				if (vals == null){
					return 0;
				}
				return vals[0].GetLength(1);
			}
		}

		public override double this[int i, int j]{
			get{
				if (isConstant){
					return constVal;
				}
				int q = vals[0].GetLength(0);
				int row0 = i / q;
				int row1 = i % q;
				return vals[row0][row1, j];
			}
			set{
				if (isConstant){
					if (value == constVal){
						return;
					}
					Init(nrows, ncols);
					if (constVal != 0){
						foreach (float[,] val in vals){
							for (int k = 0; k < val.GetLength(0); k++){
								for (int l = 0; l < val.GetLength(1); l++){
									val[k, l] = constVal;
								}
							}
						}
					}
				}
				int q = vals[0].GetLength(0);
				int row0 = i / q;
				int row1 = i % q;
				vals[row0][row1, j] = (float) value;
			}
		}

		public override double Get(int i, int j){
			if (isConstant){
				return constVal;
			}
			if (!IsInitialized()){
				return float.NaN;
			}
			int q = vals[0].GetLength(0);
			int row0 = i / q;
			int row1 = i % q;
			return vals[row0][row1, j];
		}

		public override void Set(int i, int j, double value){
			if (isConstant){
				if (value == constVal){
					return;
				}
				Init(nrows, ncols);
				if (constVal != 0){
					foreach (float[,] val in vals){
						for (int k = 0; k < val.GetLength(0); k++){
							for (int l = 0; l < val.GetLength(1); l++){
								val[k, l] = constVal;
							}
						}
					}
				}
			}
			if (!IsInitialized()){
				return;
			}
			int q = vals[0].GetLength(0);
			int row0 = i / q;
			int row1 = i % q;
			vals[row0][row1, j] = (float) value;
		}

		public override void Dispose(){
			vals = null;
		}

		public override object Clone(){
			if (isConstant){
				return new FloatMatrixIndexer(constVal, nrows, ncols);
			}
			if (vals == null){
				return new FloatMatrixIndexer((float[][,]) null);
			}
			float[][,] v = new float[vals.Length][,];
			for (int i = 0; i < v.Length; i++){
				v[i] = (float[,]) vals[i].Clone();
			}
			return new FloatMatrixIndexer(v);
		}
	}
}