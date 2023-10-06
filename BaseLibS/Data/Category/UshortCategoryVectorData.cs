using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Data.Category {
	[Serializable]
	internal class UshortCategoryVectorData : ICategoryVectorData, ISerializable {
		public string[] Values { get; }
		private readonly ushort[][] categoryData;
		internal UshortCategoryVectorData(ushort[][] categoryData, string[] allVals) {
			this.categoryData = categoryData;
			Values = allVals;
		}
		private UshortCategoryVectorData(SerializationInfo info, StreamingContext context) {
			Values = (string[]) info.GetValue("Values", typeof(string[]));
			ushort[] value = (ushort[]) info.GetValue("Value", typeof(ushort[]));
			int[] ind = (int[]) info.GetValue("Ind", typeof(int[]));
			categoryData = ArrayUtils.UnpackArrayOfArrays(value, ind);
		}
		public UshortCategoryVectorData(BinaryReader reader) {
			Values = FileUtils.ReadStringArray(reader);
			ushort[] value = FileUtils.ReadUshortArray(reader);
			int[] ind = FileUtils.ReadInt32Array(reader);
			categoryData = ArrayUtils.UnpackArrayOfArrays(value, ind);
		}
		public void Write(BinaryWriter writer) {
			FileUtils.Write(Values, writer);
			ArrayUtils.PackArrayOfArrays(categoryData, out ushort[] value, out int[] ind);
			FileUtils.Write(value, writer);
			FileUtils.Write(ind, writer);
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			info.AddValue("Values", Values);
			ArrayUtils.PackArrayOfArrays(categoryData, out ushort[] value, out int[] ind);
			info.AddValue("Value", value);
			info.AddValue("Ind", ind);
		}
		public string[] this[int i] {
			get {
				ushort[] v = categoryData[i];
				if (v == null) {
					return new string[0];
				}
				string[] result = new string[v.Length];
				for (int j = 0; j < result.Length; j++) {
					result[j] = Values[v[j]];
				}
				return result;
			}
		}
		public int Length => categoryData.Length;
		public ICategoryVectorData GetSubVector(int[] indices) {
			ushort[][] catData = SubArray(categoryData, indices);
			HashSet<ushort> allVals1 = new HashSet<ushort>();
			foreach (ushort[] q in catData) {
				if (q != null) {
					foreach (ushort t in q) {
						ushort s = t;
						if (!allVals1.Contains(s)) {
							allVals1.Add(s);
						}
					}
				}
			}
			ushort[] allVals = allVals1.ToArray();
			Array.Sort(allVals);
			if (allVals.Length == 0) {
				return new EmptyCategoryVectorData(indices.Length);
			}
			if (allVals.Length == 1) {
				return new BoolCategoryVectorData(catData, Values[allVals[0]]);
			}
			if (allVals.Length == 2 && CategoryVector.AllLengthOne(catData)) {
				bool[] vector = new bool[catData.Length];
				for (int i = 0; i < vector.Length; i++) {
					vector[i] = catData[i][0] == allVals[0];
				}
				return new BoolCategoryVectorData(vector, Values[allVals[0]], Values[allVals[1]]);
			}
			Dictionary<ushort, ushort> map = new Dictionary<ushort, ushort>();
			for (ushort i = 0; i < allVals.Length; i++) {
				map.Add(allVals[i], i);
			}
			foreach (ushort[] t in catData) {
				if (t != null) {
					for (int j = 0; j < t.Length; j++) {
						t[j] = map[t[j]];
					}
				}
			}
			return new UshortCategoryVectorData(catData, Values.SubArray(ArrayUtils.ToInts(allVals)));
		}
		public static ushort[][] SubArray(IList<ushort[]> array, IList<int> indices) {
			ushort[][] result = new ushort[indices.Count][];
			for (int i = 0; i < result.Length; i++) {
				ushort[] us = array[indices[i]];
				if (us != null) {
					result[i] = (ushort[]) us.Clone();
				}
			}
			return result;
		}
		public ICategoryVectorData Copy() {
			return new UshortCategoryVectorData((ushort[][]) categoryData.Clone(), (string[]) Values.Clone());
		}
	}
}