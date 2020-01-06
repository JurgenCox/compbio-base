using System;
using BaseLibS.Api;
using NumPluginBase.Kernel;

namespace NumPluginSvm.Svm{
    [Serializable]
    public class SvmParameter : ICloneable{
        public SvmType svmType = SvmType.CSvc;
        public IKernelFunction kernelFunction = new LinearKernelFunction();
        // these are for training only
        public double cacheSize = 100; // in MB
        public double eps = 0.001; // stopping criteria
        public double c = 100; // for C_SVC, EPSILON_SVR and NU_SVR
        public int nrWeight; // for C_SVC
        public int[] weightLabel; // for C_SVC
        public double[] weight; // for C_SVC
        public double nu = 0.5; // for NU_SVC, ONE_CLASS, and NU_SVR
        public double p = 0.1; // for EPSILON_SVR
        public bool shrinking = true; // use the shrinking heuristics
        public bool probability; // do probability estimates

        public object Clone(){
            return new SvmParameter{
                c = c,
                cacheSize = cacheSize,
                eps = eps,
                nrWeight = nrWeight,
                nu = nu,
                p = p,
                shrinking = shrinking,
                probability = probability,
                svmType = svmType,
                weight = weight,
                weightLabel = weightLabel,
                kernelFunction = (IKernelFunction) kernelFunction.Clone()
            };
        }
    }
}