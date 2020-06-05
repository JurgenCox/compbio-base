using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	[Serializable]
	public class LpDistance : AbstractDistance {
		private double P { get; set; }
		public LpDistance() : this(1.5) { }

		public LpDistance(double p) {
			P = p;
		}

		public override Parameters Parameters {
			set => P = value.GetParam<double>("P").Value;
			get => new Parameters(new DoubleParam("P", 1.5));
		}

		public override double Get(IList<float> x, IList<float> y) {
			return Calc(x, y, P);
		}

		public override double Get(IList<double> x, IList<double> y) {
			return Calc(x, y, P);
		}

		public override double Get(BaseVector x, BaseVector y) {
			return Calc(x, y, P);
		}

		public override DistanceType GetDistanceType(){
			return DistanceType.Lp;
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
					sum += Math.Pow(Math.Abs(d), P);
					c++;
				}
			}
			return c == 0 ? double.NaN : Math.Pow(sum / c * n, 1.0 / P);
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
					sum += Math.Pow(Math.Abs(d), P);
					c++;
				}
			}
			if (c == 0) {
				return double.NaN;
			}
			return c == 0 ? double.NaN : Math.Pow(sum / c * n, 1.0 / P);
		}

		public static double Calc(BaseVector x, BaseVector y, double p) {
			int n = x.Length;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (!double.IsNaN(d)) {
					sum += Math.Pow(Math.Abs(d), p);
					c++;
				}
			}
			return c == 0 ? double.NaN : Math.Pow(sum / c * n, 1.0 / p);
		}

		public static double Calc(IList<double> x, IList<double> y, double p) {
			int n = x.Count;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (!double.IsNaN(d)) {
					sum += Math.Pow(Math.Abs(d), p);
					c++;
				}
			}
			return c == 0 ? double.NaN : Math.Pow(sum / c * n, 1.0 / p);
		}

		public static double Calc(IList<float> x, IList<float> y, double p) {
			int n = x.Count;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++) {
				double d = x[i] - y[i];
				if (!double.IsNaN(d)) {
					sum += Math.Pow(Math.Abs(d), p);
					c++;
				}
			}
			return c == 0 ? double.NaN : Math.Pow(sum / c * n, 1.0 / p);
		}

		public override bool IsAngular => false;
		public override void Write(BinaryWriter writer){
			writer.Write(P);
		}

		public override void Read(BinaryReader reader){
			P = reader.ReadDouble();
		}

		public override object Clone() {
			return new LpDistance(P);
		}

		public override string Name => "Lp";
		public override string Description => "";
		public override float DisplayRank => 3;
		public override bool IsActive => true;
	}
}