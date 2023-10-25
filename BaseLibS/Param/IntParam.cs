using System;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class IntParam : Parameter<int>{
		/// <summary>
		/// only for xml serialization
		/// </summary>
		private IntParam() : this("", 0){ }

		public IntParam(string name, int value) : base(name){
			Value = value;
			Default = value;
		}

		protected IntParam(string name, string help, string url, bool visible, int value, int default1) : base(name,
			help, url, visible, value, default1){ }
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = reader.ReadInt32();
			Default = reader.ReadInt32();
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			writer.Write(Value);
			writer.Write(Default);
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = Parser.Int(value);
		}

		public override void Clear(){
			Value = 0;
		}

		public override ParamType Type => ParamType.Server;

		public override object Clone(){
			return new IntParam(Name, Help, Url, Visible, Value, Default);
		}
	}
}