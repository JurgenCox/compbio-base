using System;

namespace BaseLibS.Graph{
	[Serializable]
	public class Pen2{
		public Pen2(Color2 color) : this(color, 1f){}

		public Pen2(Color2 color, float width){
			Color = color;
			Width = width;
			DashCap = DashCap2.Flat;
		}

		public Color2 Color { get; set; }
		public float Width { get; set; }
		public DashCap2 DashCap { get; set; }
		public DashStyle2 DashStyle { get; set; }

		/// <summary>
		/// Gets or sets the distance from the start of a line to the beginning of a dash pattern.
		/// Returns the distance from the start of a line to the beginning of a dash pattern. 
		/// </summary>
		public float DashOffset { get; set; }

		/// <summary>
		/// Gets or sets an array of custom dashes and spaces.
		/// Returns an array of real numbers that specifies the lengths of alternating dashes and 
		/// spaces in dashed lines. 
		/// </summary>
		public float[] DashPattern { get; set; }

		public Pen2 Scale(float s){
			return new Pen2(Color, s*Width);
		}
	}
}