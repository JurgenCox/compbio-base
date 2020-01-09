using System;
using BaseLibS.Calc.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal abstract class Constant : GenericFunc{
		internal abstract double NumEvaluateDouble { get; }
		internal abstract ReturnType ReturnType { get; }

		internal sealed override TreeNode IndefiniteIntegral(int index, TreeNode[] arguments){
			return Product(
				new TreeNode{Func = new Variable{id = index, type = ReturnType.Real}, arguments = new TreeNode[0]},
				new TreeNode{Func = this, arguments = new TreeNode[0]});
		}

		internal sealed override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] arguments){
			return zero;
		}

		internal sealed override TreeNode Derivative(int index, TreeNode[] arguments){
			return zero;
		}
	}
}