using System;
using System.Collections.Generic;
using System.IO;
namespace BaseLibS.Table{
	[Serializable]
	public class DataRow2{
		public object[] ItemArray { get; set; }
		internal readonly Dictionary<string, int> nameMapping;

		public DataRow2(int count, Dictionary<string, int> nameMapping){
			ItemArray = new object[count];
			this.nameMapping = nameMapping;
		}

		internal DataRow2(object[] itemArray, Dictionary<string, int> nameMapping){
			ItemArray = itemArray;
			this.nameMapping = nameMapping;
		}

		public DataRow2(BinaryReader reader, IList<ColumnType> type){
			int n = reader.ReadInt32();
			ItemArray = new object[n];
			for (int i = 0; i < n; i++){
				ItemArray[i] = TableUtils.ReadElement(reader, type[i]);
			}
			n = reader.ReadInt32();
			nameMapping = new Dictionary<string, int>();
			for (int i = 0; i < n; i++) {
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				nameMapping.Add(key, value);
			}
		}
		public void Write(BinaryWriter writer, IList<ColumnType> type) {
			writer.Write(ItemArray.Length);
			for (int i = 0; i < ItemArray.Length; i++){
				TableUtils.WriteElement(writer, ItemArray[i], type[i]);
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
			set{
				if (!nameMapping.ContainsKey(colName)){
					throw new Exception("Unknown column: " + colName);
				}
				ItemArray[nameMapping[colName]] = value;
			}
		}
	}
}