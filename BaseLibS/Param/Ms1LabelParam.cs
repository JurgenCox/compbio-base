using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param {
	[Serializable]
	public class Ms1LabelParam : Parameter<int[][]> {
		public int Multiplicity { get; set; }
		public string[] Values { get; set; }

		/// <summary>
		/// for xml serialization only
		/// </summary>
		private Ms1LabelParam() : this("", new int[0][]) { }

		public Ms1LabelParam(string name, int[][] value) : base(name) {
			Value = value;
			Default = (int[][]) value.Clone();
		}

		public override ParamType Type => ParamType.Server;

		public override string StringValue {
			get => StringUtils.Concat(",", ";", Value);
			set => Value = StringUtils.SplitToInt(',', ';', value);
		}

		public override void Clear() {
			Value = new int[Multiplicity][];
		}

		public override bool IsModified => !ArrayUtils.EqualArraysOfArrays(Value, Default);
		public override float Height => 150f;

		public string[] GetLabels(int ind) {
			return ArrayUtils.SubArray(Values, Value[ind]);
		}

		public void SetLabels(string[][] labels) {
			for (int i = 0; i < labels.Length; i++) {
				Value[i] = GetIndices(Values, labels[i]);
			}
		}

		public static int[] GetIndices(string[] values, IList<string> strings) {
			int[] result = new int[strings.Count];
			for (int i = 0; i < result.Length; i++) {
				result[i] = ArrayUtils.IndexOf(values, strings[i]);
			}
			return result;
		}

		public override void ReadXml(XmlReader reader) {
			ReadBasicAttributes(reader);
			reader.MoveToAttribute("Multiplicity");
			Multiplicity = reader.ReadContentAsInt();
			reader.ReadStartElement();
			Value = reader.ReadJagged2DArrayInto(new List<List<int>>()).Select(x => x.ToArray()).ToArray();
			Values = reader.ReadInto(new List<string>()).ToArray();
			reader.ReadEndElement();
		}

		public override void WriteXml(XmlWriter writer) {
			WriteBasicAttributes(writer);
			writer.WriteStartAttribute("Multiplicity");
			writer.WriteValue(Multiplicity);
			writer.WriteStartElement("Value");
			foreach (int[] labels in Value) {
				writer.WriteStartElement("Items");
				foreach (int label in labels) {
					writer.WriteStartElement("Item");
					writer.WriteValue(label);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteValues("Values", Values);
		}
	}
}