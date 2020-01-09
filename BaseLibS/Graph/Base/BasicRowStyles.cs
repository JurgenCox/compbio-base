using System.Collections.Generic;

namespace BaseLibS.Graph.Base{
	public class BasicRowStyles{
		private readonly List<BasicRowStyle> list = new List<BasicRowStyle>();
		private readonly BasicTableLayoutView view;

		internal BasicRowStyles(BasicTableLayoutView view){
			this.view = view;
		}

		public int Count => list.Count;

		public BasicRowStyle this[int i]{
			get => list[i];
			set{
				list[i] = value;
				view.InvalidateSizes();
			}
		}

		public BasicRowStyle[] ToArray(){
			return list.ToArray();
		}

		public void Add(BasicRowStyle x){
			list.Add(x);
			view.InvalidateSizes();
		}
	}
}