using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BaseLibS.Num.Vector;

namespace BaseLibS.Api{
	[Serializable]
	public abstract class RegressionModel{
		public abstract double Predict(BaseVector x);

		public void Write(string filePath){
			Stream stream = File.Open(filePath, FileMode.Create);
			BinaryFormatter bFormatter = new BinaryFormatter();
			bFormatter.Serialize(stream, this);
			stream.Close();
		}

		public static RegressionModel Read(string filePath){
			Stream stream = File.Open(filePath, FileMode.Open);
			BinaryFormatter bFormatter = new BinaryFormatter();
			RegressionModel m = (RegressionModel) bFormatter.Deserialize(stream);
			stream.Close();
			return m;
		}
	}
}