using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class SingleChoiceWithSubParams : ParameterWithSubParams<int>{
		public IList<string> Values { get; set; }
		public IList<Parameters> SubParams { get; set; }

        /// <summary>
        /// for xml serialization only
        /// </summary>
	    private SingleChoiceWithSubParams() : this("") { }

	    public SingleChoiceWithSubParams(string name) : this(name, 0){}

		public SingleChoiceWithSubParams(string name, int value) : base(name){
			TotalWidth = 1000F;
			ParamNameWidth = 250F;
			Value = value;
			Default = value;
			Values = new List<string>(new[] { "" });
			SubParams = new List<Parameters>(new[] { new Parameters() });
		}

		public override string StringValue{
			get => Parser.ToString(Value);
			set => Value = Parser.Int(value);
		}

		public override void ResetSubParamValues(){
			Value = Default;
			foreach (Parameters p in SubParams){
				p.ResetValues();
			}
		}

		public override void ResetSubParamDefaults(){
			Default = Value;
			foreach (Parameters p in SubParams){
				p.ResetDefaults();
			}
		}

		public override bool IsModified{
			get{
				if (Value != Default){
					return true;
				}
				foreach (Parameters p in SubParams){
					if (p.IsModified){
						return true;
					}
				}
				return false;
			}
		}

		public string SelectedValue => Value < 0 || Value >= Values.Count ? null : Values[Value];

		public override Parameters GetSubParameters(){
			return Value < 0 || Value >= Values.Count ? null : SubParams[Value];
		}

		public override void Clear(){
			Value = 0;
			foreach (Parameters parameters in SubParams){
				parameters.Clear();
			}
		}

		public override float Height{
			get{
				float max = 0;
				foreach (Parameters param in SubParams){
					max = Math.Max(max, param.Height + 6);
				}
				return 44 + max;
			}
		}

		public void SetValueChangedHandlerForSubParams(ValueChangedHandler action){
			ValueChanged += action;
			foreach (Parameter p in GetSubParameters().GetAllParameters()){
				if (p is IntParam || p is DoubleParam){
					p.ValueChanged += action;
				} else{
					(p as SingleChoiceWithSubParams)?.SetValueChangedHandlerForSubParams(action);
				}
			}
		}
		public override ParamType Type => ParamType.Server;

	    public override void ReadXml(XmlReader reader)
	    {
            ReadBasicAttributes(reader);
            reader.ReadStartElement();
	        Value = reader.ReadElementContentAsInt();
	        Values = reader.ReadInto(new List<string>());
	        SubParams = reader.ReadIntoNested(new List<Parameters>());
            reader.ReadEndElement();
	    }

	    public override void WriteXml(XmlWriter writer)
	    {
	        WriteBasicAttributes(writer);
            writer.WriteStartElement("Value");
            writer.WriteValue(Value);
            writer.WriteEndElement();
            writer.WriteValues("Values", Values);
            XmlSerializer serializer = new XmlSerializer(typeof(Parameters));
            writer.WriteStartElement("SubParams");
	        foreach (Parameters parameters in SubParams)
	        {
	            serializer.Serialize(writer, parameters);
	        }
            writer.WriteEndElement();
	    }
	}
}