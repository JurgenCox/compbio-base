using System.Collections.Generic;
using BaseLibS.Mol;

namespace BaseLibS.Ms{
	public static class PepUtils{
		public static (Modification2[][][] dependentMods, bool hasDependentMods) CreateDependentMods(
			IList<Modification2[]> lMods, Modification[] vMods){
			bool hasDependentMods = false;
			Modification2[][][] result = new Modification2[lMods.Count][][];
			for (int i = 0; i < result.Length; i++){
				result[i] = new Modification2[lMods[i].Length][];
				for (int j = 0; j < result[i].Length; j++){
					result[i][j] = CreateDependentMods(lMods[i][j], vMods);
					if (result[i][j].Length > 0){
						hasDependentMods = true;
					}
				}
			}
			return (result, hasDependentMods);
		}

		private static Modification2[] CreateDependentMods(Modification2 labelMod, IEnumerable<Modification> varMods){
			if (labelMod.IsIsotopicLabel){
				return new Modification2[0];
			}
			if (labelMod.IsCterminal){
				return GetCterminalMods(varMods);
			}
			if (labelMod.IsNterminal){
				return GetNterminalMods(varMods);
			}
			return labelMod.AaCount > 0 ? GetInternalMods(varMods, labelMod.GetAaAt(0)) : new Modification2[0];
		}

		private static Modification2[] GetInternalMods(IEnumerable<Modification> varMods, char aa){
			List<Modification2> result = new List<Modification2>();
			foreach (Modification mod in varMods){
				if (mod.IsInternal && Contains(mod.Sites, aa) && !mod.IsIsotopicMod){
					result.Add(new Modification2(mod));
				}
			}
			return result.ToArray();
		}

		private static bool Contains(IEnumerable<ModificationSite> sites, char aa){
			foreach (ModificationSite site in sites){
				if (site.Aa == aa){
					return true;
				}
			}
			return false;
		}

		private static Modification2[] GetNterminalMods(IEnumerable<Modification> varMods){
			List<Modification2> result = new List<Modification2>();
			foreach (Modification mod in varMods){
				if (mod.IsNterminal){
					result.Add(new Modification2(mod));
				}
			}
			return result.ToArray();
		}

		private static Modification2[] GetCterminalMods(IEnumerable<Modification> varMods){
			List<Modification2> result = new List<Modification2>();
			foreach (Modification mod in varMods){
				if (mod.IsCterminal){
					result.Add(new Modification2(mod));
				}
			}
			return result.ToArray();
		}
	}
}