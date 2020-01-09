using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Gauss : Func1{
		internal override string ShortName => "gauss";
		internal override string Name => "Normal distribution";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.StatisticalDistributions;
		internal override TreeNode DomainMin => negInfinity;
		internal override TreeNode DomainMax => posInfinity;

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im) {
			throw new CannotEvaluateComplexDoubleException();
		}

		internal override bool HasComplexArgument => true;

		internal override double NumEvaluateDouble(double x) {
			return Math.Exp(-0.5*x*x)/Math.Sqrt(2.0*Math.PI);
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