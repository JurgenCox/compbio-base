using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal abstract class Func2 : GenericFunc{
		internal abstract double NumEvaluateDouble(double x, double y);
		internal abstract ReturnType GetValueType(ReturnType returnType1, ReturnType returnType2);

		internal sealed override TreeNode Derivative(int index, TreeNode[] args){
			return Derivative(index, args[0], args[1]);
		}

		internal sealed override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args){
			return NthDerivative(realVar, intVar, args[0], args[1]);
		}

		internal override TreeNode IndefiniteIntegral(int index, TreeNode[] args){
			throw new CannotCalculateIndefiniteIntegralException();
		}

		internal abstract TreeNode Derivative(int index, TreeNode arg1, TreeNode arg2);
		internal abstract TreeNode NthDerivative(int realVar, int intVar, TreeNode arg1, TreeNode arg2);
	}
}