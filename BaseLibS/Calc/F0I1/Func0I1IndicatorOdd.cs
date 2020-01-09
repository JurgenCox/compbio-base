using BaseLibS.Util;

namespace BaseLibS.Calc.F0I1{
	internal class Func0I1IndicatorOdd : Func0I1Indicator{
		internal override string ShortName => "odd";
		internal override string Name => "odd integer";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;

		internal override double NumEvaluateDouble(long n){
			return n % 2 == 0 ? 0 : 1;
		}
	}
}