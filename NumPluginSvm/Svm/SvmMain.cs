using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num.Vector;

namespace NumPluginSvm.Svm{
	public class SvmMain{
		public static Random rand = new Random(7);

		internal static void Info(string s){
			Console.Write(s);
		}

		private static void SolveCSvc(SvmProblem prob, SvmParameter param, double[] alpha, SvmSolver.SolutionInfo si,
			double cp, double cn){
			int l = prob.Count;
			double[] minusOnes = new double[l];
			short[] y = new short[l];
			int i;
			for (i = 0; i < l; i++){
				alpha[i] = 0;
				minusOnes[i] = -1;
				if (prob.y[i] > 0){
					y[i] = +1;
				} else{
					y[i] = -1;
				}
			}
			SvmSolver s = new SvmSolver();
			s.Solve(l, new SvcQ(prob, param, y), minusOnes, y, alpha, cp, cn, param.eps, si, param.shrinking);
			double sumAlpha = 0;
			for (i = 0; i < l; i++){
				sumAlpha += alpha[i];
			}
			if (cp == cn){
				Info("nu = " + sumAlpha/(cp*prob.Count) + "\n");
			}
			for (i = 0; i < l; i++){
				alpha[i] *= y[i];
			}
		}

		private static void SolveNuSvc(SvmProblem prob, SvmParameter param, double[] alpha, SvmSolver.SolutionInfo si){
			int i;
			int l = prob.Count;
			double nu = param.nu;
			short[] y = new short[l];
			for (i = 0; i < l; i++){
				if (prob.y[i] > 0){
					y[i] = +1;
				} else{
					y[i] = -1;
				}
			}
			double sumPos = nu*l/2;
			double sumNeg = nu*l/2;
			for (i = 0; i < l; i++){
				if (y[i] == +1){
					alpha[i] = Math.Min(1.0, sumPos);
					sumPos -= alpha[i];
				} else{
					alpha[i] = Math.Min(1.0, sumNeg);
					sumNeg -= alpha[i];
				}
			}
			double[] zeros = new double[l];
			for (i = 0; i < l; i++){
				zeros[i] = 0;
			}
			SvmSolverNu s = new SvmSolverNu();
			s.Solve(l, new SvcQ(prob, param, y), zeros, y, alpha, 1.0, 1.0, param.eps, si, param.shrinking);
			double r = si.r;
			Info("C = " + 1/r + "\n");
			for (i = 0; i < l; i++){
				alpha[i] *= y[i]/r;
			}
			si.rho /= r;
			si.obj /= (r*r);
			si.upperBoundP = 1/r;
			si.upperBoundN = 1/r;
		}

		private static void SolveOneClass(SvmProblem prob, SvmParameter param, double[] alpha, SvmSolver.SolutionInfo si){
			int l = prob.Count;
			double[] zeros = new double[l];
			short[] ones = new short[l];
			int i;
			int n = (int) (param.nu*prob.Count); // # of alpha's at upper bound
			for (i = 0; i < n; i++){
				alpha[i] = 1;
			}
			if (n < prob.Count){
				alpha[n] = param.nu*prob.Count - n;
			}
			for (i = n + 1; i < l; i++){
				alpha[i] = 0;
			}
			for (i = 0; i < l; i++){
				zeros[i] = 0;
				ones[i] = 1;
			}
			SvmSolver s = new SvmSolver();
			s.Solve(l, new OneClassQ(prob, param), zeros, ones, alpha, 1.0, 1.0, param.eps, si, param.shrinking);
		}

		private static void SolveEpsilonSvr(SvmProblem prob, SvmParameter param, IList<double> alpha,
			SvmSolver.SolutionInfo si){
			int l = prob.Count;
			double[] alpha2 = new double[2*l];
			double[] linearTerm = new double[2*l];
			short[] y = new short[2*l];
			int i;
			for (i = 0; i < l; i++){
				alpha2[i] = 0;
				linearTerm[i] = param.p - prob.y[i];
				y[i] = 1;
				alpha2[i + l] = 0;
				linearTerm[i + l] = param.p + prob.y[i];
				y[i + l] = -1;
			}
			SvmSolver s = new SvmSolver();
			s.Solve(2*l, new SvrQ(prob, param), linearTerm, y, alpha2, param.c, param.c, param.eps, si, param.shrinking);
			double sumAlpha = 0;
			for (i = 0; i < l; i++){
				alpha[i] = alpha2[i] - alpha2[i + l];
				sumAlpha += Math.Abs(alpha[i]);
			}
			Info("nu = " + sumAlpha/(param.c*l) + "\n");
		}

