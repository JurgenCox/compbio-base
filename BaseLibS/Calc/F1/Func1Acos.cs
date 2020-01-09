using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

//TODO: from here
namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Acos : Func1{
		internal override double NumEvaluateDouble(double x){
			return Math.Acos(x);
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im){
			throw new CannotEvaluateComplexDoubleException();
		}

		internal override bool HasComplexArgument => true;

		internal override ReturnType GetValueType(ReturnType returnType){
			return returnType == ReturnType.Integer ? ReturnType.Real : returnType;
		}

		internal override TreeNode OuterDerivative(TreeNode arg){
			return Minus(Inverse(Sqrt(Difference(one, Square(arg)))));
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x){
			return Difference(Product(x, Acos(x)), Sqrt(Difference(one, Square(x))));
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im){
			throw new CannotCalculateRealPartException();
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im){
			throw new CannotCalculateImaginaryPartException();
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n){
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode DomainMin => minusOne;
		internal override TreeNode DomainMax => one;
		internal override string ShortName => "acos";
		internal override string Name => "inverse cosine";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.TrigonometricFunctions;
	}
}