using System;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class EmptyParam : Parameter<bool>{
        /// <summary>
        /// for xml serialization only
        /// </summary>
	    public EmptyParam() : this("") { }

	    public EmptyParam(string name) : this(name, false){}

		public EmptyParam(string name, bool value) : base(name){
			Value = value;
			Default = value;
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = bool.Parse(value);
		}

		public override void Clear(){
			Value = false;
		}
		public override ParamType Type => ParamType.Server;
	}
}