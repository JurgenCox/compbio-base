using System;
using BaseLibS.Graph;

namespace BaseLibS.Table{
	[Serializable]
	internal class DataTable2Col{
		private readonly object data;
		private readonly ColumnType type;

		public DataTable2Col(object[] data, ColumnType type){
			this.type = type;
			if (data == null){
				return;
			}
			switch (type){
				case ColumnType.Color:{
					int[] x = new int[data.Length];
					for (int i = 0; i < x.Length; i++){
						x[i] = ((Color2) data[i]).ToArgb();
					}
					this.data = x;
					break;
				}
				case ColumnType.DashStyle:
				case ColumnType.Integer:{
					int[] x = new int[data.Length];
					for (int i = 0; i < x.Length; i++){
						x[i] = data[i] != null ? (int) data[i] : int.MaxValue;
					}
					this.data = x;
					break;
				}
				case ColumnType.Boolean:{
					bool[] x = new bool[data.Length];
					for (int i = 0; i < x.Length; i++){
						x[i] = data[i] != null && (bool) data[i];
					}
					this.data = x;
					break;
				}
				case ColumnType.Numeric:
				case ColumnType.MultiNumeric:
				case ColumnType.MultiInteger:
				case ColumnType.Categorical:
				case ColumnType.Text:
				case ColumnType.DateTime:
					this.data = data;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public object[] Data{
			get{
				switch (type){
					case ColumnType.Color:{
						int[] x = (int[]) data;
						object[] y = new object[x.Length];
						for (int i = 0; i < y.Length; i++){
							y[i] = Color2.FromArgb(x[i]);
						}
						return y;
					}
					case ColumnType.DashStyle:
					case ColumnType.Integer:{
						int[] x = (int[]) data;
						object[] y = new object[x.Length];
						for (int i = 0; i < y.Length; i++){
							y[i] = x[i];
						}
						return y;
					}
					case ColumnType.Boolean:{
						bool[] x = (bool[]) data;
						object[] y = new object[x.Length];
						for (int i = 0; i < y.Length; i++){
							y[i] = x[i];
						}
						return y;
					}
					case ColumnType.Numeric:
					case ColumnType.MultiNumeric:
					case ColumnType.MultiInteger:
					case ColumnType.Categorical:
					case ColumnType.Text:
					case ColumnType.DateTime:
						return (object[]) data;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}