using System;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class EmptyParam : Parameter<bool>{
		/// <summary>
		/// for xml serialization only
		/// </summary>
		public EmptyParam() : this(""){ }

		public EmptyParam(string name) : this(name, false){ }

		public EmptyParam(string name, bool value) : base(name){
			Value = value;
			Default = value;
		}

		protected EmptyParam(string name, string help, string url, bool visible, bool value, bool default1) : base(name,
			help, url, visible, value, default1){ }
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = reader.ReadBoolean();
			Default = reader.ReadBoolean();
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			writer.Write(Value);
			writer.Write(Default);
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = bool.Parse(value);
		}

		public override void Clear(){
			Value = false;
		}

		public override object Clone(){
			return new EmptyParam(Name, Help, Url, Visible, Value, Default);
		}

		public override ParamType Type => ParamType.Server;
	}
}