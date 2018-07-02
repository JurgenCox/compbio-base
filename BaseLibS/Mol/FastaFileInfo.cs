using System;
using System.Collections.Generic;
using BaseLibS.Util;

namespace BaseLibS.Mol {
	public class FastaFileInfo : IComparable<FastaFileInfo> {
		public readonly List<InputParameter> vals = new List<InputParameter> {
			new InputParameter<string>("fastaFilePath", "fastaFilePath"),
			new InputParameter<string>("identifierParseRule", "identifierParseRule"),
			new InputParameter<string>("descriptionParseRule", "descriptionParseRule"),
			new InputParameter<string>("taxonomyParseRule", "taxonomyParseRule"),
			new InputParameter<string>("variationParseRule", "variationParseRule"),
			new InputParameter<string>("modificationParseRule", "modificationParseRule"),
			new InputParameter<string>("taxonomyId", "taxonomyId")
		};

		public readonly Dictionary<string, InputParameter> map;
		public FastaFileInfo() : this("", "", "", "", "", "", "") { }
		public string fastaFilePath;
		public string identifierParseRule;
		public string descriptionParseRule;
		public string taxonomyParseRule;
		public string taxonomyId;
		public string variationParseRule;
		public string modificationParseRule;

		public FastaFileInfo(string fastaFilePath) : this(fastaFilePath, Tables.GetIdentifierParseRule(fastaFilePath),
			Tables.GetDescriptionParseRule(fastaFilePath), Tables.GetTaxonomyParseRule(fastaFilePath),
			Tables.GetTaxonomyId(fastaFilePath), Tables.GetVariationParseRule(fastaFilePath),
			Tables.GetModificationParseRule(fastaFilePath)) { }

		public FastaFileInfo(string fastaFilePath, string identifierParseRule, string descriptionParseRule,
			string taxonomyParseRule, string taxonomyId, string variationParseRule, string modificationParseRule) {
			map = new Dictionary<string, InputParameter>();
			foreach (InputParameter val in vals) {
				map.Add(val.Name, val);
			}
			this.fastaFilePath = fastaFilePath;
			this.identifierParseRule = identifierParseRule;
			this.descriptionParseRule = descriptionParseRule;
			this.taxonomyParseRule = taxonomyParseRule;
			this.variationParseRule = variationParseRule;
			this.modificationParseRule = modificationParseRule;
			this.taxonomyId = taxonomyId;
		}

		public FastaFileInfo(string[] s) : this() {
			fastaFilePath = s[0];
			identifierParseRule = s[1];
			descriptionParseRule = s[2];
			taxonomyParseRule = s[3];
			taxonomyId = s[4];
			variationParseRule = s[5];
			modificationParseRule = s[6];
		}

		public string[] ToStringArray() {
			return new[] {
				fastaFilePath, identifierParseRule, descriptionParseRule, taxonomyParseRule, taxonomyId, variationParseRule,
				modificationParseRule
			};
		}

		public static string[] GetFastaFilePath(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].fastaFilePath;
			}
			return result;
		}

		public static string[] GetIdentifierParseRule(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].identifierParseRule;
			}
			return result;
		}

		public static string[] GetDescriptionParseRule(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].descriptionParseRule;
			}
			return result;
		}

		public static string[] GetTaxonomyParseRule(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].taxonomyParseRule;
			}
			return result;
		}

		public static string[] GetTaxonomyId(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].taxonomyId;
			}
			return result;
		}

		public static string[] GetVariationParseRule(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].variationParseRule;
			}
			return result;
		}

		public static string[] GetModificationParseRule(FastaFileInfo[] filePaths) {
			string[] result = new string[filePaths.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = filePaths[i].modificationParseRule;
			}
			return result;
		}

		public int CompareTo(FastaFileInfo other) {
			return String.Compare(fastaFilePath, other.fastaFilePath, StringComparison.InvariantCulture);
		}

		public override string ToString() {
			return fastaFilePath;
		}
	}
}