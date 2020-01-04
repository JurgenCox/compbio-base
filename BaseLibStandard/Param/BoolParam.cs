using System;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class BoolParam : Parameter<bool>{
        /// <summary>
        /// for xml serialization only
        /// </summary>
	    public BoolParam() : this("") { }

	    public BoolParam(string name) : this(name, false){}

		public BoolParam(string name, bool value) : base(name){
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