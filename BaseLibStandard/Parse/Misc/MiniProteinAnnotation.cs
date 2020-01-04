using System.Collections.Generic;
using System.IO;
using BaseLibS.Parse.Uniprot;
using BaseLibS.Util;

namespace BaseLibS.Parse.Misc{
	public class MiniProteinAnnotation{
		public string ProteinName { get; private set; }
		public string GeneName { get; private set; }
		public string[] PfamIds { get; }
		public string[] PfamNames { get; }
		public int[] PfamStart { get; }
		public int[] PfamEnd { get; }
		public string[] Pdbs { get; private set; }
		public bool IsReviewed { get; private set; }
		public Dictionary<FeatureType, List<UniprotFeature>> Features { get; private set; }

		public MiniProteinAnnotation(string geneName, string proteinName, IList<string> pfams, string[] pdbs, bool isReviewed,
			Dictionary<FeatureType, List<UniprotFeature>> features){
			GeneName = geneName;
			ProteinName = proteinName;
			IsReviewed = isReviewed;
			PfamIds = new string[pfams.Count];
			PfamNames = new string[pfams.Count];
			PfamStart = new int[pfams.Count];
			PfamEnd = new int[pfams.Count];
			for (int i = 0; i < pfams.Count; i++){
				string[] w = pfams[i].Split(',');
				PfamIds[i] = w[0];
				PfamNames[i] = w[1];
				PfamStart[i] = Parser.Int(w[3]);
				PfamEnd[i] = Parser.Int(w[4]);
			}
			Pdbs = pdbs;
			Features = features;
		}

		public static Dictionary<string, MiniProteinAnnotation> ReadMapping(string filename, HashSet<string> allProtIds){
			StreamReader reader = FileUtils.GetReader(filename);
			Dictionary<string, MiniProteinAnnotation> result = new Dictionary<string, MiniProteinAnnotation>();
			string header = reader.ReadLine();
			string[] h = header.Split('\t');
			Dictionary<int, FeatureType> upMap = new Dictionary<int, FeatureType>();
			for (int i = 6; i < h.Length; i++){
				string q = h[i];
				if (q.StartsWith("UniprotFeature:")){
					string name = q.Substring(q.IndexOf(':') + 1);
					upMap.Add(i, FeatureType.GetFeatureType(name));
				}
			}
			string line;
			while ((line = reader.ReadLine()) != null){
				string[] a = line.Split('\t');
				if (a.Length < 8){
					continue;
				}
				Dictionary<FeatureType, List<UniprotFeature>> flist = new Dictionary<FeatureType, List<UniprotFeature>>();
				string[] accessions = a[0].Length == 0 ? new string[0] : a[0].Split(';');
				string geneName = a[1];
				string proteinName = a[2];
				string[] pfams = a[3].Length == 0 ? new string[0] : a[3].Split(';');
				string[] pdbs = a[4].Length == 0 ? new string[0] : a[4].Split(';');
				bool isReviewed = a[5].Equals("T");
				for (int i = 6; i < a.Length; i++){
					string u = a[i];
					if (u.Length > 0){
						FeatureType ft = upMap[i];
						if (!flist.ContainsKey(ft)){
							flist.Add(ft, new List<UniprotFeature>());
						}
						string[] w = u.Split(';');
						foreach (string s in w){
							string[] u1 = s.Split(',');
							if (u1.Length > 4){
								flist[ft].Add(new UniprotFeature(u1));
							}
						}
					}
				}
				MiniProteinAnnotation mpa = new MiniProteinAnnotation(geneName, proteinName, pfams, pdbs, isReviewed, flist);
				foreach (string key in accessions){
					if (!allProtIds.Contains(key)){
						continue;
					}
					if (!result.ContainsKey(key)){
						result.Add(key, mpa);
					}
				}
			}
			reader.Close();
			return result;
		}
	}
}