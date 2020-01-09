using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Tan : Func1{
		internal override double NumEvaluateDouble(double x){
			return Math.Tan(x);
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im) {
			throw new CannotEvaluateComplexDoubleException();
		}

		internal override bool HasComplexArgument => true;

		internal override ReturnType GetValueType(ReturnType returnType) {
			return returnType == ReturnType.Integer ? ReturnType.Real : returnType;
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateRealPartException();
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateImaginaryPartException();
		}

		internal override TreeNode OuterDerivative(TreeNode arg) {
			return Inverse(Square(Cos(arg)));
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x){
			return Minus(Log(Cos(x)));
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n) {
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode DomainMin => negInfinity;
		internal override TreeNode DomainMax => posInfinity;
		internal override string ShortName => "tan";
		internal override string Name => "Tangent";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.TrigonometricFunctions;
	}
}