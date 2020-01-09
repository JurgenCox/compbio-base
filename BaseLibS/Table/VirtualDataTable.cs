using System;

namespace BaseLibS.Table{
	public abstract class VirtualDataTable : TableModelImpl, ITable{
		private long rowInUse = -1;
		private object[] rowDataInUse;

		protected VirtualDataTable(string name, string description, long rowCount) : base(name, description){
			RowCount = rowCount;
		}

		public override long RowCount { get; }

		public override object GetEntry(long row, int col){
			if (row >= RowCount || row < 0){
				return null;
			}
			if (rowInUse != row){
				rowDataInUse = GetRowData(row);
				rowInUse = row;
			}
			return col >= rowDataInUse.Length ? null : rowDataInUse[col];
		}

		public override void SetEntry(long row, int column, object value){
			throw new NotImplementedException();
		}

		public abstract object[] GetRowData(long row);
	}
}