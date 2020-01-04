namespace BaseLibS.Graph{
	public class StringFormat2{
		public StringAlignment2 Alignment { get; set; } = StringAlignment2.Center;
		public StringAlignment2 LineAlignment { get; set; } = StringAlignment2.Center;
		public bool Vertical { get; set; }

		public StringFormat2(bool vertical){
			Vertical = vertical;
		}

		public StringFormat2() : this(false){}
	}
}