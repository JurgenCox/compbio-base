using System;
using BaseLibS.Num;

namespace BaseLibS.Data{
	[Serializable]
	public class EquatableArray<T>{
		public T[] Array { get; }

		public EquatableArray(T[] array){
			Array = array;
		}

		public override bool Equals(object obj){
			if (obj is EquatableArray<T>){
				return Equals((EquatableArray<T>) obj);
			}
			return false;
		}

		public bool Equals(EquatableArray<T> other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			return ReferenceEquals(this, other) || ArrayUtils.EqualArrays(other.Array, Array);
		}

		public override int GetHashCode(){
			return (Array != null ? ArrayUtils.GetArrayHashCode(Array) : 0);
		}
	}
}