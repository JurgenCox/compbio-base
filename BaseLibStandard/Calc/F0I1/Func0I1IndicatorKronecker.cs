using BaseLibS.Util;

namespace BaseLibS.Calc.F0I1{
	internal class Func0I1IndicatorKronecker : Func0I1Indicator{
		internal override string ShortName => "kron";
		internal override string Name => "kronecker delta";
		internal override string Description => "";
		internal override DocumentType DescriptionType => DocumentType.PlainText;

		internal override double NumEvaluateDouble(long n){
			return n == 0 ? 1 : 0;
		}
	}
}