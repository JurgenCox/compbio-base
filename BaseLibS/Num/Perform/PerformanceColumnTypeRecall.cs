namespace BaseLibS.Num.Perform{
	public class PerformanceColumnTypeRecall : PerformanceColumnType{
		public override string Name => "TP/(TP+FN) (Recall)";

		public override double Calculate(double tp, double tn, double fp, double fn, double np, double nn){
			return tp/(tp + fn);
		}
	}
}