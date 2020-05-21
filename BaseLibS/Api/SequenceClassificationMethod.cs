using System;
using BaseLibS.Mol;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api{
	public abstract class SequenceClassificationMethod : PredictionMethod{
		public abstract SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, string allAas, AllModifications allMods, Parameters param,
			int nthreads, Action<double> reportProgress);

		public SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, string allAas, AllModifications allMods, Parameters param,
			int nthreads){
			return Train(sequences, modifications, metadata, y, ngroups, allAas, allMods, param, nthreads, null);
		}

		public SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, string allAas, AllModifications allMods, Parameters param){
			return Train(sequences, modifications, metadata, y, ngroups, allAas, allMods, param, 1, null);
		}

		public SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, AllModifications allMods, Parameters param, int nthreads,
			Action<double> reportProgress){
			return Train(sequences, modifications, metadata, y, ngroups, aas, allMods, param, nthreads,
				reportProgress);
		}

		public SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, AllModifications allMods, Parameters param, int nthreads){
			return Train(sequences, modifications, metadata, y, ngroups, aas, allMods, param, nthreads, null);
		}

		public SequenceClassificationModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, int[][] y, int ngroups, AllModifications allMods, Parameters param){
			return Train(sequences, modifications, metadata, y, ngroups, aas, allMods, param, 1, null);
		}
	}
}