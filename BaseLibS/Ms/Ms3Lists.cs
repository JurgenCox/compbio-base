using System.Collections.Generic;
using BaseLibS.Num;

namespace BaseLibS.Ms{
	public class Ms3Lists{
		public double massMin = double.MaxValue;
		public double massMax = double.MinValue;
		public int maxNumIms;
		public List<int> prevMs2IndexList = new List<int>();
		public List<int> scansList = new List<int>();
		public List<double> rtList = new List<double>();
		public List<double> mz1List = new List<double>();
		public List<double> mz2List = new List<double>();
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
		public List<bool> faimsVoltageOn = new List<bool>();
		public List<double> faimsCv = new List<double>();

		public Ms3Lists FilterVoltage(double voltage){
			Ms3Lists result = new Ms3Lists();
			List<int> valids = new List<int>();
			for (int i = 0; i < faimsCv.Count; i++){
				if (faimsCv[i] == voltage){
					valids.Add(i);
				}
			}
			result.massMin = massMin;
			result.massMax = massMax;
			result.maxNumIms = maxNumIms;
			//TODO: needs to be updated
			result.prevMs2IndexList = prevMs2IndexList.SubList(valids);
			result.scansList = scansList.SubList(valids);
			result.rtList = rtList.SubList(valids);
			result.mz1List = mz1List.SubList(valids);
			result.mz2List = mz2List.SubList(valids);
			result.fragmentTypeList = fragmentTypeList.SubList(valids);
			result.ionInjectionTimesList = ionInjectionTimesList.SubList(valids);
			result.basepeakIntensityList = basepeakIntensityList.SubList(valids);
			result.elapsedTimesList = elapsedTimesList.SubList(valids);
			result.energiesList = energiesList.SubList(valids);
			result.summationsList = summationsList.SubList(valids);
			result.monoisotopicMzList = monoisotopicMzList.SubList(valids);
			result.ticList = ticList.SubList(valids);
			result.hasCentroidList = hasCentroidList.SubList(valids);
			result.hasProfileList = hasProfileList.SubList(valids);
			result.analyzerList = analyzerList.SubList(valids);
			result.minMassList = minMassList.SubList(valids);
			result.maxMassList = maxMassList.SubList(valids);
			result.isolationMzMinList = isolationMzMinList.SubList(valids);
			result.isolationMzMaxList = isolationMzMaxList.SubList(valids);
			result.resolutionList = resolutionList.SubList(valids);
			result.intenseCompFactor = intenseCompFactor.SubList(valids);
			result.emIntenseComp = emIntenseComp.SubList(valids);
			result.rawOvFtT = rawOvFtT.SubList(valids);
			result.agcFillList = agcFillList.SubList(valids);
			result.nImsScans = nImsScans.SubList(valids);
			result.faimsVoltageOn = faimsVoltageOn.SubList(valids);
			result.faimsCv = faimsCv.SubList(valids);
			return result;
		}
	}
}