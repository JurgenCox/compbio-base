using System;
using System.Collections.Generic;
using BaseLibS.Data;
using BaseLibS.Num;

namespace BaseLibS.Graph.Network{
	[Serializable]
	public abstract class NetworkGraphModel<Tn, Te, Tc> where Tn : NetworkGraphNode
		where Te : NetworkGraphEdge
		where Tc : NetworkGraphContainer{
		private event Action SizeChanged;
		public event Action<bool, int> SelectionChanged;
	    public event EventHandler<EventArgs> NetworkGraphModelChanged;
		public ThreadSafeHashSet<Te> edges = new ThreadSafeHashSet<Te>();
		protected ThreadSafeHashSet<Te> selectedEdges = new ThreadSafeHashSet<Te>();
		public ThreadSafeHashSet<Tn> nodes = new ThreadSafeHashSet<Tn>();
		public ThreadSafeHashSet<Tn> selectedNodes = new ThreadSafeHashSet<Tn>();
		public ThreadSafeHashSet<Tc> containers = new ThreadSafeHashSet<Tc>();
		protected ThreadSafeHashSet<Tc> selectedContainers = new ThreadSafeHashSet<Tc>();
		public int Width { get; set; } = 200;
		public int Height { get; set; } = 300;
		public bool IsSelected(object item){
			if (item is Tn){
				return IsSelected((Tn) item);
			}
			if (item is Te){
				return IsSelected((Te) item);
			}
			if (item is Tc){
				return IsSelected((Tc) item);
			}
			return false;
		}
		public void Deselect(object item){
			if (item is Tn){
				selectedNodes.Remove((Tn) item);
			}
			if (item is Te){
				selectedEdges.Remove((Te) item);
			}
			if (item is Tc){
				selectedContainers.Remove((Tc) item);
			}
		}
		public void Select(object item){
			if (item is Tn){
				selectedNodes.Add((Tn) item);
			}
			if (item is Te){
				selectedEdges.Add((Te) item);
			}
			if (item is Tc){
				selectedContainers.Add((Tc) item);
			}
		}
		public bool IsSelected(Tn n){
			return selectedNodes.Contains(n);
		}
		public void Deselect(Tn n){
			selectedNodes.Remove(n);
		}
		public void Select(Tn n){
			selectedNodes.Add(n);
		}
		public bool IsSelected(Te e){
			return selectedEdges.Contains(e);
		}
		public void Deselect(Te e){
			selectedEdges.Remove(e);
		}
		public void Select(Te e){
			selectedEdges.Add(e);
		}
		public bool IsSelected(Tc c){
			return selectedContainers.Contains(c);
		}
		public void Deselect(Tc c){
			selectedContainers.Remove(c);
		}
		public void Select(Tc c){
			selectedContainers.Add(c);
		}
		public void ClearNodeSelection(){
			selectedNodes.Clear();
		}
		public void ClearEdgeSelection(){
			selectedEdges.Clear();
		}
		public void ClearContainerSelection(){
			selectedContainers.Clear();
		}
		public void ClearSelection(){
			selectedNodes.Clear();
			selectedEdges.Clear();
			selectedContainers.Clear();
		}
		public void Add(Te e){
			edges.Add(e);
            OnNetworkGraphModelChanged();
		}
		public void Add(Tn n){
			ChangeSize(n);
			nodes.Add(n);
            OnNetworkGraphModelChanged();
		}
		public void Add(Tc c){
			ChangeSize(c);
			containers.Add(c);
            OnNetworkGraphModelChanged();
		}
		public void FireSelectionChange(bool updateTable, int selectedIndex){
			SelectionChanged?.Invoke(updateTable, selectedIndex);
		}
		private void FireSizeChange(){
			SizeChanged?.Invoke();
		}
		protected void Remove(Tn n){
			nodes.Remove(n);
			selectedNodes.Remove(n);
            OnNetworkGraphModelChanged();
		}
		protected void Remove(Te e){
			edges.Remove(e);
			selectedEdges.Remove(e);
            OnNetworkGraphModelChanged();
		}
		protected void Remove(Tc c){
			containers.Remove(c);
			selectedContainers.Remove(c);
            OnNetworkGraphModelChanged();
		}
		internal object GetItemAtPos(int x, int y){
			Tn node = GetAnyNodeAtPos(x, y);
			if (node != null){
				return node;
			}
			Te edge = GetAnyEdgeAtPos(x, y);
			if (edge != null){
				return edge;
			}
			return GetBestContainerAtPos(x, y);
		}
		internal Tn GetAnyNodeAtPos(int x, int y){
			foreach (Tn t in nodes){
				Tn node = t;
				if (node.Hits(x, y)){
					return t;
				}
			}
			return null;
		}
		internal Tc GetAnyContainerAtPos(int x, int y){
			foreach (Tc t in containers){
				Tc node = t;
				if (node.Hits(x, y)){
					return t;
				}
			}
			return null;
		}
		internal Tc GetBestContainerAtPos(int x, int y){
			List<Tc> a = GetAllContainersAtPos(x, y);
			if (a.Count == 0){
				return null;
			}
			if (a.Count == 1){
				return a[0];
			}
			int[] z = new int[a.Count];
			for (int i = 0; i < z.Length; i++){
				z[i] = a[i].Z;
			}
			return a[ArrayUtils.MaxInd(z)];
		}
		internal List<Tc> GetAllContainersAtPos(int x, int y){
			List<Tc> result = new List<Tc>();
			foreach (Tc t in containers){
				Tc node = t;
				if (node.Hits(x, y)){
					result.Add(t);
				}
			}
			return result;
		}
		internal Te GetAnyEdgeAtPos(int x, int y){
			foreach (Te t in edges){
				Te edge = t;
				if (edge.Hits(x, y)){
					return t;
				}
			}
			return null;
		}
		internal void ShiftSetectedNodes(int deltaX, int deltaY){
			float minx = int.MaxValue;
			float miny = int.MaxValue;
			foreach (Tn node in selectedNodes){
				node.X += deltaX;
				minx = Math.Min(minx, node.X);
				node.Y += deltaY;
				miny = Math.Min(miny, node.Y);
			}
			if (minx < 30){
				float d = 30 - minx;
				foreach (Tn node in nodes){
					node.X += d;
				}
			}
			if (miny < 30){
				float d = 30 - miny;
				foreach (Tn node in nodes){
					node.Y += d;
				}
			}
			float maxx = float.MinValue;
			float maxy = float.MinValue;
			foreach (Tn node in nodes){
				maxx = Math.Max(maxx, node.X + node.Width);
				maxy = Math.Max(maxy, node.Y + node.Height);
			}
			if (maxx + 100 > Width){
				Width = (int) (maxx + 100);
				FireSizeChange();
			}
			if (maxy + 100 > Height){
				Height = (int) (maxy + 100);
				FireSizeChange();
			}
		}
		internal void SelectAllNodes(){
			foreach (Tn node in nodes){
				selectedNodes.Add(node);
			}
		}
		internal Tn[] GetIntersectingNodes(int x1, int y1, int width1, int height1){
			List<Tn> result = new List<Tn>();
			foreach (Tn node1 in nodes){
				if (node1.Intersects(x1, y1, width1, height1)){
					result.Add(node1);
				}
			}
			return result.ToArray();
		}
		internal void MoveUp(int w){
			foreach (Tn node in selectedNodes.Count == 0 ? nodes : selectedNodes){
				node.Y -= w;
			}
		}
		internal void MoveDown(int w){
			foreach (Tn node in selectedNodes.Count == 0 ? nodes : selectedNodes){
				node.Y += w;
			}
		}
		internal void MoveLeft(int w){
			foreach (Tn node in selectedNodes.Count == 0 ? nodes : selectedNodes){
				node.X -= w;
			}
		}
		internal void MoveRight(int w){
			foreach (Tn node in selectedNodes.Count == 0 ? nodes : selectedNodes){
				node.X += w;
			}
		}
		protected void ChangeSize(NetworkGraphNode node){
			int newWidth = (int) Math.Max(Width, node.X + node.Width + 50);
			int newHeight = (int) Math.Max(Height, node.Y + node.Height + 50);
			if (newHeight > Height || newWidth > Width){
				Height = newHeight;
				Width = newWidth;
				FireSizeChange();
			}
		}
		protected void ChangeSize(NetworkGraphContainer node){
			int newWidth = Math.Max(Width, node.X + node.Width + 50);
			int newHeight = Math.Max(Height, node.Y + node.Height + 50);
			if (newHeight > Height || newWidth > Width){
				Height = newHeight;
				Width = newWidth;
				FireSizeChange();
			}
		}
	    protected void OnNetworkGraphModelChanged()
	    {
	        NetworkGraphModelChanged?.Invoke(this, EventArgs.Empty);
	    }
    }
}