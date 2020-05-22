using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BaseLibS.Mol;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class SequenceRegressionModel{
		public virtual double Predict(string sequence, PeptideModificationState modifications, BaseVector metadata){
			return Predict(new[]{sequence}, new[]{modifications}, metadata == null ? null : new[]{metadata})[0];
		}

		public virtual double[] Predict(string[] sequences, PeptideModificationState[] modifications,
			BaseVector[] metadata){
			double[] result = new double[sequences.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = Predict(sequences[i], modifications[i], metadata?[i]);
			}
			return result;
		}

		public virtual void Write(string filePath){
			WriteBySerialization(filePath, this);
		}

		public virtual SequenceRegressionModel Read(string filePath){
			return ReadByDeserialization(filePath);
		}

		public static void WriteBySerialization(string filePath, SequenceRegressionModel model){
			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bFormatter = new BinaryFormatter();
			bFormatter.Serialize(stream, model);
			stream.Close();
		}

		public static SequenceRegressionModel ReadByDeserialization(string filePath){
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bFormatter = new BinaryFormatter();
			SequenceRegressionModel m = (SequenceRegressionModel) bFormatter.Deserialize(stream);
			stream.Close();
			return m;
		}
	}
}