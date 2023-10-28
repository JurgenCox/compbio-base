using System;
using System.IO;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class MultiDoubleParam : Parameter<double[]>{
		/// <summary>
		/// for xml serialization only
		/// </summary>
		public MultiDoubleParam() : this(""){ }

		public MultiDoubleParam(string name) : this(name, new double[0]){ }

		public MultiDoubleParam(string name, double[] value) : base(name){
			Value = value;
			Default = ArrayUtils.FillArray(0.0, Value.Length);
			for (int i = 0; i < Value.Length; i++){
				Default[i] = Value[i];
			}
		}

		protected MultiDoubleParam(string name, string help, string url, bool visible, double[] value,
			double[] default1) : base(name, help, url, visible, value, default1){ }
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = FileUtils.ReadDoubleArray(reader);
			Default = FileUtils.ReadDoubleArray(reader);
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			FileUtils.Write(Value, writer);
			FileUtils.Write(Default, writer);
		}

		public override string StringValue{
			get => StringUtils.Concat(";", Value);
			set{
				if (value.Trim().Length == 0){
					Value = new double[0];
					return;
				}
				Value =  ParseDoubles(value);
			}
		}

		public static double[] ParseDoubles(string s){
			string[] x = s.Split(';');
			double[] y = new double[x.Length];
			for (int i = 0; i < y.Length; i++) {
				bool success = Parser.TryDouble(x[i], out double val);
				y[i] = success ? val : double.NaN;
			}
			return y;
		}

		public override bool IsModified => !ArrayUtils.EqualArrays(Default, Value);

		public override void Clear(){
			Value = new double[0];
		}

		public override float Height => 150f;
		public override ParamType Type => ParamType.Server;

		public override object Clone(){
			return new MultiDoubleParam(Name, Help, Url, Visible, Value, Default);
		}
	}
}