using System;
using System.Collections.Generic;
using BaseLibS.Util;

namespace BaseLibS.Calc{
	[Serializable]
	public class Relation{
		private readonly RelationType type;
		private readonly Function leftSide;
		private readonly Function rightSide;
		private string[] realVariableNames;
		private string[] intVariableNames;

		private Relation(RelationType type, Function leftSide, Function rightSide, string[] realVariableNames,
			string[] intVariableNames){
			this.realVariableNames = realVariableNames;
			this.intVariableNames = intVariableNames;
			this.type = type;
			this.leftSide = leftSide;
			this.rightSide = rightSide;
		}

		private Relation() {}

		public bool NumEvaluateDouble(double value){
			return NumEvaluateDouble(new Dictionary<int, double>{{0, value}});
		}

		public bool NumEvaluateDouble(Dictionary<int, double> vars){
			double left = leftSide.NumEvaluateDouble(vars);
			double right = rightSide.NumEvaluateDouble(vars);
			switch (type){
				case RelationType.Equal:
					return left == right;
				case RelationType.Greater:
					return left > right;
				case RelationType.GreaterEqual:
					return left >= right;
				case RelationType.Less:
					return left < right;
				case RelationType.LessEqual:
					return left <= right;
				case RelationType.NotEqual:
					return left != right;
				default:
					throw new Exception("Never get here");
			}
		}

		public static Relation CreateFromString(string text, string realVariableName, out string err){
			return CreateFromString(text, new[]{realVariableName}, new string[0], out err);
		}

		public static Relation CreateFromString(string text, string[] realVariableNames, string[] intVariableNames,
			out string err){
			string leftString;
			string rightString;
			RelationType type = GetRelationType(text, out leftString, out rightString, out err);
			if (err != null){
				return null;
			}
			Function leftFunc = Function.CreateFromString(leftString, realVariableNames, intVariableNames, out err);
			if (err != null){
				return null;
			}
			Function rightFunc = Function.CreateFromString(rightString, realVariableNames, intVariableNames, out err);
			return err != null ? null : new Relation(type, leftFunc, rightFunc, realVariableNames, intVariableNames);
		}

		private static RelationType GetRelationType(string text, out string leftString, out string rightString, out string err){
			err = null;
			leftString = null;
			rightString = null;
			int[] inds = StringUtils.AllIndicesOf(text, "!=");
			if (inds.Length > 1){
				err = "Multiple != signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 2, out leftString, out rightString);
				return RelationType.NotEqual;
			}
			inds = StringUtils.AllIndicesOf(text, "==");
			if (inds.Length > 1){
				err = "Multiple == signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 2, out leftString, out rightString);
				return RelationType.Equal;
			}
			inds = StringUtils.AllIndicesOf(text, ">=");
			if (inds.Length > 1){
				err = "Multiple >= signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 2, out leftString, out rightString);
				return RelationType.GreaterEqual;
			}
			inds = StringUtils.AllIndicesOf(text, "<=");
			if (inds.Length > 1){
				err = "Multiple <= signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 2, out leftString, out rightString);
				return RelationType.LessEqual;
			}
			inds = StringUtils.AllIndicesOf(text, "=");
			if (inds.Length > 1){
				err = "Multiple = signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 1, out leftString, out rightString);
				return RelationType.Equal;
			}
			inds = StringUtils.AllIndicesOf(text, ">");
			if (inds.Length > 1){
				err = "Multiple > signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 1, out leftString, out rightString);
				return RelationType.Greater;
			}
			inds = StringUtils.AllIndicesOf(text, "<");
			if (inds.Length > 1){
				err = "Multiple < signs.";
				return RelationType.Unknown;
			}
			if (inds.Length == 1){
				Extract(text, inds[0], 1, out leftString, out rightString);
				return RelationType.Less;
			}
			err = "No valid operator.";
			return RelationType.Unknown;
		}

		private static void Extract(string text, int pos, int len, out string leftString, out string rightString){
			leftString = text.Substring(0, pos).Trim();
			rightString = text.Substring(pos + len, text.Length - pos - len).Trim();
		}
	}
}