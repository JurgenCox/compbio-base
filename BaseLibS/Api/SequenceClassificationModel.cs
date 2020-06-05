using System;
using System.Collections.Generic;
using BaseLibS.Mol;
using BaseLibS.Num;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class SequenceClassificationModel{
		public virtual double[] PredictStrength(string sequence, PeptideModificationState modifications,
			BaseVector metadata){
			return PredictStrength(new[]{sequence}, new[]{modifications}, metadata == null ? null : new[]{metadata})[0];
		}

		public virtual double[][] PredictStrength(string[] sequence, PeptideModificationState[] modifications,
			BaseVector[] metadata){
			double[][] result = new double[sequence.Length][];
			for (int i = 0; i < result.Length; i++){
				result[i] = PredictStrength(sequence[i], modifications?[i], metadata?[i]);
			}
			return result;
		}

		public int PredictClass(string sequence, PeptideModificationState modifications, BaseVector metadata){
			return ArrayUtils.MaxInd(PredictStrength(sequence, modifications, metadata));
		}

		public int[] PredictClasses(string sequence, PeptideModificationState modifications, BaseVector metadata){
			double[] w = PredictStrength(sequence, modifications, metadata);
			List<int> result = new List<int>();
			for (int i = 0; i < w.Length; i++){
				if (w[i] > 0){
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public abstract void Write(string filePath);

		public abstract void Read(string filePath);
	}
}