namespace BaseLibS.Num.Perform{
	public class PerformanceColumnTypeTpNp : PerformanceColumnType{
		public override string Name => "TP/NP";

		public override double Calculate(double tp, double tn, double fp, double fn, double np, double nn){
			return tp/np;
		}
	}
}