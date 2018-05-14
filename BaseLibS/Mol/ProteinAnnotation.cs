using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Parse.Misc;
using BaseLibS.Parse.Uniprot;
using BaseLibS.Util;

namespace BaseLibS.Mol{
	[Serializable]
	public class ProteinAnnotation : IDisposable{
		public string ProteinName { get; set; }
		public string Accession { get; set; }
		public string Description { get; set; }
		public string GeneName { get; set; }
		public bool IsReviewed { get; set; }
		public string[] ModTypesPsp { get; set; }
		public int[] ModPosPsp { get; set; }
		public string[] PfamIds { get; set; }
		public string[] PfamNames { get; set; }
		public int[] PfamStart { get; set; }
		public int[] PfamEnd { get; set; }
		public string[] Pdbs { get; set; }
		public Dictionary<FeatureType, List<UniprotFeature>> Features { get; set; }

		public void Dispose(){
			Accession = null;
			Description = null;
			ModTypesPsp = null;
			ModPosPsp = null;
			PfamIds = null;
			PfamNames = null;
			PfamStart = null;
			PfamEnd = null;
			Pdbs = null;
			Features?.Clear();
			Features = null;
		}

		public ProteinAnnotation(string accession, string description){
			ModPosPsp = new int[0];
			ModTypesPsp = new string[0];
			GeneName = "";
			IsReviewed = false;
			ProteinName = "";
			Accession = accession;
			Description = description;
			if (Description.Length > 256){
				Description = Description.Substring(0, 256);
			}
			PfamIds = new string[0];
			PfamNames = new string[0];
			PfamStart = new int[0];
			PfamEnd = new int[0];
			Pdbs = new string[0];
			Features = new Dictionary<FeatureType, List<UniprotFeature>>();
		}

		public ProteinAnnotation(BinaryReader reader){
			Accession = reader.ReadString();
			Description = reader.ReadString();
			ProteinName = reader.ReadString();
			GeneName = reader.ReadString();
			IsReviewed = reader.ReadBoolean();
			ModTypesPsp = FileUtils.ReadStringArray(reader);
			ModPosPsp = FileUtils.ReadInt32Array(reader);
			PfamIds = FileUtils.ReadStringArray(reader);
			PfamNames = FileUtils.ReadStringArray(reader);
			PfamStart = FileUtils.ReadInt32Array(reader);
			PfamEnd = FileUtils.ReadInt32Array(reader);
			Pdbs = FileUtils.ReadStringArray(reader);
			Features = new Dictionary<FeatureType, List<UniprotFeature>>();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++){
				int index = reader.ReadInt32();
				FeatureType ft = FeatureType.allFeatureTypes[index];
				int len = reader.ReadInt32();
				List<UniprotFeature> luf = new List<UniprotFeature>();
				for (int j = 0; j < len; j++){
					UniprotFeature uf = new UniprotFeature(reader);
					luf.Add(uf);
				}
				Features.Add(ft, luf);
			}
		}

		public void Write(BinaryWriter writer){
			writer.Write(Accession);
			writer.Write(Description);
			writer.Write(ProteinName);
			writer.Write(GeneName);
			writer.Write(IsReviewed);
			FileUtils.Write(ModTypesPsp, writer);
			FileUtils.Write(ModPosPsp, writer);
			FileUtils.Write(PfamIds, writer);
			FileUtils.Write(PfamNames, writer);
			FileUtils.Write(PfamStart, writer);
			FileUtils.Write(PfamEnd, writer);
			FileUtils.Write(Pdbs, writer);
			writer.Write(Features.Count);
			foreach (KeyValuePair<FeatureType, List<UniprotFeature>> p in Features){
				FeatureType ft = p.Key;
				writer.Write(ft.Index);
				List<UniprotFeature> luf = p.Value;
				writer.Write(luf.Count);
				foreach (UniprotFeature t in luf){
					t.Write(writer);
				}
			}
		}

		public bool HasKnownMods => ModTypesPsp.Length > 0;
		public bool HasProteinNames => !string.IsNullOrEmpty(ProteinName);
		public bool HasGeneNames => !string.IsNullOrEmpty(GeneName);

		public static void FillInAnnotation(IDictionary<string, ProteinAnnotation> annotations,
			IDictionary<string, MiniProteinAnnotation> map, IDictionary<string, string> pspMap){
			foreach (ProteinAnnotation pa in annotations.Values){
				if (map.ContainsKey(pa.Accession)){
					MiniProteinAnnotation mpa = map[pa.Accession];
					pa.GeneName = mpa.GeneName;
					pa.IsReviewed = mpa.IsReviewed;
					pa.ProteinName = mpa.ProteinName;
					pa.PfamIds = mpa.PfamIds;
					pa.PfamNames = mpa.PfamNames;
					pa.PfamStart = mpa.PfamStart;
					pa.PfamEnd = mpa.PfamEnd;
					pa.Pdbs = mpa.Pdbs;
					pa.Features = mpa.Features;
				} else if (pa.Accession.Contains("-")){
					string acc = pa.Accession.Substring(0, pa.Accession.IndexOf('-'));
					if (map.ContainsKey(acc)){
						MiniProteinAnnotation mpa = map[acc];
						pa.GeneName = mpa.GeneName;
						pa.IsReviewed = mpa.IsReviewed;
						pa.ProteinName = mpa.ProteinName;
						pa.PfamIds = mpa.PfamIds;
						pa.PfamNames = mpa.PfamNames;
						pa.PfamStart = new int[0];
						pa.PfamEnd = new int[0];
						pa.Pdbs = new string[0];
						pa.Features = new Dictionary<FeatureType, List<UniprotFeature>>();
					}
				}
				if (pspMap.ContainsKey(pa.Accession)){
					string x = pspMap[pa.Accession];
					string[] modificationsPsp = x.Length == 0 ? new string[0] : x.Split(';');
					string[] modTypesPsp = new string[modificationsPsp.Length];
					int[] modPosPsp = new int[modificationsPsp.Length];
					for (int i = 0; i < modificationsPsp.Length; i++){
						string[] w = modificationsPsp[i].Split(',');
						modTypesPsp[i] = w[0];
						bool success = Parser.TryInt(w[1], out modPosPsp[i]);
						if (!success){
							modPosPsp[i] = -1;
						}
					}
					pa.ModTypesPsp = modTypesPsp;
					pa.ModPosPsp = modPosPsp;
				}
			}
		}

		public static void FillInAnnotation(IDictionary<string, ProteinAnnotation> annots){
			string file1 = Path.Combine(FileUtils.GetConfigPath(), "maxquantAnnot.txt");
			string file2 = file1 + ".gz";
			string file = null;
			if (File.Exists(file1)){
				file = file1;
			} else if (File.Exists(file2)){
				file = file2;
			}
			if (file != null){
				HashSet<string> acc = GetAccessions(annots);
				Dictionary<string, MiniProteinAnnotation> map = MiniProteinAnnotation.ReadMapping(file, acc);
				string phosphoSiteFolder = FileUtils.executablePath + "\\conf\\PSP";
				Dictionary<string, string> psMap = PhosphoSitePlusParser.ParsePhosphoSite(phosphoSiteFolder);
				FillInAnnotation(annots, map, psMap);
			}
		}

		private static HashSet<string> GetAccessions(IDictionary<string, ProteinAnnotation> annotations){
			HashSet<string> result = new HashSet<string>();
			foreach (ProteinAnnotation pa in annotations.Values){
				if (!result.Contains(pa.Accession)){
					result.Add(pa.Accession);
				}
			}
			return result;
		}
	}
}