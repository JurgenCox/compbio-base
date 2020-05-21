using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BaseLibS.Mol;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class SequenceRegressionModel{
		public abstract double Predict(string sequences, PeptideModificationState modifications,
			BaseVector metadata);

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