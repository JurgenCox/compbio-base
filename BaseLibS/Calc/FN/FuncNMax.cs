using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Calc.FN{
	[Serializable]
	internal class FuncNMax : FuncN{
		internal override double NumEvaluateDouble(double[] x){
			return ArrayUtils.Max(x);
		}

		internal override ReturnType GetValueType(ReturnType[] types){
			foreach (ReturnType type in types){
				if (type == ReturnType.Real){
					return ReturnType.Real;
				}
			}
			return ReturnType.Integer;
		}

		internal override string ShortName => "max";

		internal override TreeNode Derivative(int index, TreeNode[] args){
			throw new System.NotImplementedException();
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args){
			throw new NotImplementedException();
		}

		internal override TreeNode IndefiniteIntegral(int index, TreeNode[] args){
			throw new NotImplementedException();
		}

		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}