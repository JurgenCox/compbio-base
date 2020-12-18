using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BaseLibS.Num.NdArray{
	public class RowMajorIndexEnumerator : IEnumerator<int[]>{
		private readonly IEnumerator<int[]> _idxs;

		public RowMajorIndexEnumerator(int[] shape){
			_idxs = GetPerm(shape).GetEnumerator();
		}

		public void Dispose(){ }

		public bool MoveNext(){
			return _idxs.MoveNext();
		}

		public void Reset(){
			_idxs.Reset();
		}

		public int[] Current => _idxs.Current;
		object IEnumerator.Current => Current;

		public static IEnumerable<int[]> GetPerm(IList<int> n){
			if (n.Count == 1) return Enumerable.Range(0, n.Last()).Select(x => new[]{x});
			return GetPerm(n.Reverse().Skip(1).Reverse().ToList()).SelectMany(t => Enumerable.Range(0, n.Last()),
				(t1, t2) => t1.Concat(new[]{t2}).ToArray());
		}
	}

	public class RowMajorEnumerator<T> : IEnumerator<T>{
		private readonly NdArray<T> _array;
		private RowMajorIndexEnumerator _idxs;

		public RowMajorEnumerator(NdArray<T> array){
			_array = array;
			_idxs = new RowMajorIndexEnumerator(array.Shape);
		}

		public void Dispose(){ }

		public bool MoveNext(){
			return _idxs.MoveNext();
		}

		public void Reset(){
			_idxs.Reset();
		}

		object IEnumerator.Current => Current;
		public T Current => _array[_idxs.Current];
	}
}