namespace BaseLibS.Num.Perform{
	public class PerformanceColumnTypePrecision : PerformanceColumnType{
		public override string Name => "TP/(TP+FP) (Precision)";

		public override double Calculate(double tp, double tn, double fp, double fn, double np, double nn){
			if (tp + fp == 0){
				return 1;
			}
			return tp / (tp + fp);
		}
	}
}