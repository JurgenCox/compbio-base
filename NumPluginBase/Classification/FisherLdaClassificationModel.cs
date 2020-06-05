using System;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Util;
using NumPluginBase.Distance;

namespace NumPluginBase.Classification{
	[Serializable]
	public class FisherLdaClassificationModel : ClassificationModel{
		private double[,] projection;
		private double[][] projectedGroupMeans;
		private int ngroups;

		public FisherLdaClassificationModel(double[,] projection, double[][] projectedGroupMeans, int ngroups){
			this.projection = projection;
			this.projectedGroupMeans = projectedGroupMeans;
			this.ngroups = ngroups;
		}

		public override double[] PredictStrength(BaseVector x){
			double[] projectedTest = MatrixUtils.VectorTimesMatrix(x, projection);
			double[] distances = new double[ngroups];
			IDistance distance = new EuclideanDistance();
			for (int j = 0; j < ngroups; j++){
				distances[j] = -(float) distance.Get(projectedTest, projectedGroupMeans[j]);
			}
			return distances;
		}

		public override void Write(string filePath){
			BinaryWriter writer = FileUtils.GetBinaryWriter(filePath);
			FileUtils.Write(projection, writer);
			FileUtils.Write(projectedGroupMeans, writer);
			writer.Write(ngroups);
			writer.Close();
		}

		public override void Read(string filePath){
			BinaryReader reader = FileUtils.GetBinaryReader(filePath);
			projection = FileUtils.Read2DDoubleArray2(reader);
			projectedGroupMeans = FileUtils.Read2DDoubleArray(reader);
			ngroups = reader.ReadInt32();
			reader.Close();
		}
	}
}