		private static void SolveNuSvr(SvmProblem prob, SvmParameter param, IList<double> alpha, SvmSolver.SolutionInfo si){
			int l = prob.Count;
			double c = param.c;
			double[] alpha2 = new double[2*l];
			double[] linearTerm = new double[2*l];
			short[] y = new short[2*l];
			int i;
			double sum = c*param.nu*l/2;
			for (i = 0; i < l; i++){
				alpha2[i] = alpha2[i + l] = Math.Min(sum, c);
				sum -= alpha2[i];
				linearTerm[i] = -prob.y[i];
				y[i] = 1;
				linearTerm[i + l] = prob.y[i];
				y[i + l] = -1;
			}
			SvmSolverNu s = new SvmSolverNu();
			s.Solve(2*l, new SvrQ(prob, param), linearTerm, y, alpha2, c, c, param.eps, si, param.shrinking);
			Info("epsilon = " + (-si.r) + "\n");
			for (i = 0; i < l; i++){
				alpha[i] = alpha2[i] - alpha2[i + l];
			}
		}

		//
		// decision_function
		//
		private class DecisionFunction{
			internal double[] alpha;
			internal double rho;
		};

		private static DecisionFunction SvmTrainOne(SvmProblem prob, SvmParameter param, double cp, double cn){
			double[] alpha = new double[prob.Count];
			SvmSolver.SolutionInfo si = new SvmSolver.SolutionInfo();
			switch (param.svmType){
				case SvmType.CSvc:
					SolveCSvc(prob, param, alpha, si, cp, cn);
					break;
				case SvmType.NuSvc:
					SolveNuSvc(prob, param, alpha, si);
					break;
				case SvmType.OneClass:
					SolveOneClass(prob, param, alpha, si);
					break;
				case SvmType.EpsilonSvr:
					SolveEpsilonSvr(prob, param, alpha, si);
					break;
				case SvmType.NuSvr:
					SolveNuSvr(prob, param, alpha, si);
					break;
			}
			Info("obj = " + si.obj + ", rho = " + si.rho + "\n");
			// output SVs
			int nSv = 0;
			int nBsv = 0;
			for (int i = 0; i < prob.Count; i++){
				if (Math.Abs(alpha[i]) > 0){
					++nSv;
					if (prob.y[i] > 0){
						if (Math.Abs(alpha[i]) >= si.upperBoundP){
							++nBsv;
						}
					} else{
						if (Math.Abs(alpha[i]) >= si.upperBoundN){
							++nBsv;
						}
					}
				}
			}
			Info("nSV = " + nSv + ", nBSV = " + nBsv + "\n");
			DecisionFunction f = new DecisionFunction{alpha = alpha, rho = si.rho};
			return f;
		}

		// Platt's binary SVM Probablistic Output: an improvement from Lin et al.
		private static void SigmoidTrain(int l, IList<double> decValues, IList<double> labels, IList<double> probAb){
			double prior1 = 0, prior0 = 0;
			int i;
			for (i = 0; i < l; i++){
				if (labels[i] > 0){
					prior1 += 1;
				} else{
					prior0 += 1;
				}
			}
			const int maxIter = 100; // Maximal number of iterations
			const double minStep = 1e-10; // Minimal step taken in line search
			const double sigma = 1e-12; // For numerically strict PD of Hessian
			const double eps = 1e-5;
			double hiTarget = (prior1 + 1.0)/(prior1 + 2.0);
			double loTarget = 1/(prior0 + 2.0);
			double[] t = new double[l];
			double fApB;
			int iter;
			// Initial Point and Initial Fun Value
			double a = 0.0;
			double b = Math.Log((prior0 + 1.0)/(prior1 + 1.0));
			double fval = 0.0;
			for (i = 0; i < l; i++){
				if (labels[i] > 0){
					t[i] = hiTarget;
				} else{
					t[i] = loTarget;
				}
				fApB = decValues[i]*a + b;
				if (fApB >= 0){
					fval += t[i]*fApB + Math.Log(1 + Math.Exp(-fApB));
				} else{
					fval += (t[i] - 1)*fApB + Math.Log(1 + Math.Exp(fApB));
				}
			}
			for (iter = 0; iter < maxIter; iter++){
				// Update Gradient and Hessian (use H' = H + sigma I)
				double h11 = sigma;
				double h22 = sigma;
				double h21 = 0.0;
				double g1 = 0.0;
				double g2 = 0.0;
				for (i = 0; i < l; i++){
					fApB = decValues[i]*a + b;
					double p;
					double q;
					if (fApB >= 0){
						p = Math.Exp(-fApB)/(1.0 + Math.Exp(-fApB));
						q = 1.0/(1.0 + Math.Exp(-fApB));
					} else{
						p = 1.0/(1.0 + Math.Exp(fApB));
						q = Math.Exp(fApB)/(1.0 + Math.Exp(fApB));
					}
					double d2 = p*q;
					h11 += decValues[i]*decValues[i]*d2;
					h22 += d2;
					h21 += decValues[i]*d2;
					double d1 = t[i] - p;
					g1 += decValues[i]*d1;
					g2 += d1;
				}
				// Stopping Criteria
				if (Math.Abs(g1) < eps && Math.Abs(g2) < eps){
					break;
				}
				// Finding Newton direction: -inv(H') * g
				double det = h11*h22 - h21*h21;
				double dA = -(h22*g1 - h21*g2)/det;
				double dB = -(-h21*g1 + h11*g2)/det;
				double gd = g1*dA + g2*dB;
				double stepsize = 1;
				while (stepsize >= minStep){
					double newA = a + stepsize*dA;
					double newB = b + stepsize*dB;
					// New function value
					double newf = 0.0;
					for (i = 0; i < l; i++){
						fApB = decValues[i]*newA + newB;
						if (fApB >= 0){
							newf += t[i]*fApB + Math.Log(1 + Math.Exp(-fApB));
						} else{
							newf += (t[i] - 1)*fApB + Math.Log(1 + Math.Exp(fApB));
						}
					}
					// Check sufficient decrease
					if (newf < fval + 0.0001*stepsize*gd){
						a = newA;
						b = newB;
						fval = newf;
						break;
					}
					stepsize = stepsize/2.0;
				}
				if (stepsize < minStep){
					Info("Line search fails in two-class probability estimates\n");
					break;
				}
			}
			if (iter >= maxIter){
				Info("Reaching maximal iterations in two-class probability estimates\n");
			}
			probAb[0] = a;
			probAb[1] = b;
		}

