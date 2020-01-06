using System;
using System.Collections.Generic;

namespace NumPluginSvm.Svm{
    //
    // Solver for nu-svm classification and regression
    //
    // additional constraint: e^T \alpha = constant
    //
    internal class SvmSolverNu : SvmSolver{
        private SolutionInfo si;

        internal override void Solve(int l1, SvmMatrix q1, double[] p1, short[] y1, double[] alpha1, double cp1,
                                     double cn1, double eps1, SolutionInfo si1, bool shrinking){
            si = si1;
            base.Solve(l1, q1, p1, y1, alpha1, cp1, cn1, eps1, si1, shrinking);
        }

        // return 1 if already optimal, return 0 otherwise
        internal override int SelectWorkingSet(IList<int> workingSet){
            // return i,j such that y_i = y_j and
            // i: maximizes -y_i * grad(f)_i, i in I_up(\alpha)
            // j: minimizes the decrease of obj value
            //    (if quadratic coefficeint <= 0, replace it with tau)
            //    -y_j*grad(f)_j < -y_i*grad(f)_i, j in I_low(\alpha)
            double gmaxp = double.NegativeInfinity;
            double gmaxp2 = double.NegativeInfinity;
            int gmaxpIdx = -1;
            double gmaxn = double.NegativeInfinity;
            double gmaxn2 = double.NegativeInfinity;
            int gmaxnIdx = -1;
            int gminIdx = -1;
            double objDiffMin = double.PositiveInfinity;
            for (int t = 0; t < activeSize; t++){
                if (y[t] == +1){
                    if (!IsUpperBound(t)){
                        if (-g[t] >= gmaxp){
                            gmaxp = -g[t];
                            gmaxpIdx = t;
                        }
                    }
                } else{
                    if (!IsLowerBound(t)){
                        if (g[t] >= gmaxn){
                            gmaxn = g[t];
                            gmaxnIdx = t;
                        }
                    }
                }
            }
            int ip = gmaxpIdx;
            int in1 = gmaxnIdx;
            float[] qIp = null;
            float[] qIn = null;
            if (ip != -1) // null Q_ip not accessed: Gmaxp=-INF if ip=-1
            {
                qIp = q.GetQ(ip, activeSize);
            }
            if (in1 != -1){
                qIn = q.GetQ(in1, activeSize);
            }
            for (int j = 0; j < activeSize; j++){
                if (y[j] == +1){
                    if (!IsLowerBound(j)){
                        double gradDiff = gmaxp + g[j];
                        if (g[j] >= gmaxp2){
                            gmaxp2 = g[j];
                        }
                        if (gradDiff > 0){
                            double objDiff;
                            double quadCoef = qd[ip] + qd[j] - 2*qIp[j];
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
                        double gradDiff = gmaxn - g[j];
                        if (-g[j] >= gmaxn2){
                            gmaxn2 = -g[j];
                        }
                        if (gradDiff > 0){
                            double objDiff;
                            double quadCoef = qd[in1] + qd[j] - 2*qIn[j];
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
            if (Math.Max(gmaxp + gmaxp2, gmaxn + gmaxn2) < eps){
                return 1;
            }
            if (y[gminIdx] == +1){
                workingSet[0] = gmaxpIdx;
            } else{
                workingSet[0] = gmaxnIdx;
            }
            workingSet[1] = gminIdx;
            return 0;
        }

        private bool BeShrunk(int i, double gmax1, double gmax2, double gmax3, double gmax4){
            if (IsUpperBound(i)){
                return y[i] == +1 ? -g[i] > gmax1 : -g[i] > gmax4;
            }
            if (IsLowerBound(i)){
                return y[i] == +1 ? g[i] > gmax2 : g[i] > gmax3;
            }
            return false;
        }

        internal override void DoShrinking(){
            double gmax1 = double.NegativeInfinity; // max { -y_i * grad(f)_i | y_i = +1, i in I_up(\alpha) }
            double gmax2 = double.NegativeInfinity; // max { y_i * grad(f)_i | y_i = +1, i in I_low(\alpha) }
            double gmax3 = double.NegativeInfinity; // max { -y_i * grad(f)_i | y_i = -1, i in I_up(\alpha) }
            double gmax4 = double.NegativeInfinity; // max { y_i * grad(f)_i | y_i = -1, i in I_low(\alpha) }
            // find maximal violating pair first
            int i;
            for (i = 0; i < activeSize; i++){
                if (!IsUpperBound(i)){
                    if (y[i] == +1){
                        if (-g[i] > gmax1){
                            gmax1 = -g[i];
                        }
                    } else if (-g[i] > gmax4){
                        gmax4 = -g[i];
                    }
                }
                if (!IsLowerBound(i)){
                    if (y[i] == +1){
                        if (g[i] > gmax2){
                            gmax2 = g[i];
                        }
                    } else if (g[i] > gmax3){
                        gmax3 = g[i];
                    }
                }
            }
            if (unshrink == false && Math.Max(gmax1 + gmax2, gmax3 + gmax4) <= eps*10){
                unshrink = true;
                ReconstructGradient();
                activeSize = l;
            }
            for (i = 0; i < activeSize; i++){
                if (BeShrunk(i, gmax1, gmax2, gmax3, gmax4)){
                    activeSize--;
                    while (activeSize > i){
                        if (!BeShrunk(activeSize, gmax1, gmax2, gmax3, gmax4)){
                            SwapIndex(i, activeSize);
                            break;
                        }
                        activeSize--;
                    }
                }
            }
        }

        internal override double CalculateRho(){
            int nrFree1 = 0, nrFree2 = 0;
            double ub1 = double.PositiveInfinity, ub2 = double.PositiveInfinity;
            double lb1 = double.NegativeInfinity, lb2 = double.NegativeInfinity;
            double sumFree1 = 0, sumFree2 = 0;
            for (int i = 0; i < activeSize; i++){
                if (y[i] == +1){
                    if (IsLowerBound(i)){
                        ub1 = Math.Min(ub1, g[i]);
                    } else if (IsUpperBound(i)){
                        lb1 = Math.Max(lb1, g[i]);
                    } else{
                        ++nrFree1;
                        sumFree1 += g[i];
                    }
                } else{
                    if (IsLowerBound(i)){
                        ub2 = Math.Min(ub2, g[i]);
                    } else if (IsUpperBound(i)){
                        lb2 = Math.Max(lb2, g[i]);
                    } else{
                        ++nrFree2;
                        sumFree2 += g[i];
                    }
                }
            }
            double r1, r2;
            if (nrFree1 > 0){
                r1 = sumFree1/nrFree1;
            } else{
                r1 = (ub1 + lb1)/2;
            }
            if (nrFree2 > 0){
                r2 = sumFree2/nrFree2;
            } else{
                r2 = (ub2 + lb2)/2;
            }
            si.r = (r1 + r2)/2;
            return (r1 - r2)/2;
        }
    }
}