using System;
using System.Collections.Generic;
using BaseLibS.Calc.Func;

namespace BaseLibS.Calc.Util{
	//TODO: equality members
	[Serializable]
	internal class TreeNode{
		internal GenericFunc Func { get; set; }
		internal TreeNode[] arguments;
		internal ReturnType ReturnType{
			get{
				if (Func is Variable){
					return ((Variable) Func).type;
				}
				if (Func is Constant){
					return ((Constant) Func).ReturnType;
				}
				if (Func is Func1){
					return ((Func1) Func).GetValueType(arguments[0].ReturnType);
				}
				if (Func is Func2){
					return ((Func2) Func).GetValueType(arguments[0].ReturnType, arguments[1].ReturnType);
				}
				if (Func is FuncN){
					ReturnType[] types = new ReturnType[arguments.Length];
					for (int i = 0; i < types.Length; i++){
						types[i] = arguments[i].ReturnType;
					}
					return ((FuncN) Func).GetValueType(types);
				}
				//TODO
				throw new Exception("Never get here.");
			}
		}
		internal bool IsConstant{
			get{
				if (Func is Constant){
					return true;
				}
				foreach (TreeNode node in arguments){
					if (!node.IsConstant){
						return false;
					}
				}
				return true;
			}
		}

		internal bool DependsOnRealVar(int index){
			if (Func is Constant){
				return false;
			}
			if (Func is Variable){
				Variable v = (Variable) Func;
				if (v.type == ReturnType.Integer){
					return false;
				}
				return v.id == index;
			}
			foreach (TreeNode node in arguments){
				if (node.DependsOnRealVar(index)){
					return true;
				}
			}
			return false;
		}

		internal bool DependsOnIntVar(int index){
			if (Func is Constant){
				return false;
			}
			if (Func is Variable){
				Variable v = (Variable) Func;
				if (v.type == ReturnType.Real){
					return false;
				}
				return v.id == index;
			}
			foreach (TreeNode node in arguments){
				if (node.DependsOnIntVar(index)){
					return true;
				}
			}
			return false;
		}

		internal double NumEvaluateDouble(Dictionary<int, double> vars){
			if (Func is Constant){
				return ((Constant) Func).NumEvaluateDouble;
			}
			if (Func is Variable){
				int id = ((Variable) Func).id;
				if (!vars.ContainsKey(id)){
					throw new ArgumentException("Variable " + id + " is not specified", nameof(vars));
				}
				return vars[id];
			}
			if (Func is Func1){
				return ((Func1) Func).NumEvaluateDouble(arguments[0].NumEvaluateDouble(vars));
			}
			if (Func is Func2){
				return ((Func2) Func).NumEvaluateDouble(arguments[0].NumEvaluateDouble(vars), arguments[1].NumEvaluateDouble(vars));
			}
			if (Func is FuncN){
				double[] x = new double[arguments.Length];
				for (int i = 0; i < x.Length; i++){
					x[i] = arguments[i].NumEvaluateDouble(vars);
				}
				return ((FuncN) Func).NumEvaluateDouble(x);
			}
			//TODO
			throw new Exception("Never get here.");
		}

		internal TreeNode Derivative(int index){
			return Func.Derivative(index, arguments);
		}

		internal TreeNode NthDerivative(int realVar, int intVar){
			return Func.NthDerivative(realVar, intVar, arguments);
		}

		internal TreeNode SimplyfyBasic(){
			throw new NotImplementedException();
		}
	}
}