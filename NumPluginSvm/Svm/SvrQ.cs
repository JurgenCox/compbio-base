namespace NumPluginSvm.Svm{
    internal class SvrQ : SvmKernel{
        private readonly int l;
        private readonly SvmCache cache;
        private readonly short[] sign;
        private readonly int[] index;
        private int nextBuffer;
        private readonly float[][] buffer;
        private readonly double[] qd;

        internal SvrQ(SvmProblem prob, SvmParameter param) : base(prob.Count, prob.x, param){
            l = prob.Count;
            cache = new SvmCache(l, (long) (param.cacheSize*(1 << 20)));
            qd = new double[2*l];
            sign = new short[2*l];
            index = new int[2*l];
            for (int k = 0; k < l; k++){
                sign[k] = 1;
                sign[k + l] = -1;
                index[k] = k;
                index[k + l] = k;
                qd[k] = KernelFunctionEval(k, k);
                qd[k + l] = qd[k];
            }
            buffer = new float[2][];
            buffer[0] = new float[2*l];
            buffer[1] = new float[2*l];
            nextBuffer = 0;
        }

        internal override void SwapIndex(int i, int j){
            do{
                short _ = sign[i];
                sign[i] = sign[j];
                sign[j] = _;
            } while (false);
            do{
                int _ = index[i];
                index[i] = index[j];
                index[j] = _;
            } while (false);
            do{
                double _ = qd[i];
                qd[i] = qd[j];
                qd[j] = _;
            } while (false);
        }

        internal override float[] GetQ(int i, int len){
            float[][] data = new float[1][];
            int j, realI = index[i];
            if (cache.GetData(realI, data, l) < l){
                for (j = 0; j < l; j++){
                    data[0][j] = (float) KernelFunctionEval(realI, j);
                }
            }
            // reorder and copy
            float[] buf = buffer[nextBuffer];
            nextBuffer = 1 - nextBuffer;
            short si = sign[i];
            for (j = 0; j < len; j++){
                buf[j] = (float) si*sign[j]*data[0][index[j]];
            }
            return buf;
        }

        internal override double[] GetQd(){
            return qd;
        }
    }
}