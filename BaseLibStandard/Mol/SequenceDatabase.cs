using System.Xml.Serialization;

namespace BaseLibS.Mol{
	public class SequenceDatabase : StorableItem{
		/// <summary>
		/// Default Constructor for Serialization. 
		/// </summary>
		public SequenceDatabase() { }

		public SequenceDatabase(string filename, string identifierParseRule) {
			Filename = filename;
			IdentifierParseRule = identifierParseRule;
		}

		/// <summary>
		/// The filename of this database e.g. as known by Andromeda.
		/// </summary>
		[XmlAttribute("filename")]
		public string Filename { get; set; }

		/// <summary>
		/// Regular expression which describes how to parse the fasta sequence header to 
		/// obtain the protein identifier.
		/// </summary>
		[XmlAttribute("identifier_parse_rule")]
		public string IdentifierParseRule { get; set; }

		/// <summary>
		/// Regular expression which describes how to parse the fasta sequence header to 
		/// obtain the protein description.
		/// </summary>
		[XmlAttribute("description_parse_rule")]
		public string DescriptionParseRule { get; set; }

		/// <summary>
		/// Regular expression which describes how to parse the fasta sequence header to 
		/// obtain the string containing definition of mutations.
		/// </summary>
		[XmlAttribute("variation_parse_rule")]
		public string VariationParseRule { get; set; }

		/// <summary>
		/// Regular expression which describes how to parse the fasta sequence header to 
		/// obtain the string containing definition of modifications.
		/// </summary>
		[XmlAttribute("modification_parse_rule")]
		public string ModificationParseRule { get; set; }

		/// <summary>
		/// Regular expression which describes how to parse the fasta sequence header to 
		/// obtain the string containing definition of taxonomies.
		/// </summary>
		[XmlAttribute("taxonomy_parse_rule")]
		public string TaxonomyParseRule { get; set; }

		/// <summary>
		/// The human readable species of this database which should be the NCBI entry name
		/// </summary>
		[XmlAttribute("species")]
		public string Species { get; set; }

		/// <summary>
		/// The NCBI/NEWT taxonomy id for the species of this database
		/// </summary>
		[XmlAttribute("taxid")]
		public string Taxid { get; set; }

		/// <summary>
		/// Origin of this fasta file
		/// </summary>
		[XmlAttribute("source")]
		public string Source { get; set; }
	}
}