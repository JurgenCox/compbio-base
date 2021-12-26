using System;
using BaseLibS.Drawing;
using BaseLibS.Graph.Base;

namespace BaseLibS.Graph.Scroll {
	public class ScrollBarView : BasicView {
		protected readonly IScrollableControl main;
		protected ScrollBarState state = ScrollBarState.Neutral;
		protected Bitmap2 firstMark;
		protected Bitmap2 firstMarkHighlight;
		protected Bitmap2 firstMarkPress;
		protected Bitmap2 secondMark;
		protected Bitmap2 secondMarkHighlight;
		protected Bitmap2 secondMarkPress;
		protected Bitmap2 bar;
		protected Bitmap2 barHighlight;
		protected Bitmap2 barPress;
		protected int dragStart = -1;
		protected int visibleDragStart = -1;

		protected internal ScrollBarView(IScrollableControl main) {
			this.main = main;
		}

		protected int ScrollBarWidth => (GraphUtil.scrollBarWidth);
		protected int MinBarSize => (GraphUtil.minBarSize);

		public override void OnResize(EventArgs e, int width, int height) {
			bar = null;
		}
	}
}