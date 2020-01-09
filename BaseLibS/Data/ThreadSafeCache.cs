using System;
using System.Collections.Generic;

namespace BaseLibS.Data{
	public class ThreadSafeCache<Tk, Tv> : IDisposable, ICache<Tk, Tv>{
		private readonly object locker = new object();
		private CacheElem<Tk, Tv> first;
		private CacheElem<Tk, Tv> last;
		private readonly Dictionary<Tk, CacheElem<Tk, Tv>> map = new Dictionary<Tk, CacheElem<Tk, Tv>>();
		public int MaxEntries { get; set; } = 1000*1000;

		public bool ContainsKey(Tk key){
			lock (locker){
				return map.ContainsKey(key);
			}
		}

		public int Count{
			get{
				lock (locker){
					return map.Count;
				}
			}
		}

		public void Add(Tk key, Tv value){
			lock (locker){
				if (map.ContainsKey(key)){
					throw new ArgumentException("The key " + key + " is already contained in the cache.");
				}
				CacheElem<Tk, Tv> e = new CacheElem<Tk, Tv>(key, value);
				if (map.Count == 0){
					first = e;
					last = e;
				} else{
					first.previous = e;
					e.next = first;
					first = e;
				}
				map.Add(key, e);
				while (Count > MaxEntries){
					RemoveLast();
				}
			}
		}

		public Tv this[Tk key]{
			get{
				lock (locker){
					if (!map.ContainsKey(key)){
						throw new ArgumentException("The key " + key + " is not contained in the cache.");
					}
					CacheElem<Tk, Tv> e = map[key];
					if (e == first){} else if (e == last){
						last.previous.next = null;
						last = last.previous;
						e.previous = null;
						e.next = first;
						first.previous = e;
						first = e;
					} else{
						e.previous.next = e.next;
						e.next.previous = e.previous;
						e.previous = null;
						e.next = first;
						first.previous = e;
						first = e;
					}
					return e.data;
				}
			}
		}

		public void Dispose(){
			Clear();
		}

		public void Clear(){
			lock (locker){
				foreach (KeyValuePair<Tk, CacheElem<Tk, Tv>> pair in map){
					CacheElem<Tk, Tv> e = pair.Value;
					e.previous = null;
					e.next = null;
					if (e.data is IDisposable){
						((IDisposable) e.data).Dispose();
					}
					e.data = default(Tv);
					if (pair.Key is IDisposable){
						((IDisposable) pair.Key).Dispose();
					}
				}
				map.Clear();
			}
		}

		private void RemoveLast(){
			Tk index = last.index;
			last.previous.next = null;
			last = last.previous;
			map[index].previous = null;
			map[index].next = null;
			if (map[index].data is IDisposable){
				((IDisposable) map[index].data).Dispose();
			}
			map[index].data = default(Tv);
			map.Remove(index);
		}
	}
}