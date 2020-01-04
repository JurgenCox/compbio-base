using System.Collections.Generic;
using BaseLibS.Num;

namespace BaseLibS.Util{
	public class ComparableList<T>{
		private readonly IList<T> list;

		public ComparableList(IList<T> list){
			this.list = list;
		}

		public override bool Equals(object obj){
			if (!(obj is ComparableList<T>)){
				return false;
			}
			return Equals((ComparableList<T>) obj);
		}

		public bool Equals(ComparableList<T> other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			return ReferenceEquals(this, other) || ArrayUtils.EqualArrays(other.list, list);
		}

		public override int GetHashCode(){
			int result = 0;
			foreach (T t in list){
				result += 29*result + t.GetHashCode();
			}
			return result;
		}

		public T[] GetArray(){
			T[] result = new T[list.Count];
			list.CopyTo(result, 0);
			return result;
		}

		public IList<T> GetList(){
			return list;
		}
	}
}