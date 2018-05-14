using System;

namespace BaseLibS.Mol {
	/// <summary>
	/// A label modification has two ficitious molecules corresponding to the 
	/// missing/extra atoms in the labeled form (LabelingDiff1/LabelingDiff2).
	/// </summary>
	public abstract class LabelModification {
		public string Name { get; }
		public Molecule LabelingDiff1 { get; }
		public Molecule LabelingDiff2 { get; }

		protected LabelModification(string name, string formula) {
			Tuple<Molecule, Molecule> x = Molecule.GetDifferences(new Molecule(), new Molecule(formula));
			Name = name;
			LabelingDiff1 = x.Item1;
			LabelingDiff2 = x.Item2;
		}

		public double DeltaMass => LabelingDiff2.MonoIsotopicMass - LabelingDiff1.MonoIsotopicMass;

		public bool IsIsotopicLabel {
			get {
				if (!LabelingDiff1.IsIsotopicLabel && !LabelingDiff2.IsIsotopicLabel) {
					return false;
				}
				Tuple<Molecule, Molecule> d = Molecule.GetDifferences(LabelingDiff1.GetUnlabeledVersion(),
					LabelingDiff2.GetUnlabeledVersion());
				return d.Item1.IsEmpty && d.Item2.IsEmpty;
			}
		}

		public bool IsInternal => this is AminoAcidLabel;

		public static LabelModification GetLabelByName(string name) {
			Modification modification = Tables.Modifications[name];
			if (modification.ModificationType == ModificationType.Label) {
				if (modification.IsInternal) {
					return new AminoAcidLabel(modification.Name, modification.GetFormula(), modification.GetAaAt(0));
				}
				return new TerminalLabel(modification.Name, modification.GetFormula(), modification.IsNterminal);
			}
			return null;
		}

		/// <summary>
		/// Given string label, return corresponding AminoAcid object, or null for a terminal modiciation.
		/// </summary>
		/// <param name="label">string label, e.g. "DimethLys2"</param>
		/// <returns>AminoAcid object, or null for a terminal modiciation</returns>
		public static AminoAcid GetAminoAcidFromLabel(string label) {
			Modification2 m = new Modification2(label);
			return m.AaCount == 0 ? null : AminoAcids.FromLetter(m.GetAaAt(0));
		}

		public static AminoAcid GetAminoAcidFromLabel(Modification2 m) {
			return m.AaCount == 0 ? null : AminoAcids.FromLetter(m.GetAaAt(0));
		}

		public override bool Equals(object obj) {
			if (obj is LabelModification) {
				LabelModification other = (LabelModification) obj;
				return other.Name.Equals(Name);
			}
			return false;
		}

		public override int GetHashCode() {
			return Name.GetHashCode();
		}

		public override string ToString() {
			return Name;
		}

		public AminoAcid GetAminoAcid() {
			if (!(this is AminoAcidLabel)) {
				return null;
			}
			return AminoAcids.FromLetter(((AminoAcidLabel) this).Aa);
		}
	}
}