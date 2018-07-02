using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Mol {
	public class Tables {
		private static readonly object lockMods = new object();
		private static Dictionary<string, SequenceDatabase> databases;
		private static Dictionary<string, CrossLinker> crossLinkers;
		private static Dictionary<string, Enzyme> enzymes;
		private static Dictionary<string, Modification> modifications;
		private static Dictionary<string, Modification> labelModifications;
		private static Dictionary<string, Modification> isobaricLabelModifications;
		private static List<Modification> neucodeModificationList;
		private static Modification[] modificationList;
		internal static Dictionary<string, SequenceDatabase> Databases => databases ?? (databases = ReadDatabases());
		public static Dictionary<string, Enzyme> Enzymes => enzymes ?? (enzymes = ReadProteases());
		public static Dictionary<string, CrossLinker> CrossLinkers => crossLinkers ?? (crossLinkers = ReadCrossLinks());

		public static Dictionary<string, Modification> Modifications {
			get {
				lock (lockMods) {
					if (modifications != null && modifications.Count == 0) {
						modifications = null;
					}
					return modifications ?? (modifications = ReadModifications(out modificationList, out labelModifications,
						       out isobaricLabelModifications, out neucodeModificationList));
				}
			}
		}

		public static Dictionary<string, Modification> LabelModifications {
			get {
				lock (lockMods) {
					if (labelModifications == null) {
						modifications = ReadModifications(out modificationList, out labelModifications, out isobaricLabelModifications,
							out neucodeModificationList);
					}
					return labelModifications;
				}
			}
		}

		public static List<Modification> NeucodeModifications {
			get {
				lock (lockMods) {
					if (neucodeModificationList == null) {
						modifications = ReadModifications(out modificationList, out labelModifications, out isobaricLabelModifications,
							out neucodeModificationList);
					}
					return neucodeModificationList;
				}
			}
		}

		public static Dictionary<string, Modification> IsobaricLabelModifications {
			get {
				lock (lockMods) {
					if (isobaricLabelModifications == null) {
						modifications = ReadModifications(out modificationList, out labelModifications, out isobaricLabelModifications,
							out neucodeModificationList);
					}
					return isobaricLabelModifications;
				}
			}
		}

		public static Modification[] ModificationList {
			get {
				lock (lockMods) {
					if (modificationList == null) {
						modifications = ReadModifications(out modificationList, out labelModifications, out isobaricLabelModifications,
							out neucodeModificationList);
					}
					return modificationList;
				}
			}
		}

		public static void ClearModifications() {
			lock (lockMods) {
				modifications = null;
				labelModifications = null;
				isobaricLabelModifications = null;
				modificationList = null;
			}
		}

		internal static bool ContainsDatabase(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return Databases.ContainsKey(s);
		}

		internal static string GetIdentifierParseRule(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return !Databases.ContainsKey(s) ? ">(.*)" : Databases[s].IdentifierParseRule;
		}

		internal static string GetDescriptionParseRule(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return !Databases.ContainsKey(s) ? "" : Databases[s].DescriptionParseRule;
		}

		internal static string GetTaxonomyParseRule(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return !Databases.ContainsKey(s) ? "" : Databases[s].TaxonomyParseRule;
		}

		internal static string GetVariationParseRule(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return !Databases.ContainsKey(s) ? "" : Databases[s].VariationParseRule;
		}

		internal static string GetModificationParseRule(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return !Databases.ContainsKey(s) ? "" : Databases[s].ModificationParseRule;
		}

		internal static string GetTaxonomyId(string path) {
			string s = path.Contains(Path.DirectorySeparatorChar)
				? path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1)
				: path;
			return !Databases.ContainsKey(s) ? "" : Databases[s].Taxid;
		}

		public static string[] GetEnzymes() {
			string[] tmp = Enzymes.Keys.ToArray();
			Array.Sort(tmp);
			return tmp;
		}

		public static string[] GetCrossLinkers() {
			string[] tmp = CrossLinkers.Keys.ToArray();
			Array.Sort(tmp);
			return tmp;
		}

		public static string[] GetDatabasesO() {
			string[] tmp = Databases.Keys.ToArray();
			Array.Sort(tmp);
			return tmp;
		}

		public static string[] GetModifications() {
			string[] result = Modifications.Keys.ToArray();
			Array.Sort(result);
			return result;
		}

		public static string[] GetLabelModifications() {
			string[] result = LabelModifications.Keys.ToArray();
			Array.Sort(result);
			return result;
		}

		public static string[] GetNeucodeModifications() {
			string[] result = new string[NeucodeModifications.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = NeucodeModifications[i].Name;
			}
			return result;
		}

		public static string[] GetNonlabelModifications() {
			List<string> result1 = new List<string>();
			List<string> result2 = new List<string>();
			List<string> result3 = new List<string>();
			List<string> result4 = new List<string>();
			foreach (string m in Modifications.Keys) {
				if (Modifications[m].ModificationType == ModificationType.Standard) {
					result1.Add(m);
				}
				if (Modifications[m].ModificationType == ModificationType.Glycan) {
					result2.Add(m);
				}
				if (Modifications[m].ModificationType == ModificationType.CleavedCrosslink) {
					result3.Add(m);
				}
				if (Modifications[m].ModificationType == ModificationType.AaSubstitution) {
					result4.Add(m);
				}
			}
			result1.Sort();
			result2.Sort();
			result3.Sort();
			result4.Sort();
			return ArrayUtils.Concat(ArrayUtils.Concat(ArrayUtils.Concat(result1, result2), result3), result4);
		}

		public static string[] GetCleavedCrosslinkModifications() {
			List<string> result3 = new List<string>();
			foreach (string m in Modifications.Keys) {
				if (Modifications[m].ModificationType == ModificationType.CleavedCrosslink) {
					result3.Add(m);
				}
			}
			result3.Sort();
			return result3.ToArray();
		}

		public static Enzyme[] ToEnzymes(string[] enz) {
			Enzyme[] result = new Enzyme[enz.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = ToEnzyme(enz[i]);
			}
			return result;
		}

		public static Enzyme ToEnzyme(string enz) {
			return enz.ToLower().Equals("unspecific") ? null : Enzymes[enz];
		}

		private static Dictionary<string, Modification> ReadModifications(out Modification[] modList,
			out Dictionary<string, Modification> labelMods, out Dictionary<string, Modification> isobaricLabelMods,
			out List<Modification> neucodeLabelMods) {
			Dictionary<string, Modification> result = new Dictionary<string, Modification>();
			modList = null;
			labelMods = new Dictionary<string, Modification>();
			isobaricLabelMods = new Dictionary<string, Modification>();
			neucodeLabelMods = new List<Modification>();
			Modification[] list = ReadModificationList();
			if (list != null) {
				modList = list;
				foreach (Modification mod in list) {
					if (mod.Sites == null) {
						throw new Exception("No specificity is defined for modification " + mod.Name + ".");
					}
					if (!result.ContainsKey(mod.Name)) {
						result.Add(mod.Name, mod);
					}
					if (mod.ModificationType == ModificationType.Label && !labelMods.ContainsKey(mod.Name)) {
						labelMods.Add(mod.Name, mod);
					}
					if (mod.ModificationType == ModificationType.IsobaricLabel && !isobaricLabelMods.ContainsKey(mod.Name)) {
						isobaricLabelMods.Add(mod.Name, mod);
					}
					if (mod.ModificationType == ModificationType.NeuCodeLabel) {
						neucodeLabelMods.Add(mod);
					}
				}
			}
			return result;
		}

		public static Modification[] ReadModificationList() {
			string[] files = GetModificationFiles();
			ushort index = 0;
			List<Modification> result = new List<Modification>();
			HashSet<string> takenNames = new HashSet<string>();
			foreach (string file in files) {
				Modification[] mods = ReadModificationList(file);
				foreach (Modification mod in mods) {
					if (!takenNames.Contains(mod.Name)) {
						mod.Index = index;
						mod.Filename = Path.GetFileName(file);
						index++;
						result.Add(mod);
						takenNames.Add(mod.Name);
					}
				}
			}
			return result.ToArray();
		}

		public static string[] GetModificationFiles() {
			string folder = FileUtils.GetConfigPath();
			string[] allFiles = Directory.GetFiles(folder);
			Array.Sort(allFiles);
			List<string> result = new List<string> {Path.Combine(FileUtils.GetConfigPath(), "modifications.xml")};
			foreach (string file1 in allFiles) {
				string file = Path.GetFileName(file1);
				string f1 = file.ToUpper();
				if (f1.Equals("MODIFICATIONS.XML")) {
					continue;
				}
				if (f1.StartsWith("MODIFICATIONS") && f1.EndsWith(".XML")) {
					result.Add(Path.Combine(FileUtils.GetConfigPath(), file));
				}
			}
			return result.ToArray();
		}

		public static Modification[] ReadModificationList(string filename) {
			if (!File.Exists(filename)) {
				return new Modification[0];
			}
			ModificationList list = (ModificationList) XmlSerialization.DeserializeObject(filename, typeof(ModificationList));
			return list != null ? list.Modifications : new Modification[0];
		}

		private static Dictionary<string, CrossLinker> ReadCrossLinks() {
			Dictionary<string, CrossLinker> crosslinkers = new Dictionary<string, CrossLinker>();
			CrossLinker[] crosslinkerList = ReadCrossLinkList();
			if (crosslinkerList != null) {
				foreach (CrossLinker crosslinker in crosslinkerList) {
					crosslinkers.Add(crosslinker.Name, crosslinker);
				}
			}
			return crosslinkers;
		}

		public static CrossLinker[] ReadCrossLinkList() {
			string filename = Path.Combine(FileUtils.GetConfigPath(), "crosslinks.xml");
			if (!File.Exists(filename)) {
				return new CrossLinker[0];
			}
			CrossLinkerList cllist = (CrossLinkerList) XmlSerialization.DeserializeObject(filename, typeof(CrossLinkerList));
			CrossLinker[] clArr = cllist != null ? cllist.Crosslinks : new CrossLinker[0];
			for (int i = 0; i < clArr.Length; i++) {
				clArr[i].Index = (ushort) i;
			}
			return clArr;
		}

		private static Dictionary<string, Enzyme> ReadProteases() {
			Dictionary<string, Enzyme> proteases = new Dictionary<string, Enzyme>();
			foreach (Enzyme enzyme in ReadProteaseList()) {
				proteases.Add(enzyme.Name, enzyme);
			}
			return proteases;
		}

		public static List<Enzyme> ReadProteaseList() {
			List<Enzyme> proteases = new List<Enzyme>();
			string filename = Path.Combine(FileUtils.GetConfigPath(), "enzymes.xml");
			if (!File.Exists(filename)) {
				return proteases;
			}
			EnzymeList prot = (EnzymeList) XmlSerialization.DeserializeObject(filename, typeof(EnzymeList));
			if (prot != null) {
				for (int i = 0; i < prot.Enzymes.Length; i++) {
					Enzyme enzyme = prot.Enzymes[i];
					enzyme.Index = (ushort) i;
					proteases.Add(enzyme);
				}
			}
			return proteases;
		}

		private static Dictionary<string, SequenceDatabase> ReadDatabases() {
			Dictionary<string, SequenceDatabase> dbs = new Dictionary<string, SequenceDatabase>();
			SequenceDatabase[] dbx = ReadDatabaseList();
			foreach (SequenceDatabase db in dbx) {
				db.Filename = db.Filename.Trim();
				if (dbs.ContainsKey(db.Filename)) {
					dbs.Remove(db.Filename);
				}
				dbs.Add(db.Filename, db);
			}
			return dbs;
		}

		public static SequenceDatabase[] ReadDatabaseList() {
			List<SequenceDatabase> dbs = new List<SequenceDatabase>();
			string filename = Path.Combine(FileUtils.GetConfigPath(), "databases.xml");
			if (!File.Exists(filename)) {
				return dbs.ToArray();
			}
			SequenceDatabaseList prot =
				(SequenceDatabaseList) XmlSerialization.DeserializeObject(filename, typeof(SequenceDatabaseList));
			if (prot != null) {
				for (int i = 0; i < prot.SequenceDatabases.Length; i++) {
					SequenceDatabase sd = prot.SequenceDatabases[i];
					sd.Index = (ushort) i;
					dbs.Add(sd);
				}
			}
			return dbs.ToArray();
		}

		public static Modification[] ToModifications(IList<string> modNames) {
			Modification[] result = new Modification[modNames.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = Modifications[modNames[i]];
			}
			return result;
		}

		public static Modification[] ToModifications(IList<ushort> mods) {
			Modification[] result = new Modification[mods.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = ModificationList[mods[i]];
			}
			return result;
		}

		public static ushort[] ToModificationIds(string[] modNames) {
			ushort[] result = new ushort[modNames.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = Modifications[modNames[i]].Index;
			}
			return result;
		}

		public static ushort[] ToModificationIds(Modification[] mods) {
			ushort[] result = new ushort[mods.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = mods[i].Index;
			}
			return result;
		}

		public static ushort[] FromStringsToInds(string[] mods) {
			ushort[] result = new ushort[mods.Length];
			Modification[] m = FromStrings(mods);
			for (int i = 0; i < result.Length; i++) {
				result[i] = m[i].Index;
			}
			return result;
		}

		public static string[] GetIdentifierParseRules(FastaFileInfo[] fastaFiles) {
			string[] parseRules = new string[fastaFiles.Length];
			for (int j = 0; j < parseRules.Length; j++) {
				parseRules[j] = fastaFiles[j].identifierParseRule;
			}
			return parseRules;
		}

		public static string[] GetDescriptionParseRules(FastaFileInfo[] fastaFiles) {
			string[] parseRules = new string[fastaFiles.Length];
			for (int j = 0; j < parseRules.Length; j++) {
				parseRules[j] = fastaFiles[j].descriptionParseRule;
			}
			return parseRules;
		}

		public static string[] GetTaxonomyParseRules(FastaFileInfo[] fastaFiles) {
			string[] parseRules = new string[fastaFiles.Length];
			for (int j = 0; j < parseRules.Length; j++) {
				parseRules[j] = fastaFiles[j].taxonomyParseRule;
			}
			return parseRules;
		}

		public static string[] GetVariationParseRules(FastaFileInfo[] fastaFiles) {
			string[] parseRules = new string[fastaFiles.Length];
			for (int j = 0; j < parseRules.Length; j++) {
				parseRules[j] = fastaFiles[j].variationParseRule;
			}
			return parseRules;
		}

		public static string[] GetModificationParseRules(FastaFileInfo[] fastaFiles) {
			string[] parseRules = new string[fastaFiles.Length];
			for (int j = 0; j < parseRules.Length; j++) {
				parseRules[j] = fastaFiles[j].modificationParseRule;
			}
			return parseRules;
		}

		public static void ClearEnzymes() {
			enzymes = null;
		}

		public static void ClearDatabases() {
			databases = null;
		}

		public static void ClearCrosslinks() {
			crossLinkers = null;
		}

		public static Modification[] FromStrings(string[] mods) {
			if (mods == null) {
				return new Modification[0];
			}
			Modification[] result = new Modification[mods.Length];
			for (int i = 0; i < mods.Length; i++) {
				result[i] = Modifications[mods[i]];
			}
			return result;
		}

		public static Modification[] FromStrings(string[][] mods) {
			HashSet<Modification> result = new HashSet<Modification>();
			foreach (Modification modification in mods.Select(FromStrings).SelectMany(q => q)) {
				result.Add(modification);
			}
			return ArrayUtils.ToArray(result);
		}

		// Returns first match of Abbreviation
		public static Modification[] FromAbbreviation(string[] ab) {
			string[] a = new string[ModificationList.Length];
			for (int i = 0; i < ModificationList.Length; i++) {
				a[i] = ModificationList[i].Abbreviation;
			}
			Modification[] result = new Modification[ab.Length];
			for (int i = 0; i < ab.Length; i++) {
				int index = Array.IndexOf(a, ab[i]);
				if (index != -1) {
					result[i] = ModificationList[index];
				}
			}
			return result;
		}

		public static Modification[] FromIndices(ushort[] mods) {
			Modification[] result = new Modification[mods.Length];
			for (int i = 0; i < mods.Length; i++) {
				result[i] = ModificationList[mods[i]];
			}
			return result;
		}
	}
}