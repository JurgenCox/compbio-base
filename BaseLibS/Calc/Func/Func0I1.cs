using System;
using BaseLibS.Calc.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal abstract class Func0I1 : GenericFunc{
		internal abstract double NumEvaluateDouble(long n);
		internal abstract ReturnType ReturnType { get; }

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args) {
			return zero;
		}

		internal override TreeNode IndefiniteIntegral(int index, TreeNode[] args) {
			return Product(
				new TreeNode { Func = new Variable { id = index, type = ReturnType.Real }, arguments = new TreeNode[0] },
				new TreeNode { Func = this, arguments = args });
		}

		internal sealed override TreeNode Derivative(int index, TreeNode[] args) {
			return zero;
		}
	}
}