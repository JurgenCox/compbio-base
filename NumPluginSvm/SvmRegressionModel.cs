using System;
using BaseLibS.Api;
using BaseLibS.Num.Vector;
using NumPluginSvm.Svm;

namespace NumPluginSvm{
    [Serializable]
    public class SvmRegressionModel : RegressionModel{
        private readonly SvmModel model;

        public SvmRegressionModel(SvmModel model){
            this.model = model;
        }

        public override double Predict(BaseVector x){
            return SvmMain.SvmPredict(model, x);
        }
    }
}