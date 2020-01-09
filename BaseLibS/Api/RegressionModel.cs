using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class RegressionModel{
		public abstract double Predict(BaseVector x);

		public virtual void Write(string filePath){
			WriteBySerialization(filePath, this);
		}

		public virtual RegressionModel Read(string filePath){
			return ReadByDeserialization(filePath);
		}

		public static void WriteBySerialization(string filePath, RegressionModel model){
			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bFormatter = new BinaryFormatter();
			bFormatter.Serialize(stream, model);
			stream.Close();
		}

		public static RegressionModel ReadByDeserialization(string filePath){
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bFormatter = new BinaryFormatter();
			RegressionModel m = (RegressionModel) bFormatter.Deserialize(stream);
			stream.Close();
			return m;
		}
	}
}