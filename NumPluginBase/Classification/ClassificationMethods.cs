using System;
using System.Linq;
using BaseLibS.Api;
using BaseLibS.Param;
using BaseLibS.Util;

namespace NumPluginBase.Classification{
	public static class ClassificationMethods{
		private static readonly ClassificationMethod[] allMethods = InitClassificationMethods();

		private static ClassificationMethod[] InitClassificationMethods(){
			return FileUtils.GetPlugins<ClassificationMethod>(NumPluginUtils.pluginNames, true);
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

		public static ClassificationMethod Get(int index){
			return allMethods[index];
		}

		public static ClassificationMethod GetByName(string name){
			foreach (ClassificationMethod method in allMethods.Where(method => method.Name.Equals(name))){
				return method;
			}
			throw new Exception("Unknown type: " + name);
		}
	}
}