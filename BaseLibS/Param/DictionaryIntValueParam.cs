using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class DictionaryIntValueParam : Parameter<Dictionary<string, int>>{
		protected string[] keys;
		public int DefaultValue { get; set; }

		public virtual string[] Keys{
			get => keys;
			set => keys = value;
		}

        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private DictionaryIntValueParam() : this("", new Dictionary<string, int>(), new string[0] ) { }

	    public DictionaryIntValueParam(string name, Dictionary<string, int> value, string[] keys) : base(name){
			Value = value;
			Default = new Dictionary<string, int>();
			foreach (KeyValuePair<string, int> pair in value){
				Default.Add(pair.Key, pair.Value);
			}
			this.keys = keys;
		}

		public override string StringValue{
			get => StringUtils.ToString(Value);
			set => Value = DictionaryFromString(value);
		}

		public static Dictionary<string, int> DictionaryFromString(string s){
			Dictionary<string, int> result = new Dictionary<string, int>();
			foreach (string s1 in s.Split('\r')){
				string[] w = s1.Trim().Split('\t');
				result.Add(w[0], Parser.Int(w[1]));
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
			Value = new Dictionary<string, int>();
		}
		public override ParamType Type => ParamType.Server;

	    public override void WriteXml(XmlWriter writer)
	    {
            WriteBasicAttributes(writer);
            SerializableDictionary<string, int> value = new SerializableDictionary<string, int>(Value);
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            writer.WriteStartElement("Value");
            serializer.Serialize(writer, value);
            writer.WriteEndElement();
            writer.WriteStartElement("Keys");
	        foreach (string key in Keys)
	        {
	            writer.WriteElementString("Key", key);
	        }
            writer.WriteEndElement();
	    }

	    public override void ReadXml(XmlReader reader)
	    {
	        ReadBasicAttributes(reader);
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, int>));
            reader.ReadStartElement();
            reader.ReadStartElement("Value");
	        Value = ((SerializableDictionary<string, int>) serializer.Deserialize(reader)).ToDictionary();
            reader.ReadEndElement();
	        Keys = reader.ReadInto(new List<string>()).ToArray();
            reader.ReadEndElement();
	    }
	}
}