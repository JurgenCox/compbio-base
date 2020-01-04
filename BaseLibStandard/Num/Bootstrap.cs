using System;

namespace BaseLibS.Num{
	public static class Bootstrap{
		private const int bootstrapBufferLen = 999;
		private const int maxBootstrapVectorLen = 99;
		public const int nBoots = 150;
		private static readonly int[,][] bootstrapBuffer = new int[bootstrapBufferLen,maxBootstrapVectorLen][];
		private static int count;
		public static Random random = new Random(7);

		static Bootstrap(){
			for (int i = 0; i < bootstrapBufferLen; i++){
				for (int j = 0; j < maxBootstrapVectorLen; j++){
					bootstrapBuffer[i, j] = new int[j];
					for (int k = 0; k < j; k++){
						bootstrapBuffer[i, j][k] = random.Next(j);
					}
				}
			}
		}

		public static int[] GetBootstrapIndices(int n){
			if (n < maxBootstrapVectorLen){
				count = (count + 1)%bootstrapBufferLen;
				return bootstrapBuffer[count, n];
			}
			int[] result = new int[n];
			for (int i = 0; i < n; i++){
				result[i] = random.Next(n);
			}
			return result;
		}
	}
}