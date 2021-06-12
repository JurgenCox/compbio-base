﻿using System;

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

		protected SaveFolderParam(string name, string help, string url, bool visible, string value, string default1,
			Action<string> writeAction) : base(name, help, url, visible, value, default1){
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

		public override object Clone(){
			return new SaveFolderParam(Name, Help, Url, Visible, Value, Default, WriteAction);
		}
	}
}