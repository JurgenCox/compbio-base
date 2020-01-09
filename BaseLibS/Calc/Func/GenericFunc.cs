using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Calc.Const;
using BaseLibS.Calc.F1;
using BaseLibS.Calc.F2;
using BaseLibS.Calc.FN;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal abstract class GenericFunc{
		internal static readonly Constant minusOneConst = new ConstInteger("-1");
		internal static readonly Constant zeroConst = new ConstInteger("0");
		internal static readonly Constant oneConst = new ConstInteger("1");
		internal static readonly Constant twoConst = new ConstInteger("2");
		internal static readonly Constant tenConst = new ConstInteger("10");
		internal static readonly Constant eConst = new ConstE();
		internal static readonly Constant piConst = new ConstPi();
		internal static readonly Constant negativeInfinity = new ConstNegativeInfinity();
		internal static readonly Constant positiveInfinity = new ConstPositiveInfinity();
		internal static readonly TreeNode minusOne = new TreeNode{Func = minusOneConst, arguments = new TreeNode[0]};
		internal static readonly TreeNode zero = new TreeNode{Func = zeroConst, arguments = new TreeNode[0]};
		internal static readonly TreeNode one = new TreeNode{Func = oneConst, arguments = new TreeNode[0]};
		internal static readonly TreeNode two = new TreeNode{Func = twoConst, arguments = new TreeNode[0]};
		internal static readonly TreeNode ten = new TreeNode{Func = tenConst, arguments = new TreeNode[0]};
		internal static readonly TreeNode pi = new TreeNode{Func = piConst, arguments = new TreeNode[0]};
		internal static readonly TreeNode negInfinity = new TreeNode{Func = negativeInfinity, arguments = new TreeNode[0]};
		internal static readonly TreeNode posInfinity = new TreeNode{Func = positiveInfinity, arguments = new TreeNode[0]};
		internal static readonly TreeNode e = new TreeNode{Func = eConst, arguments = new TreeNode[0]};
		internal static readonly Func1 sin = new Func1Sin();
		internal static readonly Func1 cos = new Func1Cos();
		internal static readonly Func1 tan = new Func1Tan();
		internal static readonly Func1 cot = new Func1Cot();
		internal static readonly Func1 sec = new Func1Sec();
		internal static readonly Func1 csc = new Func1Csc();
		internal static readonly Func1 asin = new Func1Asin();
		internal static readonly Func1 acos = new Func1Acos();
		internal static readonly Func1 atan = new Func1Atan();
		internal static readonly Func1 acot = new Func1Acot();
		internal static readonly Func1 sinh = new Func1Sinh();
		internal static readonly Func1 cosh = new Func1Cosh();
		internal static readonly Func1 tanh = new Func1Tanh();
		internal static readonly Func1 coth = new Func1Coth();
		internal static readonly Func1 asinh = new Func1Asinh();
		internal static readonly Func1 acosh = new Func1Acosh();
		internal static readonly Func1 atanh = new Func1Atanh();
		internal static readonly Func1 acoth = new Func1Acoth();
		internal static readonly Func1 heavyside = new Func1Heavyside();
		internal static readonly Func1 log = new Func1Log();
		internal static readonly Func1 log10 = new Func1Log10();
		internal static readonly Func1 log2 = new Func1Log2();
		internal static readonly Func1 dirac = new Func1Dirac();
		internal static readonly Func1 diracComb = new Func1DiracComb();
		internal static readonly Func1 sqrt = new Func1Sqrt();
		internal static readonly Func1 exp = new Func1Exp();
		internal static readonly Func1 inverse = new Func1Inverse();
		internal static readonly Func1 minus = new Func1Minus();
		internal static readonly Func1 sign = new Func1Sign();
		internal static readonly Func2 sum = new Func2Sum();
		internal static readonly Func2 difference = new Func2Diff();
		internal static readonly Func2 product = new Func2Product();
		internal static readonly Func2 ratio = new Func2Ratio();
		internal static readonly Func2 pow = new Func2Pow();
		internal static readonly FuncN sumN = new FuncNSum();
		internal static readonly FuncN productN = new FuncNProduct();
		internal static readonly Constant[] allNamedConstants = new[]{eConst, piConst};
		internal static readonly Func1[] allFunc1 = new[]{
			sin, cos, tan, cot, sec, csc, asin, acos, atan, acot, sinh, cosh, tanh, coth, asinh, acosh, atanh, acoth,
			new Func1Abs(), new Func1Ceiling(), dirac, exp, new Func1Floor(), heavyside, inverse, log, log10, log2, minus,
			new Func1Round(), sign, sqrt, new Func1Gauss(), new Func1Erf()
		};
		internal static readonly Func2[] allFunc2 = new[]
		{difference, new Func2Log(), new Func2Atan(), new Func2Max(), new Func2Min(), pow, product, ratio, sum};
		internal static readonly FuncN[] allFuncN = new[]{new FuncNMax(), new FuncNMin(), productN, sumN};
		internal static readonly Dictionary<string, List<GenericFunc>> funcMap = new Dictionary<string, List<GenericFunc>>();
		internal static readonly Dictionary<string, Constant> constMap = new Dictionary<string, Constant>();

		internal static TreeNode Sin(TreeNode arg){
			return new TreeNode{Func = sin, arguments = new[]{arg}};
		}

		internal static TreeNode Cos(TreeNode arg){
			return new TreeNode{Func = cos, arguments = new[]{arg}};
		}

		internal static TreeNode Tan(TreeNode arg){
			return new TreeNode{Func = tan, arguments = new[]{arg}};
		}

		internal static TreeNode Cot(TreeNode arg){
			return new TreeNode{Func = cot, arguments = new[]{arg}};
		}

		internal static TreeNode Sec(TreeNode arg){
			return new TreeNode{Func = sec, arguments = new[]{arg}};
		}

		internal static TreeNode Csc(TreeNode arg){
			return new TreeNode{Func = csc, arguments = new[]{arg}};
		}

		internal static TreeNode Sinh(TreeNode arg){
			return new TreeNode{Func = sinh, arguments = new[]{arg}};
		}

		internal static TreeNode Cosh(TreeNode arg){
			return new TreeNode{Func = cosh, arguments = new[]{arg}};
		}

		internal static TreeNode Tanh(TreeNode arg){
			return new TreeNode{Func = tanh, arguments = new[]{arg}};
		}

		internal static TreeNode Coth(TreeNode arg){
			return new TreeNode{Func = coth, arguments = new[]{arg}};
		}

		internal static TreeNode Asin(TreeNode arg){
			return new TreeNode{Func = asin, arguments = new[]{arg}};
		}

		internal static TreeNode Acos(TreeNode arg){
			return new TreeNode{Func = acos, arguments = new[]{arg}};
		}

		internal static TreeNode Heavyside(TreeNode arg){
			return new TreeNode{Func = heavyside, arguments = new[]{arg}};
		}

		internal static TreeNode Atan(TreeNode arg){
			return new TreeNode{Func = atan, arguments = new[]{arg}};
		}

		internal static TreeNode Acot(TreeNode arg){
			return new TreeNode{Func = acot, arguments = new[]{arg}};
		}

		internal static TreeNode Asinh(TreeNode arg){
			return new TreeNode{Func = asinh, arguments = new[]{arg}};
		}

		internal static TreeNode Acosh(TreeNode arg){
			return new TreeNode{Func = acosh, arguments = new[]{arg}};
		}

		internal static TreeNode Atanh(TreeNode arg){
			return new TreeNode{Func = atanh, arguments = new[]{arg}};
		}

		internal static TreeNode Acoth(TreeNode arg){
			return new TreeNode{Func = acoth, arguments = new[]{arg}};
		}

		internal static TreeNode Square(TreeNode arg){
			return new TreeNode{Func = pow, arguments = new[]{arg, two}};
		}

		internal static TreeNode Exp(TreeNode arg){
			return new TreeNode{Func = exp, arguments = new[]{arg}};
		}

		internal static TreeNode Inverse(TreeNode arg){
			return new TreeNode{Func = inverse, arguments = new[]{arg}};
		}

		internal static TreeNode Minus(TreeNode arg){
			return new TreeNode{Func = minus, arguments = new[]{arg}};
		}

		internal static TreeNode Dirac(TreeNode arg){
			return new TreeNode{Func = dirac, arguments = new[]{arg}};
		}

		internal static TreeNode DiracComb(TreeNode arg){
			return new TreeNode{Func = diracComb, arguments = new[]{arg}};
		}

		internal static TreeNode Sqrt(TreeNode arg){
			return new TreeNode{Func = sqrt, arguments = new[]{arg}};
		}

		internal static TreeNode Log(TreeNode arg){
			return new TreeNode{Func = log, arguments = new[]{arg}};
		}

		internal static TreeNode Sign(TreeNode arg){
			return new TreeNode{Func = sign, arguments = new[]{arg}};
		}

		internal static TreeNode Sum(TreeNode arg1, TreeNode arg2){
			return new TreeNode{Func = sum, arguments = new[]{arg1, arg2}};
		}

		internal static TreeNode Sum(TreeNode[] args){
			return new TreeNode{Func = sumN, arguments = args};
		}

		internal static TreeNode Difference(TreeNode arg1, TreeNode arg2){
			return new TreeNode{Func = difference, arguments = new[]{arg1, arg2}};
		}

		internal static TreeNode Product(TreeNode arg1, TreeNode arg2){
			return new TreeNode{Func = product, arguments = new[]{arg1, arg2}};
		}

		internal static TreeNode Product(TreeNode[] args){
			return new TreeNode{Func = productN, arguments = args};
		}

		internal static TreeNode Ratio(TreeNode arg1, TreeNode arg2){
			return new TreeNode{Func = ratio, arguments = new[]{arg1, arg2}};
		}

		internal static TreeNode Pow(TreeNode arg1, TreeNode arg2){
			return new TreeNode{Func = pow, arguments = new[]{arg1, arg2}};
		}

		internal static TreeNode IntVar(int index){
			return new TreeNode{Func = new Variable{type = ReturnType.Integer, id = index}, arguments = new TreeNode[0]};
		}

		static GenericFunc(){
			AddRange(allFunc1);
			AddRange(allFunc2);
			AddRange(allFuncN);
			foreach (Constant constant in allNamedConstants){
				constMap.Add(constant.ShortName, constant);
			}
		}

		private static void AddRange(IEnumerable<GenericFunc> gfunc){
			foreach (GenericFunc func1 in gfunc.Where(func1 => !string.IsNullOrEmpty(func1.ShortName))){
				if (!funcMap.ContainsKey(func1.ShortName)){
					funcMap.Add(func1.ShortName, new List<GenericFunc>());
				}
				funcMap[func1.ShortName].Add(func1);
			}
		}

		internal abstract string ShortName { get; }
		internal abstract string Name { get; }
		internal abstract string Description { get; }
		internal abstract DocumentType DescriptionType { get; }
		internal abstract Topic Topic { get; }
		internal abstract TreeNode Derivative(int index, TreeNode[] args);
		internal abstract TreeNode NthDerivative(int realVar, int intVar, TreeNode[] args);
		internal abstract TreeNode IndefiniteIntegral(int index, TreeNode[] args);
	}
}