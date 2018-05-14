using System;
using System.Collections.Generic;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Calc.FN{
	[Serializable]
	internal class FuncNSum : FuncN{
		internal override double NumEvaluateDouble(double[] x){
			return ArrayUtils.Sum(x);
		}

		internal override ReturnType GetValueType(ReturnType[] types){
			foreach (ReturnType type in types){
				if (type == ReturnType.Real){
					return ReturnType.Real;
				}
			}
			return ReturnType.Integer;
		}

		internal override TreeNode Derivative(int index, TreeNode[] args){
			List<TreeNode> result = new List<TreeNode>();
			foreach (TreeNode node in args){
				if (node.DependsOnRealVar(index)){
					result.Add(node.Derivative(index));
				}
			}
			if (result.Count == 0){
				return zero;
			}
			if (result.Count == 1){
				return result[0];
			}
			return result.Count == 2 ? Sum(result[0], result[1]) : Sum(result.ToArray());
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args){
			throw new NotImplementedException();
		}

		internal override TreeNode IndefiniteIntegral(int index, TreeNode[] args){
			throw new NotImplementedException();
		}

		internal override string ShortName => "sum";
		internal override string Name => "sum";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}