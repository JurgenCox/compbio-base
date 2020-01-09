using BaseLibS.Calc.Func;
using BaseLibS.Calc.Util;
using BaseLibS.Util;

namespace BaseLibS.Calc.F0I1{
	internal class Func0I1Factorial : Func0I1{
		internal override string ShortName => "fact";
		internal override string Name => "Factorial";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;
		internal override Topic Topic => Topic.Unknown;

		internal override double NumEvaluateDouble(long n){
			throw new System.NotImplementedException();
		}

		internal override ReturnType ReturnType => ReturnType.Integer;
	}
}