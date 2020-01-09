using System;
using BaseLibS.Calc.Except;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal class Variable : GenericFunc{
		internal int id;
		internal ReturnType type;
		internal override string ShortName => null;

		internal sealed override TreeNode Derivative(int index, TreeNode[] arguments){
			return type == ReturnType.Integer || id != index ? zero : one;
		}

		internal override TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args){
			if (type == ReturnType.Integer || id != realVar){
				return zero;
			}
			throw new CannotCalculateNthDerivativeException();
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