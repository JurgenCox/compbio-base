using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace BaseLibS.Data.Category{
	/// <summary>
	/// Wrapper class around <see cref="ICategoryVectorData"/>
	/// It is used to provide a common implementation of the interface
	/// </summary>
	[Serializable]
	public class CategoryVector{
		private readonly ICategoryVectorData data;
		internal CategoryVector(ICategoryVectorData data){
			this.data = data;
		}
		public CategoryVector(string[][] vals){
			ushort[][] categoryData = new ushort[vals.Length][];
			bool ok = Fill(vals, categoryData, out var allVals);
			if (!ok){
				data = new StringCategoryVectorData(vals);
				return;
			}
			if (allVals.Length == 0){
				data = new EmptyCategoryVectorData(vals.Length);
				return;
			}
			if (allVals.Length == 1){
				data = new BoolCategoryVectorData(vals, allVals[0]);
				return;
			}
			if (allVals.Length == 2 && AllLengthOne(vals)){
				data = new BoolCategoryVectorData(vals, allVals[0], allVals[1]);
				return;
			}
			data = new UshortCategoryVectorData(categoryData, allVals);
		}
		public CategoryVector(BinaryReader reader) {
			int x = reader.ReadInt32();
			switch (x){
				case 0:
					data = new BoolCategoryVectorData(reader);
					break;
				case 1:
					data = new EmptyCategoryVectorData(reader);
					break;
				case 2:
					data = new StringCategoryVectorData(reader);
					break;
				case 3:
					data = new UshortCategoryVectorData(reader);
					break;
				default:
					throw new Exception("Never get here.");
			}
		}
		public void Write(BinaryWriter writer){
			if (data is BoolCategoryVectorData){
				writer.Write(0);
			} else if (data is EmptyCategoryVectorData) {
				writer.Write(1);
			} else if (data is StringCategoryVectorData) {
				writer.Write(2);
			} else if (data is UshortCategoryVectorData) {
				writer.Write(3);
			} else{
				throw new Exception("Never get here.");
			}
			data.Write(writer);
		}
		public string[] this[int i] => data[i];
        public int Length => data.Length;
		public string[][] GetAllData(){
			string[][] result = new string[data.Length][];
			for (int i = 0; i < result.Length; i++){
				result[i] = data[i];
			}
			return result;
		}
		public string[] Values => data.Values;
		public CategoryVector SubArray(int[] indices){
			return new CategoryVector(data.GetSubVector(indices));
		}
		public CategoryVector Copy(){
			return new CategoryVector(data.Copy());
		}
		internal static bool AllLengthOne(Array[] vals){
			foreach (Array s in vals){
				if (s?.Length != 1){
					return false;
				}
			}
			return true;
		}
		internal static bool Fill(string[][] vals, ushort[][] categoryData, out string[] allVals) {
			Cleanup(vals);
			HashSet<string> allVals1 = new HashSet<string>();
			foreach (string[] q in vals.Where(q => q != null)){
				foreach (string t in q){
					if (!allVals1.Contains(t)){
						if (allVals1.Count >= ushort.MaxValue){
							allVals = null;
							return false;
						}
						allVals1.Add(t);
					}
				}
			}
			allVals = allVals1.ToArray();
			Array.Sort(allVals);
			Dictionary<string, ushort> categoryValuesInv = new Dictionary<string, ushort>();
			for (ushort i = 0; i < allVals.Length; i++){
				categoryValuesInv.Add(allVals[i], i);
			}
			for (int i = 0; i < vals.Length; i++){
				string[] q = vals[i];
				if (q != null && q.Length > 0){
					ushort[] w = new ushort[q.Length];
					for (int j = 0; j < w.Length; j++){
						string s = q[j];
						w[j] = categoryValuesInv[s];
					}
					categoryData[i] = w;
				}
			}
			return true;
		}
		private static void Cleanup(IList<string[]> vals) {
			for (int i = 0; i < vals.Count; i++) {
				string[] q = vals[i];
				if (q != null && q.Length > 0) {
					HashSet<string> r = new HashSet<string>();
					foreach (string s in q) {
						string t = s?.Trim();
						if (t?.Length > 0) {
							r.Add(t);
						}
					}
					vals[i] = r.ToArray();
					Array.Sort(vals[i]);
				}
			}
		}
	}
}