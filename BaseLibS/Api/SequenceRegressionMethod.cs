using System;
using BaseLibS.Mol;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api{
	public abstract class SequenceRegressionMethod : PredictionMethod{
		public abstract SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, string allAas, AllModifications allMods, Parameters param, int nthreads,
			Action<double> reportProgress);

		public SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, string allAas, AllModifications allMods, Parameters param, int nthreads){
			return Train(sequences, modifications, metadata, y, allAas, allMods, param, nthreads, null);
		}

		public SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, string allAas, AllModifications allMods, Parameters param){
			return Train(sequences, modifications, metadata, y, allAas, allMods, param, 1, null);
		}

		public SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, AllModifications allMods, Parameters param, int nthreads,
			Action<double> reportProgress){
			return Train(sequences, modifications, metadata, y, aas, allMods, param, nthreads, reportProgress);
		}

		public SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, AllModifications allMods, Parameters param, int nthreads){
			return Train(sequences, modifications, metadata, y, aas, allMods, param, nthreads, null);
		}

		public SequenceRegressionModel Train(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata, double[] y, AllModifications allMods, Parameters param){
			return Train(sequences, modifications, metadata, y, aas, allMods, param, 1, null);
		}
	}
}