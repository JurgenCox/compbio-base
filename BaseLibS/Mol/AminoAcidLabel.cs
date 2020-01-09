namespace BaseLibS.Mol{
	public class AminoAcidLabel : LabelModification{
		public char Aa { get; set; }

		public AminoAcidLabel(string name, string formula, char aa) : base(name, formula){
			Aa = aa;
		}
	}
}