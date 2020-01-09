using System;
using System.Collections.Generic;
using BaseLibS.Util;

namespace BaseLibS.Mol {
	public class Modification2 {
		private char[] aas;
		public double DeltaMass { get; set; }
		public bool IsProteinTerminal { get; set; }
		public bool IsInternal { get; set; }
		public bool IsNterminal { get; set; }
		public bool IsCterminal { get; set; }
		public bool IsIsotopicLabel { get; set; }
		public bool IsIsotopicMod { get; set; }
		public bool HasNeutralLoss { get; set; }
		public ushort Index { get; set; }
		public string Name { get; set; }
		public ModificationPosition Position { get; set; }
		public ModificationType ModificationType { get; set; }

		public Modification2(Modification m) {
			Fill(m);
		}

		public Modification2(string modName) {
			if (Tables.Modifications.ContainsKey(modName)) {
				Modification m = Tables.Modifications[modName];
				Fill(m);
			} else if (modName.StartsWith("Mass:")) {
				int ind = modName.LastIndexOf(':');
				DeltaMass = Parser.Double(modName.Substring(7, ind - 7));
				int modInd = Parser.Int(modName.Substring(ind + 1));
				Index = (ushort) (ushort.MaxValue - modInd - 1);
				Position = ModificationPosition.anywhere;
				IsProteinTerminal = false;
				IsInternal = true;
				IsNterminal = false;
				IsCterminal = false;
				IsIsotopicLabel = true;
				IsIsotopicMod = true;
				HasNeutralLoss = false;
				ModificationType = ModificationType.Label;
				aas = new[] {modName[5]};
				Name = modName;
			} else {
				throw new Exception("Illegal modification name: " + modName);
			}
		}

		private void Fill(Modification m) {
			DeltaMass = m.DeltaMass;
			Position = m.Position;
			IsProteinTerminal = m.IsProteinTerminal;
			IsInternal = m.IsInternal;
			IsNterminal = m.IsNterminal;
			IsCterminal = m.IsCterminal;
			IsIsotopicLabel = m.IsIsotopicLabel;
			IsIsotopicMod = m.IsIsotopicMod;
			ModificationType = m.ModificationType;
			HasNeutralLoss = m.HasNeutralLoss;
			aas = new char[m.AaCount];
			for (int i = 0; i < aas.Length; i++) {
				aas[i] = m.GetAaAt(i);
			}
			Index = m.Index;
			Name = m.Name;
		}

		public int AaCount => aas.Length;

		public char GetAaAt(int i) {
			return aas[i];
		}

		public static double CalcMonoisotopicMass(string sequence, Modification2[] modifications, bool isProteinNterm,
			bool isProteinCterm) {
			double m = CalcMonoisotopicMass(sequence);
			m += GetFixedModificationMass(sequence, modifications, isProteinNterm, isProteinCterm);
			return m;
		}

		public static double CalcMonoisotopicMass(string sequence) {
			double result = AminoAcids.massNormalCTerminus + AminoAcids.massNormalNTerminus;
			foreach (char aa in sequence) {
				result += AminoAcids.AaMonoMasses[aa];
			}
			return result;
		}

		public static double GetFixedModificationMass(string sequence, Modification2[] modifications, bool isProteinNterm,
			bool isProteinCterm) {
			double monoisotopicMass = 0;
			foreach (Modification2 mod in modifications) {
				monoisotopicMass += GetDeltaMass(mod, sequence, isProteinNterm, isProteinCterm);
			}
			return monoisotopicMass;
		}

		private static double GetDeltaMass(Modification2 mod, string sequence, bool isNterm, bool isCterm) {
			ModificationPosition pos = mod.Position;
			double deltaMass = 0;
			for (int i = 0; i < mod.AaCount; i++) {
				for (int j = 0; j < sequence.Length; j++) {
					if ((pos == ModificationPosition.notNterm || pos == ModificationPosition.notTerm) && j == 0) {
						continue;
					}
					if ((pos == ModificationPosition.notCterm || pos == ModificationPosition.notTerm) && j == sequence.Length - 1) {
						continue;
					}
					if (sequence[j] == mod.GetAaAt(i)) {
						deltaMass += mod.DeltaMass;
					}
				}
			}
			if (pos == ModificationPosition.anyNterm) {
				deltaMass += mod.DeltaMass;
			}
			if (pos == ModificationPosition.anyCterm) {
				deltaMass += mod.DeltaMass;
			}
			if (pos == ModificationPosition.proteinNterm && isNterm) {
				deltaMass += mod.DeltaMass;
			}
			if (pos == ModificationPosition.proteinCterm && isCterm) {
				deltaMass += mod.DeltaMass;
			}
			return deltaMass;
		}

		public static Modification2[] FromStrings(IList<string> modNames) {
			if (modNames == null) {
				return new Modification2[0];
			}
			Modification2[] result = new Modification2[modNames.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = new Modification2(modNames[i]);
			}
			return result;
		}
	}
}