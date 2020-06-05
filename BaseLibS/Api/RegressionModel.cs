using System;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class RegressionModel{
		public virtual double Predict(BaseVector x){
			return Predict(new []{x})[0];
		}

		public virtual double[] Predict(BaseVector[] x){
			double[] result = new double[x.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = Predict(x[i]);
			}
			return result;
		}

		public abstract void Write(string filePath);

		public abstract void Read(string filePath);
	}
}