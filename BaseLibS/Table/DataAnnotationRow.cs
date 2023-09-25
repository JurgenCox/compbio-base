using System;
using System.Collections.Generic;
using System.IO;
namespace BaseLibS.Table{
	[Serializable]
	public class DataAnnotationRow{
		public object[] ItemArray { get; set; }
		private readonly Dictionary<string, int> nameMapping;

		public DataAnnotationRow(int count, Dictionary<string, int> nameMapping){
			ItemArray = new object[count];
			this.nameMapping = nameMapping;
		}
		//TODO: nameMapping is written many times
		public DataAnnotationRow(BinaryReader reader, IList<ColumnType> types){
			int n = reader.ReadInt32();
			ItemArray = new object[n];
			for (int i = 0; i < n; i++){
				ItemArray[i] = TableUtils.ReadElement(reader, types[i]);
			}
			n = reader.ReadInt32();
			nameMapping = new Dictionary<string, int>();
			for (int i = 0; i < n; i++) {
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				nameMapping.Add(key, value);
			}
		}

		public void Write(BinaryWriter writer, IList<ColumnType> types){
			writer.Write(ItemArray.Length);
			for (int i = 0; i < ItemArray.Length; i++){
				TableUtils.WriteElement(writer, ItemArray[i], types[i]);
			}
			writer.Write(nameMapping.Count);
			foreach (KeyValuePair<string, int> pair in nameMapping){
				writer.Write(pair.Key);
				writer.Write(pair.Value);
			}
		}

		public object this[int column]{
			get => ItemArray[column];
			set => ItemArray[column] = value;
		}

		public object this[string colName]{
			get => ItemArray[nameMapping[colName]];
			set => ItemArray[nameMapping[colName]] = value;
		}
	}
}