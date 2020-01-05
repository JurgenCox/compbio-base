using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	[Serializable]
	public class L1Distance : AbstractDistance {
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

		public override double Get(float[,] data1, float[,] data2, int index1, int index2, MatrixAccess access1,
			MatrixAccess access2) {
			int n = data1.GetLength(access1 == MatrixAccess.Rows ? 1 : 0);
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d1 = access1 == MatrixAccess.Rows ? data1[index1, i] : data1[i, index1];
				double d2 = access2 == MatrixAccess.Rows ? data2[index2, i] : data2[i, index2];
				double d = d1 - d2;
				if (!double.IsNaN(d)) {
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0) {
				return double.NaN;
			}
			return sum / c * n;
		}

		public override double Get(double[,] data1, double[,] data2, int index1, int index2, MatrixAccess access1,
			MatrixAccess access2) {
			int n = data1.GetLength(access1 == MatrixAccess.Rows ? 1 : 0);
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d1 = access1 == MatrixAccess.Rows ? data1[index1, i] : data1[i, index1];
				double d2 = access2 == MatrixAccess.Rows ? data2[index2, i] : data2[i, index2];
				double d = d1 - d2;
				if (!double.IsNaN(d)) {
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0) {
				return double.NaN;
			}
			return sum / c * n;
		}

		public static double Calc(BaseVector x, BaseVector y) {
			int n = x.Length;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (!double.IsNaN(d)) {
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0) {
				return double.NaN;
			}
			return sum / c * n;
		}

		public static double Calc(IList<double> x, IList<double> y) {
			int n = x.Count;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (!double.IsNaN(d)) {
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0) {
				return double.NaN;
			}
			return sum / c * n;
		}

		public static double Calc(IList<float> x, IList<float> y) {
			int n = x.Count;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (!double.IsNaN(d)) {
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0) {
				return double.NaN;
			}
			return sum / c * n;
		}

		public override bool IsAngular => false;

		public override object Clone() {
			return new L1Distance();
		}

		public override string Name => "L1";
		public override string Description => "";
		public override float DisplayRank => 1;
		public override bool IsActive => true;
	}
}