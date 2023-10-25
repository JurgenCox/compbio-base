using System;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class DoubleParam : Parameter<double>{
		/// <summary>
		/// only for xml serialization
		/// </summary>
		private DoubleParam() : this("", 0.0){ }

		public DoubleParam(string name, double value) : base(name){
			Value = value;
			Default = value;
		}

		protected DoubleParam(string name, string help, string url, bool visible, double value, double default1) : base(
			name, help, url, visible, value, default1){ }
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = reader.ReadDouble();
			Default = reader.ReadDouble();
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			writer.Write(Value);
			writer.Write(Default);
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = Parser.Double(value);
		}

		public override void Clear(){
			Value = 0;
		}

		public override ParamType Type => ParamType.Server;

		public override object Clone(){
			return new DoubleParam(Name, Help, Url, Visible, Value, Default);
		}
	}
}