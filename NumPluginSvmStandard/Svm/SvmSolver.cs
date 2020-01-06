using System;
using System.Collections.Generic;

namespace NumPluginSvm.Svm{
	// An SMO algorithm in Fan et al., JMLR 6(2005), p. 1889--1918
	// Solves:
	//
	//	min 0.5(\alpha^T Q \alpha) + p^T \alpha
	//
	//		y^T \alpha = \delta
	//		y_i = +1 or -1
	//		0 <= alpha_i <= Cp for y_i = 1
	//		0 <= alpha_i <= Cn for y_i = -1
	//
	// Given:
	//
	//	Q, p, y, Cp, Cn, and an initial feasible point \alpha
	//	l is the size of vectors and matrices
	//	eps is the stopping tolerance
	//
	// solution will be put in \alpha, objective value will be put in obj
	//
	internal class SvmSolver{
		internal int activeSize;
		internal short[] y;
		internal double[] g; // gradient of objective function
		private const byte lowerBound = 0;
		private const byte upperBound = 1;
		private const byte free = 2;
		private byte[] alphaStatus; // LOWER_BOUND, UPPER_BOUND, FREE
		private double[] alpha;
		internal SvmMatrix q;
		internal double[] qd;
		internal double eps;
		private double cp, cn;
		private double[] p;
		private int[] activeSet;
		private double[] gBar; // gradient, if we treat free variables as 0
		internal int l;
		internal bool unshrink; // XXX

		private double GetC(int i){
			return (y[i] > 0) ? cp : cn;
		}

		private void UpdateAlphaStatus(int i){
			if (alpha[i] >= GetC(i)){
				alphaStatus[i] = upperBound;
			} else if (alpha[i] <= 0){
				alphaStatus[i] = lowerBound;
			} else{
				alphaStatus[i] = free;
			}
		}

		internal bool IsUpperBound(int i){
			return alphaStatus[i] == upperBound;
		}

		internal bool IsLowerBound(int i){
			return alphaStatus[i] == lowerBound;
		}

		private bool IsFree(int i){
			return alphaStatus[i] == free;
		}

		// java: information about solution except alpha,
		// because we cannot return multiple values otherwise...
		internal class SolutionInfo{
			internal double obj;
			internal double rho;
			internal double upperBoundP;
			internal double upperBoundN;
			internal double r; // for Solver_NU
		}

		internal void SwapIndex(int i, int j){
			q.SwapIndex(i, j);
			do{
				short tmp = y[i];
				y[i] = y[j];
				y[j] = tmp;
			} while (false);
			do{
				double tmp = g[i];
				g[i] = g[j];
				g[j] = tmp;
			} while (false);
			do{
				byte tmp = alphaStatus[i];
				alphaStatus[i] = alphaStatus[j];
				alphaStatus[j] = tmp;
			} while (false);
			do{
				double tmp = alpha[i];
				alpha[i] = alpha[j];
				alpha[j] = tmp;
			} while (false);
			do{
				double tmp = p[i];
				p[i] = p[j];
				p[j] = tmp;
			} while (false);
			do{
				int tmp = activeSet[i];
				activeSet[i] = activeSet[j];
				activeSet[j] = tmp;
			} while (false);
			do{
				double tmp = gBar[i];
				gBar[i] = gBar[j];
				gBar[j] = tmp;
			} while (false);
		}

		internal void ReconstructGradient(){
			// reconstruct inactive elements of G from G_bar and free variables
			if (activeSize == l){
				return;
			}
			int nrFree = 0;
			for (int j = activeSize; j < l; j++){
				g[j] = gBar[j] + p[j];
			}
			for (int j = 0; j < activeSize; j++){
				if (IsFree(j)){
					nrFree++;
				}
			}
			if (2*nrFree < activeSize){
				SvmMain.Info("\nWARNING: using -h 0 may be faster\n");
			}
			if (nrFree*l > 2*activeSize*(l - activeSize)){
				for (int i = activeSize; i < l; i++){
					float[] qI = q.GetQ(i, activeSize);
					for (int j = 0; j < activeSize; j++){
						if (IsFree(j)){
							g[i] += alpha[j]*qI[j];
						}
					}
				}
			} else{
				for (int i = 0; i < activeSize; i++){
					if (IsFree(i)){
						float[] qI = q.GetQ(i, l);
						double alphaI = alpha[i];
						for (int j = activeSize; j < l; j++){
							g[j] += alphaI*qI[j];
						}
					}
				}
			}
		}

