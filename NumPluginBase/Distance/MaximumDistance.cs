using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	[Serializable]
	public class MaximumDistance : AbstractDistance {
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

		public override DistanceType GetDistanceType(){
			return DistanceType.Maximum;
		}

		public override double Get(float[,] data1, float[,] data2, int index1, int index2, MatrixAccess access1,
			MatrixAccess access2) {
			int n = data1.GetLength(access1 == MatrixAccess.Rows ? 1 : 0);
			double max = double.MinValue;
			for (int i = 0; i < n; i++) {
				double d1 = access1 == MatrixAccess.Rows ? data1[index1, i] : data1[i, index1];
				double d2 = access2 == MatrixAccess.Rows ? data2[index2, i] : data2[i, index2];
				double d = d1 - d2;
				if (double.IsNaN(d) || double.IsInfinity(d)) {
					continue;
				}
				double dist = Math.Abs(d);
				if (dist > max) {
					max = dist;
				}
			}
			return max == double.MinValue ? double.NaN : max;
		}

		public override double Get(double[,] data1, double[,] data2, int index1, int index2, MatrixAccess access1,
			MatrixAccess access2) {
			int n = data1.GetLength(access1 == MatrixAccess.Rows ? 1 : 0);
			double max = double.MinValue;
			for (int i = 0; i < n; i++) {
				double d1 = access1 == MatrixAccess.Rows ? data1[index1, i] : data1[i, index1];
				double d2 = access2 == MatrixAccess.Rows ? data2[index2, i] : data2[i, index2];
				double d = d1 - d2;
				if (double.IsNaN(d) || double.IsInfinity(d)) {
					continue;
				}
				double dist = Math.Abs(d);
				if (dist > max) {
					max = dist;
				}
			}
			return max == double.MinValue ? double.NaN : max;
		}

		public static double Calc(IList<float> x, IList<float> y) {
			int n = x.Count;
			float max = float.MinValue;
			for (int i = 0; i < n; i++) {
				float d = x[i] - y[i];
				if (float.IsNaN(d) || float.IsInfinity(d)) {
					continue;
				}
				float dist = Math.Abs(d);
				if (dist > max) {
					max = dist;
				}
			}
			return max == float.MinValue ? double.NaN : max;
		}

		public static double Calc(BaseVector x, BaseVector y) {
			int n = x.Length;
			double max = double.MinValue;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d) || double.IsInfinity(d)) {
					continue;
				}
				double dist = Math.Abs(d);
				if (dist > max) {
					max = dist;
				}
			}
			return max == double.MinValue ? double.NaN : max;
		}

		public static double Calc(IList<double> x, IList<double> y) {
			int n = x.Count;
			double max = double.MinValue;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (double.IsNaN(d) || double.IsInfinity(d)) {
					continue;
				}
				double dist = Math.Abs(d);
				if (dist > max) {
					max = dist;
				}
			}
			return max == double.MinValue ? double.NaN : max;
		}

		public override bool IsAngular => false;
		public override void Write(BinaryWriter writer){
			
		}

		public override void Read(BinaryReader reader){
		}

		public override object Clone() {
			return new MaximumDistance();
		}

		public override string Name => "Maximum";
		public override string Description => "";
		public override float DisplayRank => 2;
		public override bool IsActive => true;
	}
}