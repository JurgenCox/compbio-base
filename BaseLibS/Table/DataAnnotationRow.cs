using System;
using System.Collections.Generic;

namespace BaseLibS.Table{
	[Serializable]
	public class DataAnnotationRow{
		public object[] ItemArray { get; set; }
		private readonly Dictionary<string, int> nameMapping;

		public DataAnnotationRow(int count, Dictionary<string, int> nameMapping){
			ItemArray = new object[count];
			this.nameMapping = nameMapping;
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