namespace BaseLibS.Graph.Scroll{
	public interface ICompoundScrollableControlModel : IScrollableControlModel{
		void Register(ICompoundScrollableControl control);
		float UserSf { get; set; }
	}
}