using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F2{
	[Serializable]
	internal class Func2Atan : Func2{
		internal override string ShortName => "atan";
		internal override string Name => throw new NotImplementedException();
		internal override string Description => throw new NotImplementedException();
		internal override DocumentType DescriptionType => throw new NotImplementedException();
		internal override Topic Topic => throw new NotImplementedException();

		internal override double NumEvaluateDouble(double x, double y){
			return Math.Atan2(y, x);
		}

		internal override ReturnType GetValueType(ReturnType returnType1, ReturnType returnType2){
			throw new NotImplementedException();
		}

		internal override TreeNode Derivative(int index, TreeNode x, TreeNode y){
			throw new NotImplementedException();
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode x, TreeNode y){
			throw new NotImplementedException();
		}
	}
}