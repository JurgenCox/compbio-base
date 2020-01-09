using System;

namespace BaseLibS.Num{
	internal class Combination{
		private readonly int n;
		private readonly int k;

		internal Combination(int n, int k){
			if (n < 0 || k < 0){
				throw new ArgumentException("Negative parameter in constructor");
			}
			this.n = n;
			this.k = k;
			Data = new int[k];
			for (int i = 0; i < k; ++i){
				Data[i] = i;
			}
		}

		internal int[] Data { get; }

		internal Combination Successor{
			get{
				if (Data[0] == n - k){
					return null;
				}
				Combination ans = new Combination(n, k);
				long i;
				for (i = 0; i < k; ++i){
					ans.Data[i] = Data[i];
				}
				for (i = k - 1; i > 0 && ans.Data[i] == n - k + i; --i){}
				++ans.Data[i];
				for (long j = i; j < k - 1; ++j){
					ans.Data[j + 1] = ans.Data[j] + 1;
				}
				return ans;
			}
		}
	}
}