		private static double SigmoidPredict(double decisionValue, double a, double b){
			double fApB = decisionValue*a + b;
			return fApB >= 0 ? Math.Exp(-fApB)/(1.0 + Math.Exp(-fApB)) : 1.0/(1 + Math.Exp(fApB));
		}

		// Method 2 from the multiclass_prob paper by Wu, Lin, and Weng
		private static void MulticlassProbability(int k, IList<double[]> r, IList<double> p){
			int t, j;
			int iter, maxIter = Math.Max(100, k);
			double[][] q = new double[k][];
			for (int i = 0; i < k; i++){
				q[i] = new double[k];
			}
			double[] qp = new double[k];
			double eps = 0.005/k;
			for (t = 0; t < k; t++){
				p[t] = 1.0/k; // Valid if k = 1
				q[t][t] = 0;
				for (j = 0; j < t; j++){
					q[t][t] += r[j][t]*r[j][t];
					q[t][j] = q[j][t];
				}
				for (j = t + 1; j < k; j++){
					q[t][t] += r[j][t]*r[j][t];
					q[t][j] = -r[j][t]*r[t][j];
				}
			}
			for (iter = 0; iter < maxIter; iter++){
				// stopping condition, recalculate QP,pQP for numerical accuracy
				double pQp = 0;
				for (t = 0; t < k; t++){
					qp[t] = 0;
					for (j = 0; j < k; j++){
						qp[t] += q[t][j]*p[j];
					}
					pQp += p[t]*qp[t];
				}
				double maxError = 0;
				for (t = 0; t < k; t++){
					double error = Math.Abs(qp[t] - pQp);
					if (error > maxError){
						maxError = error;
					}
				}
				if (maxError < eps){
					break;
				}
				for (t = 0; t < k; t++){
					double diff = (-qp[t] + pQp)/q[t][t];
					p[t] += diff;
					pQp = (pQp + diff*(diff*q[t][t] + 2*qp[t]))/(1 + diff)/(1 + diff);
					for (j = 0; j < k; j++){
						qp[j] = (qp[j] + diff*q[t][j])/(1 + diff);
						p[j] /= (1 + diff);
					}
				}
			}
			if (iter >= maxIter){
				Info("Exceeds max_iter in multiclass_prob\n");
			}
		}

