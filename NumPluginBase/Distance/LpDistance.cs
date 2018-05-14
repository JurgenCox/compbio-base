using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Distance{
	[Serializable]
	public class LpDistance : IDistance{
		private double P { get; set; }
		public LpDistance() : this(1.5) { }
		public LpDistance(double p) { P = p; }
		public Parameters Parameters { set => P = value.GetParam<double>("P").Value;
			get => new Parameters(new DoubleParam("P", 1.5));
		}
		public double Get(IList<float> x, IList<float> y) { return Calc(x, y); }
		public double Get(IList<double> x, IList<double> y) { return Calc(x, y); }
		public double Get(BaseVector x, BaseVector y) { return Calc(x, y); }

		public double Get(float[,] data1, float[,] data2, int index1, int index2, MatrixAccess access){
			if (access == MatrixAccess.Rows){
				int n = data1.GetLength(1);
				int c = 0;
				double sum = 0;
				for (int i = 0; i < n; i++){
					double d = data1[index1, i] - data2[index2, i];
					if (!double.IsNaN(d)){
						sum += Math.Abs(d);
						c++;
					}
				}
				if (c == 0){
					return double.NaN;
				}
				return sum/c*n;
			} else{
				int n = data1.GetLength(0);
				int c = 0;
				double sum = 0;
				for (int i = 0; i < n; i++){
					double d = data1[i, index1] - data2[i, index2];
					if (!double.IsNaN(d)){
						sum += Math.Abs(d);
						c++;
					}
				}
				if (c == 0){
					return double.NaN;
				}
				return sum/c*n;
			}
		}

		public double Get(double[,] data1, double[,] data2, int index1, int index2, MatrixAccess access){
			if (access == MatrixAccess.Rows){
				int n = data1.GetLength(1);
				int c = 0;
				double sum = 0;
				for (int i = 0; i < n; i++){
					double d = data1[index1, i] - data2[index2, i];
					if (!double.IsNaN(d)){
						sum += Math.Abs(d);
						c++;
					}
				}
				if (c == 0){
					return double.NaN;
				}
				return sum/c*n;
			} else{
				int n = data1.GetLength(0);
				int c = 0;
				double sum = 0;
				for (int i = 0; i < n; i++){
					double d = data1[i, index1] - data2[i, index2];
					if (!double.IsNaN(d)){
						sum += Math.Abs(d);
						c++;
					}
				}
				if (c == 0){
					return double.NaN;
				}
				return sum/c*n;
			}
		}

		//TODO
		public static double Calc(BaseVector x, BaseVector y){
			int n = x.Length;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++){
				double d = x[i] - y[i];
				if (!double.IsNaN(d)){
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0){
				return double.NaN;
			}
			return sum/c*n;
		}

		public static double Calc(IList<double> x, IList<double> y){
			int n = x.Count;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++){
				double d = x[i] - y[i];
				if (!double.IsNaN(d)){
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0){
				return double.NaN;
			}
			return sum/c*n;
		}

		public static double Calc(IList<float> x, IList<float> y){
			int n = x.Count;
			int c = 0;
			double sum = 0;
			for (int i = 0; i < n; i++){
				double d = x[i] - y[i];
				if (!double.IsNaN(d)){
					sum += Math.Abs(d);
					c++;
				}
			}
			if (c == 0){
				return double.NaN;
			}
			return sum/c*n;
		}

		public object Clone() { return new LpDistance(P); }
		public string Name => "Lp";
		public string Description => "";
		public float DisplayRank => 3;
		public bool IsActive => true;
	}
}