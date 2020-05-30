using System;
using System.Collections.Generic;

namespace BaseLibS.Mol{
	[Serializable]
	public class AllModifications{
		public Dictionary<char, ushort[]> Modifications{ get; set; }
		public ushort[] NTermModifications{ get; set; }
		public ushort[] CTermModifications{ get; set; }

		public AllModifications(string[] vmods){
			List<ushort> nt = new List<ushort>();
			List<ushort> ct = new List<ushort>();
			Dictionary<char, List<ushort>> inte = new Dictionary<char, List<ushort>>();
			foreach (string vmod in vmods){
				Modification m = Tables.Modifications[vmod];
				if (m.IsNterminal){
					nt.Add(m.Index);
				}
				if (m.IsCterminal){
					ct.Add(m.Index);
				}
				if (m.IsInternal){
					foreach (ModificationSite site in m.Sites){
						if (!inte.ContainsKey(site.Aa)){
							inte.Add(site.Aa, new List<ushort>());
						}
						inte[site.Aa].Add(m.Index);
					}
				}
			}
			nt.Sort();
			ct.Sort();
			NTermModifications = nt.ToArray();
			CTermModifications = ct.ToArray();
			Modifications = new Dictionary<char, ushort[]>();
			foreach (KeyValuePair<char, List<ushort>> p in inte){
				Modifications.Add(p.Key, p.Value.ToArray());
			}
		}
	}
}