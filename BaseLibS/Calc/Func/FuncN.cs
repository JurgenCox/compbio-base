using System;
using BaseLibS.Calc.Util;

namespace BaseLibS.Calc.Func{
	[Serializable]
	internal abstract class FuncN : GenericFunc{
		internal abstract double NumEvaluateDouble(double[] x);
		internal abstract ReturnType GetValueType(ReturnType[] types);
	}
}