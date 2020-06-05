using System;
using System.Collections.Generic;
using BaseLibS.Num;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	/// <summary>
	/// Class storing the information resulting from the training process of a classification algorithm.
	/// Each classifier will have its own implementation of <code>ClassificationModel</code>.
	/// </summary>
	[Serializable]
	public abstract class ClassificationModel{
		/// <summary>
		/// Class prediction for a single instance.
		/// </summary>
		/// <param name="x">Test instance</param>
		/// <returns>Prediction strength for the different classes. The one with the biggest value is the assigned class.</returns>
		public virtual double[] PredictStrength(BaseVector x){
			return PredictStrength(new []{x})[0];
		}

		public virtual double[][] PredictStrength(BaseVector[] x){
			double[][] result = new double[x.Length][];
			for (int i = 0; i < result.Length; i++){
				result[i] = PredictStrength(x[i]);
			}
			return result;
		}

		public int PredictClass(BaseVector x){
			return ArrayUtils.MaxInd(PredictStrength(x));
		}

		public int[] PredictClasses(BaseVector x){
			double[] w = PredictStrength(x);
			List<int> result = new List<int>();
			for (int i = 0; i < w.Length; i++){
				if (w[i] > 0){
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public abstract void Write(string filePath);

		public abstract void Read(string filePath);
	}
}