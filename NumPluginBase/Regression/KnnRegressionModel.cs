using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using NumPluginBase.Classification;

namespace NumPluginBase.Regression{
	[Serializable]
	public class KnnRegressionModel : RegressionModel{
		private readonly BaseVector[] x;
		private readonly double[] y;
		private readonly int k;
		private readonly IDistance distance;

		public KnnRegressionModel(IList<BaseVector> x, IList<double> y, int k, IDistance distance){
			List<int> v = new List<int>();
			for (int i = 0; i < y.Count; i++){
				if (!double.IsNaN(y[i]) && !double.IsInfinity(y[i])){
					v.Add(i);
				}
			}
			this.x = ArrayUtils.SubArray(x, v);
			this.y = ArrayUtils.SubArray(y, v);
			this.k = k;
			this.distance = distance;
		}

		public override double Predict(BaseVector xTest){
			int[] inds = KnnClassificationModel.GetNeighborInds(x, xTest, k, distance);
			double result = 0;
			foreach (int ind in inds){
				result += y[ind];
			}
			result /= inds.Length;
			return result;
		}
	}
}