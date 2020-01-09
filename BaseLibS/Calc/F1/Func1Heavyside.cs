using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Heavyside : Func1{
		internal override double NumEvaluateDouble(double x){
			if (x < 0){
				return 0;
			}
			return x > 0 ? 1 : 0.5;
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im) {
			return null;
		}

		internal override bool HasComplexArgument => false;

		internal override TreeNode RealPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateRealPartException();
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateImaginaryPartException();
		}

		internal override ReturnType GetValueType(ReturnType returnType) {
			return ReturnType.Real;
		}

		internal override TreeNode OuterDerivative(TreeNode argument){
			return Dirac(argument);
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x){
			throw new CannotCalculateIndefiniteIntegralException();
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n) {
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode DomainMin => throw new CannotCalculateDomainException();
		internal override TreeNode DomainMax => throw new CannotCalculateDomainException();
		internal override string ShortName => "heavyside";
		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.UtilityFunctions;
	}
}