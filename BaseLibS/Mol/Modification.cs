using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BaseLibS.Mol{
	public class Modification : StorableItem{
		private double deltaMass = double.NaN;
		private ModificationSite[] sites = new ModificationSite[0];
		private Dictionary<char, ModificationSite> sitesMap;

		[XmlAttribute("reporterCorrectionM2")]
		public double ReporterCorrectionM2 { get; set; }

		[XmlAttribute("reporterCorrectionM1")]
		public double ReporterCorrectionM1 { get; set; }

		[XmlAttribute("reporterCorrectionP1")]
		public double ReporterCorrectionP1 { get; set; }

		[XmlAttribute("reporterCorrectionP2")]
		public double ReporterCorrectionP2 { get; set; }

		[XmlAttribute("reporterCorrectionType")]
		public bool ReporterCorrectionType { get; set; }

		/// <summary>
		/// Monoisotopic mass
		/// </summary>
		[XmlAttribute("delta_mass"), XmlIgnore]
		public double DeltaMass{
			get{
				if (double.IsNaN(deltaMass)){
					deltaMass = ChemElements.GetMassFromComposition(Composition);
				}
				return deltaMass;
			}
			set => deltaMass = value;
		}

		[XmlAttribute("composition")]
		public string Composition { get; set; }

		[XmlAttribute("filename"), XmlIgnore]
		public string Filename { get; set; }

		/// <summary>
		/// Equivalent Unimod id
		/// </summary>
		[XmlAttribute("unimod")]
		public string Unimod { get; set; }

		/// <summary>
		/// Position of Modification
		/// </summary>
		[XmlElement("position", typeof (ModificationPosition))]
		public ModificationPosition Position { get; set; } = ModificationPosition.anywhere;

		[XmlElement("modification_site")]
		public ModificationSite[] Sites{
			set{
				sites = value ?? new ModificationSite[0];
				sitesMap = new Dictionary<char, ModificationSite>();
				foreach (ModificationSite modificationSite in sites){
					sitesMap.Add(modificationSite.Aa, modificationSite);
				}
			}
			get => sites;
		}

		/// <summary>
		/// Determines if this is a standard modification, a label or an isobaric label, etc
		/// </summary>
		[XmlElement("type", typeof (ModificationType))]
		public ModificationType ModificationType { get; set; } = ModificationType.Standard;

		[XmlElement("terminus_type", typeof (NewTerminusType))]
		public NewTerminusType NewTerminusType { get; set; } = NewTerminusType.none;

		public int AaCount => sites.Length;
		public string Abbreviation => Name.Substring(0, 2).ToLower();
		public bool IsPhosphorylation => Math.Abs(deltaMass - 79.96633) < 0.0001;

		public bool IsInternal
			=>
				Position == ModificationPosition.anywhere || Position == ModificationPosition.notNterm ||
				Position == ModificationPosition.notCterm || Position == ModificationPosition.notTerm;

		public bool IsNterminal => Position == ModificationPosition.anyNterm || Position == ModificationPosition.proteinNterm;
		public bool IsCterminal => Position == ModificationPosition.anyCterm || Position == ModificationPosition.proteinCterm;

		public bool IsNterminalStep
			=>
				Position == ModificationPosition.anyNterm || Position == ModificationPosition.proteinNterm ||
				Position == ModificationPosition.anywhere || Position == ModificationPosition.notCterm;

		public bool IsCterminalStep
			=>
				Position == ModificationPosition.anyCterm || Position == ModificationPosition.proteinCterm ||
				Position == ModificationPosition.anywhere || Position == ModificationPosition.notNterm;

		public bool IsProteinTerminal
			=> Position == ModificationPosition.proteinNterm || Position == ModificationPosition.proteinCterm;

		public ModificationSite GetSite(char aa){
			return sitesMap.ContainsKey(aa) ? sitesMap[aa] : null;
		}

		public override bool Equals(object obj){
			if (this == obj){
				return true;
			}
			if (obj is Modification){
				return (((Modification) obj).Name != Name);
			}
			return false;
		}

		public override int GetHashCode(){
			return Name.GetHashCode();
		}

		public bool HasAa(char aa){
			foreach (ModificationSite x in sites){
				if (x.Aa == aa){
					return true;
				}
			}
			return false;
		}

		public char GetAaAt(int j){
			return sites[j].Aa;
		}

		public static string[] ToStrings(Modification[] mods){
			string[] result = new string[mods.Length];
			for (int i = 0; i < mods.Length; i++){
				result[i] = mods[i].Name;
			}
			return result;
		}

		public override string ToString(){
			return Name;
		}

		public static Dictionary<char, ushort> ToDictionary(Modification[] modifications){
			Dictionary<char, ushort> result = new Dictionary<char, ushort>();
			foreach (Modification modification in modifications.Where(modification => modification.IsInternal)){
				for (int i = 0; i < modification.AaCount; i++){
					char c = modification.GetAaAt(i);
					if (result.ContainsKey(c)){
						throw new ArgumentException("Conflicting modifications.");
					}
					result.Add(c, modification.Index);
				}
			}
			return result;
		}

		public string GetFormula(){
			string formula = Composition;
			formula = formula.Replace("(", "");
			formula = formula.Replace(")", "");
			formula = formula.Replace(" ", "");
			formula = formula.Trim();
			return formula;
		}

		public bool HasNeutralLoss{
			get{
				foreach (ModificationSite site in sites){
					if (site.HasNeutralLoss){
						return true;
					}
				}
				return false;
			}
		}

		public bool IsIsotopicLabel{
			get{
				if (ModificationType == ModificationType.IsobaricLabel || IsStandardVarMod(ModificationType)){
					return false;
				}
				return IsIsotopicMod;
			}
		}

		public bool IsIsotopicMod{
			get{
				Tuple<Molecule, Molecule> x = Molecule.GetDifferences(new Molecule(), new Molecule(GetFormula()));
				Molecule labelingDiff1 = x.Item1;
				Molecule labelingDiff2 = x.Item2;
				if (!labelingDiff1.IsIsotopicLabel && !labelingDiff2.IsIsotopicLabel){
					return false;
				}
				Molecule d1 = labelingDiff1.GetUnlabeledVersion();
				Molecule d2 = labelingDiff2.GetUnlabeledVersion();
				Tuple<Molecule, Molecule> d = Molecule.GetDifferences(d1, d2);
				return d.Item1.IsEmpty && d.Item2.IsEmpty;
			}
		}

		public static bool IsStandardVarMod(ushort m){
			return m < Tables.ModificationList.Length && IsStandardVarMod(Tables.ModificationList[m].ModificationType);
		}

		public static bool IsStandardVarMod(ModificationType type){
			switch (type){
				case ModificationType.Standard:
				case ModificationType.AaSubstitution:
				case ModificationType.Glycan:
				case ModificationType.CleavedCrosslink:
					return true;
			}
			return false;
		}

		public Molecule GetMolecule(){
			return new Molecule(GetFormula());
		}

		public static bool IsIsobaricLabelMod(ushort m){
			if (m >= Tables.ModificationList.Length){
				return false;
			}
			return Tables.ModificationList[m].ModificationType == ModificationType.IsobaricLabel;
		}
	}
}