using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F2{
	[Serializable]
	internal class Func2Pow : Func2{
		internal override double NumEvaluateDouble(double x, double y){
			return Math.Pow(x, y);
		}

		internal override ReturnType GetValueType(ReturnType returnType1, ReturnType returnType2){
			return ReturnType.Real;
		}

		internal override string ShortName => "pow";

		internal override TreeNode Derivative(int index, TreeNode arg1, TreeNode arg2){
			if (arg1.DependsOnRealVar(index)){
				if (arg2.DependsOnRealVar(index)){
					return Sum(Product(new[]{arg2, Pow(arg1, Difference(arg2, one)), arg1.Derivative(index)}),
						Product(new[]{Log(arg1), Pow(arg1, arg2), arg2.Derivative(index)}));
				}
				return Product(new[]{arg2, Pow(arg1, Difference(arg2, one)), arg1.Derivative(index)});
			}
			return arg2.DependsOnRealVar(index) ? Product(new[]{Log(arg1), Pow(arg1, arg2), arg2.Derivative(index)}) : zero;
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode arg1, TreeNode arg2){
			throw new NotImplementedException();
		}

		internal override string Name => "Power";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}