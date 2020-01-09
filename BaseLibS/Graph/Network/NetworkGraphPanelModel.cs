using System;

namespace BaseLibS.Graph.Network{
	public abstract class NetworkGraphPanelModel<Tn, Te, Tc, Tm, Ti> : ISimpleScrollableControlModel
		where Tn : NetworkGraphNode<Te, Ti>
		where Te : NetworkGraphEdge<Tn>
		where Tc : NetworkGraphContainer
		where Tm : NetworkGraphModel<Tn, Te, Tc>
		where Ti : IDrawOptions{
		private readonly Pen2 indicatorPen = new Pen2(Color2.Orange);
		private readonly Brush2 indicatorBrush = new Brush2(Color2.FromArgb(60, Color2.CornflowerBlue));
		private const int ioCircleRadius = 3;
		private readonly Pen2 ioPen = Pens2.Black;
		private int indicatorX1 = -1;
		private int indicatorX2 = -1;
		private int indicatorY1 = -1;
		private int indicatorY2 = -1;
		private NetworkGraphDragType dragType = NetworkGraphDragType.Undefined;
		private float dpiScaleX;
		private ISimpleScrollableControl control;
		private bool HasMoved(){
			return indicatorX1 != indicatorX2 || indicatorY1 != indicatorY2;
		}
		private int MinIndicatorX => Math.Min(indicatorX1, indicatorX2);
		private int MaxIndicatorX => Math.Max(indicatorX1, indicatorX2);
		private int AbsDeltaIndicatorX => Math.Abs(indicatorX1 - indicatorX2);
		private int DeltaIndicatorX => indicatorX2 - indicatorX1;
		private int MinIndicatorY => Math.Min(indicatorY1, indicatorY2);
		private int MaxIndicatorY => Math.Max(indicatorY1, indicatorY2);
		private int AbsDeltaIndicatorY => Math.Abs(indicatorY1 - indicatorY2);
		private int DeltaIndicatorY => indicatorY2 - indicatorY1;
		public abstract void EditProperties(float f, float f1);
		public abstract Ti Options { get; }
		public void ProcessCmdKey(Keys2 keyData){
			switch (keyData){
				case Keys2.Control | Keys2.A:
					GraphModel.SelectAllNodes();
					GraphModel.FireSelectionChange(true, 0);
					control.Invalidate(true);
					break;
				case Keys2.Up:
					GraphModel.MoveUp(5);
					control.Invalidate(true);
					break;
				case Keys2.Down:
					GraphModel.MoveDown(5);
					control.Invalidate(true);
					break;
				case Keys2.Left:
					GraphModel.MoveLeft(5);
					control.Invalidate(true);
					break;
				case Keys2.Right:
					GraphModel.MoveRight(5);
					control.Invalidate(true);
					break;
			}
		}
		public void InvalidateBackgroundImages(){}
		public void OnSizeChanged(){}
		public abstract Tm GraphModel { get; }
		public void Register(ISimpleScrollableControl control1){
			control = control1;
			dpiScaleX = control1.DpiScale;
			control1.OnPaintMainView = (g, x, y, width, height, isOverview) =>{
				if (GraphModel == null){
					return;
				}
				g.SmoothingMode = SmoothingMode2.AntiAlias;
				PaintWorkflow(g, GraphModel, x, y, Options, isOverview);
				if (dragType == NetworkGraphDragType.Select){
					g.FillRectangle(indicatorBrush, MinIndicatorX, MinIndicatorY, AbsDeltaIndicatorX, AbsDeltaIndicatorY);
					g.DrawRectangle(indicatorPen, MinIndicatorX, MinIndicatorY, AbsDeltaIndicatorX, AbsDeltaIndicatorY);
				}
				g.SmoothingMode = SmoothingMode2.Default;
			};
			control1.OnMouseIsDownMainView = e =>{
				indicatorX1 = e.X;
				indicatorX2 = indicatorX1;
				indicatorY1 = e.Y;
				indicatorY2 = indicatorY1;
				object item = GraphModel.GetItemAtPos(e.X + control1.VisibleX, e.Y + control1.VisibleY);
				dragType = item != null ? NetworkGraphDragType.Move : NetworkGraphDragType.Select;
				if (e.ControlPressed){
					if (item != null){
						if (GraphModel.IsSelected(item)){
							GraphModel.Deselect(item);
						} else{
							GraphModel.Select(item);
						}
					}
				} else{
					if (item != null){
						if (!GraphModel.IsSelected(item)){
							GraphModel.ClearSelection();
							GraphModel.Select(item);
						}
					} else{
						GraphModel.ClearSelection();
					}
				}
				GraphModel.FireSelectionChange(true, 0);
				control1.Invalidate(true);
			};
			control1.OnMouseIsUpMainView = e =>{
				if (HasMoved()){
					switch (dragType){
						case NetworkGraphDragType.Select:
							bool add = e.ControlPressed;
							SelectNodes(MinIndicatorX, MaxIndicatorX, MinIndicatorY, MaxIndicatorY, add);
							control1.InvalidateOverview();
							break;
						case NetworkGraphDragType.Move:
							GraphModel.ShiftSetectedNodes(DeltaIndicatorX, DeltaIndicatorY);
							control1.InvalidateOverview();
							break;
					}
				}
				indicatorX1 = -1;
				indicatorX2 = -1;
				indicatorY1 = -1;
				indicatorY2 = -1;
				dragType = NetworkGraphDragType.Undefined;
				control1.InvalidateMainView();
			};
			control1.OnMouseDraggedMainView = e =>{
				indicatorX2 = e.X;
				indicatorY2 = e.Y;
				control1.InvalidateMainView();
			};
			control1.OnMouseClickMainView = e =>{
				if (!e.IsMainButton){
					indicatorX1 = -1;
					indicatorX2 = -1;
					indicatorY1 = -1;
					indicatorY2 = -1;
					dragType = NetworkGraphDragType.Undefined;
					Tuple<int, int> p = control1.GetOrigin();
					EditProperties(p.Item2/dpiScaleX + 15, p.Item1/dpiScaleX);
					control1.InvalidateMainView();
				}
			};
			control1.OnMouseHoverMainView = e =>{
				//Point p = Cursor.Position;
				//WorkflowNode node = WorkflowModel.GetNodeAtPos(p.X + VisibleX, p.Y + VisibleY);
				//if (node != null){
				//	MessageBox.Show("" + node.name);
				//}
			};
			control1.TotalWidth = () => GraphModel?.Width ?? 100;
			control1.TotalHeight = () => GraphModel?.Height ?? 100;
		}
		private void SelectNodes(int minX, int maxX, int minY, int maxY, bool add){
			if (!add){
				GraphModel.ClearSelection();
			}
			Tn[] nodes = GraphModel.GetIntersectingNodes(minX + control.VisibleX, minY + control.VisibleY, maxX - minX,
				maxY - minY);
			foreach (Tn node in nodes){
				GraphModel.Select(node);
			}
			GraphModel.FireSelectionChange(true, 0);
			control.Invalidate(true);
		}
		private void PaintNode(IGraphics g, bool selected, Tn node, int x1, int y1, Ti options, bool isOverview){
			float x = node.X - x1;
			float y = node.Y - y1;
			if (dragType == NetworkGraphDragType.Move && selected){
				x += DeltaIndicatorX;
				y += DeltaIndicatorY;
			}
			bool isOutside = x + node.Width < 0 || x > control.VisibleWidth/control.ZoomFactor || y + node.Height < 0 ||
							y > control.VisibleHeight/control.ZoomFactor;
			if (!isOverview && isOutside){
				return;
			}
			node.Paint(g, x, y, options);
			if (selected){
				node.PaintSelected(g, x, y);
			}
		}
		private void PaintContainer(IGraphics g, bool selected, Tc container, int x1, int y1, bool isOverview){
			int x = container.X - x1;
			int y = container.Y - y1;
			if (dragType == NetworkGraphDragType.Move && selected){
				x += DeltaIndicatorX;
				y += DeltaIndicatorY;
			}
			bool isOutside = x + container.Width < 0 || x > control.VisibleWidth/control.ZoomFactor || y + container.Height < 0 ||
							y > control.VisibleHeight/control.ZoomFactor;
			if (!isOverview && isOutside){
				return;
			}
			container.Paint(g, x, y);
			if (selected){
				container.PaintSelected(g, x, y);
			}
		}
		private void PaintIoPoint(IGraphics g, float x, float y, Brush2 brush, bool isOverview){
			bool isOutside = x + ioCircleRadius < 0 || x - ioCircleRadius > control.VisibleWidth/control.ZoomFactor ||
							y + ioCircleRadius < 0 || y - ioCircleRadius > control.VisibleHeight/control.ZoomFactor;
			if (!isOverview && isOutside){
				return;
			}
			g.FillEllipse(brush, x - ioCircleRadius, y - ioCircleRadius, ioCircleRadius*2, ioCircleRadius*2);
			g.DrawEllipse(ioPen, x - ioCircleRadius, y - ioCircleRadius, ioCircleRadius*2, ioCircleRadius*2);
		}
		private void PaintIoPoints(IGraphics g, bool selected, Tn node, int x, int y, bool isOverview){
			int dx = -x;
			int dy = -y;
			if (dragType == NetworkGraphDragType.Move && selected){
				dx += DeltaIndicatorX;
				dy += DeltaIndicatorY;
			}
			for (int i = 0; i < node.InputCount; i++){
				PaintIoPoint(g, node.GetInputPosX(i) + dx, node.GetInputPosY(i) + dy, node.GetInputBrush(i), isOverview);
			}
			for (int i = 0; i < node.OutputCount; i++){
				PaintIoPoint(g, node.GetOutputPosX(i) + dx, node.GetOutputPosY(i) + dy, node.GetOutputBrush(i), isOverview);
			}
		}
		private void PaintEdge(IGraphics g, Te edge, bool fromMoves, bool toMoves, int x, int y, bool isOverview){
			Tn fromNode = edge.FromNode;
			float x1 = fromNode.GetOutputPosX(edge.FromOutputIndex) - x;
			float y1 = fromNode.GetOutputPosY(edge.FromOutputIndex) - y;
			if (fromMoves){
				x1 += DeltaIndicatorX;
				y1 += DeltaIndicatorY;
			}
			if (edge.ToNode == null){
				return;
			}
			Tn toNode = edge.ToNode;
			float x2 = toNode.GetInputPosX(edge.ToInputIndex) - x;
			float y2 = toNode.GetInputPosY(edge.ToInputIndex) - y;
			if (toMoves){
				x2 += DeltaIndicatorX;
				y2 += DeltaIndicatorY;
			}
			float xMax = Math.Max(x1, x2);
			float xMin = Math.Min(x1, x2);
			float yMax = Math.Max(y1, y2);
			float yMin = Math.Min(y1, y2);
			bool isOutside = xMax < 0 || xMin > control.VisibleWidth/control.ZoomFactor || yMax < 0 ||
							yMin > control.VisibleHeight/control.ZoomFactor;
			if (!isOverview && isOutside){
				return;
			}
			edge.PaintEdge(g, x1, y1, x2, y2);
		}
		private void PaintWorkflow(IGraphics g, Tm model, int x, int y, Ti options, bool isOverview){
			foreach (Tc container in model.containers){
				PaintContainer(g, model.IsSelected(container), container, x, y, isOverview);
			}
			foreach (Tn node in model.nodes){
				PaintNode(g, model.IsSelected(node), node, x, y, options, isOverview);
			}
			foreach (Te edge in model.edges){
				bool fromMoves = dragType == NetworkGraphDragType.Move && model.IsSelected(edge.FromNode);
				bool toMoves = dragType == NetworkGraphDragType.Move && model.IsSelected(edge.ToNode);
				PaintEdge(g, edge, fromMoves, toMoves, x, y, isOverview);
			}
			foreach (Tn node in model.nodes){
				PaintIoPoints(g, model.IsSelected(node), node, x, y, isOverview);
			}
		}
	}
}