		internal virtual void Solve(int l1, SvmMatrix q1, double[] p1, short[] y1, double[] alpha1, double cp1, double cn1,
			double eps1, SolutionInfo si, bool shrinking){
			l = l1;
			q = q1;
			qd = q1.GetQd();
			p = (double[]) p1.Clone();
			y = (short[]) y1.Clone();
			alpha = (double[]) alpha1.Clone();
			cp = cp1;
			cn = cn1;
			eps = eps1;
			unshrink = false;
			// initialize alpha_status
				{
					alphaStatus = new byte[l1];
					for (int i = 0; i < l1; i++){
						UpdateAlphaStatus(i);
					}
				}
			// initialize active set (for shrinking)
				{
					activeSet = new int[l1];
					for (int i = 0; i < l1; i++){
						activeSet[i] = i;
					}
					activeSize = l1;
				}
			// initialize gradient
				{
					g = new double[l1];
					gBar = new double[l1];
					int i;
					for (i = 0; i < l1; i++){
						g[i] = p[i];
						gBar[i] = 0;
					}
					for (i = 0; i < l1; i++){
						if (!IsLowerBound(i)){
							float[] qI = q1.GetQ(i, l1);
							double alphaI = alpha[i];
							int j;
							for (j = 0; j < l1; j++){
								g[j] += alphaI*qI[j];
							}
							if (IsUpperBound(i)){
								for (j = 0; j < l1; j++){
									gBar[j] += GetC(i)*qI[j];
								}
							}
						}
					}
				}
			// optimization step
			int iter = 0;
			int maxIter = Math.Max(10000000, l1 > int.MaxValue/100 ? int.MaxValue : 100*l1);
			int counter = Math.Min(l1, 1000) + 1;
			int[] workingSet = new int[2];
			while (iter < maxIter){
				// show progress and do shrinking
				if (--counter == 0){
					counter = Math.Min(l1, 1000);
					if (shrinking){
						DoShrinking();
					}
					SvmMain.Info(".");
				}
				if (SelectWorkingSet(workingSet) != 0){
					// reconstruct the whole gradient
					ReconstructGradient();
					// reset active set size and check
					activeSize = l1;
					SvmMain.Info("*");
					if (SelectWorkingSet(workingSet) != 0){
						break;
					}
					counter = 1; // do shrinking next iteration
				}
				int i = workingSet[0];
				int j = workingSet[1];
				++iter;
				// update alpha[i] and alpha[j], handle bounds carefully
				float[] qI = q1.GetQ(i, activeSize);
				float[] qJ = q1.GetQ(j, activeSize);
				double cI = GetC(i);
				double cJ = GetC(j);
				double oldAlphaI = alpha[i];
				double oldAlphaJ = alpha[j];
				if (y[i] != y[j]){
					double quadCoef = qd[i] + qd[j] + 2*qI[j];
					if (quadCoef <= 0){
						quadCoef = 1e-12;
					}
					double delta = (-g[i] - g[j])/quadCoef;
					double diff = alpha[i] - alpha[j];
					alpha[i] += delta;
					alpha[j] += delta;
					if (diff > 0){
						if (alpha[j] < 0){
							alpha[j] = 0;
							alpha[i] = diff;
						}
					} else{
						if (alpha[i] < 0){
							alpha[i] = 0;
							alpha[j] = -diff;
						}
					}
					if (diff > cI - cJ){
						if (alpha[i] > cI){
							alpha[i] = cI;
							alpha[j] = cI - diff;
						}
					} else{
						if (alpha[j] > cJ){
							alpha[j] = cJ;
							alpha[i] = cJ + diff;
						}
					}
				} else{
					double quadCoef = qd[i] + qd[j] - 2*qI[j];
					if (quadCoef <= 0){
						quadCoef = 1e-12;
					}
					double delta = (g[i] - g[j])/quadCoef;
					double sum = alpha[i] + alpha[j];
					alpha[i] -= delta;
					alpha[j] += delta;
					if (sum > cI){
						if (alpha[i] > cI){
							alpha[i] = cI;
							alpha[j] = sum - cI;
						}
					} else{
						if (alpha[j] < 0){
							alpha[j] = 0;
							alpha[i] = sum;
						}
					}
					if (sum > cJ){
						if (alpha[j] > cJ){
							alpha[j] = cJ;
							alpha[i] = sum - cJ;
						}
					} else{
						if (alpha[i] < 0){
							alpha[i] = 0;
							alpha[j] = sum;
						}
					}
				}
				// update G
				double deltaAlphaI = alpha[i] - oldAlphaI;
				double deltaAlphaJ = alpha[j] - oldAlphaJ;
				for (int k = 0; k < activeSize; k++){
					g[k] += qI[k]*deltaAlphaI + qJ[k]*deltaAlphaJ;
				}
				// update alpha_status and G_bar
				{
					bool ui = IsUpperBound(i);
					bool uj = IsUpperBound(j);
					UpdateAlphaStatus(i);
					UpdateAlphaStatus(j);
					int k;
					if (ui != IsUpperBound(i)){
						qI = q1.GetQ(i, l1);
						if (ui){
							for (k = 0; k < l1; k++){
								gBar[k] -= cI*qI[k];
							}
						} else{
							for (k = 0; k < l1; k++){
								gBar[k] += cI*qI[k];
							}
						}
					}
					if (uj != IsUpperBound(j)){
						qJ = q1.GetQ(j, l1);
						if (uj){
							for (k = 0; k < l1; k++){
								gBar[k] -= cJ*qJ[k];
							}
						} else{
							for (k = 0; k < l1; k++){
								gBar[k] += cJ*qJ[k];
							}
						}
					}
				}
			}
			if (iter >= maxIter){
				if (activeSize < l1){
					// reconstruct the whole gradient to calculate objective value
					ReconstructGradient();
					activeSize = l1;
					SvmMain.Info("*");
				}
				SvmMain.Info("\nWARNING: reaching max number of iterations");
			}
			// calculate rho
			si.rho = CalculateRho();
			// calculate objective value
				{
					double v = 0;
					int i;
					for (i = 0; i < l1; i++){
						v += alpha[i]*(g[i] + p[i]);
					}
					si.obj = v/2;
				}
			// put back the solution
				{
					for (int i = 0; i < l1; i++){
						alpha1[activeSet[i]] = alpha[i];
					}
				}
			si.upperBoundP = cp1;
			si.upperBoundN = cn1;
			SvmMain.Info("\noptimization finished, #iter = " + iter + "\n");
		}

