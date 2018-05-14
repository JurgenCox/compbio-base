namespace BaseLibS.Ms {
    /// <summary>
    /// A collection of fields describing a scan. Information on scans is set in a device-dependent way.
    /// </summary>
	public class ScanInfo{
		public MsLevel msLevel;
		public bool isSim;
		public bool hasCentroid;
		public bool hasProfile;
		public bool positiveIonMode;
		public double min;  // minimum m/z
        public double max;  // minimum m/z
		public double agcFill;
		public double ionInjectionTime;
		public double basepeakIntensity;
		public double elapsedTime;
		public double ms2MonoMz;
		public double ms2ParentMz;
		public double ms3ParentMz;
		public FragmentationTypeEnum ms2FragType;
		public double tic;  // total ion current
		public double rt;  // retention time
		public int ionMobilityIndex;
		public double ms2IsolationMin;
		public double ms2IsolationMax;
        public MassAnalyzerEnum analyzer;
		public double resolution;
		public double summations;
		public double energy;
		public double intenseCompFactor;
		public double emIntenseComp;
		public double rawOvFtT;
	    public int nImsScans;
    }
}
