using System;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Util;
using NumPluginSvm.Svm;

namespace NumPluginSvm{
	[Serializable]
	public class SvmRegressionModel : RegressionModel{
		private SvmModel model;

		public SvmRegressionModel(SvmModel model){
			this.model = model;
		}

		public SvmRegressionModel(){ }

		public override double Predict(BaseVector x){
			return SvmMain.SvmPredict(model, x);
		}

		public override RegressionModel Read(string filePath){
			SvmRegressionModel result = new SvmRegressionModel();
			BinaryReader reader = FileUtils.GetBinaryReader(filePath);
			result.model = new SvmModel(reader);
			reader.Close();
			return result;
		}

		public override void Write(string filePath){
			BinaryWriter writer = FileUtils.GetBinaryWriter(filePath);
			model.Write(writer);
			writer.Close();
		}
	}
}