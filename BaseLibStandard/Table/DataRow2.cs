using System;
using System.Collections.Generic;

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