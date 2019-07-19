namespace BaseLibS.Ms.Data.Protein{
	public class ProtInfo3{
		public int Length { get; }
		public string Description { get; }
		public string TaxonomyId { get; }
		public bool Contaminant { get; }
		public bool Decoy { get; }

		public ProtInfo3(Protein protein) {
			Length = protein.Length;
			Contaminant = protein.Contaminant;
			TaxonomyId = protein.TaxonomyId;
			Decoy = protein.Decoy;
			Description = protein.Description;
			if (Description.Length > 256){
				Description = Description.Substring(0, 256);
			}
		}
	}
}