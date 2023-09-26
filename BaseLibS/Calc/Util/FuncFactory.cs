using System;
using BaseLibS.Calc.Const;
using BaseLibS.Calc.F0I1;
using BaseLibS.Calc.F0I2;
using BaseLibS.Calc.F1;
using BaseLibS.Calc.F2;
using BaseLibS.Calc.FN;
using BaseLibS.Calc.Func;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Calc.Util{
	internal static class FuncFactory{
		internal static GenericFunc Create(string s){
			(string token, string[] args) = Tokenize(s);
			switch (token){
				case "var":
					int id = Parser.Int(args[0]);
					ReturnType type = (ReturnType) Parser.Int(args[1]);
					return new Variable{id = id, type = type};
				case "e":
					return new ConstE();
				case "euler":
					return new ConstEuler();
				case "constInt":
					return new ConstInteger(args[0]);
				case "-inf":
					return new ConstNegativeInfinity();
				case "pi":
					return new ConstPi();
				case "+inf":
					return new ConstPositiveInfinity();
				case "constRational":
					return new ConstRational(args[0], args[1]);
				case "fact":
					return new Func0I1Factorial();
				case "even":
					return new Func0I1IndicatorEven();
				case "ge":
					return new Func0I1IndicatorGreaterEqual();
				case "kron":
					return new Func0I1IndicatorKronecker();
				case "le":
					return new Func0I1IndicatorLessEqual();
				case "odd":
					return new Func0I1IndicatorOdd();
				case "bico":
					return new Func0I2BinomCoeff();
				case "abs":
					return new Func1Abs();
				case "acos":
					return new Func1Acos();
				case "acosh":
					return new Func1Acosh();
				case "acot":
					return new Func1Acot();
				case "acoth":
					return new Func1Acoth();
				case "asin":
					return new Func1Asin();
				case "asinh":
					return new Func1Asinh();
				case "atan":
					return new Func1Atan();
				case "atanh":
					return new Func1Atanh();
				case "ceil":
					return new Func1Ceiling();
				case "conj":
					return new Func1Conjugate();
				case "cos":
					return new Func1Cos();
				case "cosh":
					return new Func1Cosh();
				case "cot":
					return new Func1Cot();
				case "coth":
					return new Func1Coth();
				case "csc":
					return new Func1Csc();
				case "dirac":
					return new Func1Dirac();
				case "dcomb":
					return new Func1DiracComb();
				case "erf":
					return new Func1Erf();
				case "exp":
					return new Func1Exp();
				case "floor":
					return new Func1Floor();
				case "gamma":
					return new Func1Gamma();
				case "gauss":
					return new Func1Gauss();
				case "heavyside":
					return new Func1Heavyside();
				case "inverse":
					return new Func1Inverse();
				case "log":
					return new Func1Log();
				case "log10":
					return new Func1Log10();
				case "log2":
					return new Func1Log2();
				case "minus":
					return new Func1Minus();
				case "round":
					return new Func1Round();
				case "sec":
					return new Func1Sec();
				case "sign":
					return new Func1Sign();
				case "sin":
					return new Func1Sin();
				case "sinh":
					return new Func1Sinh();
				case "sqrt":
					return new Func1Sqrt();
				case "tan":
					return new Func1Tan();
				case "tanh":
					return new Func1Tanh();
				case "atan2":
					return new Func2Atan();
				case "diff":
					return new Func2Diff();
				case "Log":
					return new Func2Log();
				case "max":
					return new Func2Max();
				case "min":
					return new Func2Min();
				case "pow":
					return new Func2Pow();
				case "times":
					return new Func2Product();
				case "ratio":
					return new Func2Ratio();
				case "sum":
					return new Func2Sum();
				case "Max":
					return new FuncNMax();
				case "Min":
					return new FuncNMin();
				case "Times":
					return new FuncNProduct();
				case "Sum":
					return new FuncNSum();
				default:
					throw new Exception("Unknown token: " + token);
			}
		}
		private static (string token, string[] args) Tokenize(string s){
			string[] t = s.Split(';');
			if (t.Length == 1){
				return (t[0], new string[0]);
			}
			return (t[0], t.SubArray(1, t.Length));
		}
	}
}