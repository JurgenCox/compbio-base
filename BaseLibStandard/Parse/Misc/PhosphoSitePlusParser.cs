using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Parse.Misc{
	public static class PhosphoSitePlusParser{
		internal static readonly Dictionary<string, string> fileMap = CreateFileMap();

		private static Dictionary<string, string> CreateFileMap(){
			string folder = FileUtils.GetConfigPath() + "\\PSP\\";
			return new Dictionary<string, string>{
				{"Acetylation", folder + "Acetylation_site_dataset"},
				{"Methylation", folder + "Methylation_site_dataset"},
				{"O-GlcNAc", folder + "O-GlcNAc_site_dataset"},
				{"O-GalNAc", folder + "O-GalNAc_site_dataset"},
				{"Phosphorylation", folder + "Phosphorylation_site_dataset"},
				{"Sumoylation", folder + "Sumoylation_site_dataset"},
				{"Ubiquitination", folder + "Ubiquitination_site_dataset"}
			};
		}

		public static string[] GetAllMods(){
			return fileMap.Keys.ToArray();
		}

		public static string GetFilenameForMod(string mod){
			string filename = fileMap[mod];
			if (!File.Exists(filename)){
				if (File.Exists(filename + ".gz")){
					filename = filename + ".gz";
				} else{
					return null;
				}
			}
			return filename;
		}

		public static void ParseKnownMod(string[] mod, out string[] seqWins, out string[] accs, out string[] pubmedLtp,
			out string[] pubmedMs2, out string[] cstMs2, string species){
			string[] seqWins1;
			string[] accs1;
			string[] pubmedLtp1;
			string[] pubmedMs21;
			string[] cstMs21;
			string[] species1;
			ParseKnownMod(mod, out seqWins1, out accs1, out pubmedLtp1, out pubmedMs21, out cstMs21, out species1);
			List<int> v = new List<int>();
			for (int i = 0; i < species1.Length; i++){
				if (species1[i].Equals(species)){
					v.Add(i);
				}
			}
			seqWins = ArrayUtils.SubArray(seqWins1, v);
			accs = ArrayUtils.SubArray(accs1, v);
			pubmedLtp = ArrayUtils.SubArray(pubmedLtp1, v);
			pubmedMs2 = ArrayUtils.SubArray(pubmedMs21, v);
			cstMs2 = ArrayUtils.SubArray(cstMs21, v);
		}

		public static void ParseKnownMod(string[] mod, out string[] seqWins, out string[] accs, out string[] pubmedLtp,
			out string[] pubmedMs2, out string[] cstMs2, out string[] species){
			List<string> seqWins1 = new List<string>();
			List<string> accs1 = new List<string>();
			List<string> pubmedLtp1 = new List<string>();
			List<string> pubmedMs21 = new List<string>();
			List<string> cstMs21 = new List<string>();
			List<string> species1 = new List<string>();
			foreach (string m in mod){
				string[] seqWinsX;
				string[] accsX;
				string[] pubmedLtpX;
				string[] pubmedMs2X;
				string[] cstMs2X;
				string[] speciesX;
				ParseKnownMod(m, out seqWinsX, out accsX, out pubmedLtpX, out pubmedMs2X, out cstMs2X, out speciesX);
				for (int i = 0; i < seqWinsX.Length; i++){
					seqWins1.Add(seqWinsX[i]);
					accs1.Add(accsX[i]);
					pubmedLtp1.Add(pubmedLtpX[i]);
					pubmedMs21.Add(pubmedMs2X[i]);
					cstMs21.Add(cstMs2X[i]);
					species1.Add(speciesX[i]);
				}
			}
			seqWins = seqWins1.ToArray();
			accs = accs1.ToArray();
			pubmedLtp = pubmedLtp1.ToArray();
			pubmedMs2 = pubmedMs21.ToArray();
			cstMs2 = cstMs21.ToArray();
			species = species1.ToArray();
		}

		public static void ParseKnownMod(string mod, out string[] seqWins, out string[] accs, out string[] pubmedLtp,
			out string[] pubmedMs2, out string[] cstMs2, out string[] species){
			string filename = GetFilenameForMod(mod);
			if (filename == null){
				seqWins = null;
				accs = null;
				pubmedLtp = null;
				pubmedMs2 = null;
				cstMs2 = null;
				species = null;
				return;
			}
			ParseKnownMods(filename, out seqWins, out accs, out pubmedLtp, out pubmedMs2, out cstMs2, out species);
		}

		public static void ParseKnownMods(string filename, out string[] seqWins, out string[] accs, out string[] pubmedLtp,
			out string[] pubmedMs2, out string[] cstMs2, out string[] species){
			seqWins = TabSep.GetColumn("SITE_+/-7_AA", filename, 3, '\t');
			accs = TabSep.GetColumn("ACC_ID", filename, 3, '\t');
			pubmedLtp = TabSep.GetColumn("LT_LIT", filename, 3, '\t');
			pubmedMs2 = TabSep.GetColumn("MS_LIT", filename, 3, '\t');
			cstMs2 = TabSep.GetColumn("MS_CST", filename, 3, '\t');
			species = TabSep.GetColumn("ORGANISM", filename, 3, '\t');
		}

		public static string[] GetAllKinaseSubstrateOrganisms(){
			string filename = GetKinaseSubstrateFile();
			if (filename == null){
				return null;
			}
			string[] species = TabSep.GetColumn("KIN_ORGANISM", filename, 3, '\t');
			return ArrayUtils.UniqueValues(species);
		}

		public static void ParseKinaseSubstrate(out string[] seqWins, out string[] subAccs, out string[] kinases,
			out string[] kinAccs, string species){
			string[] seqWins1;
			string[] subAccs1;
			string[] kinases1;
			string[] kinAccs1;
			string[] species1;
			ParseKinaseSubstrate(out seqWins1, out subAccs1, out kinases1, out kinAccs1, out species1);
			List<int> v = new List<int>();
			for (int i = 0; i < species1.Length; i++){
				if (species1[i].Equals(species)){
					v.Add(i);
				}
			}
			seqWins = ArrayUtils.SubArray(seqWins1, v);
			subAccs = ArrayUtils.SubArray(subAccs1, v);
			kinases = ArrayUtils.SubArray(kinases1, v);
			kinAccs = ArrayUtils.SubArray(kinAccs1, v);
		}

		public static void ParseKinaseSubstrate(out string[] seqWins, out string[] subAccs, out string[] kinases,
			out string[] kinAccs, out string[] species){
			string filename = GetKinaseSubstrateFile();
			if (filename == null){
				seqWins = null;
				subAccs = null;
				kinases = null;
				kinAccs = null;
				species = null;
				return;
			}
			ParseKinaseSubstrate(filename, out seqWins, out subAccs, out kinases, out kinAccs, out species);
		}

		public static void ParseKinaseSubstrate(string filename, out string[] seqWins, out string[] subAccs,
			out string[] kinases, out string[] kinAccs, out string[] species){
			seqWins = TabSep.GetColumn("SITE_+/-7_AA", filename, 3, '\t');
			subAccs = TabSep.GetColumn("SUB_ACC_ID", filename, 3, '\t');
			kinases = TabSep.GetColumn("KINASE", filename, 3, '\t');
			kinAccs = TabSep.GetColumn("KIN_ACC_ID", filename, 3, '\t');
			species = TabSep.GetColumn("KIN_ORGANISM", filename, 3, '\t');
		}

		public static void ParseRegulatorySites(out string[] seqWins, out string[] accs, out string[] function,
			out string[] process, out string[] protInteract, out string[] otherInteract, out string[] notes, string species){
			string[] seqWins1;
			string[] accs1;
			string[] function1;
			string[] process1;
			string[] protInteract1;
			string[] otherInteract1;
			string[] notes1;
			string[] species1;
			ParseRegulatorySites(out seqWins1, out accs1, out function1, out process1, out protInteract1, out otherInteract1,
				out notes1, out species1);
			List<int> v = new List<int>();
			for (int i = 0; i < seqWins1.Length; i++){
				if (species1[i].Equals(species)){
					v.Add(i);
				}
			}
			seqWins = ArrayUtils.SubArray(seqWins1, v);
			accs = ArrayUtils.SubArray(accs1, v);
			function = ArrayUtils.SubArray(function1, v);
			process = ArrayUtils.SubArray(process1, v);
			protInteract = ArrayUtils.SubArray(protInteract1, v);
			otherInteract = ArrayUtils.SubArray(otherInteract1, v);
			notes = ArrayUtils.SubArray(notes1, v);
		}

		public static void ParseRegulatorySites(out string[] seqWins, out string[] accs, out string[] function,
			out string[] process, out string[] protInteract, out string[] otherInteract, out string[] notes, out string[] species){
			string filename = GetRegulatorySitesFile();
			if (filename == null){
				seqWins = null;
				accs = null;
				function = null;
				process = null;
				protInteract = null;
				otherInteract = null;
				notes = null;
				species = null;
				return;
			}
			ParseRegulatorySites(filename, out seqWins, out accs, out function, out process, out protInteract, out otherInteract,
				out notes, out species);
		}

		public static void ParseRegulatorySites(string filename, out string[] seqWins, out string[] accs,
			out string[] function, out string[] process, out string[] protInteract, out string[] otherInteract,
			out string[] notes, out string[] species){
			seqWins = TabSep.GetColumn("SITE_+/-7_AA", filename, 3, '\t');
			accs = TabSep.GetColumn("ACC_ID", filename, 3, '\t');
			function = TabSep.GetColumn("ON_FUNCTION", filename, 3, '\t');
			process = TabSep.GetColumn("ON_PROCESS", filename, 3, '\t');
			protInteract = TabSep.GetColumn("ON_PROT_INTERACT", filename, 3, '\t');
			otherInteract = TabSep.GetColumn("ON_OTHER_INTERACT", filename, 3, '\t');
			notes = TabSep.GetColumn("NOTES", filename, 3, '\t');
			species = TabSep.GetColumn("ORGANISM", filename, 3, '\t');
		}

		public static string GetKinaseSubstrateFile(){
			string folder = FileUtils.GetConfigPath() + "\\PSP\\";
			string filename = folder + "Kinase_Substrate_Dataset";
			if (!File.Exists(filename)){
				if (File.Exists(filename + ".gz")){
					filename = filename + ".gz";
				} else{
					return null;
				}
			}
			return filename;
		}

		public static string GetRegulatorySitesFile(){
			string folder = FileUtils.GetConfigPath() + "\\PSP\\";
			string filename = folder + "Regulatory_sites";
			if (!File.Exists(filename)){
				if (File.Exists(filename + ".gz")){
					filename = filename + ".gz";
				} else{
					return null;
				}
			}
			return filename;
		}

		private static readonly string[] phosphositeFilenames ={
			"Acetylation_site_dataset", "Methylation_site_dataset",
			"O-GlcNAc_site_dataset", "O-GalNAc_site_dataset", "Phosphorylation_site_dataset", "Sumoylation_site_dataset",
			"Ubiquitination_site_dataset"
		};

		public static Dictionary<string, string> ParsePhosphoSite(string phosphoSiteFolder){
			Dictionary<string, string> map = new Dictionary<string, string>();
			foreach (string phosphositeFilename in phosphositeFilenames){
				string path = Path.Combine(phosphoSiteFolder, phosphositeFilename);
				if (!File.Exists(path)){
					continue;
				}
				try{
					StreamReader reader = new StreamReader(path);
					ParsePhosphoSite(reader, map);
				} catch (Exception){}
			}
			return map;
		}

		private static void ParsePhosphoSite(TextReader reader, IDictionary<string, string> map){
			while (!reader.ReadLine().StartsWith("PROTEIN")){}
			string line;
			while ((line = reader.ReadLine()) != null){
				string[] w = line.Split('\t');
				string accession = w[1];
				string type = w[4];
				string residue = w[5];
				int position = Parser.Int(residue.Substring(1));
				if (map.ContainsKey(accession)){
					map[accession] += ";" + type + "," + position;
				} else{
					map[accession] = type + "," + position;
				}
			}
		}
	}
}