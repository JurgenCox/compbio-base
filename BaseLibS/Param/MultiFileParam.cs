using System;
using System.Xml;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class MultiFileParam : Parameter<string[]>{
		public string Filter { get; set; }

        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private MultiFileParam() : this("") { } 
	    public MultiFileParam(string name) : this(name, new string[0]){}

		public MultiFileParam(string name, string[] value) : base(name){
			Value = value;
			Default = new string[Value.Length];
			for (int i = 0; i < Value.Length; i++){
				Default[i] = Value[i];
			}
			Filter = null;
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

		public override float Height => 120;
		public override ParamType Type => ParamType.Server;

	    public override void ReadXml(XmlReader reader)
	    {
	        Filter = reader.GetAttribute("Filter");
	        base.ReadXml(reader);
	    }

	    public override void WriteXml(XmlWriter writer)
	    {
	        writer.WriteAttributeString("Filter", Filter);
            base.WriteXml(writer);
	    }
	}
}