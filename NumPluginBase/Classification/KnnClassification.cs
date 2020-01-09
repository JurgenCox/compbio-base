using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using NumPluginBase.Distance;

namespace NumPluginBase.Classification {
	public class KnnClassification : ClassificationMethod {
		public override ClassificationModel Train(BaseVector[] x, int[] nominal, int[][] y, int ngroups, Parameters param,
			int nthreads, Action<double> reportProgress) {
			x = ToOneHotEncoding(x, nominal);
			int k = param.GetParam<int>("Number of neighbours").Value;
			IDistance distance = Distances.GetDistanceFunction(param);
			return new KnnClassificationModel(x, y, ngroups, k, distance);
		}

		public override Parameters Parameters => new Parameters(Distances.GetDistanceParameters(),
			new IntParam("Number of neighbours", 5));

		public override string Name => "KNN";
		public override string Description => "";
		public override float DisplayRank => 2;
		public override bool IsActive => true;
	}
}