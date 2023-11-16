using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using BaseLibS.Num;
using BaseLibS.Util;
namespace BaseLibS.Param{
	/// <summary>
	/// Parameter value format: 8 item string array
	/// [0] file name: string
	/// [1] column names: string
	/// [2-6] main, num, cat, text, multi-numeric: integers separated by ';'
	/// [7] shorten column names: bool
	/// </summary>
	[Serializable]
	public class PerseusLoadMatrixParam : Parameter<string[]>{
		public string Filter{ get; set; }

		/// <summary>
		/// Puts thresholds on parameters that are used for filtering rows on upload.
		/// Allows for the use of very large matrices as input by only partially loading them into memory
		/// </summary>
		public IList<Parameters[]> FilterParameterValues{ get; set; }

		/// <summary>
		/// for xml serialization only
		/// </summary>
		public PerseusLoadMatrixParam() : this(""){ }

		public PerseusLoadMatrixParam(string name) : base(name){
			Value = new string[8];
			Default = new string[8];
			for (int i = 0; i < 8; i++){
				Value[i] = "";
				Default[i] = "";
			}
			Filter = null;
			FilterParameterValues = new List<Parameters[]>(new Parameters[6][]);
		}

		protected PerseusLoadMatrixParam(string name, string help, string url, bool visible, string[] value,
			string[] default1, string filter) : base(name, help, url, visible, value, default1){
			Filter = filter;
		}

		public override string StringValue{
			get => StringUtils.Concat("\n", Value);
			set => Value = value.Split('\n');
		}

		public override bool IsDropTarget => true;

		public override void Clear(){
			Value = new string[8];
			for (int i = 0; i < 8; i++){
				Value[i] = "";
			}
		}

		public override float Height => 790;


		public string Filename => Value[0];
		public string[] Items => Value[1].Length > 0 ? Value[1].Split(';') : new string[0];
		public override bool IsModified => !ArrayUtils.EqualArrays(Default, Value);

		private int[] GetIntValues(int i){
			string x = Value[i + 2];
			string[] q = x.Length > 0 ? x.Split(';') : new string[0];
			int[] result = new int[q.Length];
			for (int i1 = 0; i1 < q.Length; i1++){
				result[i1] = Parser.Int(q[i1]);
			}
			return result;
		}

		public int[] MainColumnIndices => GetIntValues(0);
		public int[] NumericalColumnIndices => GetIntValues(1);
		public int[] CategoryColumnIndices => GetIntValues(2);
		public int[] TextColumnIndices => GetIntValues(3);
		public int[] MultiNumericalColumnIndices => GetIntValues(4);
		public bool ShortenExpressionColumnNames => bool.Parse(Value[7]);
		public Parameters[] MainFilterParameters => FilterParameterValues[0] ?? new Parameters[0];
		public Parameters[] NumericalFilterParameters => FilterParameterValues[1] ?? new Parameters[0];

		public override void WriteXml(XmlWriter writer){
			WriteBasicAttributes(writer);
			writer.WriteStartElement("Value");
			writer.WriteString(StringValue);
			writer.WriteEndElement();
			XmlSerializer serializer = new XmlSerializer(typeof(Parameters[]));
			writer.WriteStartElement("MainFilterParameters");
			serializer.Serialize(writer, MainFilterParameters);
			writer.WriteEndElement();
			writer.WriteStartElement("NumericalFilterParameters");
			serializer.Serialize(writer, NumericalFilterParameters);
			writer.WriteEndElement();
		}

		public override object Clone(){
			PerseusLoadMatrixParam result =
				new PerseusLoadMatrixParam(Name, Help, Url, Visible, Value, Default, Filter){
					FilterParameterValues = new List<Parameters[]>()
				};
			foreach (Parameters[] value in FilterParameterValues){
				if (value == null){
					result.FilterParameterValues.Add(null);
				} else{
					Parameters[] x = new Parameters[value.Length];
					for (int i = 0; i < x.Length; i++){
						x[i] = (Parameters) value[i].Clone();
					}
					result.FilterParameterValues.Add(x);
				}
			}
			return result;
		}

		public override void ReadXml(XmlReader reader){
			ReadBasicAttributes(reader);
			reader.ReadStartElement();
			StringValue = reader.ReadElementContentAsString("Value", "");
			XmlSerializer serializer = new XmlSerializer(typeof(Parameters[]));
			reader.ReadStartElement("MainFilterParameters");
			FilterParameterValues[0] = (Parameters[]) serializer.Deserialize(reader);
			reader.ReadEndElement();
			reader.ReadStartElement("NumericalFilterParameters");
			FilterParameterValues[1] = (Parameters[]) serializer.Deserialize(reader);
			reader.ReadEndElement();
			reader.ReadEndElement();
		}
		public override ParamType Type => ParamType.Server;
	}
}