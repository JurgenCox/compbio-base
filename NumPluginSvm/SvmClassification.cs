using System;
using System.Collections.Generic;
using BaseLibS.Api;
using BaseLibS.Num;
using BaseLibS.Num.Vector;
using BaseLibS.Param;
using BaseLibS.Util;
using NumPluginBase.Kernel;
using NumPluginSvm.Svm;

namespace NumPluginSvm {
	public class SvmClassification : ClassificationMethod {
		public const string cHelp =
			"The C parameter tells the SVM optimization how much you want to avoid misclassifying each training example. " +
			"For large values of C, the optimization will choose a smaller-margin hyperplane if that hyperplane does a " +
			"better job of getting all the training points classified correctly. Conversely, a very small value of C will " +
			"cause the optimizer to look for a larger-margin separating hyperplane, even if that hyperplane misclassifies " +
			"more points.";

		public override ClassificationModel Train(BaseVector[] x, int[] nominal, int[][] y, int ngroups, Parameters param, int nthreads,
			Action<double> reportProgress) {
			x = ToOneHotEncoding(x, nominal);
			string err = CheckInput(x, y, ngroups);
			if (err != null) {
				throw new Exception(err);
			}
			ParameterWithSubParams<int> kernelParam = param.GetParamWithSubParams<int>("Kernel");
			SvmParameter sp = new SvmParameter {
				kernelFunction = KernelFunctions.GetKernelFunction(kernelParam.Value, kernelParam.GetSubParameters()),
				svmType = SvmType.CSvc,
				c = param.GetParam<double>("C").Value
			};
			bool[] invert;
			SvmProblem[] problems = CreateProblems(x, y, ngroups, out invert);
			SvmModel[] models = new SvmModel[problems.Length];
			ThreadDistributor td =
				new ThreadDistributor(nthreads, models.Length, i => { models[i] = SvmMain.SvmTrain(problems[i], sp); }) {
					ReportProgress = fractionDone => { reportProgress?.Invoke(fractionDone); }
				};
			td.Start();
			return new SvmClassificationModel(models, invert);
		}

		internal static string CheckInput(BaseVector[] x, int[][] y, int ngroups) {
			if (ngroups < 2) {
				return "Number of groups has to be at least two.";
			}
			foreach (int[] ints in y) {
				if (ints.Length == 0) {
					return "There are unassigned items";
				}
				Array.Sort(ints);
			}
			int[] vals = ArrayUtils.UniqueValues(ArrayUtils.Concat(y));
			for (int i = 0; i < vals.Length; i++) {
				if (vals[i] != i) {
					//return "At least one group has no training example.";
				}
			}
			return null;
		}

		private static SvmProblem[] CreateProblems(IList<BaseVector> x, IList<int[]> y, int ngroups, out bool[] invert) {
			if (ngroups == 2) {
				invert = new bool[1];
				return new[] {CreateProblem(x, y, 0, out invert[0])};
			}
			SvmProblem[] result = new SvmProblem[ngroups];
			invert = new bool[ngroups];
			for (int i = 0; i < ngroups; i++) {
				result[i] = CreateProblem(x, y, i, out invert[i]);
			}
			return result;
		}

		private static SvmProblem CreateProblem(IList<BaseVector> x, IList<int[]> y, int index, out bool invert) {
			double[] y1 = new double[y.Count];
			for (int i = 0; i < y.Count; i++) {
				if (Array.BinarySearch(y[i], index) >= 0) {
					y1[i] = 1;
				} else {
					y1[i] = 0;
				}
			}
			invert = Array.BinarySearch(y[0], index) < 0;
			return new SvmProblem(x, y1);
		}

		public override Parameters Parameters => new Parameters(KernelFunctions.GetKernelParameters(),
			new DoubleParam("C", 10) {Help = cHelp});

		public override string Name => "Support vector machine";
		public override string Description => "";
		public override float DisplayRank => 0;
		public override bool IsActive => true;
	}
}