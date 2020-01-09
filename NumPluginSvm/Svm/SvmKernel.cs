using BaseLibS.Api;
using BaseLibS.Num.Vector;

namespace NumPluginSvm.Svm{
	public abstract class SvmKernel : SvmMatrix{
		private readonly BaseVector[] x;
		private readonly double[] xSquare;
		private readonly IKernelFunction kernelFunction;

		internal SvmKernel(int l, BaseVector[] x1, SvmParameter param){
			kernelFunction = param.kernelFunction;
			x = (BaseVector[]) x1.Clone();
			if (kernelFunction.UsesSquares){
				xSquare = new double[l];
				for (int i = 0; i < l; i++){
					xSquare[i] = x[i].Dot(x[i]);
				}
			} else{
				xSquare = null;
			}
		}

		internal override void SwapIndex(int i, int j){
			{
				BaseVector tmp = x[i];
				x[i] = x[j];
				x[j] = tmp;
			}
			if (xSquare != null){
				{
					double tmp = xSquare[i];
					xSquare[i] = xSquare[j];
					xSquare[j] = tmp;
				}
			}
		}

		internal static double Powi(double base1, int times){
			double tmp = base1, ret = 1.0;
			for (int t = times; t > 0; t /= 2){
				if (t%2 == 1){
					ret *= tmp;
				}
				tmp = tmp*tmp;
			}
			return ret;
		}

		internal double KernelFunctionEval(int i, int j){
			return kernelFunction.Evaluate(x[i], x[j], xSquare?[i] ?? double.NaN,
				xSquare?[j] ?? double.NaN);
		}
	}
}