using System;
using BaseLibS.Mol;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class SequenceRegressionModel{
		public virtual double[] Predict(string sequence, PeptideModificationState modifications, BaseVector metadata){
			return Predict(new[]{sequence}, new[]{modifications}, metadata == null ? null : new[]{metadata})[0];
		}

		public virtual double[][] Predict(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata){
			double[][] result = new double[sequences.Length][];
			for (int i = 0; i < result.Length; i++){
				result[i] = Predict(sequences[i], modifications[i], metadata?[i]);
			}
			return result;
		}

		public abstract void Write(string filePath);

		public abstract void Read(string filePath);
	}
}