using BaseLibS.Util;

namespace BaseLibS.Calc.F0I1{
	internal class Func0I1IndicatorGreaterEqual : Func0I1Indicator{
		internal override string ShortName => "ge";
		internal override string Name => "greater or equal zero";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;

		internal override double NumEvaluateDouble(long n){
			return n >= 0 ? 1 : 0;
		}
	}
}