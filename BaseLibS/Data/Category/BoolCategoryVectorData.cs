using System;
using System.IO;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Data.Category{
	[Serializable]
	internal class BoolCategoryVectorData : ICategoryVectorData{
		private bool[] vector;
		private string value = "";
		private string falseValue = "";
		internal BoolCategoryVectorData(Array[] vals, string value){
			this.value = value;
			vector = new bool[vals.Length];
			for (int i = 0; i < vals.Length; i++){
				if ((vals[i]?.Length ?? -1) > 0){
					vector[i] = true;
				}
			}
		}
		internal BoolCategoryVectorData(string[][] vals, string value, string falseValue){
			this.value = value;
			this.falseValue = falseValue;
			vector = new bool[vals.Length];
			for (int i = 0; i < vals.Length; i++){
				if (vals[i][0].Equals(value)){
					vector[i] = true;
				}
			}
		}
		private BoolCategoryVectorData(){}
		public BoolCategoryVectorData(bool[] vector, string value, string falseValue){
			this.vector = vector;
			this.value = value;
			this.falseValue = falseValue;
		}
		public BoolCategoryVectorData(BinaryReader reader){
			vector = FileUtils.ReadBooleanArray(reader);
			value = reader.ReadString();
			falseValue = reader.ReadString();
		}
		public void Write(BinaryWriter writer) {
			FileUtils.Write(vector, writer);
			writer.Write(value);
			writer.Write(falseValue);
		}

		public string[] this[int i]{
			get{
				if (vector[i]){
					return new[]{value};
				}
				return string.IsNullOrEmpty(falseValue) ? new string[0] : new[]{falseValue};
			}
		}
		public int Length => vector.Length;
		public ICategoryVectorData GetSubVector(int[] indices){
			bool[] vec = vector.SubArray(indices);
			bool allTrue = true;
			bool allFalse = true;
			foreach (bool b in vec){
				if (b){
					allFalse = false;
				} else{
					allTrue = false;
				}
				if (!allTrue && !allFalse){
					break;
				}
			}
			if (allTrue){
				return new BoolCategoryVectorData{falseValue = null, value = value, vector = vec};
			}
			if (allFalse){
				if (string.IsNullOrEmpty(falseValue)){
					return new EmptyCategoryVectorData(vec.Length);
				}
				return new BoolCategoryVectorData{
					falseValue = null,
					value = falseValue,
					vector = ArrayUtils.FillArray(true, vec.Length)
				};
			}
			return new BoolCategoryVectorData { falseValue = falseValue, value = value, vector = vec };
		}
		public string[] Values => string.IsNullOrEmpty(falseValue) ? new[]{value} : new[]{value, falseValue};
		public ICategoryVectorData Copy(){
			return new BoolCategoryVectorData{falseValue = falseValue, value = value, vector = (bool[]) vector.Clone()};
		}
	}
}