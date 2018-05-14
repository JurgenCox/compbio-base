using System;

namespace BaseLibS.Num.Matrix{
    public interface IBoolMatrixIndexer : ICloneable, IDisposable, IEquatable<IBoolMatrixIndexer>
    {
        void Init(int nrows, int ncols);
		int RowCount { get; }
		int ColumnCount { get; }
		bool this[int i, int j] { get; set; }
		void Set(bool[,] value);
		bool[] GetRow(int row);
		bool[] GetColumn(int col);
		bool IsInitialized();
		IBoolMatrixIndexer ExtractRows(int[] rows);
		void ExtractRowsInPlace(int[] rows);
		IBoolMatrixIndexer ExtractColumns(int[] columns);
		void ExtractColumnsInPlace(int[] columns);
		IBoolMatrixIndexer Transpose();
		void TransposeInPlace();
	}
}