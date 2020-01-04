using System;
using System.Collections.Generic;
using BaseLibS.Calc.Util;

namespace BaseLibS.Calc{
	[Serializable]
	public class Function{
		private TreeNode root;
		private string[] realVariableNames;
		private string[] intVariableNames;

		private Function(TreeNode root, string[] realVariableNames, string[] intVariableNames){
			this.realVariableNames = realVariableNames;
			this.intVariableNames = intVariableNames;
			this.root = root;
		}

		public double NumEvaluateDouble(double value){
			return NumEvaluateDouble(new Dictionary<int, double>{{0, value}});
		}

		public double NumEvaluateDouble(Dictionary<int, double> vars){
			return root.NumEvaluateDouble(vars);
		}

		public static Function CreateFromString(string text, string realVariableName, out string err){
			return CreateFromString(text, new[]{realVariableName}, new string[0], out err);
		}

		public static Function CreateFromString(string text, string[] realVariableNames, string[] intVariableNames,
			out string err){
			err = null;
			TreeNode root = FuncUtils.ParseString(text, realVariableNames, intVariableNames, ref err);
			return err != null ? null : new Function(root, realVariableNames, intVariableNames);
		}

		private Function() {}

		public Function Derivative(int index){
			return new Function{
				realVariableNames = realVariableNames,
				intVariableNames = intVariableNames,
				root = root.Derivative(index)
			};
		}

		internal Function SimplyfyBasic(){
			return new Function{
				realVariableNames = realVariableNames,
				intVariableNames = intVariableNames,
				root = root.SimplyfyBasic()
			};
		}
	}
}