		// Cross-validation decision values for probability estimates
		internal static void SvmBinarySvcProbability(SvmProblem prob, SvmParameter param, double cp, double cn,
			IList<double> probAb){
			int i;
			const int nrFold = 5;
			int[] perm = new int[prob.Count];
			double[] decValues = new double[prob.Count];
			// random shuffle
			for (i = 0; i < prob.Count; i++){
				perm[i] = i;
			}
			for (i = 0; i < prob.Count; i++){
				int j = i + rand.Next(prob.Count - i);
				do{
					int _ = perm[i];
					perm[i] = perm[j];
					perm[j] = _;
				} while (false);
			}
			for (i = 0; i < nrFold; i++){
				int begin = i*prob.Count/nrFold;
				int end = (i + 1)*prob.Count/nrFold;
				int j;
				int count = prob.Count - (end - begin);
				SvmProblem subprob = new SvmProblem{x = new BaseVector[count], y = new double[count]};
				int k = 0;
				for (j = 0; j < begin; j++){
					subprob.x[k] = prob.x[perm[j]];
					subprob.y[k] = prob.y[perm[j]];
					++k;
				}
				for (j = end; j < prob.Count; j++){
					subprob.x[k] = prob.x[perm[j]];
					subprob.y[k] = prob.y[perm[j]];
					++k;
				}
				int pCount = 0, nCount = 0;
				for (j = 0; j < k; j++){
					if (subprob.y[j] > 0){
						pCount++;
					} else{
						nCount++;
					}
				}
				if (pCount == 0 && nCount == 0){
					for (j = begin; j < end; j++){
						decValues[perm[j]] = 0;
					}
				} else if (pCount > 0 && nCount == 0){
					for (j = begin; j < end; j++){
						decValues[perm[j]] = 1;
					}
				} else if (pCount == 0 && nCount > 0){
					for (j = begin; j < end; j++){
						decValues[perm[j]] = -1;
					}
				} else{
					SvmParameter subparam = (SvmParameter) param.Clone();
					subparam.probability = false;
					subparam.c = 1.0;
					subparam.nrWeight = 2;
					subparam.weightLabel = new int[2];
					subparam.weight = new double[2];
					subparam.weightLabel[0] = +1;
					subparam.weightLabel[1] = -1;
					subparam.weight[0] = cp;
					subparam.weight[1] = cn;
					SvmModel submodel = SvmTrain(subprob, subparam);
					for (j = begin; j < end; j++){
						double[] decValue = new double[1];
						SvmPredictValues(submodel, prob.x[perm[j]], decValue);
						decValues[perm[j]] = decValue[0];
						// ensure +1 -1 order; reason not using CV subroutine
						decValues[perm[j]] *= submodel.label[0];
					}
				}
			}
			SigmoidTrain(prob.Count, decValues, prob.y, probAb);
		}

		// Return parameter of a Laplace distribution 
		internal static double SvmSvrProbability(SvmProblem prob, SvmParameter param){
			int i;
			const int nrFold = 5;
			double[] ymv = new double[prob.Count];
			double mae = 0;
			SvmParameter newparam = (SvmParameter) param.Clone();
			newparam.probability = false;
			SvmCrossValidation(prob, newparam, nrFold, ymv);
			for (i = 0; i < prob.Count; i++){
				ymv[i] = prob.y[i] - ymv[i];
				mae += Math.Abs(ymv[i]);
			}
			mae /= prob.Count;
			double std = Math.Sqrt(2*mae*mae);
			int count = 0;
			mae = 0;
			for (i = 0; i < prob.Count; i++){
				if (Math.Abs(ymv[i]) > 5*std){
					count = count + 1;
				} else{
					mae += Math.Abs(ymv[i]);
				}
			}
			mae /= (prob.Count - count);
			Info(
				"Prob. model for test data: target value = predicted value + z,\nz: Laplace distribution e^(-|z|/sigma)/(2sigma),sigma=" +
				mae + "\n");
			return mae;
		}

		// label: label name, start: begin of each class, count: #data of classes, perm: indices to the original data
		// perm, length l, must be allocated before calling this subroutine
		private static void SvmGroupClasses(SvmProblem prob, IList<int> nrClassRet, IList<int[]> labelRet,
			IList<int[]> startRet, IList<int[]> countRet, IList<int> perm){
			int l = prob.Count;
			int maxNrClass = 16;
			int nrClass = 0;
			int[] label = new int[maxNrClass];
			int[] count = new int[maxNrClass];
			int[] dataLabel = new int[l];
			int i;
			for (i = 0; i < l; i++){
				int thisLabel = (int) (prob.y[i]);
				int j;
				for (j = 0; j < nrClass; j++){
					if (thisLabel == label[j]){
						++count[j];
						break;
					}
				}
				dataLabel[i] = j;
				if (j == nrClass){
					if (nrClass == maxNrClass){
						maxNrClass *= 2;
						int[] newData = new int[maxNrClass];
						Array.Copy(label, 0, newData, 0, label.Length);
						label = newData;
						newData = new int[maxNrClass];
						Array.Copy(count, 0, newData, 0, count.Length);
						count = newData;
					}
					label[nrClass] = thisLabel;
					count[nrClass] = 1;
					++nrClass;
				}
			}
			int[] start = new int[nrClass];
			start[0] = 0;
			for (i = 1; i < nrClass; i++){
				start[i] = start[i - 1] + count[i - 1];
			}
			for (i = 0; i < l; i++){
				perm[start[dataLabel[i]]] = i;
				++start[dataLabel[i]];
			}
			start[0] = 0;
			for (i = 1; i < nrClass; i++){
				start[i] = start[i - 1] + count[i - 1];
			}
			nrClassRet[0] = nrClass;
			labelRet[0] = label;
			startRet[0] = start;
			countRet[0] = count;
		}

