using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal abstract class Func1 : GenericFunc{
		internal sealed override TreeNode Derivative(int index, TreeNode[] args){
			TreeNode arg = args[0];
			return arg.DependsOnRealVar(index) ? Product(OuterDerivative(arg), arg.Derivative(index)) : zero;
		}

		internal sealed override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args){
			TreeNode arg = args[0];
			if (!arg.DependsOnRealVar(realVar)){
				return zero;
			}
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode IndefiniteIntegral(int index, TreeNode[] args){
			throw new CannotCalculateIndefiniteIntegralException();
		}

		internal abstract double NumEvaluateDouble(double x);
		internal abstract Tuple<double, double> NumEvaluateComplexDouble(double re, double im);
		internal abstract bool HasComplexArgument { get; }
		internal abstract ReturnType GetValueType(ReturnType returnType);
		internal abstract TreeNode IndefiniteIntegral(TreeNode x);
		internal abstract TreeNode RealPart(TreeNode re, TreeNode im);
		internal abstract TreeNode ImaginaryPart(TreeNode re, TreeNode im);
		internal abstract TreeNode OuterDerivative(TreeNode x);
		internal abstract TreeNode OuterNthDerivative(TreeNode x, TreeNode n);
		internal abstract TreeNode DomainMin { get; }
		internal abstract TreeNode DomainMax { get; }
	}
}