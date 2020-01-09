using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Kernel{
	[Serializable]
	public class PolynomialKernelFunction : IKernelFunction{
		public int Degree { get; set; }
		public double Gamma { get; set; }
		public double Offset { get; set; }

		public PolynomialKernelFunction(int degree, double gamma, double offset){
			Degree = degree;
			Gamma = gamma;
			Offset = offset;
		}

		public PolynomialKernelFunction() : this(3, 0.01, 0){}
		public bool UsesSquares => false;
		public string Name => "Polynomial";

		public Parameters Parameters{
			get => new Parameters(
				new IntParam("Degree", Degree){
					Help = "The degree of the polynomial. A degree of one will reproduce the linear kernel"
				},
				new DoubleParam("Gamma", Gamma){Help = "Coefficient in front of the scalar product."},
				new DoubleParam("Offset", Offset){Help = "Shift parameter."});
			set{
				Degree = value.GetParam<int>("Degree").Value;
				Gamma = value.GetParam<double>("Gamma").Value;
				Offset = value.GetParam<double>("Offset").Value;
			}
		}

		public double Evaluate(BaseVector xi, BaseVector xj, double xSquarei, double xSquarej){
			return Powi(Gamma*xi.Dot(xj) + Offset, Degree);
		}

		public object Clone(){
			return new PolynomialKernelFunction(Degree, Gamma, Offset);
		}

		private static double Powi(double base1, int times){
			double tmp = base1;
			double ret = 1.0;
			for (int t = times; t > 0; t /= 2){
				if (t%2 == 1){
					ret *= tmp;
				}
				tmp = tmp*tmp;
			}
			return ret;
		}

		public string Description => "";
		public float DisplayRank => 2;
		public bool IsActive => true;
	}
}