		//
		// Interface functions
		//
		public static SvmModel SvmTrain(SvmProblem prob, SvmParameter param){
			SvmModel model = new SvmModel{param = param};
			if (param.svmType == SvmType.OneClass || param.svmType == SvmType.EpsilonSvr || param.svmType == SvmType.NuSvr){
				// regression or one-class-svm
				model.nrClass = 2;
				model.label = null;
				model.nSv = null;
				model.probA = null;
				model.probB = null;
				model.svCoef = new double[1][];
				if (param.probability && (param.svmType == SvmType.EpsilonSvr || param.svmType == SvmType.NuSvr)){
					model.probA = new double[1];
					model.probA[0] = SvmSvrProbability(prob, param);
				}
				DecisionFunction f = SvmTrainOne(prob, param, 0, 0);
				model.rho = new double[1];
				model.rho[0] = f.rho;
				int nSv = 0;
				int i;
				for (i = 0; i < prob.Count; i++){
					if (Math.Abs(f.alpha[i]) > 0){
						++nSv;
					}
				}
				model.l = nSv;
				model.sv = new BaseVector[nSv];
				model.svCoef[0] = new double[nSv];
				int j = 0;
				for (i = 0; i < prob.Count; i++){
					if (Math.Abs(f.alpha[i]) > 0){
						model.sv[j] = prob.x[i];
						model.svCoef[0][j] = f.alpha[i];
						++j;
					}
				}
			} else{
				// classification
				int l = prob.Count;
				int[] tmpNrClass = new int[1];
				int[][] tmpLabel = new int[1][];
				int[][] tmpStart = new int[1][];
				int[][] tmpCount = new int[1][];
				int[] perm = new int[l];
				// group training data of the same class
				SvmGroupClasses(prob, tmpNrClass, tmpLabel, tmpStart, tmpCount, perm);
				int nrClass = tmpNrClass[0];
				int[] label = tmpLabel[0];
				int[] start = tmpStart[0];
				int[] count = tmpCount[0];
				if (nrClass == 1){
					Info("WARNING: training data in only one class. See README for details.\n");
				}
				BaseVector[] x = new BaseVector[l];
				int i;
				for (i = 0; i < l; i++){
					x[i] = prob.x[perm[i]];
				}
				// calculate weighted C
				double[] weightedC = new double[nrClass];
				for (i = 0; i < nrClass; i++){
					weightedC[i] = param.c;
				}
				for (i = 0; i < param.nrWeight; i++){
					int j;
					for (j = 0; j < nrClass; j++){
						if (param.weightLabel[i] == label[j]){
							break;
						}
					}
					if (j == nrClass){
						Info("WARNING: class label " + param.weightLabel[i] + " specified in weight is not found\n");
					} else{
						weightedC[j] *= param.weight[i];
					}
				}
				// train k*(k-1)/2 models
				bool[] nonzero = new bool[l];
				for (i = 0; i < l; i++){
					nonzero[i] = false;
				}
				DecisionFunction[] f = new DecisionFunction[nrClass*(nrClass - 1)/2];
				double[] probA = null, probB = null;
				if (param.probability){
					probA = new double[nrClass*(nrClass - 1)/2];
					probB = new double[nrClass*(nrClass - 1)/2];
				}
				int p = 0;
				for (i = 0; i < nrClass; i++){
					for (int j = i + 1; j < nrClass; j++){
						int si = start[i], sj = start[j];
						int ci = count[i], cj = count[j];
						int c = ci + cj;
						SvmProblem subProb = new SvmProblem{x = new BaseVector[c], y = new double[c]};
						int k;
						for (k = 0; k < ci; k++){
							subProb.x[k] = x[si + k];
							subProb.y[k] = +1;
						}
						for (k = 0; k < cj; k++){
							subProb.x[ci + k] = x[sj + k];
							subProb.y[ci + k] = -1;
						}
						if (param.probability){
							double[] probAb = new double[2];
							SvmBinarySvcProbability(subProb, param, weightedC[i], weightedC[j], probAb);
							probA[p] = probAb[0];
							probB[p] = probAb[1];
						}
						f[p] = SvmTrainOne(subProb, param, weightedC[i], weightedC[j]);
						for (k = 0; k < ci; k++){
							if (!nonzero[si + k] && Math.Abs(f[p].alpha[k]) > 0){
								nonzero[si + k] = true;
							}
						}
						for (k = 0; k < cj; k++){
							if (!nonzero[sj + k] && Math.Abs(f[p].alpha[ci + k]) > 0){
								nonzero[sj + k] = true;
							}
						}
						++p;
					}
				}
				// build output
				model.nrClass = nrClass;
				model.label = new int[nrClass];
				for (i = 0; i < nrClass; i++){
					model.label[i] = label[i];
				}
				model.rho = new double[nrClass*(nrClass - 1)/2];
				for (i = 0; i < nrClass*(nrClass - 1)/2; i++){
					model.rho[i] = f[i].rho;
				}
				if (param.probability){
					model.probA = new double[nrClass*(nrClass - 1)/2];
					model.probB = new double[nrClass*(nrClass - 1)/2];
					for (i = 0; i < nrClass*(nrClass - 1)/2; i++){
						model.probA[i] = probA[i];
						model.probB[i] = probB[i];
					}
				} else{
					model.probA = null;
					model.probB = null;
				}
				int nnz = 0;
				int[] nzCount = new int[nrClass];
				model.nSv = new int[nrClass];
				for (i = 0; i < nrClass; i++){
					int nSv = 0;
					for (int j = 0; j < count[i]; j++){
						if (nonzero[start[i] + j]){
							++nSv;
							++nnz;
						}
					}
					model.nSv[i] = nSv;
					nzCount[i] = nSv;
				}
				Info("Total nSV = " + nnz + "\n");
				model.l = nnz;
				model.sv = new BaseVector[nnz];
				p = 0;
				for (i = 0; i < l; i++){
					if (nonzero[i]){
						model.sv[p++] = x[i];
					}
				}
				int[] nzStart = new int[nrClass];
				nzStart[0] = 0;
				for (i = 1; i < nrClass; i++){
					nzStart[i] = nzStart[i - 1] + nzCount[i - 1];
				}
				model.svCoef = new double[nrClass - 1][];
				for (i = 0; i < nrClass - 1; i++){
					model.svCoef[i] = new double[nnz];
				}
				p = 0;
				for (i = 0; i < nrClass; i++){
					for (int j = i + 1; j < nrClass; j++){
						// classifier (i,j): coefficients with
						// i are in sv_coef[j-1][nz_start[i]...],
						// j are in sv_coef[i][nz_start[j]...]
						int si = start[i];
						int sj = start[j];
						int ci = count[i];
						int cj = count[j];
						int q = nzStart[i];
						int k;
						for (k = 0; k < ci; k++){
							if (nonzero[si + k]){
								model.svCoef[j - 1][q++] = f[p].alpha[k];
							}
						}
						q = nzStart[j];
						for (k = 0; k < cj; k++){
							if (nonzero[sj + k]){
								model.svCoef[i][q++] = f[p].alpha[ci + k];
							}
						}
						++p;
					}
				}
			}
			return model;
		}

