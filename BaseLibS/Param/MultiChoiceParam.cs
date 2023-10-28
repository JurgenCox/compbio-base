using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Param{
	[Serializable]
	public class MultiChoiceParam : Parameter<int[]>{
		public bool Repeats{ get; set; }
		public IList<string> Values{ get; set; }
		public List<string> DefaultSelectionNames{ get; set; }
		public List<string[]> DefaultSelections{ get; set; }
		public MultiChoiceParam(string name) : this(name, new int[0]){ }

		/// <summary>
		/// for xml serialization only
		/// </summary>
		public MultiChoiceParam() : this("", new int[0]){ }

		public MultiChoiceParam(string name, int[] value) : base(name){
			Value = value;
			Default = new int[Value.Length];
			for (int i = 0; i < Value.Length; i++){
				Default[i] = Value[i];
			}
			Values = new string[0];
			Repeats = false;
			DefaultSelectionNames = new List<string>();
			DefaultSelections = new List<string[]>();
		}

		protected MultiChoiceParam(string name, string help, string url, bool visible, int[] value, int[] default1,
			bool repeats, IList<string> values, List<string> defaultSelectionNames, List<string[]> defaultSelections) :
			base(name, help, url, visible, value, default1){
			Repeats = repeats;
			Values = values;
			DefaultSelectionNames = defaultSelectionNames;
			DefaultSelections = defaultSelections;
		}
		public override void Read(BinaryReader reader) {
			base.Read(reader);
			Value = FileUtils.ReadInt32Array(reader);
			Default = FileUtils.ReadInt32Array(reader);
			Repeats = reader.ReadBoolean();
			Values = FileUtils.ReadStringArray(reader);
			DefaultSelectionNames = new List<string>(FileUtils.ReadStringArray(reader));
			int n = reader.ReadInt32();
			DefaultSelections = new List<string[]>();
			for (int i = 0; i < n; i++){
				DefaultSelections.Add(FileUtils.ReadStringArray(reader));
			}
		}
		public override void Write(BinaryWriter writer) {
			base.Write(writer);
			FileUtils.Write(Value, writer);
			FileUtils.Write(Default, writer);
			writer.Write(Repeats);
			FileUtils.Write(DefaultSelectionNames.ToArray(), writer);
			writer.Write(DefaultSelections.Count);
			foreach (string[] strings in DefaultSelections){
				FileUtils.Write(strings, writer);
			}
		}

		public override string StringValue{
			get => StringUtils.Concat(";", Values.SubArray(Value));
			set{
				if (value.Trim().Length == 0){
					Value = new int[0];
					return;
				}
				string[] q = value.Trim().Split(';');
				Value = new int[q.Length];
				for (int i = 0; i < Value.Length; i++){
					Value[i] = Values.IndexOf(q[i]);
				}
				Value = Filter(Value);
			}
		}

		private static int[] Filter(IEnumerable<int> value){
			List<int> result = new List<int>();
			foreach (int i in value){
				if (i >= 0){
					result.Add(i);
				}
			}
			return result.ToArray();
		}

		public override bool IsModified => !ArrayUtils.EqualArrays(Value, Default);

		public override void Clear(){
			Value = new int[0];
		}

		public override float Height => 160f;

		public void AddSelectedIndex(int index){
			if (Array.BinarySearch(Value, index) >= 0){
				return;
			}
			Value = InsertSorted(Value, index);
		}

		private static int[] InsertSorted(IList<int> value, int index){
			int[] result = ArrayUtils.Concat(value, index);
			Array.Sort(result);
			return result;
		}

		public void AddDefaultSelector(string title, string[] sel){
			DefaultSelectionNames.Add(title);
			DefaultSelections.Add(sel);
		}

		public void SetFromStrings(string[] x){
			List<int> indices = new List<int>();
			foreach (string s in x){
				int ind = Values.IndexOf(s);
				indices.Add(ind);
			}
			indices.Sort();
			Value = indices.ToArray();
		}

		public override ParamType Type => ParamType.Server;

		public override void ReadXml(XmlReader reader){
			ReadBasicAttributes(reader);
			reader.MoveToAttribute("Repeats");
			Repeats = reader.ReadContentAsBoolean();
			reader.ReadStartElement();
			Value = reader.ReadInto(new List<int>()).ToArray();
			Values = reader.ReadInto(new List<string>()).ToArray();
			reader.ReadEndElement();
		}

		public override void WriteXml(XmlWriter writer){
			WriteBasicAttributes(writer);
			writer.WriteStartAttribute("Repeats");
			writer.WriteValue(Repeats);
			writer.WriteValues("Value", Value);
			writer.WriteValues("Values", Values);
		}

		public override object Clone(){
			return new MultiChoiceParam(Name, Help, Url, Visible, Value, Default, Repeats, Values,
				DefaultSelectionNames, DefaultSelections);
		}
	}
}