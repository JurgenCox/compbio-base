using System;
using System.Collections.Generic;
using BaseLibS.Num.Vector;

namespace BaseLibS.Num.Matrix{
	[Serializable]
    public abstract class MatrixIndexer : ICloneable, IDisposable, IEquatable<MatrixIndexer>
    {
        public abstract void Init(int nrows, int ncols);
		public abstract bool IsInitialized();
		public abstract int RowCount { get; }
		public abstract int ColumnCount { get; }
		/// <summary>
		/// uncheked
		/// </summary>
		public abstract double this[int i, int j] { get; set; }
		/// <summary>
		/// checked
		/// </summary>
		public abstract double Get(int i, int j);
		/// <summary>
		/// checked
		/// </summary>
		public abstract void Set(int i, int j, double value);
		public abstract void Set(double[,] value);
		public abstract BaseVector GetRow(int row);
		public abstract BaseVector GetColumn(int col);
		public abstract MatrixIndexer ExtractRows(IList<int> rows);
		public abstract void ExtractRowsInPlace(IList<int> rows);
		public abstract MatrixIndexer ExtractColumns(IList<int> columns);
		public abstract void ExtractColumnsInPlace(IList<int> columns);
		public abstract MatrixIndexer Transpose();

		/// <summary>
		/// True if at least one entry is NaN or Infinity.
		/// </summary>
		public abstract bool ContainsNaNOrInf();

		/// <summary>
		/// True if all entries are NaN or Infinity in that row.
		/// </summary>
		public abstract bool IsNanOrInfRow(int row);

		/// <summary>
		/// True if all entries are NaN or Infinity in that column.
		/// </summary>
		public abstract bool IsNanOrInfColumn(int column);

		public abstract object Clone();
		public abstract void Dispose();

        public bool Equals(MatrixIndexer other)
        {
            if (other == null)
            {
                return false;
            }
            if (!IsInitialized() && !other.IsInitialized())
            {
                return true;
            }
            if (!other.IsInitialized())
            {
                return false;
            }
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    if (this[i, j] != other[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
	}
}