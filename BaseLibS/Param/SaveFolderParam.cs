using System;

namespace BaseLibS.Param{
	[Serializable]
	public class SaveFolderParam : Parameter<string>{
		public Action<string> WriteAction{ get; set; }
		private SaveFolderParam() : this(""){ }

		public SaveFolderParam(string name) : this(name, "", s => { }){ }

		public SaveFolderParam(string name, string value, Action<string> writeAction) : base(name){
			Value = value;
			Default = value;
			WriteAction = writeAction;
		}

		public override string StringValue{
			get => Value;
			set => Value = value;
		}

		public override void Clear(){
			Value = "";
		}

		public override ParamType Type => ParamType.Server;
	}
}