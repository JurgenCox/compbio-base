using System;
using System.Numerics;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.Const{
	[Serializable]
	internal class ConstInteger : Constant{
		private readonly BigInteger number;

		internal ConstInteger(string text){
			number = BigInteger.Parse(text);
		}

		internal override string ShortName => null;
		internal override double NumEvaluateDouble => double.Parse(number.ToString());
		internal override ReturnType ReturnType => ReturnType.Integer;
		internal override string Name => "";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}