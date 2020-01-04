using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BaseLibS.Param{
	[Serializable]
	public class RegexReplaceParam : Parameter<Tuple<Regex, string>>
    {
        public List<string> Previews { get; set; }

        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private RegexReplaceParam() : this("", Tuple.Create(new Regex("(.*)"), "$1"), new List<string>()) { }

	    public RegexReplaceParam(string name, Regex regex, string replace, List<string> previews) : this(name, Tuple.Create(regex, replace), previews) { }

	    public RegexReplaceParam(string name, Tuple<Regex, string> value, List<string> previews) : base(name)
	    {
			Value = value;
		    Default = value;
            Previews = previews;
	    }

	    public override void Clear()
	    {
	        Value = Default;
            Previews = new List<string>();
	    }

        public override ParamType Type => ParamType.Server;

        public override string StringValue
        {
            get => Value.ToString();
	        set => throw new NotImplementedException($"Setting string value for {typeof(RegexReplaceParam)} not implemented");
        }

	    public override void ReadXml(XmlReader reader)
	    {
	        ReadBasicAttributes(reader);
            ReadValue(reader);
	        SerializationHelper.ReadInto(reader, Previews);
	        reader.ReadEndElement();
	    }

	    private void ReadValue(XmlReader reader)
	    {
	        reader.ReadStartElement();
	        reader.ReadStartElement("Value");
	        Regex regex = new Regex(reader.ReadElementContentAsString());
	        string replace = reader.ReadElementContentAsString();
	        Value = Tuple.Create(regex, replace);
	        reader.ReadEndElement();
	    }

	    public override void WriteXml(XmlWriter writer)
	    {
            WriteBasicAttributes(writer);
            // Value
            writer.WriteStartElement("Value");
            writer.WriteElementString("Regex", Value.Item1.ToString());
            writer.WriteElementString("Replace", Value.Item2);
            writer.WriteEndElement();
            // Previews
            writer.WriteStartElement("Previews");
	        foreach (string preview in Previews)
	        {
                writer.WriteElementString("Preview", preview);
	        }
            writer.WriteEndElement();
	    }
    }

    public static class RegexExtensions
    {
        public static SerializableRegex ToSerializableRegex(this Regex regex)
        {
            return new SerializableRegex(regex);
        }
    }

    public class SerializableRegex : IXmlSerializable
    {
        public Regex Regex { get; private set; }

        public SerializableRegex(Regex regex)
        {
            Regex = regex;
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            Regex = new Regex(reader.ReadElementContentAsString());
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue(Regex.ToString());
        }
    }
}