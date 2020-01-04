using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Ms.Data.Protein{
	public class ProtInfo1{
		private readonly HashSet<string> keys = new HashSet<string>();
		private readonly string filePath;
		private BinaryWriter writer;

		public ProtInfo1(string combinedFolder){
			filePath = Path.Combine(combinedFolder, "infoFile");
			if (File.Exists(filePath)){
				File.Delete(filePath);
			}
			writer = FileUtils.GetBinaryWriter(filePath);
			writer.Write(0);
		}

		public void Add(string key, string[] value){
			if (!keys.Contains(key)){
				keys.Add(key);
				writer.Write(key);
				FileUtils.Write(value, writer);
			}
		}

		public void Finish(){
			writer.BaseStream.Seek(0, SeekOrigin.Begin);
			writer.Write(keys.Count);
			writer.Close();
			writer = null;
		}

		public bool ContainsKey(string key){
			return keys.Contains(key);
		}

		public Dictionary<string, Dictionary<string, bool>> Contains(Dictionary<string, HashSet<string>> map){
			Dictionary<string, Dictionary<string, bool>> result = new Dictionary<string, Dictionary<string, bool>>();
			BinaryReader reader = FileUtils.GetBinaryReader(filePath);
			int n = reader.ReadInt32();
			for (int i = 0; i < n; i++){
				string protId = reader.ReadString();
				string[] peptides = FileUtils.ReadStringArray(reader);
				if (map.ContainsKey(protId)){
					result.Add(protId, new Dictionary<string, bool>());
					Dictionary<string, bool> x = result[protId];
					HashSet<string> peptideSearch = map[protId];
					foreach (string s in peptideSearch){
						bool contains = Array.BinarySearch(peptides, s) >= 0;
						x.Add(s, contains);
					}
				}
			}
			reader.Close();
			return result;
		}
	}
}