		// Stratified cross validation
		public static void SvmCrossValidation(SvmProblem prob, SvmParameter param, int nrFold, double[] target){
			int i;
			int[] foldStart = new int[nrFold + 1];
			int l = prob.Count;
			int[] perm = new int[l];
			// stratified cv may not give leave-one-out rate
			// Each class to l folds -> some folds may have zero elements
			if ((param.svmType == SvmType.CSvc || param.svmType == SvmType.NuSvc) && nrFold < l){
				int[] tmpNrClass = new int[1];
				int[][] tmpLabel = new int[1][];
				int[][] tmpStart = new int[1][];
				int[][] tmpCount = new int[1][];
				SvmGroupClasses(prob, tmpNrClass, tmpLabel, tmpStart, tmpCount, perm);
				int nrClass = tmpNrClass[0];
				int[] start = tmpStart[0];
				int[] count = tmpCount[0];
				// random shuffle and then data grouped by fold using the array perm
				int[] foldCount = new int[nrFold];
				int[] index = new int[l];
				for (i = 0; i < l; i++){
					index[i] = perm[i];
				}
				for (int c = 0; c < nrClass; c++){
					for (i = 0; i < count[c]; i++){
						int j = i + rand.Next(count[c] - i);
						do{
							int _ = index[start[c] + j];
							index[start[c] + j] = index[start[c] + i];
							index[start[c] + i] = _;
						} while (false);
					}
				}
				for (i = 0; i < nrFold; i++){
					foldCount[i] = 0;
					for (int c = 0; c < nrClass; c++){
						foldCount[i] += (i + 1)*count[c]/nrFold - i*count[c]/nrFold;
					}
				}
				foldStart[0] = 0;
				for (i = 1; i <= nrFold; i++){
					foldStart[i] = foldStart[i - 1] + foldCount[i - 1];
				}
				for (int c = 0; c < nrClass; c++){
					for (i = 0; i < nrFold; i++){
						int begin = start[c] + i*count[c]/nrFold;
						int end = start[c] + (i + 1)*count[c]/nrFold;
						for (int j = begin; j < end; j++){
							perm[foldStart[i]] = index[j];
							foldStart[i]++;
						}
					}
				}
				foldStart[0] = 0;
				for (i = 1; i <= nrFold; i++){
					foldStart[i] = foldStart[i - 1] + foldCount[i - 1];
				}
			} else{
				for (i = 0; i < l; i++){
					perm[i] = i;
				}
				for (i = 0; i < l; i++){
					int j = i + rand.Next(l - i);
					do{
						int _ = perm[i];
						perm[i] = perm[j];
						perm[j] = _;
					} while (false);
				}
				for (i = 0; i <= nrFold; i++){
					foldStart[i] = i*l/nrFold;
				}
			}
			for (i = 0; i < nrFold; i++){
				int begin = foldStart[i];
				int end = foldStart[i + 1];
				int j;
				int count = l - (end - begin);
				SvmProblem subprob = new SvmProblem{x = new BaseVector[count], y = new double[count]};
				int k = 0;
				for (j = 0; j < begin; j++){
					subprob.x[k] = prob.x[perm[j]];
					subprob.y[k] = prob.y[perm[j]];
					++k;
				}
				for (j = end; j < l; j++){
					subprob.x[k] = prob.x[perm[j]];
					subprob.y[k] = prob.y[perm[j]];
					++k;
				}
				SvmModel submodel = SvmTrain(subprob, param);
				if (param.probability && (param.svmType == SvmType.CSvc || param.svmType == SvmType.NuSvc)){
					double[] probEstimates = new double[submodel.nrClass];
					for (j = begin; j < end; j++){
						target[perm[j]] = SvmPredictProbability(submodel, prob.x[perm[j]], probEstimates);
					}
				} else{
					for (j = begin; j < end; j++){
						target[perm[j]] = SvmPredict(submodel, prob.x[perm[j]]);
					}
				}
			}
		}

