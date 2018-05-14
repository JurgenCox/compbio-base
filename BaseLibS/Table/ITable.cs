namespace BaseLibS.Table{
	public interface ITable{
		void AddColumn(string colName, int width, ColumnType columnType, string description);
		void AddColumn(string colName, int width, ColumnType columnType);
		string Description { get; set; }
	}
}