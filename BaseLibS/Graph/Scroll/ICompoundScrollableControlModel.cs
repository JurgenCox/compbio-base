namespace BaseLibS.Graph.Scroll{
	public interface ICompoundScrollableControlModel : IControlModel{
		void Register(ICompoundScrollableControl control);
		float UserSf { get; set; }
	}
}