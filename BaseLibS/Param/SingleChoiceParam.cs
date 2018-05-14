using System;
using System.Collections.Generic;
using System.Xml;

namespace BaseLibS.Param{
	[Serializable]
	public class SingleChoiceParam : Parameter<int>{
		public IList<string> Values { get; set; }

        /// <summary>
        /// only for xml serialization
        /// </summary>
	    private SingleChoiceParam() : this("") { }

	    public SingleChoiceParam(string name) : this(name, 0){}

		public SingleChoiceParam(string name, int value) : base(name){
			Value = value;
			Default = value;
			Values = new[]{""};
		}

		public override string StringValue{
			get{
				if (Value < 0 || Value >= Values.Count){
					return null;
				}
				return Values[Value];
			}
			set{
				for (int i = 0; i < Values.Count; i++){
					if (Values[i].Equals(value)){
						Value = i;
						break;
					}
				}
			}
		}

		public override void Clear(){
			Value = 0;
		}
		public override ParamType Type => ParamType.Server;

	    public override void ReadXml(XmlReader reader)
	    {
            ReadBasicAttributes(reader);
            reader.ReadStartElement();
	        Value = reader.ReadElementContentAsInt();
	        Values = reader.ReadInto(new List<string>()).ToArray();
            reader.ReadEndElement();
	    }

	    public override void WriteXml(XmlWriter writer)
	    {
            WriteBasicAttributes(writer);
            writer.WriteStartElement("Value");
            writer.WriteValue(Value);
            writer.WriteEndElement();
            writer.WriteValues("Values", Values);
	    }
	}
}