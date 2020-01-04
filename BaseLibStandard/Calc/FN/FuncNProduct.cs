using System;
using System.Collections.Generic;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Calc.FN{
	[Serializable]
	internal class FuncNProduct : FuncN{
		internal override double NumEvaluateDouble(double[] x){
			return ArrayUtils.Product(x);
		}

		internal override ReturnType GetValueType(ReturnType[] types){
			foreach (ReturnType type in types){
				if (type == ReturnType.Real){
					return ReturnType.Real;
				}
			}
			return ReturnType.Integer;
		}

		internal override string ShortName => null;

		internal override TreeNode Derivative(int index, TreeNode[] args){
			List<TreeNode> result = new List<TreeNode>();
			for (int i = 0; i < args.Length; i++){
				TreeNode node = args[i];
				if (node.DependsOnRealVar(index)){
					TreeNode[] w = new TreeNode[args.Length];
					for (int j = 0; j < args.Length; j++){
						if (i == j){
							w[j] = args[j].Derivative(index);
						} else{
							w[j] = args[j];
						}
					}
					result.Add(Product(w));
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

		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}