namespace BaseLibS.Num.Perform{
	public class PerformanceColumnTypeFpTp : PerformanceColumnType{
		public override string Name => "FP/TP";

		public override double Calculate(double tp, double tn, double fp, double fn, double np, double nn){
			return fp/tp;
		}
	}
}