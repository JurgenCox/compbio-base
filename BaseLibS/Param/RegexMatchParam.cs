using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace BaseLibS.Param{
	[Serializable]
	public class RegexMatchParam : Parameter<Regex>{
		/// <summary>
		/// for xml serialization only
		/// </summary>
		private RegexMatchParam() : this("", new Regex(".*"), new List<string>()){ }

		public List<string> Previews{ get; set; }

		public RegexMatchParam(string name, Regex value, List<string> previews) : base(name){
			Value = value;
			Default = value;
			Previews = previews;
		}

		protected RegexMatchParam(string name, string help, string url, bool visible, Regex value, Regex default1,
			List<string> previews) : base(name, help, url, visible, value, default1){
			Previews = previews;
		}
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = new Regex(reader.ReadString());
			Default = new Regex(reader.ReadString());
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			writer.Write(Value.ToString());
			writer.Write(Default.ToString());
		}

		public override void Clear(){
			Value = Default;
			Previews = new List<string>();
		}

		public override ParamType Type => ParamType.Server;

		public override string StringValue{
			get => Value.ToString();
			set =>
				throw new NotImplementedException(
					$"Setting string value for {typeof(RegexReplaceParam)} not implemented");
		}

		public override void ReadXml(XmlReader reader){
			ReadBasicAttributes(reader);
			reader.ReadStartElement();
			Value = new Regex(reader.ReadElementContentAsString());
			reader.ReadInto(Previews);
			reader.ReadEndElement();
		}

		public override void WriteXml(XmlWriter writer){
			WriteBasicAttributes(writer);
			writer.WriteStartElement("Value");
			writer.WriteValue(Value.ToString());
			writer.WriteEndElement();
			writer.WriteStartElement("Previews");
			foreach (string preview in Previews){
				writer.WriteElementString("Preview", preview);
			}
			writer.WriteEndElement();
		}

		public override object Clone(){
			return new RegexMatchParam(Name, Help, Url, Visible, Value, Default, Previews);
		}
	}
}