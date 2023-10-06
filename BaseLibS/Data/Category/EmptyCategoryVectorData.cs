using System;
using System.IO;
namespace BaseLibS.Data.Category{
	[Serializable]
	internal class EmptyCategoryVectorData : ICategoryVectorData{
		public int Length { get; }
		internal EmptyCategoryVectorData(int len) {
			Length = len;
		}
		internal EmptyCategoryVectorData(BinaryReader reader) {
			Length = reader.ReadInt32();
		}
		public void Write(BinaryWriter writer) {
			writer.Write(Length);
		}
		public string[] this[int i] => new string[0];
		public ICategoryVectorData GetSubVector(int[] indices){
			return new EmptyCategoryVectorData(indices.Length);
		}
		public string[] Values => new string[0];
		public ICategoryVectorData Copy(){
			return new EmptyCategoryVectorData(Length);
		}
	}
}