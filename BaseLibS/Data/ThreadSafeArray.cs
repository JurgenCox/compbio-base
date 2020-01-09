using System;

namespace BaseLibS.Data{
	[Serializable]
	public class ThreadSafeArray<T>{
		private readonly object locker = new object();

		public ThreadSafeArray(int len){
			ToArray = new T[len];
		}

		public T this[int i]{
			get{
				lock (locker){
					return ToArray[i];
				}
			}
			set{
				lock (locker){
					ToArray[i] = value;
				}
			}
		}

		public int Length => ToArray.Length;
		public T[] ToArray { get; }
	}
}