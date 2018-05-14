using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F2{
	[Serializable]
	internal class Func2Diff : Func2{
		internal override double NumEvaluateDouble(double x, double y){
			return x - y;
		}

		internal override ReturnType GetValueType(ReturnType returnType1, ReturnType returnType2){
			if (returnType1 == ReturnType.Integer && returnType2 == ReturnType.Integer){
				return ReturnType.Integer;
			}
			return ReturnType.Real;
		}

		internal override string ShortName => null;

		internal override TreeNode Derivative(int index, TreeNode arg1, TreeNode arg2){
			if (arg1.DependsOnRealVar(index)){
				return arg2.DependsOnRealVar(index)
					? Difference(arg1.Derivative(index), arg2.Derivative(index)) : arg1.Derivative(index);
			}
			return arg2.DependsOnRealVar(index) ? Minus(arg2.Derivative(index)) : zero;
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode arg1, TreeNode arg2){
			throw new NotImplementedException();
		}

		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}