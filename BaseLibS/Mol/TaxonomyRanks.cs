using System;

namespace BaseLibS.Mol{
	public class TaxonomyRanks{
		public static TaxonomyRank[] mainRanks = {
			TaxonomyRank.Superkingdom, TaxonomyRank.Kingdom, TaxonomyRank.Phylum, TaxonomyRank.Class, TaxonomyRank.Order,
			TaxonomyRank.Family, TaxonomyRank.Genus, TaxonomyRank.Species
		};
		public static string[] mainRankStrings = {"Domain", "Kingdom", "Phylum", "Class", "Order", "Family", "Genus", "Species"};

		public static TaxonomyRank FromString(string s){
			switch (s){
				case "Domain":
					return TaxonomyRank.Superkingdom;
				case "Kingdom":
					return TaxonomyRank.Kingdom;
				case "Phylum":
					return TaxonomyRank.Phylum;
				case "Class":
					return TaxonomyRank.Class;
				case "Order":
					return TaxonomyRank.Order;
				case "Family":
					return TaxonomyRank.Family;
				case "Genus":
					return TaxonomyRank.Genus;
				case "Species":
					return TaxonomyRank.Species;
				default:
					throw new Exception("Never get here");
			}
		}
	}
}