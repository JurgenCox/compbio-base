using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using NumPluginBase.Distance;

namespace NumPluginBase.Regression {
	public class KnnRegression : RegressionMethod {
		public override RegressionModel Train(BaseVector[] x, int[] nominal, double[] y, Parameters param, int ntheads, Action<double> reportProgress) {
			x = ClassificationMethod.ToOneHotEncoding(x, nominal);
			int k = param.GetParam<int>("Number of neighbours").Value;
			IDistance distance = Distances.GetDistanceFunction(param);
			return new KnnRegressionModel(x, y, k, distance);
		}

		public override Parameters Parameters => new Parameters(Distances.GetDistanceParameters(),
			new IntParam("Number of neighbours", 5));

		public override string Name => "KNN";
		public override string Description => "";
		public override float DisplayRank => 2;
		public override bool IsActive => true;
	}
}