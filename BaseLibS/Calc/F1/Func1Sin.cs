using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Sin : Func1{
		internal override double NumEvaluateDouble(double x){
			return Math.Sin(x);
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im){
			return new Tuple<double, double>(Math.Sin(re)*Math.Cosh(im), Math.Cos(re)*Math.Sinh(im));
		}

		internal override bool HasComplexArgument => true;

		internal override ReturnType GetValueType(ReturnType returnType){
			return returnType == ReturnType.Integer ? ReturnType.Real : returnType;
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im){
			return Product(Sin(re), Cosh(im));
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im){
			return Product(Cos(re), Sinh(im));
		}

		internal override TreeNode OuterDerivative(TreeNode x){
			return Cos(x);
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x){
			return Minus(Cos(x));
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n){
			return Sin(Sum(x, Product(Ratio(pi, two), n)));
		}

		internal override TreeNode DomainMin => negInfinity;
		internal override TreeNode DomainMax => posInfinity;
		internal override string ShortName => "sin";
		internal override string Name => "Sine";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.TrigonometricFunctions;
	}
}