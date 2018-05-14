using System.Collections.Generic;

namespace BaseLibS.Ms{
	public class Ms2Lists{
		public double massMin = double.MaxValue;
		public double massMax = double.MinValue;
		public int maxNumIms = 0;
		public List<int> prevMs1IndexList = new List<int>();
		public List<int> scansList = new List<int>();
		public List<double> rtList = new List<double>();
		public List<double> mzList = new List<double>();
		public List<FragmentationTypeEnum> fragmentTypeList = new List<FragmentationTypeEnum>();
		public List<double> ionInjectionTimesList = new List<double>();
		public List<double> basepeakIntensityList = new List<double>();
		public List<double> elapsedTimesList = new List<double>();
		public List<double> energiesList = new List<double>();
		public List<double> summationsList = new List<double>();
		public List<double> monoisotopicMzList = new List<double>();
		public List<double> ticList = new List<double>();
		public List<bool> hasCentroidList = new List<bool>();
		public List<bool> hasProfileList = new List<bool>();
		public List<MassAnalyzerEnum> analyzerList = new List<MassAnalyzerEnum>();
		public List<double> minMassList = new List<double>();
		public List<double> maxMassList = new List<double>();
		public List<double> isolationMzMinList = new List<double>();
		public List<double> isolationMzMaxList = new List<double>();
		public List<double> resolutionList = new List<double>();
		public List<double> intenseCompFactor = new List<double>();
		public List<double> emIntenseComp = new List<double>();
		public List<double> rawOvFtT = new List<double>();
		public List<double> agcList = new List<double>();
		public List<double> agcFillList = new List<double>();
		public List<int> nImsScans = new List<int>();
	}
}