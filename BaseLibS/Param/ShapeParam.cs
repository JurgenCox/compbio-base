using System;

namespace BaseLibS.Param {
	[Serializable]
	public class ShapeParam : Parameter<string> {
		/// <summary>
		/// only for xml serialization
		/// </summary>
		private ShapeParam() : this("") { }

		public ShapeParam(string name) : this(name, "") { }

		public ShapeParam(string name, string value) : base(name) {
			Value = value;
			Default = value;
		}

		public override string StringValue {
			get => Value;
			set => Value = value;
		}

		public override void Clear() {
			Value = "";
		}

		public override ParamType Type => ParamType.Server;
	}
}