using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Data.Category{
	[Serializable]
	internal class StringCategoryVectorData : ICategoryVectorData, ISerializable{
		private string[][] categoryData;
		public string[] Values { get; private set; }
		internal StringCategoryVectorData(string[][] vals){
			categoryData = vals;
			Values = GetValues(vals);
			Array.Sort(Values);
		}
		private StringCategoryVectorData(SerializationInfo info, StreamingContext context) {
			Values = (string[])info.GetValue("Values", typeof(string[]));
			string[] value = (string[])info.GetValue("Value", typeof(string[]));
			int[] ind = (int[])info.GetValue("Ind", typeof(int[]));
			categoryData = ArrayUtils.UnpackArrayOfArrays(value, ind);
		}
		public StringCategoryVectorData(BinaryReader reader){
			Values = FileUtils.ReadStringArray(reader);
			string[] value = FileUtils.ReadStringArray(reader);
			int[] ind = FileUtils.ReadInt32Array(reader);
			categoryData = ArrayUtils.UnpackArrayOfArrays(value, ind);
		}
		public void Write(BinaryWriter writer) {
			FileUtils.Write(Values, writer);
			ArrayUtils.PackArrayOfArrays(categoryData, out string[] value, out int[] ind);
			FileUtils.Write(value, writer);
			FileUtils.Write(ind, writer);
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue("Values", Values);
			ArrayUtils.PackArrayOfArrays(categoryData, out string[] value, out int[] ind);
			info.AddValue("Value", value);
			info.AddValue("Ind", ind);
		}
		private StringCategoryVectorData(){}
		public string[] this[int i] => categoryData[i];
		public int Length => categoryData.Length;
		public ICategoryVectorData GetSubVector(int[] indices){
			string[][] vals = categoryData.SubArray(indices);
			ushort[][] categoryData1 = new ushort[vals.Length][];
			bool ok = CategoryVector.Fill(vals, categoryData1, out var allVals);
			if (!ok){
				return new StringCategoryVectorData(vals);
			}
			if (allVals.Length == 0){
				return new EmptyCategoryVectorData(vals.Length);
			}
			if (allVals.Length == 1){
				return new BoolCategoryVectorData(vals, allVals[0]);
			}
			if (allVals.Length == 2 && CategoryVector.AllLengthOne(vals)){
				return new BoolCategoryVectorData(vals, allVals[0], allVals[1]);
			}
			return new UshortCategoryVectorData(categoryData1, allVals);
		}
		private static string[] GetValues(IEnumerable<string[]> terms){
			HashSet<string> result = new HashSet<string>();
			foreach (string t in terms.SelectMany(s => s)){
				result.Add(t);
			}
			return result.ToArray();
		}
		public ICategoryVectorData Copy(){
			return new StringCategoryVectorData{
				Values = (string[]) Values.Clone(),
				categoryData = (string[][]) categoryData.Clone()
			};
		}
	}
}