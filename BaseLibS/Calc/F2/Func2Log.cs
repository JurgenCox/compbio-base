using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F2{
	[Serializable]
	internal class Func2Log : Func2{
		internal override double NumEvaluateDouble(double x, double y){
			return Math.Log(x, y);
		}

		internal override ReturnType GetValueType(ReturnType returnType1, ReturnType returnType2){
			return ReturnType.Real;
		}

		internal override TreeNode Derivative(int index, TreeNode arg1, TreeNode arg2){
			if (arg1.DependsOnRealVar(index)){
				if (arg2.DependsOnRealVar(index)){
					return Difference(Ratio(arg1.Derivative(index), Product(arg1, Log(arg2))),
						Ratio(Product(Log(arg1), arg2.Derivative(index)), Product(arg2, Square(arg2))));
				}
				return Ratio(arg1.Derivative(index), Product(arg1, Log(arg2)));
			}
			return arg2.DependsOnRealVar(index) ? Minus(Ratio(Product(Log(arg1), arg2.Derivative(index)), Product(arg2, Square(arg2)))) : zero;
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode arg1, TreeNode arg2){
			throw new NotImplementedException();
		}

		internal override string ShortName => "log";
		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}