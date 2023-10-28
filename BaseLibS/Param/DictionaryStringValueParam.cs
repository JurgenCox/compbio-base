using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class DictionaryStringValueParam : Parameter<Dictionary<string, string>>{
		/// <summary>
		/// for xml serialization only
		/// </summary>
		public DictionaryStringValueParam() : this("", new Dictionary<string, string>()){ }

		public DictionaryStringValueParam(string name, Dictionary<string, string> value) : base(name){
			Value = value;
			Default = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> pair in value){
				Default.Add(pair.Key, pair.Value);
			}
		}

		protected DictionaryStringValueParam(string name, string help, string url, bool visible,
			Dictionary<string, string> value, Dictionary<string, string> default1, string keyName, string valueName) :
			base(name, help, url, visible, value, default1){
			KeyName = keyName;
			ValueName = valueName;
		}
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = FileUtils.ReadDictionaryStringString(reader);
			Default = FileUtils.ReadDictionaryStringString(reader);
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			FileUtils.Write(Value, writer);
			FileUtils.Write(Default, writer);
		}

		public string KeyName{ get; set; } = "Keys";
		public string ValueName{ get; set; } = "Values";

		public override string StringValue{
			get => StringUtils.ToString(Value);
			set => Value = DictionaryFromString(value);
		}

		public static Dictionary<string, string> DictionaryFromString(string s){
			Dictionary<string, string> result = new Dictionary<string, string>();
			foreach (string s1 in s.Split('\r')){
				string[] w = s1.Trim().Split('\t');
				result.Add(w[0], w[1]);
			}
			return result;
		}

		public override bool IsModified{
			get{
				if (Value.Count != Default.Count){
					return true;
				}
				foreach (string k in Value.Keys){
					if (!Default.ContainsKey(k)){
						return true;
					}
					if (Default[k] != Value[k]){
						return true;
					}
				}
				return false;
			}
		}

		public override void Clear(){
			Value = new Dictionary<string, string>();
		}

		public override ParamType Type => ParamType.Server;
		public override float Height => 250f;

		public override void WriteXml(XmlWriter writer){
			WriteBasicAttributes(writer);
			SerializableDictionary<string, string> value = new SerializableDictionary<string, string>(Value);
			XmlSerializer serializer = new XmlSerializer(value.GetType());
			writer.WriteStartElement("Value");
			serializer.Serialize(writer, value);
			writer.WriteEndElement();
		}

		public override void ReadXml(XmlReader reader){
			ReadBasicAttributes(reader);
			XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, int>));
			reader.ReadStartElement("Value");
			Value = ((SerializableDictionary<string, string>) serializer.Deserialize(reader)).ToDictionary();
			reader.ReadEndElement();
		}

		public override object Clone(){
			return new DictionaryStringValueParam(Name, Help, Url, Visible, Value, Default, KeyName, ValueName);
		}
	}
}