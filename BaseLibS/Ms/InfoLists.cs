using System;
using System.Collections.Generic;

namespace BaseLibS.Ms{
	/// <summary>
	/// Fields, other than allMassRanges, are Ms1Lists, SimLists, and Ms2Lists, only one of which 
	/// is actually given values, depending on the value of scanInfo.msLevel.
	/// </summary>
	public class InfoLists{
		public readonly Ms1Lists ms1Lists;
		public readonly Ms2Lists ms2Lists;
		public readonly Ms3Lists ms3Lists;
		public readonly Dictionary<double, object> allMassRanges;
		public readonly List<int> currentMs2Inds = new List<int>();

		/// <summary>
		/// Constructor does not set any data, except passing the firstScanNumber arg to the SimLists constructor.
		/// </summary>
		public InfoLists(){
			ms1Lists = new Ms1Lists();
			ms2Lists = new Ms2Lists();
			ms3Lists = new Ms3Lists();
			allMassRanges = new Dictionary<double, object>();
		}

		/// <summary>
		/// Take the ScanInfo object and add its contents to the field simLists, ms1Lists, or ms2Lists, 
		/// depending on the values of the msLevel and isSim fields of the ScanInfo.
		/// </summary>
		/// <param name="scanInfo">an object containing details about this scan</param>
		/// <param name="scanNum">the unique number of this scan</param>
		public void Add(ScanInfo scanInfo, int scanNum){
			switch (scanInfo.msLevel){
				case MsLevel.Ms1:
					//if (!scanInfo.isSim){
						currentMs2Inds.Clear();
						if (scanInfo.min < ms1Lists.massMin){
							ms1Lists.massMin = scanInfo.min;
						}
						if (scanInfo.max > ms1Lists.massMax){
							ms1Lists.massMax = scanInfo.max;
						}
						if (scanInfo.nImsScans > ms1Lists.maxNumIms){
							ms1Lists.maxNumIms = scanInfo.nImsScans;
						}
						ms1Lists.scans.Add(scanNum);
						ms1Lists.rts.Add(scanInfo.rt);
						ms1Lists.ionInjectionTimes.Add(scanInfo.ionInjectionTime);
						ms1Lists.basepeakIntensities.Add(scanInfo.basepeakIntensity);
						ms1Lists.elapsedTimes.Add(scanInfo.elapsedTime);
						if (scanInfo.hasProfile && !allMassRanges.ContainsKey(scanInfo.min)){
							allMassRanges.Add(scanInfo.min, null);
						} else if (!scanInfo.hasProfile && !allMassRanges.ContainsKey(0)){
							allMassRanges.Add(0, null);
						}
						ms1Lists.tics.Add(scanInfo.tic);
						ms1Lists.hasCentroids.Add(scanInfo.hasCentroid);
						ms1Lists.hasProfiles.Add(scanInfo.hasProfile);
						ms1Lists.massAnalyzer.Add(scanInfo.analyzer);
						ms1Lists.minMasses.Add(scanInfo.min);
						ms1Lists.maxMasses.Add(scanInfo.max);
						ms1Lists.resolutions.Add(scanInfo.resolution);
						ms1Lists.intenseCompFactors.Add(scanInfo.intenseCompFactor);
						ms1Lists.emIntenseComp.Add(scanInfo.emIntenseComp);
						ms1Lists.rawOvFtT.Add(scanInfo.rawOvFtT);
						ms1Lists.agcFillList.Add(scanInfo.agcFill);
						ms1Lists.nImsScans.Add(scanInfo.nImsScans);
						ms1Lists.isSim.Add(scanInfo.isSim);
					//}
					break;
				case MsLevel.Ms2:
					if (scanInfo.min < ms2Lists.massMin){
						ms2Lists.massMin = scanInfo.min;
					}
					if (scanInfo.max > ms2Lists.massMax){
						ms2Lists.massMax = scanInfo.max;
					}
					if (scanInfo.nImsScans > ms2Lists.maxNumIms){
						ms2Lists.maxNumIms = scanInfo.nImsScans;
					}
					currentMs2Inds.Add(ms2Lists.scansList.Count);
					ms2Lists.scansList.Add(scanNum);
					ms2Lists.prevMs1IndexList.Add(ms1Lists.scans.Count - 1);
					ms2Lists.mzList.Add(scanInfo.ms2ParentMz);
					ms2Lists.fragmentTypeList.Add(scanInfo.ms2FragType);
					ms2Lists.rtList.Add(scanInfo.rt);
					ms2Lists.ionInjectionTimesList.Add(scanInfo.ionInjectionTime);
					ms2Lists.basepeakIntensityList.Add(scanInfo.basepeakIntensity);
					ms2Lists.elapsedTimesList.Add(scanInfo.elapsedTime);
					ms2Lists.energiesList.Add(scanInfo.energy);
					ms2Lists.summationsList.Add(scanInfo.summations);
					ms2Lists.monoisotopicMzList.Add(scanInfo.ms2MonoMz);
					ms2Lists.ticList.Add(scanInfo.tic);
					ms2Lists.hasCentroidList.Add(scanInfo.hasCentroid);
					ms2Lists.hasProfileList.Add(scanInfo.hasProfile);
					ms2Lists.analyzerList.Add(scanInfo.analyzer);
					ms2Lists.minMassList.Add(scanInfo.min);
					ms2Lists.maxMassList.Add(scanInfo.max);
					ms2Lists.isolationMzMinList.Add(scanInfo.ms2IsolationMin);
					ms2Lists.isolationMzMaxList.Add(scanInfo.ms2IsolationMax);
					ms2Lists.resolutionList.Add(scanInfo.resolution);
					ms2Lists.intenseCompFactor.Add(scanInfo.intenseCompFactor);
					ms2Lists.emIntenseComp.Add(scanInfo.emIntenseComp);
					ms2Lists.rawOvFtT.Add(scanInfo.rawOvFtT);
					ms2Lists.agcFillList.Add(scanInfo.agcFill);
					ms2Lists.nImsScans.Add(scanInfo.nImsScans);
					break;
				case MsLevel.Ms3:
					if (scanInfo.min < ms3Lists.massMin){
						ms3Lists.massMin = scanInfo.min;
					}
					if (scanInfo.max > ms3Lists.massMax){
						ms3Lists.massMax = scanInfo.max;
					}
					if (scanInfo.nImsScans > ms3Lists.maxNumIms){
						ms3Lists.maxNumIms = scanInfo.nImsScans;
					}
					ms3Lists.scansList.Add(scanNum);
					ms3Lists.prevMs2IndexList.Add(GetPrecursorMs2(currentMs2Inds, scanInfo.ms2ParentMz));
					//ms3Lists.ms3PrevMs2IndexList.Add(ms2Lists.ms2ScansList.Count - 1);
					ms3Lists.mz1List.Add(scanInfo.ms2ParentMz);
					ms3Lists.mz2List.Add(scanInfo.ms3ParentMz);
					ms3Lists.fragmentTypeList.Add(scanInfo.ms2FragType);
					ms3Lists.rtList.Add(scanInfo.rt);
					ms3Lists.ionInjectionTimesList.Add(scanInfo.ionInjectionTime);
					ms3Lists.basepeakIntensityList.Add(scanInfo.basepeakIntensity);
					ms3Lists.elapsedTimesList.Add(scanInfo.elapsedTime);
					ms3Lists.energiesList.Add(scanInfo.energy);
					ms3Lists.summationsList.Add(scanInfo.summations);
					ms3Lists.monoisotopicMzList.Add(scanInfo.ms2MonoMz);
					ms3Lists.ticList.Add(scanInfo.tic);
					ms3Lists.hasCentroidList.Add(scanInfo.hasCentroid);
					ms3Lists.hasProfileList.Add(scanInfo.hasProfile);
					ms3Lists.analyzerList.Add(scanInfo.analyzer);
					ms3Lists.minMassList.Add(scanInfo.min);
					ms3Lists.maxMassList.Add(scanInfo.max);
					ms3Lists.isolationMzMinList.Add(scanInfo.ms2IsolationMin);
					ms3Lists.isolationMzMaxList.Add(scanInfo.ms2IsolationMax);
					ms3Lists.resolutionList.Add(scanInfo.resolution);
					ms3Lists.intenseCompFactor.Add(scanInfo.intenseCompFactor);
					ms3Lists.emIntenseComp.Add(scanInfo.emIntenseComp);
					ms3Lists.rawOvFtT.Add(scanInfo.rawOvFtT);
					ms3Lists.agcFillList.Add(scanInfo.agcFill);
					ms3Lists.nImsScans.Add(scanInfo.nImsScans);
					break;
				default:
					throw new Exception("Invalid MS level.");
			}
		}

		private int GetPrecursorMs2(IEnumerable<int> ms2Inds, double pmz){
			foreach (int ind in ms2Inds){
				if (ms2Lists.mzList[ind] == pmz){
					return ind;
				}
			}
			return -1;
		}
	}
}