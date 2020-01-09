using System;

namespace BaseLibS.Util{
	public class ThreadDistributorTriangularMatrix{
		private readonly ThreadDistributor td;

		public ThreadDistributorTriangularMatrix(int nThreads, int n, Action<int, int> calculation){
			int nTasks = n*(n - 1)/2;
			nThreads = Math.Min(nThreads, nTasks);
			td = new ThreadDistributor(nThreads, nTasks, i =>{
				int j;
				int k;
				GetIndices(i, out j, out k);
				calculation(j, k);
			});
		}

		private static void GetIndices(int i, out int j, out int k){
			j = (int) (0.5 + Math.Sqrt(0.25 + 2*i) + 1e-6);
			k = i - j*(j - 1)/2;
		}

		public void Abort(){
			td.Abort();
		}

		public void Start(){
			td.Start();
		}
	}
}