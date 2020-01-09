namespace BaseLibS.Api{
	public interface INamedItem{
		/// <summary>
		/// This is the name that e.g. appears in drop-down menus.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The context help that will appear in tool tips etc. 
		/// </summary>
		string Description { get; }
	}
}