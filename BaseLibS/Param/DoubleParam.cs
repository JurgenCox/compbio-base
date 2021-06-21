using System;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class DoubleParam : Parameter<double>{
        /// <summary>
        /// only for xml serialization
        /// </summary>
	    private DoubleParam() : this("", 0.0) { }

	    public DoubleParam(string name, double value) : base(name){
			Value = value;
			Default = value;
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = Parser.Double(value);
		}

		public override void Clear(){
			Value = 0;
		}
		public override ParamType Type => ParamType.Server;
	}
}