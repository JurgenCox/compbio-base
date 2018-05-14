using System;

namespace BaseLibS.Param{
	[Serializable]
	public class FolderParam : Parameter<string>{

        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private FolderParam() : this("") { }

	    public FolderParam(string name) : this(name, ""){}

		public FolderParam(string name, string value) : base(name){
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