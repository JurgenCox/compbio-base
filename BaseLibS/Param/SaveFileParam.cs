using System;
using System.IO;

namespace BaseLibS.Param{
	[Serializable]
	public class SaveFileParam : Parameter<string>{
		public string FileName{ get; set; }
		public string Filter{ get; set; }
		public Action<string> WriteAction{ get; set; }
		public SaveFileParam() : this(""){ }

		public SaveFileParam(string name) : this(name, "", "", "", s => { }){ }

		public SaveFileParam(string name, string value, string fileName, string filter, Action<string> writeAction) :
			base(name){
			Value = value;
			Default = value;
			FileName = fileName;
			Filter = filter;
			WriteAction = writeAction;
		}

		protected SaveFileParam(string name, string help, string url, bool visible, string value, string default1,
			string fileName, string filter, Action<string> writeAction) : base(name, help, url, visible, value,
			default1){
			FileName = fileName;
			Filter = filter;
			WriteAction = writeAction;
		}
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = reader.ReadString();
			Default = reader.ReadString();
			FileName = reader.ReadString();
			Filter = reader.ReadString();
			WriteAction = s => { };
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			writer.Write(Value);
			writer.Write(Default);
			writer.Write(FileName);
			writer.Write(Filter);
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
			return new SaveFileParam(Name, Help, Url, Visible, Value, Default, FileName, Filter, WriteAction);
		}
	}
}