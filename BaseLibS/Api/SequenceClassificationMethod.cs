using BaseLibS.Mol;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using BaseLibS.Util;

namespace BaseLibS.Api{
	public abstract class SequenceClassificationMethod : PredictionMethod{
		public abstract SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, AllModifications allMods, Parameters param, int nthreads,
			Responder responder, string path);

		public SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, AllModifications allMods, Parameters param, int nthreads,
			Responder responder){
			return Train(sequences, modifications, metadata, y, ngroups, allMods, param, nthreads, responder, null);
		}
	}
}