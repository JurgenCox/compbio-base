namespace NumPluginSvm.Svm{
    internal class OneClassQ : SvmKernel{
        private readonly SvmCache cache;
        private readonly double[] qd;

        internal OneClassQ(SvmProblem prob, SvmParameter param) : base(prob.Count, prob.x, param){
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
                    data[0][j] = (float) KernelFunctionEval(i, j);
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
                double _ = qd[i];
                qd[i] = qd[j];
                qd[j] = _;
            } while (false);
        }
    }
}