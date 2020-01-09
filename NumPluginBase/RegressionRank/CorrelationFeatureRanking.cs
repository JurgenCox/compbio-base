using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.RegressionRank {
	public class CorrelationFeatureRanking : RegressionFeatureRankingMethod {
		public override int[] Rank(BaseVector[] x, double[] y, Parameters param, IGroupDataProvider data, int nthreads) {
			int nfeatures = x[0].Length;
			double[] s = new double[nfeatures];
			for (int i = 0; i < nfeatures; i++) {
				double[] xx = new double[x.Length];
				for (int j = 0; j < xx.Length; j++) {
					xx[j] = x[j][i];
				}
				s[i] = CalcScore(xx, y);
			}
			return ArrayUtils.Order(s);
		}

		private static double CalcScore(IList<double> xx, IList<double> yy) {
			return 1 - Math.Abs(ArrayUtils.Correlation(xx, yy));
		}

		public override Parameters GetParameters(IGroupDataProvider data) {
			return new Parameters();
		}

		public override string Name => "Abs(Pearson correlation)";
		public override string Description => "";
		public override float DisplayRank => 0;
		public override bool IsActive => true;
	}
}