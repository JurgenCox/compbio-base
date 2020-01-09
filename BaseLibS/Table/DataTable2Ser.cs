using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BaseLibS.Table{
	[Serializable]
	public class DataTable2Ser{
		private readonly Dictionary<string, int> nameMapping;
		private readonly DataTable2Col[] dataCols;
		private readonly int nrows;
		public DataTable2Ser(IList<DataRow2> rows, IList<ColumnType> types){
			if (rows.Count > 0){
				nameMapping = rows[0].nameMapping;
			}
			dataCols = new DataTable2Col[types.Count];
			for (int i = 0; i < types.Count; i++){
				object[] oo = new object[rows.Count];
				for (int j = 0; j < rows.Count; j++){
					oo[j] = rows[j].ItemArray[i];
				}
				dataCols[i] = new DataTable2Col(oo, types[i]);
			}
			nrows = rows.Count;
		}

		public Collection<DataRow2> GetData(){
			Collection<DataRow2> rows = new Collection<DataRow2>();
			object[][] x = new object[dataCols.Length][];
			for (int i = 0; i < x.Length; i++){
				x[i] = dataCols[i].Data;
			}

			for (int i = 0; i < nrows; i++){
				object[] t = new object[dataCols.Length];
				for (int j = 0; j < t.Length; j++){
					t[j] = x[j][i];
				}
				rows.Add(new DataRow2(t, nameMapping));
			}
			return rows;
		}
	}
}