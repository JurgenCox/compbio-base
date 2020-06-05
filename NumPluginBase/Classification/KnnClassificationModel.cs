using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Util;
using NumPluginBase.Distance;

namespace NumPluginBase.Classification{
	[Serializable]
	public class KnnClassificationModel : ClassificationModel{
		private BaseVector[] x;
		private int[][] y;
		private int ngroups;
		private int k;
		private IDistance distance;

		public KnnClassificationModel(BaseVector[] x, int[][] y, int ngroups, int k, IDistance distance){
			this.x = x;
			this.y = y;
			this.ngroups = ngroups;
			this.k = k;
			this.distance = distance;
		}

		public override double[] PredictStrength(BaseVector xTest){
			int[] inds = GetNeighborInds(x, xTest, k, distance);
			double[] result = new double[ngroups];
			foreach (int ind in inds){
				foreach (int i in y[ind]){
					result[i]++;
				}
			}
			for (int i = 0; i < ngroups; i++){
				result[i] /= k;
			}
			return result;
		}

		public override void Write(string filePath){
			BinaryWriter writer = FileUtils.GetBinaryWriter(filePath);
			FileUtils.Write(x, writer);
			FileUtils.Write(y, writer);
			writer.Write(ngroups);
			writer.Write(k);
			distance.Write(writer);
			writer.Close();
		}

		public override void Read(string filePath){
			BinaryReader reader = FileUtils.GetBinaryReader(filePath);
			x = FileUtils.ReadBaseVectorArray(reader);
			y = FileUtils.Read2DInt32Array(reader);
			ngroups = reader.ReadInt32();
			k = reader.ReadInt32();
			DistanceType type = (DistanceType) reader.ReadInt32();
			distance = Distances.ReadDistance(type, reader);
			reader.Close();
		}

		public static int[] GetNeighborInds(IList<BaseVector> x, BaseVector xTest, int k, IDistance distance){
			double[] d = CalcDistances(x, xTest, distance);
			int[] o = d.Order();
			List<int> result = new List<int>();
			for (int i = 0; i < d.Length; i++){
				if (!double.IsNaN(d[o[i]]) && !double.IsInfinity(d[o[i]])){
					result.Add(o[i]);
				}
				if (result.Count >= k){
					break;
				}
			}
			return result.ToArray();
		}

		private static double[] CalcDistances(IList<BaseVector> x, BaseVector xTest, IDistance distance){
			double[] result = new double[x.Count];
			for (int i = 0; i < x.Count; i++){
				result[i] = distance.Get(x[i], xTest);
			}
			return result;
		}
	}
}