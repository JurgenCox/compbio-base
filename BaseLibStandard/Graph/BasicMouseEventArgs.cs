using System;

namespace BaseLibS.Graph{
	public class BasicMouseEventArgs{
		public int X { get; }
		public int Y { get; }
		public int Width { get; }
		public int Height { get; }
		public bool IsMainButton { get; }
		private readonly Func<bool> controlPressed;
		private readonly Action<string, int, int, int> showTip;

		public BasicMouseEventArgs(int x, int y, bool isMainButton, int width, int height, Func<bool> controlPressed,
			Action<string, int, int, int> showTip){
			X = x;
			Y = y;
			Width = width;
			Height = height;
			IsMainButton = isMainButton;
			this.controlPressed = controlPressed;
			this.showTip = showTip;
		}

		public BasicMouseEventArgs(BasicMouseEventArgs e, int dx, int dy, int width, int height){
			X = e.X - dx;
			Y = e.Y - dy;
			Width = width;
			Height = height;
			IsMainButton = e.IsMainButton;
			controlPressed = e.controlPressed;
			showTip = e.showTip;
		}

		public bool ControlPressed => controlPressed();

		public void ViewToolTip(string text){
			showTip(text, X + 10, Y + 10, 5000);
		}

		public BasicMouseEventArgs Scale(float s){
			return new BasicMouseEventArgs((int) (X/s), (int) (Y/s), IsMainButton, (int) (Width/s), (int) (Height/s),
				controlPressed, showTip);
		}
	}
}