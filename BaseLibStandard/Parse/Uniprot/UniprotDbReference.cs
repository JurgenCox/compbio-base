using System;
using System.Collections.Generic;

namespace BaseLibS.Parse.Uniprot{
	[Serializable]
	public class UniprotDbReference{
		public readonly Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();

		public void AddProperty(string type, string value){
			if (!properties.ContainsKey(type)){
				properties.Add(type, new List<string>());
			}
			properties[type].Add(value);
		}

		public string[] GetPropertyValues(string type) { return !properties.ContainsKey(type) ? new string[0] : properties[type].ToArray(); }
	}
}