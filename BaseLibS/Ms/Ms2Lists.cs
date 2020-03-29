using System.Collections.Generic;
using BaseLibS.Num;

namespace BaseLibS.Ms{
	public class Ms2Lists{
		public double massMin = double.MaxValue;
		public double massMax = double.MinValue;
		public int maxNumIms;
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
		public List<double> agcFillList = new List<double>();
		public List<int> nImsScans = new List<int>();
		public List<int> windowGroupList = new List<int>();
		public List<bool> faimsVoltageOn = new List<bool>();
		public List<double> faimsCv = new List<double>();

		public Ms2Lists FilterVoltage(double voltage){
			Ms2Lists result = new Ms2Lists();
			List<int> valids = new List<int>();
			for (int i = 0; i < faimsCv.Count; i++){
				if (faimsCv[i] == voltage){
					valids.Add(i);
				}
			}
			result.massMin = massMin;
			result.massMax = massMax;
			result.maxNumIms = maxNumIms;
			result.prevMs1IndexList = prevMs1IndexList.MsSubList(valids);
			result.scansList = scansList.MsSubList(valids);
			result.rtList = rtList.MsSubList(valids);
			result.mzList = mzList.MsSubList(valids);
			result.fragmentTypeList = fragmentTypeList.MsSubList(valids);
			result.ionInjectionTimesList = ionInjectionTimesList.MsSubList(valids);
			result.basepeakIntensityList = basepeakIntensityList.MsSubList(valids);
			result.elapsedTimesList = elapsedTimesList.MsSubList(valids);
			result.energiesList = energiesList.MsSubList(valids);
			result.summationsList = summationsList.MsSubList(valids);
			result.monoisotopicMzList = monoisotopicMzList.MsSubList(valids);
			result.ticList = ticList.MsSubList(valids);
			result.hasCentroidList = hasCentroidList.MsSubList(valids);
			result.hasProfileList = hasProfileList.MsSubList(valids);
			result.analyzerList = analyzerList.MsSubList(valids);
			result.minMassList = minMassList.MsSubList(valids);
			result.maxMassList = maxMassList.MsSubList(valids);
			result.isolationMzMinList = isolationMzMinList.MsSubList(valids);
			result.isolationMzMaxList = isolationMzMaxList.MsSubList(valids);
			result.resolutionList = resolutionList.MsSubList(valids);
			result.intenseCompFactor = intenseCompFactor.MsSubList(valids);
			result.emIntenseComp = emIntenseComp.MsSubList(valids);
			result.rawOvFtT = rawOvFtT.MsSubList(valids);
			result.agcFillList = agcFillList.MsSubList(valids);
			result.nImsScans = nImsScans.MsSubList(valids);
			result.windowGroupList = windowGroupList.MsSubList(valids);
			result.faimsVoltageOn = faimsVoltageOn.MsSubList(valids);
			result.faimsCv = faimsCv.MsSubList(valids);
			return result;
		}
	}
}