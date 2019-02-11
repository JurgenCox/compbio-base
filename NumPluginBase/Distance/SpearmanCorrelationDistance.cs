using System;
using System.Collections.Generic;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	[Serializable]
	public class SpearmanCorrelationDistance : AbstractDistance {
		public override Parameters Parameters {
			set { }
			get { return new Parameters(); }
		}

		public override double Get(IList<float> x, IList<float> y) {
			return Calc(x, y);
		}

		public override double Get(IList<double> x, IList<double> y) {
			return Calc(x, y);
		}

		public override double Get(BaseVector x, BaseVector y) {
			return Calc(x, y);
		}

		public override bool IsAngular => true;

		public static double Calc(IList<double> x, IList<double> y) {
			int n = x.Count;
			List<int> valids = new List<int>();
			for (int i = 0; i < n; i++) {
				double xx = x[i];
				double yy = y[i];
				if (double.IsNaN(xx) || double.IsNaN(yy) || double.IsInfinity(xx) || double.IsInfinity(yy)) {
					continue;
				}
				valids.Add(i);
			}
			if (valids.Count < 3) {
				return double.NaN;
			}
			return PearsonCorrelationDistance.Calc(ArrayUtils.Rank(ArrayUtils.SubArray(x, valids)),
				ArrayUtils.Rank(ArrayUtils.SubArray(y, valids)));
		}

		public static double Calc(BaseVector x, BaseVector y) {
			int n = x.Length;
			List<int> valids = new List<int>();
			for (int i = 0; i < n; i++) {
				double xx = x[i];
				double yy = y[i];
				if (double.IsNaN(xx) || double.IsNaN(yy) || double.IsInfinity(xx) || double.IsInfinity(yy)) {
					continue;
				}
				valids.Add(i);
			}
			if (valids.Count < 3) {
				return double.NaN;
			}
			return PearsonCorrelationDistance.Calc(ArrayUtils.Rank(x.SubArray(valids)), ArrayUtils.Rank(y.SubArray(valids)));
		}

		public static double Calc(IList<float> x, IList<float> y) {
			int n = x.Count;
			List<int> valids = new List<int>();
			for (int i = 0; i < n; i++) {
				double xx = x[i];
				double yy = y[i];
				if (double.IsNaN(xx) || double.IsNaN(yy) || double.IsInfinity(xx) || double.IsInfinity(yy)) {
					continue;
				}
				valids.Add(i);
			}
			if (valids.Count < 3) {
				return double.NaN;
			}
			return PearsonCorrelationDistance.Calc(ArrayUtils.RankF(ArrayUtils.SubArray(x, valids)),
				ArrayUtils.RankF(ArrayUtils.SubArray(y, valids)));
		}

		public override object Clone() {
			return new SpearmanCorrelationDistance();
		}

		public override string Name => "Spearman correlation";
		public override string Description => "";
		public override float DisplayRank => 5;
		public override bool IsActive => true;
	}
}