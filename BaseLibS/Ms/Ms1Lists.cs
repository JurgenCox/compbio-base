using System.Collections.Generic;
using BaseLibS.Num;

namespace BaseLibS.Ms{
	public class Ms1Lists{
		public double massMin = double.MaxValue;
		public double massMax = double.MinValue;
		public int maxNumIms;
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
		public List<bool> faimsVoltageOn = new List<bool>();
		public List<double> faimsCv = new List<double>();

		public Ms1Lists FilterVoltage(double voltage){
			Ms1Lists result = new Ms1Lists();
			List<int> valids = new List<int>();
			for (int i = 0; i < faimsCv.Count; i++){
				if (faimsCv[i] == voltage){
					valids.Add(i);
				}
			}
			result.massMin = massMin;
			result.massMax = massMax;
			result.maxNumIms = maxNumIms;
			result.scans = scans.SubList(valids);
			result.rts = rts;
			result.ionInjectionTimes = ionInjectionTimes;
			result.basepeakIntensities = basepeakIntensities;
			result.elapsedTimes = elapsedTimes;
			result.tics = tics;
			result.hasCentroids = hasCentroids;
			result.hasProfiles = hasProfiles;
			result.massAnalyzer = massAnalyzer;
			result.minMasses = minMasses;
			result.maxMasses = maxMasses;
			result.resolutions = resolutions;
			result.intenseCompFactors = intenseCompFactors;
			result.emIntenseComp = emIntenseComp;
			result.rawOvFtT = rawOvFtT;
			result.agcFillList = agcFillList;
			result.nImsScans = nImsScans;
			result.isSim = isSim;
			result.faimsVoltageOn = faimsVoltageOn;
			result.faimsCv = faimsCv;
			return result;
		}
	}
}