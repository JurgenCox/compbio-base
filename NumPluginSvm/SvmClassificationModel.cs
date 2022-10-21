using System;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Util;
using NumPluginSvm.Svm;

namespace NumPluginSvm{
	[Serializable]
	public class SvmClassificationModel : ClassificationModel{
		private SvmModel[] models;
		private bool[] invert;

		public SvmClassificationModel(SvmModel[] models, bool[] invert){
			this.models = models;
			this.invert = invert;
		}

		public SvmClassificationModel(){ }

		public override void Read(string filePath){
			BinaryReader reader = FileUtils.GetBinaryReader(filePath);
			int len = reader.ReadInt32();
			models = new SvmModel[len];
			for (int i = 0; i < len; i++){
				models[i] = new SvmModel(reader);
			}
			invert = FileUtils.ReadBooleanArray(reader);
			reader.Close();
		}

		public override void Write(string filePath){
			BinaryWriter writer = FileUtils.GetBinaryWriter(filePath);
			writer.Write(models.Length);
			foreach (SvmModel model in models){
				model.Write(writer);
			}
			FileUtils.Write(invert, writer);
			writer.Close();
		}

		public override double[] PredictStrength(BaseVector x){
			if (models.Length == 1){
				double[] result = new double[2];
				double[] decVal = new double[1];
				SvmMain.SvmPredictValues(models[0], x, decVal);
				result[0] = invert[0] ? -(float) decVal[0] : (float) decVal[0];
				result[1] = -result[0];
				return result;
			}
			double[] result1 = new double[models.Length];
			for (int i = 0; i < result1.Length; i++){
				double[] decVal = new double[1];
				SvmMain.SvmPredictValues(models[i], x, decVal);
				result1[i] = invert[i] ? -(float) decVal[0] : (float) decVal[0];
			}
			return result1;
		}

		public double[] GetBinaryClassifierWeights(int nfeatures) {
			return models[0].ComputeBinaryClassifierWeights(nfeatures);
		}
	}
}