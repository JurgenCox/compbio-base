namespace BaseLibS.Mol{
	public class TerminalLabel : LabelModification{
		public bool IsNterm { get; set; }

		public TerminalLabel(string name, string formula, bool isNterm) : base(name, formula){
			IsNterm = isNterm;
		}
	}
}