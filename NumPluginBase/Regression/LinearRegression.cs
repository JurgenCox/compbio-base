using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Regression {
	public class LinearRegression : RegressionMethod {
		public override RegressionModel Train(BaseVector[] x, int[] nominal, double[] y, Parameters param, int nthreads, Action<double> reportProgress) {
			x = ClassificationMethod.ToOneHotEncoding(x, nominal);
			throw new System.NotImplementedException();
		}

		public override Parameters Parameters => new Parameters();
		public override string Name => "Linear regression";
		public override string Description => "";
		public override float DisplayRank => 1;
		public override bool IsActive => true;
	}
}