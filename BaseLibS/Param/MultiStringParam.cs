using System;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class MultiStringParam : Parameter<string[]>{

        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private MultiStringParam() : this("") { }

	    public MultiStringParam(string name) : this(name, new string[0]){}

		public MultiStringParam(string name, string[] value) : base(name){
			Value = value;
			Default = new string[Value.Length];
			for (int i = 0; i < Value.Length; i++){
				Default[i] = Value[i];
			}
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
	}
}