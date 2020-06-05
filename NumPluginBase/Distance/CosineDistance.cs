using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	[Serializable]
	public class CosineDistance : AbstractDistance {
		public override Parameters Parameters {
			set { }
			get => new Parameters();
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

		public static double Calc(BaseVector x, BaseVector y) {
			int n = x.Length;
			int c = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d)) {
					continue;
				}
				c++;
			}
			if (c < 3) {
				return double.NaN;
			}
			double sx = 0;
			double sy = 0;
			double sxy = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d)) {
					continue;
				}
				double wx = x[i];
				double wy = y[i];
				sx += wx * wx;
				sy += wy * wy;
				sxy += wx * wy;
			}
			sx /= c;
			sy /= c;
			sxy /= c;
			double corr = sxy / Math.Sqrt(sx * sy);
			return 1 - corr;
		}

		public static double Calc(IList<double> x, IList<double> y) {
			int n = x.Count;
			int c = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d)) {
					continue;
				}
				c++;
			}
			if (c < 3) {
				return double.NaN;
			}
			double sx = 0;
			double sy = 0;
			double sxy = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d)) {
					continue;
				}
				double wx = x[i];
				double wy = y[i];
				sx += wx * wx;
				sy += wy * wy;
				sxy += wx * wy;
			}
			sx /= c;
			sy /= c;
			sxy /= c;
			double corr = sxy / Math.Sqrt(sx * sy);
			return 1 - corr;
		}

		public static double Calc(IList<float> x, IList<float> y) {
			int n = x.Count;
			int c = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d)) {
					continue;
				}
				c++;
			}
			if (c < 3) {
				return double.NaN;
			}
			double sx = 0;
			double sy = 0;
			double sxy = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d)) {
					continue;
				}
				double wx = x[i];
				double wy = y[i];
				sx += wx * wx;
				sy += wy * wy;
				sxy += wx * wy;
			}
			sx /= c;
			sy /= c;
			sxy /= c;
			double corr = sxy / Math.Sqrt(sx * sy);
			return 1 - corr;
		}

		public override bool IsAngular => true;
		public override void Write(BinaryWriter writer){
		}

		public override void Read(BinaryReader reader){
		}

		public override DistanceType GetDistanceType(){
			return DistanceType.Cosine;
		}

		public override object Clone() {
			return new CosineDistance();
		}

		public override string Name => "Cosine";
		public override string Description => "";
		public override float DisplayRank => 6;
		public override bool IsActive => true;
	}
}