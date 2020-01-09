using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLibS.Calc.Const;
using BaseLibS.Calc.F1;
using BaseLibS.Calc.F2;
using BaseLibS.Calc.Func;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Calc.Util{
	internal static class FuncUtils{
		private static readonly HashSet<char> operators = new HashSet<char>{'+', '-', '*', '/', '^'};
		private static readonly HashSet<char> numberChars = new HashSet<char>
		{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'};
		private static readonly HashSet<char> opening = new HashSet<char>{'(', '{', '['};
		private static readonly HashSet<char> closing = new HashSet<char>{')', '}', ']'};
		private static readonly Dictionary<char, char> brackets = new Dictionary<char, char>
		{{'(', ')'}, {'{', '}'}, {'[', ']'}};

		internal static TreeNode ParseString(string text, IList<string> realVariableNames, IList<string> intVariableNames,
			ref string errMsg){
			text = StringUtils.RemoveWhitespace(text);
			text = text.ToLowerInvariant();
			text = PreprocessScientificNotation(text);
			return ParseImpl(text, realVariableNames, intVariableNames, ref errMsg);
		}

		private static string PreprocessScientificNotation(string text){
			int[] epos = GetEPositions(text);
			if (epos.Length == 0){
				return text;
			}
			int[] starts;
			int[] ends;
			string[] mants;
			int[] exps;
			GetEPositions(text, epos, out starts, out ends, out mants, out exps);
			if (starts.Length == 0){
				return text;
			}
			StringBuilder result = new StringBuilder();
			result.Append(text.Substring(0, starts[0]));
			result.Append(GetNumberString(mants[0], exps[0]));
			for (int i = 1; i < starts.Length; i++){
				result.Append(text.Substring(ends[i - 1], starts[i] - ends[i - 1]));
				result.Append(GetNumberString(mants[i], exps[i]));
			}
			result.Append(text.Substring(ends[ends.Length - 1]));
			return result.ToString();
		}

		private static string GetNumberString(string mant, int exp){
			if (exp == 0){
				return mant;
			}
			return exp > 0 ? Enlarge(mant, exp) : Decrease(mant, -exp);
		}

		private static string Decrease(string mant, int i){
			string preDot;
			string postDot;
			SplitDot(mant, out preDot, out postDot);
			if (i < preDot.Length){
				return preDot.Substring(0, preDot.Length - i) + "." + preDot.Substring(preDot.Length - i) + postDot;
			}
			return "0." + StringUtils.Repeat('0', i - preDot.Length) + preDot + postDot;
		}

		private static void SplitDot(string mant, out string preDot, out string postDot){
			int index = mant.IndexOf('.');
			if (index < 0){
				preDot = mant;
				postDot = "";
				return;
			}
			preDot = mant.Substring(0, index);
			postDot = mant.Substring(index + 1);
		}

		private static string Enlarge(string mant, int i){
			string preDot;
			string postDot;
			SplitDot(mant, out preDot, out postDot);
			if (i >= postDot.Length){
				return preDot + postDot + StringUtils.Repeat('0', i - postDot.Length);
			}
			return preDot + postDot.Substring(0, i) + "." + postDot.Substring(i);
		}

		private static void GetEPositions(string text, IEnumerable<int> epos, out int[] starts, out int[] ends,
			out string[] mants, out int[] exps){
			List<int> starts1 = new List<int>();
			List<int> ends1 = new List<int>();
			List<string> mants1 = new List<string>();
			List<int> exps1 = new List<int>();
			foreach (int ei in epos){
				GetEPosition(text, ei, starts1, ends1, mants1, exps1);
			}
			starts = starts1.ToArray();
			ends = ends1.ToArray();
			mants = mants1.ToArray();
			exps = exps1.ToArray();
		}

		private static void GetEPosition(string text, int ei, ICollection<int> starts1, ICollection<int> ends1,
			ICollection<string> mants1, ICollection<int> exps1){
			int start = ei;
			while (start > 0 && numberChars.Contains(text[start - 1])){
				start--;
			}
			int end = ei + 1;
			bool hasSign = text[ei + 1] == '+' || text[ei + 1] == '-';
			bool negative = false;
			if (hasSign){
				end++;
				if (text[ei + 1] == '-'){
					negative = true;
				}
			}
			while (end < text.Length && numberChars.Contains(text[end]) && text[end] != '.'){
				end++;
			}
			string mant = text.Substring(start, ei - start);
			if (!IsNumber(mant)){
				return;
			}
			int expStart = hasSign ? ei + 2 : ei + 1;
			if (end - expStart <= 0){
				return;
			}
			int exponent = int.Parse(text.Substring(expStart, end - expStart));
			if (negative){
				exponent = -exponent;
			}
			starts1.Add(start);
			ends1.Add(end);
			mants1.Add(mant);
			exps1.Add(exponent);
		}

		private static int[] GetEPositions(string text){
			List<int> result = new List<int>();
			for (int i = 1; i < text.Length - 1; i++){
				if (text[i] == 'e'){
					char m1 = text[i - 1];
					if (m1 >= '0' && m1 <= '9'){
						char p1 = text[i + 1];
						if ((p1 >= '0' && p1 <= '9') || p1 == '+' || p1 == '-'){
							result.Add(i);
						}
					}
				}
			}
			return result.ToArray();
		}

		private static TreeNode ParseImpl(string text, IList<string> realVariableNames, IList<string> intVariableNames,
			ref string errMsg){
			string err = null;
			text = RemoveFullEnclosingBrackets(text);
			Tuple<int, int>[] pos = GetTopLevelBrackets(text, ref err);
			if (err != null){
				errMsg = err;
				return null;
			}
			string[] funcPrefix = GetFuncPrefixes(text, pos);
			int[] opPos = GetTopLevelOperators(text, pos, funcPrefix);
			switch (opPos.Length){
				case 0:
					if (pos.Length > 1){
						errMsg = "Something is wrong";
						return null;
					}
					if (pos.Length == 1){
						if (funcPrefix[0] != null){
							return ParseFunctionCall(text, funcPrefix[0], realVariableNames, intVariableNames, ref errMsg);
						}
						Tuple<int, int> x = pos[0];
						if (x.Item1 >= 0 || x.Item2 != text.Length - 1){
							errMsg = "Wrong placement of brackets: " + text;
							return null;
						}
						return ParseBrackets(text, realVariableNames, intVariableNames, ref errMsg);
					}
					int ind = ArrayUtils.IndexOf(realVariableNames, text);
					if (ind >= 0){
						return ParseRealVariable(ind);
					}
					ind = ArrayUtils.IndexOf(intVariableNames, text);
					if (ind >= 0){
						return ParseIntVariable(ind);
					}
					if (GenericFunc.constMap.ContainsKey(text)){
						return ParseMathConstant(GenericFunc.constMap[text]);
					}
					if (IsNumber(text)){
						return ParseNumber(text);
					}
					errMsg = "Cannot parse " + text;
					return null;
				case 1:
					return ParseOperator(text, opPos[0], realVariableNames, intVariableNames, ref errMsg);
				default:
					int[] opPos1;
					int[] opPos2;
					int[] opPos3;
					SplitOperators(text, opPos, out opPos1, out opPos2, out opPos3);
					if (opPos1.Length > 0){
						return ParseOperator(text, opPos1[opPos1.Length - 1], realVariableNames, intVariableNames, ref errMsg);
					}
					return ParseOperator(text, opPos2.Length > 0 ? opPos2[opPos2.Length - 1] : opPos3[opPos3.Length - 1],
						realVariableNames, intVariableNames, ref errMsg);
			}
		}

		private static string RemoveFullEnclosingBrackets(string text){
			if (text.Length < 2){
				return text;
			}
			if (!opening.Contains(text[0])){
				return text;
			}
			char close = brackets[text[0]];
			int ind = text.IndexOf(close);
			if (ind == text.Length - 1){
				return text.Substring(1, text.Length - 2);
			}
			return text;
		}

		private static TreeNode ParseFunctionCall(string text, string funcName, IList<string> realVariableNames,
			IList<string> intVariableNames, ref string errMsg){
			if (!text.StartsWith(funcName)){
				errMsg = "Cannot parse as function: " + text;
				return null;
			}
			string argStr = text.Substring(funcName.Length + 1, text.Length - funcName.Length - 2);
			if (argStr.Length == 0){
				errMsg = "Empty argument for " + funcName;
				return null;
			}
			string[] args = argStr.Split(',');
			int numArgs = args.Length;
			List<GenericFunc> funcs = GenericFunc.funcMap[funcName];
			GenericFunc func = GetSuitableFunc(funcs, numArgs);
			if (func == null){
				errMsg = "Could not find suitable function: " + funcName;
			}
			if (func is Func1){
				Func1 func1 = (Func1) func;
				TreeNode result = new TreeNode{Func = func1, arguments = new TreeNode[1]};
				result.arguments[0] = ParseImpl(args[0], realVariableNames, intVariableNames, ref errMsg);
				return errMsg == null ? result : null;
			}
			if (func is Func2){
				Func2 func2 = (Func2) func;
				TreeNode result = new TreeNode{Func = func2, arguments = new TreeNode[2]};
				result.arguments[0] = ParseImpl(args[0], realVariableNames, intVariableNames, ref errMsg);
				result.arguments[1] = ParseImpl(args[1], realVariableNames, intVariableNames, ref errMsg);
				return errMsg == null ? result : null;
			}
			if (func is FuncN){
				FuncN funcN = (FuncN) func;
				TreeNode result = new TreeNode{Func = funcN, arguments = new TreeNode[args.Length]};
				for (int i = 0; i < args.Length; i++){
					result.arguments[i] = ParseImpl(args[i], realVariableNames, intVariableNames, ref errMsg);
					if (errMsg != null){
						return null;
					}
				}
				return result;
			}
			throw new Exception("Never get here.");
		}

		private static GenericFunc GetSuitableFunc(List<GenericFunc> funcs, int numArgs){
			if (numArgs == 1){
				foreach (Func1 func in funcs.OfType<Func1>()){
					return func;
				}
			}
			if (numArgs == 2){
				foreach (Func2 func in funcs.OfType<Func2>()){
					return func;
				}
				foreach (FuncN func in funcs.OfType<FuncN>()){
					return func;
				}
			}
			foreach (FuncN func in funcs.OfType<FuncN>()){
				return func;
			}
			return null;
		}

		private static TreeNode ParseBrackets(string text, IList<string> realVariableNames, IList<string> intVariableNames,
			ref string errMsg){
			return ParseImpl(text.Substring(1, text.Length - 2), realVariableNames, intVariableNames, ref errMsg);
		}

		private static TreeNode ParseRealVariable(int id){
			return new TreeNode{Func = new Variable{id = id, type = ReturnType.Real}, arguments = new TreeNode[0]};
		}

		private static TreeNode ParseIntVariable(int id){
			return new TreeNode{Func = new Variable{id = id, type = ReturnType.Integer}, arguments = new TreeNode[0]};
		}

		private static TreeNode ParseMathConstant(GenericFunc constant){
			return new TreeNode{Func = constant, arguments = new TreeNode[0]};
		}

		private static TreeNode ParseNumber(string text){
			return text.Contains(".")
				? new TreeNode{Func = new ConstRational(text), arguments = new TreeNode[0]}
				: new TreeNode{Func = new ConstInteger(text), arguments = new TreeNode[0]};
		}

		private static bool IsNumber(string text){
			if (text.Length == 0){
				return false;
			}
			int numDots = 0;
			foreach (char c in text){
				if (!numberChars.Contains(c)){
					return false;
				}
				if (c == '.'){
					numDots++;
				}
			}
			return numDots < 2;
		}

		private static void SplitOperators(string text, IEnumerable<int> opPos, out int[] opPos1, out int[] opPos2,
			out int[] opPos3){
			List<int> o1 = new List<int>();
			List<int> o2 = new List<int>();
			List<int> o3 = new List<int>();
			foreach (int opPo in opPos){
				char x = text[opPo];
				switch (x){
					case '-':
					case '+':
						o1.Add(opPo);
						break;
					case '^':
						o3.Add(opPo);
						break;
					default:
						o2.Add(opPo);
						break;
				}
			}
			opPos1 = o1.ToArray();
			opPos2 = o2.ToArray();
			opPos3 = o3.ToArray();
		}

		private static TreeNode ParseOperator(string text, int opPos, IList<string> realVariableNames,
			IList<string> intVariableNames, ref string errMsg){
			char op = text[opPos];
			string arg1 = text.Substring(0, opPos);
			string arg2 = text.Substring(opPos + 1);
			TreeNode result = new TreeNode();
			if (op == '-' && arg1.Length == 0){
				result.Func = new Func1Minus();
				result.arguments = new TreeNode[2];
				result.arguments[0] = ParseImpl(arg2, realVariableNames, intVariableNames, ref errMsg);
				return errMsg == null ? result : null;
			}
			switch (op){
				case '+':
					result.Func = new Func2Sum();
					break;
				case '-':
					result.Func = new Func2Diff();
					break;
				case '*':
					result.Func = new Func2Product();
					break;
				case '/':
					result.Func = new Func2Ratio();
					break;
				case '^':
					result.Func = new Func2Pow();
					break;
				default:
					throw new Exception("Never get here.");
			}
			result.arguments = new TreeNode[2];
			result.arguments[0] = ParseImpl(arg1, realVariableNames, intVariableNames, ref errMsg);
			result.arguments[1] = ParseImpl(arg2, realVariableNames, intVariableNames, ref errMsg);
			return errMsg == null ? result : null;
		}

		private static int[] GetTopLevelOperators(string text, IList<Tuple<int, int>> pos, IList<string> funcPrefix){
			int[] starts = new int[pos.Count];
			for (int i = 0; i < starts.Length; i++){
				if (funcPrefix[i] == null){
					starts[i] = pos[i].Item1;
				} else{
					starts[i] = pos[i].Item1 - funcPrefix[i].Length;
				}
			}
			bool[] indicators = new bool[text.Length];
			for (int i = 0; i < pos.Count; i++){
				for (int j = starts[i]; j <= pos[i].Item2; j++){
					indicators[j] = true;
				}
			}
			List<int> result = new List<int>();
			for (int i = 0; i < text.Length; i++){
				if (!indicators[i] && operators.Contains(text[i])){
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		private static string[] GetFuncPrefixes(string text, IList<Tuple<int, int>> pos){
			string[] funcPrefix = new string[pos.Count];
			for (int i = 0; i < funcPrefix.Length; i++){
				int index = pos[i].Item1;
				if (text[index] == '('){
					funcPrefix[i] = GetFuncPrefix(text, index);
				}
			}
			return funcPrefix;
		}

		private static string GetFuncPrefix(string text, int index){
			if (index == 0){
				return null;
			}
			string s = text.Substring(0, index);
			List<string> candidates = new List<string>();
			foreach (string name in GenericFunc.funcMap.Keys){
				if (s.EndsWith(name)){
					candidates.Add(name);
				}
			}
			if (candidates.Count == 0){
				return null;
			}
			if (candidates.Count == 1){
				return candidates[0];
			}
			int[] len = new int[candidates.Count];
			for (int i = 0; i < len.Length; i++){
				len[i] = candidates[i].Length;
			}
			int ind = ArrayUtils.MaxInd(len);
			return candidates[ind];
		}

		private static Tuple<int, int>[] GetTopLevelBrackets(string text, ref string errMsg){
			List<Tuple<int, int>> result = new List<Tuple<int, int>>();
			int level = 0;
			int startPos = 0;
			Stack<char> stack = new Stack<char>();
			for (int i = 0; i < text.Length; i++){
				char c = text[i];
				if (brackets.ContainsKey(c)){
					stack.Push(c);
					if (level == 0){
						startPos = i;
					}
					level++;
				} else if (closing.Contains(c)){
					level--;
					if (level < 0){
						errMsg = "Something is wrong with the brackets in " + text;
						return null;
					}
					char open = stack.Pop();
					if (brackets[open] != c){
						errMsg = "Something is wrong with the brackets in " + text;
						return null;
					}
					if (level == 0){
						result.Add(new Tuple<int, int>(startPos, i));
					}
				}
			}
			return result.ToArray();
		}
	}
}