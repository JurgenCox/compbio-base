using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BaseLibS.Mol;
using BaseLibS.Num;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class SequenceClassificationModel{
		public abstract double[] PredictStrength(string sequence, PeptideModificationState modifications,
			BaseVector metadata);

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

		public virtual void Write(string filePath){
			WriteBySerialization(filePath, this);
		}

		public virtual SequenceClassificationModel Read(string filePath){
			return ReadByDeserialization(filePath);
		}

		public static void WriteBySerialization(string filePath, SequenceClassificationModel model){
			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bFormatter = new BinaryFormatter();
			bFormatter.Serialize(stream, model);
			stream.Close();
		}

		public static SequenceClassificationModel ReadByDeserialization(string filePath){
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bFormatter = new BinaryFormatter();
			SequenceClassificationModel m = (SequenceClassificationModel) bFormatter.Deserialize(stream);
			stream.Close();
			return m;
		}
	}
}