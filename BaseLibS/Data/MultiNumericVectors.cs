using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Data {
	[Serializable]
	public class MultiNumericVectors : ISerializable {
		public List<double[][]> MultiNumericVecs { get; set; } = new List<double[][]>();
		public MultiNumericVectors() { }
		private MultiNumericVectors(SerializationInfo info, StreamingContext context) {
			int length = info.GetInt32("Length");
			MultiNumericVecs.Clear();
			for (int i = 0; i < length; i++) {
				double[] value = (double[]) info.GetValue("Value" + i, typeof(double[]));
				int[] ind = (int[]) info.GetValue("Ind" + i, typeof(int[]));
				MultiNumericVecs.Add(ArrayUtils.UnpackArrayOfArrays(value, ind));
			}
		}
		public MultiNumericVectors(BinaryReader reader){
			int length = reader.ReadInt32();
			MultiNumericVecs.Clear();
			for (int i = 0; i < length; i++) {
				double[] value = FileUtils.ReadDoubleArray(reader);
				int[] ind = FileUtils.ReadInt32Array(reader);
				MultiNumericVecs.Add(ArrayUtils.UnpackArrayOfArrays(value, ind));
			}
		}
		public void Write(BinaryWriter writer){
			int length = MultiNumericVecs.Count;
			writer.Write(length);
			for (int i = 0; i < length; i++) {
				ArrayUtils.PackArrayOfArrays(MultiNumericVecs[i], out double[] value, out int[] ind);
				FileUtils.Write(value, writer);
				FileUtils.Write(ind, writer);
			}
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			int length = MultiNumericVecs.Count;
			info.AddValue("Length", length);
			for (int i = 0; i < length; i++) {
				ArrayUtils.PackArrayOfArrays(MultiNumericVecs[i], out double[] value, out int[] ind);
				info.AddValue("Value" + i, value);
				info.AddValue("Ind" + i, ind);
			}
		}
	}
}