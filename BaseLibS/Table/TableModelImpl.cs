using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BaseLibS.Table{
	/// <summary>
	/// Partial implementation of <code>ITableModel</code>, implementing all functionality that is shared 
	/// between the full implementations of <code>ITableModel</code>.
	/// </summary>
	[Serializable]
	public abstract class TableModelImpl : ITableModel, ISerializable{
		public string Name { get; set; }
		public string Description { get; set; }
		protected readonly List<string> columnNames = new List<string>();
		private readonly List<int> columnWidths = new List<int>();
		protected readonly List<ColumnType> columnTypes = new List<ColumnType>();
		private readonly List<RenderTableCell> cellRenderers = new List<RenderTableCell>();
		private readonly List<string> columnDescriptions = new List<string>();
		private readonly List<string> annotationRowNames = new List<string>();
		private readonly List<string> annotationRowDescriptions = new List<string>();
		private readonly Collection<DataAnnotationRow> annotationRows = new Collection<DataAnnotationRow>();
		protected readonly Dictionary<string, int> nameMapping = new Dictionary<string, int>();

		protected TableModelImpl(string name, string description){
			Name = name;
			Description = description;
		}

		protected TableModelImpl(SerializationInfo info, StreamingContext ctxt){
			Name = info.GetString("Name");
			Description = info.GetString("Description");
			columnNames = (List<string>) info.GetValue("columnNames", typeof (List<string>));
			columnWidths = (List<int>) info.GetValue("columnWidths", typeof (List<int>));
			columnTypes = (List<ColumnType>) info.GetValue("columnTypes", typeof (List<ColumnType>));
			cellRenderers = (List<RenderTableCell>) info.GetValue("cellRenderers", typeof (List<RenderTableCell>));
			columnDescriptions = (List<string>) info.GetValue("columnDescriptions", typeof (List<string>));
			annotationRowNames = (List<string>) info.GetValue("annotationRowNames", typeof (List<string>));
			annotationRowDescriptions = (List<string>) info.GetValue("annotationRowDescriptions", typeof (List<string>));
			annotationRows =
				(Collection<DataAnnotationRow>) info.GetValue("annotationRows", typeof (Collection<DataAnnotationRow>));
			nameMapping = (Dictionary<string, int>) info.GetValue("nameMapping", typeof (Dictionary<string, int>));
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context){
			info.AddValue("Name", Name);
			info.AddValue("Description", Description);
			info.AddValue("columnNames", columnNames, typeof (List<string>));
			info.AddValue("columnWidths", columnWidths, typeof (List<int>));
			info.AddValue("columnTypes", columnTypes, typeof (List<ColumnType>));
			info.AddValue("cellRenderers", cellRenderers, typeof (List<RenderTableCell>));
			info.AddValue("columnDescriptions", columnDescriptions, typeof (List<string>));
			info.AddValue("annotationRowNames", annotationRowNames, typeof (List<string>));
			info.AddValue("annotationRowDescriptions", annotationRowDescriptions, typeof (List<string>));
			info.AddValue("annotationRows", annotationRows, typeof (Collection<DataAnnotationRow>));
			info.AddValue("nameMapping", nameMapping, typeof (Dictionary<string, int>));
		}

		public RenderTableCell GetColumnRenderer(int col){
			return cellRenderers[col];
		}

		public int ColumnCount => columnNames.Count;

		public int GetColumnWidth(int col){
			return columnWidths[col];
		}

		public string GetColumnName(int col){
			return columnNames[col];
		}

		public bool IsColumnEditable(int column){
			return false;
		}

		public string GetColumnDescription(int col){
			return columnDescriptions[col];
		}

		public ColumnType GetColumnType(int col){
			return col < 0 ? ColumnType.Text : columnTypes[col];
		}

		public int GetColumnIndex(string colName){
			return nameMapping.ContainsKey(colName) ? nameMapping[colName] : -1;
		}

		public object GetEntry(long row, string colname){
			int colInd = GetColumnIndex(colname);
			return colInd < 0 ? null : GetEntry(row, colInd);
		}

		public void SetEntry(int row, string colname, object value){
			int colInd = GetColumnIndex(colname);
			if (colInd < 0){
				return;
			}
			SetEntry(row, colInd, value);
		}

		public abstract long RowCount { get; }
		public abstract object GetEntry(long row, int column);
		public abstract void SetEntry(long row, int column, object value);

		public DataAnnotationRow NewAnnotationRow(){
			return new DataAnnotationRow(columnNames.Count, nameMapping);
		}

		public void AddAnnotationRow(DataAnnotationRow row, string name, string description){
			annotationRows.Add(row);
			annotationRowNames.Add(name);
			annotationRowDescriptions.Add(description);
		}

		public int AnnotationRowsCount => annotationRowNames.Count;

		public string GetAnnotationRowName(int index){
			return annotationRowNames[index];
		}

		public string GetAnnotationRowDescription(int index){
			return annotationRowDescriptions[index];
		}

		public object GetAnnotationRowValue(int index, int column){
			return annotationRows[index][column];
		}

		public object GetAnnotationRowValue(int index, string colname){
			return GetAnnotationRowValue(index, GetColumnIndex(colname));
		}

		public void AddColumn(string colName, int width, ColumnType columnType, string description) {
			AddColumn(colName, width, columnType, description, null);
		}

		public void AddColumn(string colName, int width, ColumnType columnType) {
			AddColumn(colName, width, columnType, null, null);
		}

		public void AddColumn(string colName, int width, ColumnType columnType, string description, RenderTableCell renderer){
			if (nameMapping.ContainsKey(colName)){
				return;
			}
			nameMapping.Add(colName, columnNames.Count);
			columnNames.Add(colName);
			columnWidths.Add(width);
			columnTypes.Add(columnType);
			columnDescriptions.Add(description);
			cellRenderers.Add(renderer);
		}

		public static string ColumnTypeToString(ColumnType ct){
			switch (ct){
				case ColumnType.Boolean:
					return "C";
				case ColumnType.Categorical:
					return "C";
				case ColumnType.Color:
					return "C";
				case ColumnType.DateTime:
					return "T";
				case ColumnType.DashStyle:
					return "C";
				case ColumnType.Integer:
					return "N";
				case ColumnType.MultiInteger:
					return "M";
				case ColumnType.MultiNumeric:
					return "M";
				case ColumnType.Numeric:
					return "N";
				case ColumnType.Text:
					return "T";
				default:
					return "T";
			}
		}
	}
}