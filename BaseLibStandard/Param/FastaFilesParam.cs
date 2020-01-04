using System;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param {
	[Serializable]
	public class FastaFilesParam : Parameter<string[][]> {
		/// <summary>
		/// for xml serialization only
		/// </summary>
		private FastaFilesParam() : this("") { }

		public FastaFilesParam(string name) : this(name, new string[0][]) { }

		public FastaFilesParam(string name, string[][] value) : base(name) {
			Value = value;
			Default = new string[Value.Length][];
			for (int i = 0; i < Value.Length; i++) {
				Default[i] = new string[Value[i].Length];
				for (int j = 0; j < Value[i].Length; j++) {
					Default[i][j] = Value[i][j];
				}
			}
		}

		public bool HasVariationData { get; set; }

		public bool HasModifications { get; set; }

		public override string StringValue {
			get => StringUtils.Concat(";", ",", Value);
			set {
				if (value.Trim().Length == 0) {
					Value = new string[0][];
					return;
				}
				string[] x = value.Split(';');
				Value = new string[x.Length][];
				for (int i = 0; i < x.Length; i++) {
					Value[i] = x[i].Split(',');
				}
			}
		}

		public override bool IsModified => !ArrayUtils.EqualArraysOfArrays(Default, Value);

		public override void Clear() {
			Value = new string[0][];
		}

		public override float Height => 200f;
		public override ParamType Type => ParamType.Server;
	}
}