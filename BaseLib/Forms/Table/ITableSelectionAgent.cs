namespace BaseLib.Forms.Table{
	public interface ITableSelectionAgent{
		string Title { get; }
		void AddTable(TableView tableView);
		void RemoveTable(TableView tableView);
	}
}