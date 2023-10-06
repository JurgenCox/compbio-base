using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Data {
	[Serializable]
	public class StringVectors : ISerializable {
		public List<string[]> StringVecs { get; set; } = new List<string[]>();
		public StringVectors() { }
		public StringVectors(BinaryReader reader){
			int len = reader.ReadInt32();
			StringVecs.Clear();
			for (int i = 0; i < len; i++) {
				string value = reader.ReadString();
				int[] ind = FileUtils.ReadInt32Array(reader);
				StringVecs.Add(ArrayUtils.UnpackArrayOfStrings(value, ind));
			}

		}
		public void Write(BinaryWriter writer){
			writer.Write(StringVecs.Count);
			for (int i = 0; i < StringVecs.Count; i++) {
				ArrayUtils.PackArrayOfStrings(StringVecs[i], out string value, out int[] ind);
				writer.Write(value);
				FileUtils.Write(ind, writer);
			}
		}
		private StringVectors(SerializationInfo info, StreamingContext context) {
			int length = info.GetInt32("Length");
			StringVecs.Clear();
			for (int i = 0; i < length; i++) {
				string value = (string)info.GetValue("Value" + i, typeof(string));
				int[] ind = (int[])info.GetValue("Ind" + i, typeof(int[]));
				StringVecs.Add(ArrayUtils.UnpackArrayOfStrings(value, ind));
			}
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			int length = StringVecs.Count;
			info.AddValue("Length", length);
			for (int i = 0; i < length; i++) {
				ArrayUtils.PackArrayOfStrings(StringVecs[i], out string value, out int[] ind);
				info.AddValue("Value" + i, value);
				info.AddValue("Ind" + i, ind);
			}
		}
	}
}
