using System;
using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.Const{
	[Serializable]
	internal class ConstE : Constant{
		internal override double NumEvaluateDouble => Math.E;
		internal override string ShortName => "e";
		internal override ReturnType ReturnType => ReturnType.Real;
		internal override string Name => "euler number";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;
	}
}