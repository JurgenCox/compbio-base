using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BaseLibS.Util{
	public static class XmlSerialization{
		/// <summary>
		/// Method to convert a custom Object to XML string
		/// </summary>
		/// <param name="item">Object that is to be serialized to XML</param>
		/// <param name="path"></param>
		public static void SerializeObject(object item, string path){
			if (File.Exists(path)){
				File.Delete(path);
			}
			FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			WriteToStream(stream, item);
			stream.Close();
		}

		public static void WriteToStream(Stream stream, object item){
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8)
			{Formatting = Formatting.Indented, Indentation = 3};
			XmlSerializer xs = new XmlSerializer(item.GetType());
			xs.Serialize(xmlTextWriter, item);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}

		/// <summary>
		/// Method to convert a custom Object to XML string
		/// </summary>
		/// <param name="item">Object that is to be serialized to XML</param>
		/// <param name="stream"></param>
		public static void Save(object item, Stream stream){
			WriteToStream(stream, item);
			stream.Close();
		}

		/// <summary>
		/// Method to reconstruct an Object from XML string
		/// </summary>
		/// <param name="path">Filepath of serialized Object</param>
		/// <param name="type">Type of Object in XML</param>     
		/// <returns>Object in xml. If error occured or validation not passed than null.</returns>
		public static object DeserializeObject(string path, Type type){
			FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			object result = DeserializeObject(stream, type);
			stream.Flush();
			stream.Close();
			return result;
		}

		public static object DeserializeObject(Stream stream, Type type){
			XmlTextReader reader = new XmlTextReader(stream);
			XmlSerializer xs = new XmlSerializer(type);
			return xs.Deserialize(reader);
		}
	}
}