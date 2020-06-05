using System;
using System.IO;
using BaseLibS.Api;
using BaseLibS.Param;

namespace NumPluginBase.Distance {
	public static class Distances {
		/// <summary>
		/// Return all distances found dynamically from plugins implementing <see cref="IDistance"/>.
		/// </summary>
		public static readonly IDistance[] allDistances = new IDistance[]{
			new EuclideanDistance(), new L1Distance(),
			new MaximumDistance(), new LpDistance(), new PearsonCorrelationDistance(), new SpearmanCorrelationDistance(), 
			new CosineDistance(), new CanberraDistance()
		};

		public static SingleChoiceWithSubParams GetDistanceParameters() {
			return GetDistanceParameters("");
		}

		public static SingleChoiceWithSubParams GetDistanceParameters(string help) {
			return GetDistanceParameters(help, 666, 100);
		}

		public static SingleChoiceWithSubParams GetDistanceParameters(string help, int totalWidth, int paramNameWidth) {
			return new SingleChoiceWithSubParams("Distance") {
				Values = GetAllNames(),
				SubParams = GetAllParameters(),
				Value = 0,
				Help = help,
				TotalWidth = totalWidth,
				ParamNameWidth = paramNameWidth
			};
		}

		public static IDistance GetDistanceFunction(Parameters param) {
			ParameterWithSubParams<int> distParam = param.GetParamWithSubParams<int>("Distance");
			return GetDistanceFunction(distParam.Value, distParam.GetSubParameters());
		}

		private static string[] GetAllNames() {
			string[] result = new string[allDistances.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = allDistances[i].Name;
			}
			return result;
		}

		private static Parameters[] GetAllParameters() {
			Parameters[] result = new Parameters[allDistances.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = allDistances[i].Parameters;
			}
			return result;
		}

		private static IDistance GetDistanceFunction(int index, Parameters param) {
			IDistance kf = (IDistance) allDistances[index].Clone();
			kf.Parameters = param;
			return kf;
		}

		public static IDistance ReadDistance(DistanceType type, BinaryReader reader){
			IDistance dist;
			switch (type){
				case DistanceType.Canberra:
					dist = new CanberraDistance();
					break;
				case DistanceType.Cosine:
					dist = new CosineDistance();
					break;
				case DistanceType.Euclidean:
					dist = new EuclideanDistance();
					break;
				case DistanceType.L1:
					dist = new L1Distance();
					break;
				case DistanceType.Lp:
					dist = new LpDistance();
					break;
				case DistanceType.Maximum:
					dist = new MaximumDistance();
					break;
				case DistanceType.Pearson:
					dist = new PearsonCorrelationDistance();
					break;
				case DistanceType.Spearman:
					dist = new SpearmanCorrelationDistance();
					break;
				default:
					throw new Exception("Never get here.");
			}
			dist.Read(reader);
			return dist;
		}

	}
}