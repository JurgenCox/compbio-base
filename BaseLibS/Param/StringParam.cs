using System;
using System.IO;

namespace BaseLibS.Param{
	[Serializable]
	public class StringParam : Parameter<string>{
		/// <summary>
		/// only for xml serialization
		/// </summary>
		public StringParam() : this(""){ }

		public StringParam(string name) : this(name, ""){ }

		public StringParam(string name, string value) : base(name){
			Value = value;
			Default = value;
		}

		protected StringParam(string name, string help, string url, bool visible, string value, string default1) : base(
			name, help, url, visible, value, default1){ }
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = reader.ReadString();
			Default = reader.ReadString();
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			writer.Write(Value);
			writer.Write(Default);
		}

		public override string StringValue{
			get => Value;
			set => Value = value;
		}

		public override void Clear(){
			Value = "";
		}

		public override ParamType Type => ParamType.Server;

		public override object Clone(){
			return new StringParam(Name, Help, Url, Visible, Value, Default);
		}
	}
}