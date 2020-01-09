using System;

namespace BaseLibS.Graph{
	public class NavigatorData{
		public int indicatorX1 = -1;
		public int indicatorX2 = -1;
		public int indicatorY1 = -1;
		public int indicatorY2 = -1;
		public int visibleXStart = -1;
		public int visibleYStart = -1;

		public void Reset(){
			indicatorX1 = -1;
			indicatorX2 = -1;
			indicatorY1 = -1;
			indicatorY2 = -1;
			visibleXStart = -1;
			visibleYStart = -1;
		}

		public void Start(int x, int y, int visX, int visY){
			indicatorX1 = x;
			indicatorX2 = x;
			indicatorY1 = y;
			indicatorY2 = y;
			visibleXStart = visX;
			visibleYStart = visY;
		}

		public bool IsMoving(){
			return visibleXStart != -1;
		}

		public PointI2 Dragging(int x, int y, int width, int height, int totalWidth, int totalHeight, int visibleWidth,
			int visibleHeight, float zoomFactor){
			Size2 overview = GraphUtil.CalcOverviewSize(width, height, totalWidth, totalHeight);
			indicatorX2 = x;
			indicatorY2 = (int) (y - height + overview.Height);
			int newX = visibleXStart + (int) Math.Round((indicatorX2 - indicatorX1)*totalWidth/overview.Width);
			newX = (int) Math.Min(Math.Max(newX, 0), totalWidth - visibleWidth/zoomFactor);
			int newY = visibleYStart + (int) Math.Round((indicatorY2 - indicatorY1)*totalHeight/overview.Height);
			newY = (int) Math.Min(Math.Max(newY, 0), totalHeight - visibleHeight/zoomFactor);
			return new PointI2(newX, newY);
		}
	}
}