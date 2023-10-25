using System;
using System.IO;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class MultiShapeParam : Parameter<string[]>{
		/// <summary>
		/// for xml serialization only
		/// </summary>
		private MultiShapeParam() : this(""){ }

		public MultiShapeParam(string name) : this(name, new string[0]){ }

		public MultiShapeParam(string name, string[] value) : base(name){
			Value = value;
			Default = new string[Value.Length];
			for (int i = 0; i < Value.Length; i++){
				Default[i] = Value[i];
			}
		}

		protected MultiShapeParam(string name, string help, string url, bool visible, string[] value, string[] default1)
			: base(name, help, url, visible, value, default1){ }
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = FileUtils.ReadStringArray(reader);
			Default = FileUtils.ReadStringArray(reader);
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			FileUtils.Write(Value, writer);
			FileUtils.Write(Default, writer);
		}

		public override string StringValue{
			get => StringUtils.Concat(",", Value);
			set{
				if (value.Trim().Length == 0){
					Value = new string[0];
					return;
				}
				Value = value.Split(',');
			}
		}

		public override bool IsModified => !ArrayUtils.EqualArrays(Default, Value);

		public override void Clear(){
			Value = new string[0];
		}

		public override float Height => 150f;
		public override ParamType Type => ParamType.Server;

		public override object Clone(){
			return new MultiShapeParam(Name, Help, Url, Visible, Value, Default);
		}
	}
}