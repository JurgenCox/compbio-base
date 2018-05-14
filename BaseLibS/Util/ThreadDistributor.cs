using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BaseLibS.Util {
	public interface IThreadDistributor {
		void Start();
		void Abort();
	}

	public class ThreadDistributor : IThreadDistributor {
		protected readonly int nThreads;
		protected readonly int nTasks;
		protected Thread[] allWorkThreads;
		protected Stack<int> toBeProcessed;
		private readonly Action<int, int> calculation;
		private readonly object locker = new object();
		public Action<double> ReportProgress { get; set; }
		public int DelayMs { get; set; }
		private int tasksDone;

		public ThreadDistributor(int nThreads, int nTasks, Action<int> calculation) : this(nThreads, nTasks,
			(itask, ithread) => calculation(itask)) { }

		public ThreadDistributor(int nThreads, int nTasks, Action<int, int> calculation) {
			this.nThreads = Math.Min(nThreads, nTasks);
			this.nTasks = nTasks;
			this.calculation = calculation;
		}

		public void Abort() {
			if (allWorkThreads != null) {
				foreach (Thread t in allWorkThreads.Where(t => t != null)) {
					t.Abort();
				}
			}
		}

		public void Start() {
			toBeProcessed = new Stack<int>();
			for (int index = nTasks - 1; index >= 0; index--) {
				toBeProcessed.Push(index);
			}
			allWorkThreads = new Thread[nThreads];
			for (int i = 0; i < nThreads; i++) {
				allWorkThreads[i] = new Thread(Work);
				allWorkThreads[i].Start(i);
				if (DelayMs > 0) {
					Thread.Sleep(DelayMs);
				}
			}
			for (int i = 0; i < nThreads; i++) {
				allWorkThreads[i].Join();
			}
		}

		private void Work(object ithread) {
			ReportProgress?.Invoke(0);
			while (true) {
				int x;
				lock (locker) {
					if (toBeProcessed.Count == 0) {
						break;
					}
					x = toBeProcessed.Pop();
				}
				calculation(x, (int) ithread);
				lock (locker) {
					tasksDone++;
					ReportProgress?.Invoke(tasksDone / (double) nTasks);
				}
			}
		}
	}

	public class BinaryTreeThreadDistributor<T> : IThreadDistributor {
		protected Thread[] allWorkThreads;
		private readonly int nThreads;
		private readonly Action<T, T> action;
		private readonly Node[] nodes;
		private readonly object locker = new object();

		public BinaryTreeThreadDistributor(T[] objects, Action<T, T> action, int nThreads) {
			this.action = action;
			this.nThreads = nThreads;
			nodes = new Node[2 * objects.Length - 1];
			Queue<Node> nodeQueue = new Queue<Node>();
			int j = 0;
			for (; j < objects.Length; j++) {
				nodes[j] = new Node(null, null, objects[j], true, true);
				nodeQueue.Enqueue(nodes[j]);
			}
			for (; j < nodes.Length; j++) {
				Node n1 = nodeQueue.Dequeue();
				Node n2 = nodeQueue.Dequeue();
				nodes[j] = new Node(n1, n2, n1.Data);
				nodeQueue.Enqueue(nodes[j]);
			}
		}

		public void Start() {
			allWorkThreads = new Thread[nThreads];
			for (int i = 0; i < nThreads; i++) {
				allWorkThreads[i] = new Thread(Work);
				allWorkThreads[i].Start(i);
			}
			for (int i = 0; i < nThreads; i++) {
				allWorkThreads[i].Join();
			}
		}

		private void Work() {
			while (true) {
				int k;
				lock (locker) {
					k = 0;
					while (k < nodes.Length && !nodes[k].Started && !nodes[k].Finished && nodes[k].Left.Finished &&
					       nodes[k].Right.Finished) k++;
					if (k != nodes.Length) {
						nodes[k].Started = true;
					} else {
						break;
					}
				}
				action(nodes[k].Left.Data, nodes[k].Right.Data);
				lock (locker) {
					nodes[k].Finished = true;
				}
			}
		}

		public void Abort() {
			if (allWorkThreads == null) return;
			foreach (Thread t in allWorkThreads.Where(t => t != null)) {
				t.Abort();
			}
		}

		private class Node {
			public Node Left { get; }
			public Node Right { get; }
			public T Data { get; }
			public bool Started { get; set; }
			public bool Finished { get; set; }

			public Node(Node left, Node right, T data, bool started = false, bool finished = false) {
				Left = left;
				Right = right;
				Data = data;
				Started = started;
				Finished = finished;
			}
		}
	}
}