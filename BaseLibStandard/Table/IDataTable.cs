namespace BaseLibS.Table{
	public interface IDataTable : ITable{
		DataRow2 NewRow();
		void AddRow(DataRow2 row);
		void Clear();
		void Close();
	}
}