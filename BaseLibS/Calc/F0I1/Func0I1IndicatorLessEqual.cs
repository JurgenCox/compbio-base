using BaseLibS.Util;

namespace BaseLibS.Calc.F0I1{
	internal class Func0I1IndicatorLessEqual : Func0I1Indicator {
		internal override string ShortName => "le";
		internal override string Name => "less or equal zero";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;

		internal override double NumEvaluateDouble(long n){
			return n <= 0 ? 1 : 0;
		}
	}
}