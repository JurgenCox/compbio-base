using System;
using BaseLibS.Util;

namespace BaseLibS.Param {
	[Serializable]
	public class IntParam : Parameter<int> {
		/// <summary>
		/// only for xml serialization
		/// </summary>
		private IntParam() : this("", 0) { }

		public IntParam(string name, int value) : base(name) {
			Value = value;
			Default = value;
		}

		public override string StringValue {
			get => Parser.ToString(Value);
			set => Value = Parser.Int(value);
		}

		public override void Clear() {
			Value = 0;
		}

		public override ParamType Type => ParamType.Server;
	}
}