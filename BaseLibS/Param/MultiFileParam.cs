using System;
using System.IO;
using System.Xml;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class MultiFileParam : Parameter<string[]>{
		public string Filter{ get; set; }

		/// <summary>
		/// for xml serialization only
		/// </summary>
		private MultiFileParam() : this(""){ }

		public MultiFileParam(string name) : this(name, new string[0]){ }

		public MultiFileParam(string name, string[] value) : base(name){
			if (value == null){
				value = new string[0];
			}
			Value = value;
			Default = new string[Value.Length];
			for (int i = 0; i < Value.Length; i++){
				Default[i] = Value[i];
			}
			Filter = null;
		}

		protected MultiFileParam(string name, string help, string url, bool visible, string[] value, string[] default1,
			string filter) : base(name, help, url, visible, value, default1){
			Filter = filter;
		}
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = FileUtils.ReadStringArray(reader);
			Default = FileUtils.ReadStringArray(reader);
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			FileUtils.Write(Value, writer);
			FileUtils.Write(Default, writer);
		}

		public override string StringValue{
			get => StringUtils.Concat(",", Value);
			set{
				if (value.Trim().Length == 0){
					Value = new string[0];
					return;
				}
				Value = value.Split(',');
			}
		}

		public override bool IsModified => !ArrayUtils.EqualArrays(Default, Value);

		public override void Clear(){
			Value = new string[0];
		}

		public override float Height => 120;
		public override ParamType Type => ParamType.Server;

		public override void ReadXml(XmlReader reader){
			Filter = reader.GetAttribute("Filter");
			base.ReadXml(reader);
		}

		public override void WriteXml(XmlWriter writer){
			writer.WriteAttributeString("Filter", Filter);
			base.WriteXml(writer);
		}

		public override object Clone(){
			return new MultiFileParam(Name, Help, Url, Visible, Value, Default, Filter);
		}
	}
}