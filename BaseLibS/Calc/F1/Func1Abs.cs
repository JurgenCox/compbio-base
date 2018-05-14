using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Abs : Func1{
		internal override double NumEvaluateDouble(double x){
			return Math.Abs(x);
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im){
			return new Tuple<double, double>(Math.Sqrt(re*re + im*im), 0);
		}

		internal override bool HasComplexArgument => true;

		internal override ReturnType GetValueType(ReturnType inputType){
			return inputType == ReturnType.Complex ? ReturnType.Real : inputType;
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x){
			return Ratio(Product(Sign(x), Square(x)), two);
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im){
			return Sqrt(Sum(Square(re), Square(im)));
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im){
			return zero;
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n){
			if (n.Equals(one)){
				return Sign(x);
			}
			if (n.Equals(two)){
				return Product(two, Dirac(x));
			}
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode DomainMin => negInfinity;
		internal override TreeNode DomainMax => posInfinity;

		internal override TreeNode OuterDerivative(TreeNode arg){
			return Sign(arg);
		}

		internal override string ShortName => "abs";
		internal override string Name => "absolute value";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.UtilityFunctions;
	}
}