		public static void SvmGetLabels(SvmModel model, int[] label){
			if (model.label != null){
				for (int i = 0; i < model.nrClass; i++){
					label[i] = model.label[i];
				}
			}
		}

		public static double SvmGetSvrProbability(SvmModel model){
			if ((model.param.svmType == SvmType.EpsilonSvr || model.param.svmType == SvmType.NuSvr) && model.probA != null){
				return model.probA[0];
			}
			Info("Model doesn't contain information for SVR probability inference\n");
			return 0;
		}

		public static double SvmPredictValues(SvmModel model, BaseVector x, double[] decValues){
			if (model.l == 0){
				return double.NaN;
			}
			if (model.param.svmType == SvmType.OneClass || model.param.svmType == SvmType.EpsilonSvr ||
				model.param.svmType == SvmType.NuSvr){
				double[] svCoef = model.svCoef[0];
				double sum = 0;
				for (int i = 0; i < model.l; i++){
					sum += svCoef[i]*KFunction(x, model.sv[i], model.param);
				}
				sum -= model.rho[0];
				decValues[0] = sum;
				if (model.param.svmType == SvmType.OneClass){
					return sum > 0 ? 1 : -1;
				}
				return sum;
			}
			int nrClass = model.nrClass;
			int l = model.l;
			double[] kvalue = new double[l];
			for (int i = 0; i < l; i++){
				kvalue[i] = KFunction(x, model.sv[i], model.param);
			}
			int[] start = new int[nrClass];
			start[0] = 0;
			for (int i = 1; i < nrClass; i++){
				start[i] = start[i - 1] + model.nSv[i - 1];
			}
			int[] vote = new int[nrClass];
			for (int i = 0; i < nrClass; i++){
				vote[i] = 0;
			}
			int p = 0;
			for (int i = 0; i < nrClass; i++){
				for (int j = i + 1; j < nrClass; j++){
					double sum = 0;
					int si = start[i];
					int sj = start[j];
					int ci = model.nSv[i];
					int cj = model.nSv[j];
					int k;
					double[] coef1 = model.svCoef[j - 1];
					double[] coef2 = model.svCoef[i];
					for (k = 0; k < ci; k++){
						sum += coef1[si + k]*kvalue[si + k];
					}
					for (k = 0; k < cj; k++){
						sum += coef2[sj + k]*kvalue[sj + k];
					}
					sum -= model.rho[p];
					decValues[p] = sum;
					if (decValues[p] > 0){
						++vote[i];
					} else{
						++vote[j];
					}
					p++;
				}
			}
			int voteMaxIdx = 0;
			for (int i = 1; i < nrClass; i++){
				if (vote[i] > vote[voteMaxIdx]){
					voteMaxIdx = i;
				}
			}
			return model.label[voteMaxIdx];
		}