		// return 1 if already optimal, return 0 otherwise
		internal virtual int SelectWorkingSet(IList<int> workingSet){
			// return i,j such that
			// i: maximizes -y_i * grad(f)_i, i in I_up(\alpha)
			// j: mimimizes the decrease of obj value
			//    (if quadratic coefficeint <= 0, replace it with tau)
			//    -y_j*grad(f)_j < -y_i*grad(f)_i, j in I_low(\alpha)
			double gmax = double.NegativeInfinity;
			double gmax2 = double.NegativeInfinity;
			int gmaxIdx = -1;
			int gminIdx = -1;
			double objDiffMin = double.PositiveInfinity;
			for (int t = 0; t < activeSize; t++){
				if (y[t] == +1){
					if (!IsUpperBound(t)){
						if (-g[t] >= gmax){
							gmax = -g[t];
							gmaxIdx = t;
						}
					}
				} else{
					if (!IsLowerBound(t)){
						if (g[t] >= gmax){
							gmax = g[t];
							gmaxIdx = t;
						}
					}
				}
			}
			int i = gmaxIdx;
			float[] qI = null;
			if (i != -1) // null Q_i not accessed: Gmax=-INF if i=-1
			{
				qI = q.GetQ(i, activeSize);
			}
			for (int j = 0; j < activeSize; j++){
				if (y[j] == +1){
					if (!IsLowerBound(j)){
						double gradDiff = gmax + g[j];
						if (g[j] >= gmax2){
							gmax2 = g[j];
						}
						if (gradDiff > 0){
							double objDiff;
							double quadCoef = qd[i] + qd[j] - 2.0*y[i]*qI[j];
							if (quadCoef > 0){
								objDiff = -(gradDiff*gradDiff)/quadCoef;
							} else{
								objDiff = -(gradDiff*gradDiff)/1e-12;
							}
							if (objDiff <= objDiffMin){
								gminIdx = j;
								objDiffMin = objDiff;
							}
						}
					}
				} else{
					if (!IsUpperBound(j)){
						double gradDiff = gmax - g[j];
						if (-g[j] >= gmax2){
							gmax2 = -g[j];
						}
						if (gradDiff > 0){
							double objDiff;
							double quadCoef = qd[i] + qd[j] + 2.0*y[i]*qI[j];
							if (quadCoef > 0){
								objDiff = -(gradDiff*gradDiff)/quadCoef;
							} else{
								objDiff = -(gradDiff*gradDiff)/1e-12;
							}
							if (objDiff <= objDiffMin){
								gminIdx = j;
								objDiffMin = objDiff;
							}
						}
					}
				}
			}
			if (gmax + gmax2 < eps){
				return 1;
			}
			workingSet[0] = gmaxIdx;
			workingSet[1] = gminIdx;
			return 0;
		}

