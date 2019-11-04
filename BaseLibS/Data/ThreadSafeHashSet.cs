using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibS.Data{
	[Serializable]
	public class ThreadSafeHashSet<T> : ISet<T>{
		private readonly HashSet<T> hashSet;
		private readonly object locker = new object();

		public ThreadSafeHashSet(){
			hashSet = new HashSet<T>();
		}

		public ThreadSafeHashSet(IEnumerable<T> collection){
			hashSet = new HashSet<T>(collection);
		}

		public IEnumerator<T> GetEnumerator(){
			HashSet<T> clone;
			lock (locker){
				clone = new HashSet<T>(hashSet);
			}
			return clone.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

		public bool Add(T item){
			bool elementWasAdded;
			lock (locker){
				elementWasAdded = hashSet.Add(item);
			}
			return elementWasAdded;
		}

		public bool Remove(T item){
			bool foundAndRemoved;
			lock (locker){
				foundAndRemoved = hashSet.Remove(item);
			}
			return foundAndRemoved;
		}

		public void ExceptWith(IEnumerable<T> other){
			lock (locker){
				hashSet.ExceptWith(other);
			}
		}

		public void IntersectWith(IEnumerable<T> other){
			lock (locker){
				hashSet.IntersectWith(other);
			}
		}

		public bool IsProperSubsetOf(IEnumerable<T> other){
			bool isProperSubsetOf = hashSet.IsProperSubsetOf(other);
			return isProperSubsetOf;
		}

		public bool IsProperSupersetOf(IEnumerable<T> other){
			bool isProperSupersetOf = hashSet.IsProperSupersetOf(other);
			return isProperSupersetOf;
		}

		public bool IsSubsetOf(IEnumerable<T> other){
			bool isSubsetOf = hashSet.IsSubsetOf(other);
			return isSubsetOf;
		}

		public bool IsSupersetOf(IEnumerable<T> other){
			bool isSupersetOf = hashSet.IsSupersetOf(other);
			return isSupersetOf;
		}

		public bool Overlaps(IEnumerable<T> other){
			bool overlaps = hashSet.Overlaps(other);
			return overlaps;
		}

		public bool SetEquals(IEnumerable<T> other){
			bool setsAreEqual = hashSet.Overlaps(other);
			return setsAreEqual;
		}

		public void SymmetricExceptWith(IEnumerable<T> other){
			lock (locker){
				hashSet.SymmetricExceptWith(other);
			}
		}

		public void UnionWith(IEnumerable<T> other){
			lock (locker){
				hashSet.UnionWith(other);
			}
		}

		void ICollection<T>.Add(T item){
			Add(item);
		}

		public void Clear(){
			lock (locker){
				hashSet.Clear();
			}
		}

		public bool Contains(T item){
			lock (locker){
				return hashSet.Contains(item);
			}
		}

		public void CopyTo(T[] array, int arrayIndex){
			int i = 0;
			foreach (T item in this){
				array[arrayIndex + i] = item;
				i++;
			}
		}

		public int Count => hashSet.Count;
		public bool IsReadOnly => ((ISet<T>) hashSet).IsReadOnly;

		public int RemoveWhere(Predicate<T> predicate){
			HashSet<T> toBeRemoved = new HashSet<T>();
			foreach (T item in this){ // GetEnumerator() is thread safe {
				if (predicate(item)){
					toBeRemoved.Add(item);
				}
			}
			int foundAndRemovedCount = 0;
			lock (locker){
				foreach (T item in toBeRemoved){
					bool foundAndRemoved = hashSet.Remove(item);
					if (foundAndRemoved){
						foundAndRemovedCount++;
					}
				}
			}
			return foundAndRemovedCount;
		}

		public T[] ToArray(){
			T[] result;
			lock (locker){
				result = hashSet.ToArray();
			}
			return result;
		}
	}
}