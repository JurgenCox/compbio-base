using System;
using System.Collections.Generic;

namespace BaseLibS.Data{
	[Serializable]
	public class ThreadSafeDictionary<Tk, Tv>{
		private readonly Dictionary<Tk, Tv> hashSet;
		private readonly object locker = new object();
		public ThreadSafeDictionary() { hashSet = new Dictionary<Tk, Tv>(); }

		public void Add(Tk key, Tv value){
			lock (locker){
				if (!hashSet.ContainsKey(key)){
					hashSet.Add(key, value);
				}
			}
		}

		public bool Remove(Tk item){
			bool foundAndRemoved;
			lock (locker){
				foundAndRemoved = hashSet.Remove(item);
			}
			return foundAndRemoved;
		}

		public bool ContainsKey(Tk key){
			lock (locker){
				return hashSet.ContainsKey(key);
			}
		}

		public bool TryGetValue(Tk key, out Tv value){
			lock (locker){
				return hashSet.TryGetValue(key, out value);
			}
		}

		public Dictionary<Tk, Tv>.KeyCollection Keys{
			get{
				lock (locker){
					return hashSet.Keys;
				}
			}
		}

		public Tv this[Tk key]{
			get{
				lock (locker){
					return hashSet[key];
				}
			}
			set{
				lock (locker){
					hashSet[key] = value;
				}
			}
		}
	}
}