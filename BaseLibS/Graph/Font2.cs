namespace BaseLibS.Graph{
	public class Font2{
		public string Name { get; }
		public float Size { get; }
		public FontStyle2 Style { get; }
		public Font2(string name, float size) : this(name, size, FontStyle2.Regular){}

		public Font2(string name, float size, FontStyle2 style){
			Name = name;
			Size = size;
			Style = style;
		}

		public int Height => (int) (1.6*Size);
		public bool Bold => Style == FontStyle2.Bold;

		public Font2 Scale(float s){
			return new Font2(Name, Size*s, Style);
		}

	    public override string ToString()
	    {
	        return $"Name = \"{Name}\" Size={Size}";
	    }
	}
}