		public static double KFunction(BaseVector x, BaseVector y, SvmParameter param){
			IKernelFunction kf = param.kernelFunction;
			double sx = double.NaN;
			double sy = double.NaN;
			if (kf.UsesSquares){
				sx = x.Dot(x);
				sy = y.Dot(y);
			}
			return kf.Evaluate(x, y, sx, sy);
		}

		public static double SvmPredict(SvmModel model, BaseVector x){
			int nrClass = model.nrClass;
			double[] decValues;
			if (model.param.svmType == SvmType.OneClass || model.param.svmType == SvmType.EpsilonSvr ||
				model.param.svmType == SvmType.NuSvr){
				decValues = new double[1];
			} else{
				decValues = new double[nrClass*(nrClass - 1)/2];
			}
			double predResult = SvmPredictValues(model, x, decValues);
			return predResult;
		}

		public static double SvmPredictProbability(SvmModel model, BaseVector x, double[] probEstimates){
			if ((model.param.svmType == SvmType.CSvc || model.param.svmType == SvmType.NuSvc) && model.probA != null &&
				model.probB != null){
				int nrClass = model.nrClass;
				double[] decValues = new double[nrClass*(nrClass - 1)/2];
				SvmPredictValues(model, x, decValues);
				const double minProb = 1e-7;
				double[][] pairwiseProb = new double[nrClass][];
				for (int m = 0; m < nrClass; m++){
					pairwiseProb[m] = new double[nrClass];
				}
				int k = 0;
				for (int i = 0; i < nrClass; i++){
					for (int j = i + 1; j < nrClass; j++){
						pairwiseProb[i][j] = Math.Min(Math.Max(SigmoidPredict(decValues[k], model.probA[k], model.probB[k]), minProb),
							1 - minProb);
						pairwiseProb[j][i] = 1 - pairwiseProb[i][j];
						k++;
					}
				}
				MulticlassProbability(nrClass, pairwiseProb, probEstimates);
				int probMaxIdx = 0;
				for (int i = 1; i < nrClass; i++){
					if (probEstimates[i] > probEstimates[probMaxIdx]){
						probMaxIdx = i;
					}
				}
				return model.label[probMaxIdx];
			}
			return SvmPredict(model, x);
		}

		public static string SvmCheckParameter(SvmProblem prob, SvmParameter param){
			SvmType svmType = param.svmType;
			// cache_size,eps,C,nu,p,shrinking
			if (param.cacheSize <= 0){
				return "cache_size <= 0";
			}
			if (param.eps <= 0){
				return "eps <= 0";
			}
			if (svmType == SvmType.CSvc || svmType == SvmType.EpsilonSvr || svmType == SvmType.NuSvr){
				if (param.c <= 0){
					return "C <= 0";
				}
			}
			if (svmType == SvmType.NuSvc || svmType == SvmType.OneClass || svmType == SvmType.NuSvr){
				if (param.nu <= 0 || param.nu > 1){
					return "nu <= 0 or nu > 1";
				}
			}
			if (svmType == SvmType.EpsilonSvr){
				if (param.p < 0){
					return "p < 0";
				}
			}
			if (param.probability && svmType == SvmType.OneClass){
				return "one-class SVM probability output not supported yet";
			}
			// check whether nu-svc is feasible
			if (svmType == SvmType.NuSvc){
				int l = prob.Count;
				int maxNrClass = 16;
				int nrClass = 0;
				int[] label = new int[maxNrClass];
				int[] count = new int[maxNrClass];
				int i;
				for (i = 0; i < l; i++){
					int thisLabel = (int) prob.y[i];
					int j;
					for (j = 0; j < nrClass; j++){
						if (thisLabel == label[j]){
							++count[j];
							break;
						}
					}
					if (j == nrClass){
						if (nrClass == maxNrClass){
							maxNrClass *= 2;
							int[] newData = new int[maxNrClass];
							Array.Copy(label, 0, newData, 0, label.Length);
							label = newData;
							newData = new int[maxNrClass];
							Array.Copy(count, 0, newData, 0, count.Length);
							count = newData;
						}
						label[nrClass] = thisLabel;
						count[nrClass] = 1;
						++nrClass;
					}
				}
				for (i = 0; i < nrClass; i++){
					int n1 = count[i];
					for (int j = i + 1; j < nrClass; j++){
						int n2 = count[j];
						if (param.nu*(n1 + n2)/2 > Math.Min(n1, n2)){
							return "specified nu is infeasible";
						}
					}
				}
			}
			return null;
		}

		public static int SvmCheckProbabilityModel(SvmModel model){
			if (((model.param.svmType == SvmType.CSvc || model.param.svmType == SvmType.NuSvc) && model.probA != null &&
				model.probB != null) ||
				((model.param.svmType == SvmType.EpsilonSvr || model.param.svmType == SvmType.NuSvr) && model.probA != null)){
				return 1;
			}
			return 0;
		}
	}
}