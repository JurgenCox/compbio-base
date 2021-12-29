using System.Collections.Generic;

namespace BaseLibS.Graph.Base{
	public class BasicColumnStyles{
		private readonly List<BasicColumnStyle> list = new List<BasicColumnStyle>();
		private readonly TableLayoutModel view;

		internal BasicColumnStyles(TableLayoutModel view){
			this.view = view;
		}

		public int Count => list.Count;

		public BasicColumnStyle this[int i]{
			get => list[i];
			set{
				list[i] = value;
				view.InvalidateSizes();
			}
		}

		public BasicColumnStyle[] ToArray(){
			return list.ToArray();
		}

		public void Add(BasicColumnStyle x){
			list.Add(x);
			view.InvalidateSizes();
		}
	}
}