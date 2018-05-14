using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Test.Univariate.NSample;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.ClassificationRank{
	public class AnovaFeatureRanking : ClassificationFeatureRankingMethod{
		public static string s0Help =
			"Parameter controlling the trade-off between significance as measured by the p-value and the size of the effect.";

		public override int[] Rank(BaseVector[] x, int[][] y, int ngroups, Parameters param, IGroupDataProvider data,
			int nthreads, Action<double> reportProgress){
			double s0 = param.GetParam<double>("s0").Value;
			int nfeatures = x[0].Length;
			int[][] yy = RearrangeGroups(y, ngroups);
			double[] s = new double[nfeatures];
			for (int i = 0; i < nfeatures; i++){
				double[] xx = new double[x.Length];
				for (int j = 0; j < xx.Length; j++){
					xx[j] = x[j][i];
				}
				s[i] = CalcPvalue(xx, yy, ngroups, s0);
			}
			return ArrayUtils.Order(s);
		}

		private static int[][] RearrangeGroups(IList<int[]> y, int ngroups){
			List<int>[] result = new List<int>[ngroups];
			for (int i = 0; i < ngroups; i++){
				result[i] = new List<int>();
			}
			for (int i = 0; i < y.Count; i++){
				foreach (int w in y[i]){
					result[w].Add(i);
				}
			}
			int[][] r = new int[ngroups][];
			for (int i = 0; i < r.Length; i++){
				r[i] = result[i].ToArray();
			}
			return r;
		}

		private static double CalcPvalue(IList<double> x, IList<int[]> y, int ngroups, double s0){
			double[][] data = new double[ngroups][];
			for (int i = 0; i < ngroups; i++){
				data[i] = ArrayUtils.ExtractValidValues(ArrayUtils.SubArray(x, y[i]));
			}
			double statistic;
			double pvalS0;
			double[] gmeans;
			OneWayAnovaTest.TestImpl(data, out statistic, s0, out pvalS0, out gmeans);
			return pvalS0;
		}

		public override Parameters GetParameters(IGroupDataProvider data){
			return new Parameters(new Parameter[]{new DoubleParam("s0", 0){Help = s0Help}});
		}

		public override string Name => "ANOVA";
		public override string Description => "";
		public override float DisplayRank => 0;
		public override bool IsActive => true;
	}
}