using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Acot : Func1{
		internal override double NumEvaluateDouble(double x){
			return Math.PI*0.5 - Math.Atan(x);
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im) {
			throw new CannotEvaluateComplexDoubleException();
		}

		internal override bool HasComplexArgument => true;

		internal override ReturnType GetValueType(ReturnType returnType) {
			return returnType == ReturnType.Integer ? ReturnType.Real : returnType;
		}

		internal override TreeNode OuterDerivative(TreeNode x){
			return Minus(Inverse(Sum(one, Square(x))));
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateRealPartException();
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateImaginaryPartException();
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x) {
			throw new CannotCalculateIndefiniteIntegralException();
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n) {
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode DomainMin => throw new CannotCalculateDomainException();
		internal override TreeNode DomainMax => throw new CannotCalculateDomainException();
		internal override string ShortName => "acot";
		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}