using System;
using System.Linq;
using BaseLibS.Api;
using BaseLibS.Param;
using BaseLibS.Util;

namespace NumPluginBase.Regression{
	public static class RegressionMethods{
		private static readonly RegressionMethod[] allMethods = InitRegressionMethod();

		private static RegressionMethod[] InitRegressionMethod(){
			return FileUtils.GetPlugins<RegressionMethod>(NumPluginUtils.pluginNames, true);
		}

		public static string[] GetAllNames(){
			string[] result = new string[allMethods.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = allMethods[i].Name;
			}
			return result;
		}

		public static Parameters[] GetAllSubParameters(){
			Parameters[] result = new Parameters[allMethods.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = allMethods[i].Parameters;
			}
			return result;
		}

		public static RegressionMethod Get(int index){
			return allMethods[index];
		}

		public static RegressionMethod GetByName(string name){
			foreach (RegressionMethod method in allMethods.Where(method => method.Name.Equals(name))){
				return method;
			}
			throw new Exception("Unknown type: " + name);
		}
	}
}