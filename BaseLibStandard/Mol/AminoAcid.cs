namespace BaseLibS.Mol{
	public class AminoAcid : Molecule{
		internal readonly bool isStandard;
		public string Abbreviation { get; }
		public string Type { get; }
		public char Letter { get; }
		public double Occurence { get; }
		public string[] Codons { get; }
		public double Gravy { get; }

		internal AminoAcid(string empiricalFormula, string name, string abbreviation, char letter, double occurence,
			string[] codons, string type, bool isStandard, double gravy) : base(empiricalFormula){
			Abbreviation = abbreviation;
			Letter = letter;
			Occurence = occurence/100.0;
			Gravy = gravy;
			Codons = codons;
			Type = type;
			this.isStandard = isStandard;
			Name = name;
		}

		public override bool Equals(object obj){
			if (obj == null){
				return false;
			}
			if (this == obj){
				return true;
			}
			AminoAcid other = obj as AminoAcid;
			return other?.Letter == Letter;
		}

		public override int GetHashCode() { return Letter + 1; }
	}
}