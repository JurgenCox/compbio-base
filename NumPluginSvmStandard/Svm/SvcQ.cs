namespace NumPluginSvm.Svm{
	internal class SvcQ : SvmKernel{
		private readonly short[] y;
		private readonly SvmCache cache;
		private readonly double[] qd;

		internal SvcQ(SvmProblem prob, SvmParameter param, short[] y1) : base(prob.Count, prob.x, param){
			y = (short[]) y1.Clone();
			cache = new SvmCache(prob.Count, (long) (param.cacheSize*(1 << 20)));
			qd = new double[prob.Count];
			for (int i = 0; i < prob.Count; i++){
				qd[i] = KernelFunctionEval(i, i);
			}
		}

		internal override float[] GetQ(int i, int len){
			float[][] data = new float[1][];
			int start;
			if ((start = cache.GetData(i, data, len)) < len){
				int j;
				for (j = start; j < len; j++){
					data[0][j] = (float) (y[i]*y[j]*KernelFunctionEval(i, j));
				}
			}
			return data[0];
		}

		internal override double[] GetQd(){
			return qd;
		}

		internal override void SwapIndex(int i, int j){
			cache.SwapIndex(i, j);
			base.SwapIndex(i, j);
			do{
				short _ = y[i];
				y[i] = y[j];
				y[j] = _;
			} while (false);
			do{
				double _ = qd[i];
				qd[i] = qd[j];
				qd[j] = _;
			} while (false);
		}
	}
}