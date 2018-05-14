using BaseLibS.Util;

namespace BaseLibS.Calc.F0I1{
	internal class Func0I1IndicatorEven : Func0I1Indicator{
		internal override string ShortName => "even";
		internal override string Name => "even integer";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;

		internal override double NumEvaluateDouble(long n){
			return n%2 == 0 ? 1 : 0;
		}
	}
}