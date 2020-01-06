using System;
using BaseLibS.Num.Vector;

namespace NumPluginSvm.Svm{
	[Serializable]
	public class SvmModel{
		public SvmParameter param; // parameter
		public int nrClass; // number of classes, = 2 in regression/one class svm
		public int l; // total #SV
		public BaseVector[] sv; // SVs (SV[l])
		public double[][] svCoef; // coefficients for SVs in decision functions (sv_coef[k-1][l])
		public double[] rho; // constants in decision functions (rho[k*(k-1)/2])
		public double[] probA; // pariwise probability information
		public double[] probB;
		// for classification only
		public int[] label; // label of each class (label[k])
		public int[] nSv; // number of SVs for each class (nSV[k])
		// nSV[0] + nSV[1] + ... + nSV[k-1] = l
		public double[] ComputeBinaryClassifierWeights(int nFeatures){
			double[] weights = new double[nFeatures];
			const int nc = 2;
			int[] start = new int[nc];
			start[0] = 0;
			for (int i = 1; i < nc; i++){
				start[i] = start[i - 1] + nSv[i - 1];
			}
			for (int i = 0; i < nc; i++){
				for (int j = i + 1; j < nc; j++){
					int si = start[i];
					int sj = start[j];
					int ci = nSv[i];
					int cj = nSv[j];
					int k;
					double[] coef1 = svCoef[j - 1];
					double[] coef2 = svCoef[i];
					for (k = 0; k < ci; k++){
						for (int index = 0; index < sv[si + k].Length; index++){
							double tmp = coef1[si + k]*sv[si + k][index];
							weights[index] += tmp;
						}
					}
					for (k = 0; k < cj; k++){
						for (int index = 0; index < sv[sj + k].Length; index++){
							double tmp = coef2[sj + k]*sv[sj + k][index];
							weights[index] += tmp;
						}
					}
				}
			}
			return weights;
		}
	}
}