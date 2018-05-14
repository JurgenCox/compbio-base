namespace BaseLibS.Mol {
	public class FastaFileInfo {
		public string fastaFilePath;
		public string identifierParseRule;
		public string descriptionParseRule;
		public string taxonomyParseRule;
		public string variationParseRule;
		public string modificationParseRule;
		public string taxonomyId;

		public FastaFileInfo(string fastaFilePath) : this(fastaFilePath, Tables.GetIdentifierParseRule(fastaFilePath),
			Tables.GetDescriptionParseRule(fastaFilePath), Tables.GetTaxonomyParseRule(fastaFilePath),
			Tables.GetVariationParseRule(fastaFilePath), Tables.GetModificationParseRule(fastaFilePath),
			Tables.GetTaxonomyId(fastaFilePath)) { }

		public FastaFileInfo(string fastaFilePath, string identifierParseRule, string descriptionParseRule,
			string taxonomyParseRule, string variationParseRule, string modificationParseRule, string taxonomyId) {
			this.fastaFilePath = fastaFilePath;
			this.identifierParseRule = identifierParseRule;
			this.descriptionParseRule = descriptionParseRule;
			this.taxonomyParseRule = taxonomyParseRule;
			this.variationParseRule = variationParseRule;
			this.modificationParseRule = modificationParseRule;
			this.taxonomyId = taxonomyId;
		}

		public static string[] ToString(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].fastaFilePath;
			}
			return result;
		}

		public static FastaFileInfo[] ToInfo(string[] filePaths) {
			FastaFileInfo[] result = new FastaFileInfo[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = new FastaFileInfo(filePaths[i]);
			}
			return result;
		}

		public override string ToString() {
			return fastaFilePath;
		}
	}
}