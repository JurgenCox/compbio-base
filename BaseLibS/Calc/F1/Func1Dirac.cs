using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F1{
	[Serializable]
	internal class Func1Dirac : Func1{
		internal override double NumEvaluateDouble(double x){
			return x != 0 ? 0 : double.PositiveInfinity;
		}

		internal override Tuple<double, double> NumEvaluateComplexDouble(double re, double im){
			return new Tuple<double, double>((re != 0 || im != 0) ? 0 : double.PositiveInfinity, 0);
		}

		internal override bool HasComplexArgument => true;

		internal override ReturnType GetValueType(ReturnType returnType){
			return ReturnType.Real;
		}

		internal override TreeNode RealPart(TreeNode re, TreeNode im){
			throw new CannotCalculateRealPartException();
		}

		internal override TreeNode ImaginaryPart(TreeNode re, TreeNode im){
			throw new CannotCalculateImaginaryPartException();
		}

		internal override TreeNode OuterDerivative(TreeNode arg){
			throw new CannotCalculateDerivativeException();
		}

		internal override TreeNode IndefiniteIntegral(TreeNode x){
			return Heavyside(x);
		}

		internal override TreeNode OuterNthDerivative(TreeNode x, TreeNode n){
			throw new CannotCalculateNthDerivativeException();
		}

		internal override TreeNode DomainMin => throw new CannotCalculateDomainException();
		internal override TreeNode DomainMax => throw new CannotCalculateDomainException();
		internal override string ShortName => "dirac";
		internal override string Name => "Dirac's delta distribution";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}