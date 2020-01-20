using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibS.Num{
	[Serializable]
	public class LongList<T> : IEnumerable<T>, IEnumerator<T>{
		private const long maxLen = 1L << 24;
		private readonly List<List<T>> lists = new List<List<T>>();
		public long Count{ get; private set; }
		private long position = -1;

		public void Add(T elem){
			if (lists.Count == 0){
				lists.Add(new List<T>());
			}
			if (lists.Last().Count == maxLen){
				lists.Add(new List<T>());
			}
			lists.Last().Add(elem);
			Count++;
		}

		public void AddRange(T[] elems){
			foreach (T elem in elems){
				Add(elem);
			}
		}

		public IEnumerator<T> GetEnumerator(){
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

		public void Dispose(){
			Clear();
		}

		public void Clear(){
			foreach (List<T> list in lists){
				list.Clear();
			}
			lists.Clear();
			position = 0;
			Count = 0;
		}

		public bool MoveNext(){
			position++;
			return position < Count;
		}

		public void Reset(){
			position = -1;
		}

		public T Current => this[position];
		object IEnumerator.Current => Current;

		public T this[long ind]{
			get{
				if (ind < maxLen){
					return lists[0][(int) ind];
				}
				int bin = (int) (ind / maxLen);
				int i = (int) (ind - bin * maxLen);
				return lists[bin][i];
			}
			set{
				if (ind < maxLen){
					lists[0][(int) ind] = value;
				}
				int bin = (int) (ind / maxLen);
				int i = (int) (ind - bin * maxLen);
				lists[bin][i] = value;
			}
		}

		public static LongList<T> Fill<T>(IEnumerable<T> x){
			LongList<T> result = new LongList<T>();
			foreach (T t in x){
				result.Add(t);
			}
			return result;
		}
	}
}