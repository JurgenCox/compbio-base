using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F2{
	[Serializable]
	internal class Func2Max : Func2{
		internal override double NumEvaluateDouble(double x, double y){
			return Math.Max(x, y);
		}

		internal override ReturnType GetValueType(ReturnType returnType1, ReturnType returnType2){
			if (returnType1 == ReturnType.Integer && returnType2 == ReturnType.Integer){
				return ReturnType.Integer;
			}
			return ReturnType.Real;
		}

		internal override TreeNode Derivative(int index, TreeNode arg1, TreeNode arg2){
			return Sum(Product(Heavyside(Difference(arg1, arg2)), arg1.Derivative(index)),
				Product(Heavyside(Difference(arg2, arg1)), arg2.Derivative(index)));
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode arg1, TreeNode arg2){
			throw new CannotCalculateNthDerivativeException();
		}

		internal override string ShortName => "max";
		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}