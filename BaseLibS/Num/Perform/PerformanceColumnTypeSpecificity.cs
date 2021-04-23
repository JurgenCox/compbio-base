namespace BaseLibS.Num.Perform{
	public class PerformanceColumnTypeSpecificity : PerformanceColumnType {
		public override string Name => "TN/(TN+FP) (Specificity)";

		public override double Calculate(double tp, double tn, double fp, double fn, double np, double nn){
			return tn/(tn + fp);
		}
	}
}