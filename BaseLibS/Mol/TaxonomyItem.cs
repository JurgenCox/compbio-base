using System.Collections.Generic;

namespace BaseLibS.Mol{
	public class TaxonomyItem{
		private readonly int parentTaxId;
		private int divisionId;
		private int geneticCodeId;
		private int mitoGeneticCodeId;
		private readonly List<string> names = new List<string>();
		private readonly List<TaxonomyNameType> nameTypes = new List<TaxonomyNameType>();

		public TaxonomyItem(int taxId, int parentTaxId, TaxonomyRank rank, int divisionId, int geneticCodeId,
			int mitoGeneticCodeId){
			this.TaxId = taxId;
			this.parentTaxId = parentTaxId;
			this.Rank = rank;
			this.divisionId = divisionId;
			this.geneticCodeId = geneticCodeId;
			this.mitoGeneticCodeId = mitoGeneticCodeId;
		}

		public TaxonomyRank Rank { get; }
		public int TaxId { get; }

		public void AddName(string name, TaxonomyNameType nameType){
			names.Add(name);
			nameTypes.Add(nameType);
		}

		public string GetScientificName(){
			for (int i = 0; i < names.Count; i++){
				if (nameTypes[i] == TaxonomyNameType.ScientificName){
					return names[i];
				}
			}
			return names[0];
		}

		public TaxonomyItem GetParentOfRank(TaxonomyRank rank1){
			if (rank1 == Rank){
				return this;
			}
			if (!TaxonomyItems.taxId2Item.ContainsKey(parentTaxId)){
				return null;
			}
			TaxonomyItem parent = TaxonomyItems.taxId2Item[parentTaxId];
			return parent.GetParentOfRank(rank1);
		}
	}
}