using System.Collections.Generic;

namespace BaseLibS.Ms{
	public class Ms1Lists{
		public double massMin = double.MaxValue;
		public double massMax = double.MinValue;
		public int maxNumIms = 0;
		public List<int> scans = new List<int>();
		public List<double> rts = new List<double>();
		public List<double> ionInjectionTimes = new List<double>();
		public List<double> basepeakIntensities = new List<double>();
		public List<double> elapsedTimes = new List<double>();
		public List<double> tics = new List<double>();
		public List<bool> hasCentroids = new List<bool>();
		public List<bool> hasProfiles = new List<bool>();
		public List<MassAnalyzerEnum> massAnalyzer = new List<MassAnalyzerEnum>();
		public List<double> minMasses = new List<double>();
		public List<double> maxMasses = new List<double>();
		public List<double> resolutions = new List<double>();
		public List<double> intenseCompFactors = new List<double>();
		public List<double> emIntenseComp = new List<double>();
		public List<double> rawOvFtT = new List<double>();
		public List<double> agcFillList = new List<double>();
		public List<int> nImsScans = new List<int>();
		public List<bool> isSim = new List<bool>();
	}
}