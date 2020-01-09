using System;
using System.Numerics;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.Const{
	[Serializable]
	internal class ConstRational : Constant{
		private BigInteger numerator;
		private BigInteger denominator;

		public ConstRational(string text){
			int dot = text.IndexOf('.');
			int pot = text.Length - dot - 1;
			text = text.Remove(dot, 1);
			numerator = BigInteger.Parse(text);
			denominator = BigInteger.Pow(new BigInteger(10), pot);
			BigInteger div = BigInteger.GreatestCommonDivisor(numerator, denominator);
			numerator /= div;
			denominator /= div;
		}

		internal override double NumEvaluateDouble{
			get{
				double n = double.Parse(numerator.ToString());
				double d = double.Parse(denominator.ToString());
				return n/d;
			}
		}
		internal override string ShortName => null;
		internal override ReturnType ReturnType => ReturnType.Real;
		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}