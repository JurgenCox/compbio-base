using System;
using System.Xml;
using System.Xml.Serialization;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class BoolWithSubParams : ParameterWithSubParams<bool>{
		public Parameters SubParamsFalse { get; set; }
		public Parameters SubParamsTrue { get; set; }
        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private BoolWithSubParams() : this("", false) { }

	    public BoolWithSubParams(string name) : this(name, false){}

		public BoolWithSubParams(string name, bool value) : base(name){
			TotalWidth = 1000F;
			ParamNameWidth = 250F;
			Value = value;
			Default = value;
			SubParamsFalse = new Parameters();
			SubParamsTrue = new Parameters();
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = bool.Parse(value);
		}

		public override void ResetSubParamValues(){
			SubParamsTrue.ResetValues();
			SubParamsFalse.ResetValues();
		}

		public override void ResetSubParamDefaults(){
			SubParamsTrue.ResetDefaults();
			SubParamsFalse.ResetDefaults();
		}

		public override bool IsModified{
			get{
				if (Value != Default){
					return true;
				}
				return SubParamsTrue.IsModified || SubParamsFalse.IsModified;
			}
		}

		public override Parameters GetSubParameters(){
			return Value ? SubParamsTrue : SubParamsFalse;
		}

		public override void Clear(){
			Value = false;
			SubParamsTrue.Clear();
			SubParamsFalse.Clear();
		}

		public override float Height => 50 + Math.Max(SubParamsFalse.Height, SubParamsTrue.Height);
		public override ParamType Type => ParamType.Server;

	    public override void ReadXml(XmlReader reader)
	    {
	        ReadBasicAttributes(reader);
            reader.ReadStartElement();
	        Value = reader.ReadElementContentAsBoolean("Value", "");
            XmlSerializer serializer = new XmlSerializer(SubParamsFalse.GetType());
            reader.ReadStartElement("SubParamsFalse");
	        SubParamsFalse = (Parameters) serializer.Deserialize(reader);
            reader.ReadEndElement();
            reader.ReadStartElement("SubParamsTrue");
	        SubParamsTrue = (Parameters) serializer.Deserialize(reader);
            reader.ReadEndElement();
            reader.ReadEndElement();
	    }

	    public override void WriteXml(XmlWriter writer)
	    {
	        WriteBasicAttributes(writer);
            writer.WriteStartElement("Value");
            writer.WriteValue(Value);
            writer.WriteEndElement();
            XmlSerializer serializer = new XmlSerializer(SubParamsTrue.GetType());
            writer.WriteStartElement("SubParamsFalse");
            serializer.Serialize(writer, SubParamsFalse);
            writer.WriteEndElement();
            writer.WriteStartElement("SubParamsTrue");
            serializer.Serialize(writer, SubParamsTrue);
            writer.WriteEndElement();
	    }
	}
}