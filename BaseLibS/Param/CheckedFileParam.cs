using System;

namespace BaseLibS.Param{
	[Serializable]
	public class CheckedFileParam : FileParam{
		public Func<string, Tuple<string, bool>> checkFileName;

		private CheckedFileParam() : this("", null) { }
		public CheckedFileParam(string name, Func<string, Tuple<string, bool>> checkFileName) : this(name, "",
			checkFileName){ }

		public CheckedFileParam(string name, string value, Func<string, Tuple<string, bool>> checkFileName) : base(name,
			value){
			this.checkFileName = checkFileName;
		}

		public override ParamType Type => ParamType.Server;
	}
}