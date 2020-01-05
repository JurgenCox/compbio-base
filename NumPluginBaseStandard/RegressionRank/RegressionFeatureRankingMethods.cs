using System;
using System.Linq;
using BaseLibS.Api;
using BaseLibS.Param;
using BaseLibS.Util;

namespace NumPluginBase.RegressionRank{
	public static class RegressionFeatureRankingMethods{
		private static readonly RegressionFeatureRankingMethod[] allMethods = InitRankingMethods();
		private static RegressionFeatureRankingMethod[] InitRankingMethods() { return FileUtils.GetPlugins<RegressionFeatureRankingMethod>(NumPluginUtils.pluginNames, true); }

		public static string[] GetAllNames(){
			string[] result = new string[allMethods.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = allMethods[i].Name;
			}
			return result;
		}

		public static Parameters[] GetAllSubParameters(IGroupDataProvider data){
			Parameters[] result = new Parameters[allMethods.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = allMethods[i].GetParameters(data);
			}
			return result;
		}

		public static RegressionFeatureRankingMethod Get(int index) { return allMethods[index]; }

		public static RegressionFeatureRankingMethod GetByName(string name){
			foreach (RegressionFeatureRankingMethod method in allMethods.Where(method => method.Name.Equals(name))){
				return method;
			}
			throw new Exception("Unknown type: " + name);
		}
	}
}