using System;

namespace BaseLibS.Param{
	[Serializable]
	public class StringParam : Parameter<string> {
        /// <summary>
        /// only for xml serialization
        /// </summary>
	    private StringParam() : this("") { }

	    public StringParam(string name) : this(name, ""){}

		public StringParam(string name, string value) : base(name){
			Value = value;
			Default = value;
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