		internal bool BeShrunk(int i, double gmax1, double gmax2){
			if (IsUpperBound(i)){
				return y[i] == +1 ? -g[i] > gmax1 : -g[i] > gmax2;
			}
			if (IsLowerBound(i)){
				return y[i] == +1 ? g[i] > gmax2 : g[i] > gmax1;
			}
			return (false);
		}

		internal virtual void DoShrinking(){
			int i;
			double gmax1 = double.NegativeInfinity; // max { -y_i * grad(f)_i | i in I_up(\alpha) }
			double gmax2 = double.NegativeInfinity; // max { y_i * grad(f)_i | i in I_low(\alpha) }
			// find maximal violating pair first
			for (i = 0; i < activeSize; i++){
				if (y[i] == +1){
					if (!IsUpperBound(i)){
						if (-g[i] >= gmax1){
							gmax1 = -g[i];
						}
					}
					if (!IsLowerBound(i)){
						if (g[i] >= gmax2){
							gmax2 = g[i];
						}
					}
				} else{
					if (!IsUpperBound(i)){
						if (-g[i] >= gmax2){
							gmax2 = -g[i];
						}
					}
					if (!IsLowerBound(i)){
						if (g[i] >= gmax1){
							gmax1 = g[i];
						}
					}
				}
			}
			if (unshrink == false && gmax1 + gmax2 <= eps*10){
				unshrink = true;
				ReconstructGradient();
				activeSize = l;
			}
			for (i = 0; i < activeSize; i++){
				if (BeShrunk(i, gmax1, gmax2)){
					activeSize--;
					while (activeSize > i){
						if (!BeShrunk(activeSize, gmax1, gmax2)){
							SwapIndex(i, activeSize);
							break;
						}
						activeSize--;
					}
				}
			}
		}

		internal virtual double CalculateRho(){
			double r;
			int nrFree = 0;
			double ub = double.PositiveInfinity, lb = double.NegativeInfinity, sumFree = 0;
			for (int i = 0; i < activeSize; i++){
				double yG = y[i]*g[i];
				if (IsLowerBound(i)){
					if (y[i] > 0){
						ub = Math.Min(ub, yG);
					} else{
						lb = Math.Max(lb, yG);
					}
				} else if (IsUpperBound(i)){
					if (y[i] < 0){
						ub = Math.Min(ub, yG);
					} else{
						lb = Math.Max(lb, yG);
					}
				} else{
					++nrFree;
					sumFree += yG;
				}
			}
			if (nrFree > 0){
				r = sumFree/nrFree;
			} else{
				r = (ub + lb)/2;
			}
			return r;
		}
	}
}