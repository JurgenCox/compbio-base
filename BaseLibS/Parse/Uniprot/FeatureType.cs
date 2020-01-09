using System;
using System.Collections.Generic;
using BaseLibS.Num;

namespace BaseLibS.Parse.Uniprot{
	[Serializable]
	public class FeatureType : IComparable<FeatureType>{
		public static FeatureType chain = new FeatureType("chain");
		public static FeatureType compositionallyBiasedRegion = new FeatureType("compositionally biased region");
		public static FeatureType signalPeptide = new FeatureType("signal peptide");
		public static FeatureType glycosylationSite = new FeatureType("glycosylation site");
		public static FeatureType site = new FeatureType("site");
		public static FeatureType modifiedResidue = new FeatureType("modified residue");
		public static FeatureType transmembraneRegion = new FeatureType("transmembrane region");
		public static FeatureType sequenceConflict = new FeatureType("sequence conflict");
		public static FeatureType activeSite = new FeatureType("active site");
		public static FeatureType topologicalDomain = new FeatureType("topological domain");
		public static FeatureType domain = new FeatureType("domain");
		public static FeatureType regionOfInterest = new FeatureType("region of interest");
		public static FeatureType disulfideBond = new FeatureType("disulfide bond");
		public static FeatureType sequenceVariant = new FeatureType("sequence variant");
		public static FeatureType repeat = new FeatureType("repeat");
		public static FeatureType propeptide = new FeatureType("propeptide");
		public static FeatureType initiatorMethionine = new FeatureType("initiator methionine");
		public static FeatureType bindingSite = new FeatureType("binding site");
		public static FeatureType shortSequenceMotif = new FeatureType("short sequence motif");
		public static FeatureType metalIonBindingSite = new FeatureType("metal ion-binding site");
		public static FeatureType transitPeptide = new FeatureType("transit peptide");
		public static FeatureType nucleotidePhosphateBindingRegion = new FeatureType("nucleotide phosphate-binding region");
		public static FeatureType spliceVariant = new FeatureType("splice variant");
		public static FeatureType lipidMoietyBindingRegion = new FeatureType("lipid moiety-binding region");
		public static FeatureType nonTerminalResidue = new FeatureType("non-terminal residue");
		public static FeatureType mutagenesisSite = new FeatureType("mutagenesis site");
		public static FeatureType helix = new FeatureType("helix");
		public static FeatureType strand = new FeatureType("strand");
		public static FeatureType turn = new FeatureType("turn");
		public static FeatureType coiledCoilRegion = new FeatureType("coiled-coil region");
		public static FeatureType dnaBindingRegion = new FeatureType("dna-binding region");
		public static FeatureType peptide = new FeatureType("peptide");
		public static FeatureType zincFingerRegion = new FeatureType("zinc finger region");
		public static FeatureType calciumBindingRegion = new FeatureType("calcium-binding region");
		public static FeatureType crossLink = new FeatureType("cross-link");
		public static FeatureType unsureResidue = new FeatureType("unsure residue");
		public static FeatureType nonConsecutiveResidues = new FeatureType("non-consecutive residues");
		public static FeatureType intramembraneRegion = new FeatureType("intramembrane region");
		public static FeatureType nonStandardAminoAcid = new FeatureType("non-standard amino acid");
		public static string[] allFeatureTypeStrings;
		public static FeatureType[] allFeatureTypes = CreateFeatureTypeList();

		private static FeatureType[] CreateFeatureTypeList(){
			List<FeatureType> ft = new List<FeatureType>{
				chain,
				compositionallyBiasedRegion,
				signalPeptide,
				glycosylationSite,
				site,
				modifiedResidue,
				transmembraneRegion,
				sequenceConflict,
				activeSite,
				topologicalDomain,
				domain,
				regionOfInterest,
				disulfideBond,
				sequenceVariant,
				repeat,
				propeptide,
				initiatorMethionine,
				bindingSite,
				shortSequenceMotif,
				metalIonBindingSite,
				transitPeptide,
				nucleotidePhosphateBindingRegion,
				spliceVariant,
				lipidMoietyBindingRegion,
				nonTerminalResidue,
				mutagenesisSite,
				helix,
				strand,
				turn,
				coiledCoilRegion,
				dnaBindingRegion,
				peptide,
				zincFingerRegion,
				calciumBindingRegion,
				crossLink,
				unsureResidue,
				nonConsecutiveResidues,
				intramembraneRegion,
				nonStandardAminoAcid
			};
			allFeatureTypeStrings = new string[ft.Count];
			for (int i = 0; i < allFeatureTypeStrings.Length; i++){
				allFeatureTypeStrings[i] = ft[i].UniprotName;
			}
			int[] o = ArrayUtils.Order(allFeatureTypeStrings);
			allFeatureTypeStrings = ArrayUtils.SubArray(allFeatureTypeStrings, o);
			FeatureType[] result = new FeatureType[ft.Count];
			for (int i = 0; i < result.Length; i++){
				result[i] = ft[o[i]];
				result[i].Index = i;
			}
			return result;
		}

		public static FeatureType GetFeatureType(string s){
			string q = s.ToLower();
			int index = Array.BinarySearch(allFeatureTypeStrings, q);
			if (index < 0){
				throw new Exception("Unknown feature type: " + s);
			}
			return allFeatureTypes[index];
		}

		public string UniprotName { get; set; }
		public int Index { get; set; }
		public FeatureType(string uniprotName) { UniprotName = uniprotName; }
		public int CompareTo(FeatureType other) { return Index.CompareTo(other.Index); }

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == typeof (FeatureType) && Equals((FeatureType) obj);
		}

		public bool Equals(FeatureType other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return other.Index == Index;
		}

		public override int GetHashCode() { return Index; }
	}
}