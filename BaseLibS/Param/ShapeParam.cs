﻿using System;

namespace BaseLibS.Param{
	[Serializable]
	public class ShapeParam : Parameter<string>{
		/// <summary>
		/// only for xml serialization
		/// </summary>
		private ShapeParam() : this(""){ }

		public ShapeParam(string name) : this(name, ""){ }

		public ShapeParam(string name, string value) : base(name){
			Value = value;
			Default = value;
		}

		protected ShapeParam(string name, string help, string url, bool visible, string value, string default1) : base(
			name, help, url, visible, value, default1){ }

		public override string StringValue{
			get => Value;
			set => Value = value;
		}

		public override void Clear(){
			Value = "";
		}

		public override ParamType Type => ParamType.Server;

		public override object Clone(){
			return new ShapeParam(Name, Help, Url, Visible, Value, Default);
		}
	}
}