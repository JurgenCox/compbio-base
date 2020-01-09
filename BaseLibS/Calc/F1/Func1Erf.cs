using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Erf : Func1{
		internal override string ShortName => "erf";
		internal override string Name => "error function";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
		internal override TreeNode DomainMin => throw new CannotCalculateDomainException();
		internal override TreeNode DomainMax => throw new CannotCalculateDomainException();

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im) {
			throw new CannotEvaluateComplexDoubleException();
		}

		internal override bool HasComplexArgument => true;

		internal override double NumEvaluateDouble(double x) {
			return NumUtils.Erff(x);
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateRealPartException();
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im) {
			throw new CannotCalculateImaginaryPartException();
		}

		internal override ReturnType GetValueType(ReturnType returnType) {
			return ReturnType.Real;
		}

		internal override TreeNode IndefiniteIntegral(TreeNode arg){
			throw new NotImplementedException();
		}

		internal override TreeNode OuterDerivative(TreeNode arg){
			throw new NotImplementedException();
		}

		internal override TreeNode OuterNthDerivative(TreeNode arg, TreeNode n){
			throw new NotImplementedException();
		}
	}
}