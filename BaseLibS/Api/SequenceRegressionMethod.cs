using BaseLibS.Mol;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using BaseLibS.Util;

namespace BaseLibS.Api{
	public abstract class SequenceRegressionMethod : PredictionMethod{
		public abstract SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, AllModifications allMods, Parameters param, int nthreads,
			Responder responder, string path);

		public SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, AllModifications allMods, Parameters param, int nthreads,
			Responder responder){
			return Train(sequences, modifications, metadata, y, allMods, param, nthreads, responder, null);
		}
	}
}