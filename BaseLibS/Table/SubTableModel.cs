using System;

namespace BaseLibS.Table{
	public class SubTableModel : ITableModel{
		private readonly ITableModel baseModel;
		private readonly int[] rows;
		private readonly int[] columns;
		public SubTableModel(ITableModel baseModel, int[] rows) : this(baseModel, rows, null) {}

		public SubTableModel(ITableModel baseModel, int[] rows, int[] columns){
			this.baseModel = baseModel;
			this.rows = rows;
			this.columns = columns;
		}

		public RenderTableCell GetColumnRenderer(int column){
			return baseModel.GetColumnRenderer(columns?[column] ?? column);
		}

		public long RowCount => rows.Length;
		public int ColumnCount => columns?.Length ?? baseModel.ColumnCount;
		public string Name => baseModel.Name;
		public string Description => baseModel.Description;

		public string GetColumnName(int column) {
			return baseModel.GetColumnName(columns?[column] ?? column);
		}

		public bool IsColumnEditable(int column) {
			return baseModel.IsColumnEditable(columns?[column] ?? column);
		}

		public string GetColumnDescription(int column) {
			return baseModel.GetColumnDescription(columns?[column] ?? column);
		}

		public ColumnType GetColumnType(int column){
			return baseModel.GetColumnType(columns?[column] ?? column);
		}

		public int GetColumnWidth(int column){
			return baseModel.GetColumnWidth(columns?[column] ?? column);
		}

		public object GetEntry(long row, int column){
			return baseModel.GetEntry(rows[row], columns?[column] ?? column);
		}

		public object GetEntry(long row, string colname){
			return GetEntry(row, GetColumnIndex(colname));
		}

		public void SetEntry(long row, int column, object value){
			baseModel.SetEntry(rows[row], columns?[column] ?? column, value);
		}

		public int AnnotationRowsCount => baseModel.AnnotationRowsCount;

		public string GetAnnotationRowName(int index){
			return baseModel.GetAnnotationRowName(index);
		}

		public string GetAnnotationRowDescription(int index){
			return baseModel.GetAnnotationRowDescription(index);
		}

		public object GetAnnotationRowValue(int index, int column){
			return baseModel.GetAnnotationRowValue(index, columns?[column] ?? column);
		}

		public object GetAnnotationRowValue(int index, string colname){
			return GetAnnotationRowValue(index, GetColumnIndex(colname));
		}

		public int GetColumnIndex(string columnName){
			if (columns == null){
				int index = baseModel.GetColumnIndex(columnName);
				if (index == -1){
					throw new ArgumentException($"Could not find column name {columnName} in table {Name}");
				}
				return index;
			}
			for (int i = 0; i < ColumnCount; i++){
				if (GetColumnName(i).Equals(columnName)){
					return i;
				}
			}
			throw new ArgumentException($"Could not find column name {columnName} in table {Name}");
		}
	}
}