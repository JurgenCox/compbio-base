namespace BaseLibS.Graph{
	public interface ICompoundScrollableControlModel : IScrollableControlModel{
		void Register(ICompoundScrollableControl control, float sfx);
		float UserSf { get; set; }
	}
}