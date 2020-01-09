using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace NumPluginBase.Kernel{
	[Serializable]
	public class RbfKernelFunction : IKernelFunction{
		private double Sigma { get; set; }
		public RbfKernelFunction() : this(1){}

		public RbfKernelFunction(double sigma){
			Sigma = sigma;
		}

		public bool UsesSquares => true;
		public string Name => "RBF";

		public Parameters Parameters{
			get => new Parameters(new DoubleParam("Sigma", Sigma){Help = "Standard deviation parameter."});
			set => Sigma = value.GetParam<double>("Sigma").Value;
		}

		public double Evaluate(BaseVector xi, BaseVector xj, double xSquarei, double xSquarej){
			return Math.Exp(-(xSquarei + xSquarej - 2*xi.Dot(xj))/2.0/xi.Length/Sigma/Sigma);
		}

		public object Clone(){
			return new RbfKernelFunction(Sigma);
		}

		public string Description => "";
		public float DisplayRank => 1;
		public bool IsActive => true;
	}
}