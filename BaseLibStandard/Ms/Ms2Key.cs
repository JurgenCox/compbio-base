namespace BaseLibS.Ms{
	public class Ms2Key{
		private readonly MassAnalyzerEnum massAnalyzer;
		private readonly FragmentationTypeEnum fragmentationType;
		private readonly SignalType signalType;
		private int hash;

		public Ms2Key(MassAnalyzerEnum massAnalyzer, FragmentationTypeEnum fragmentationType, SignalType signalType){
			this.massAnalyzer = massAnalyzer;
			this.fragmentationType = fragmentationType;
			this.signalType = signalType;
		}

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == typeof (Ms2Key) && Equals((Ms2Key) obj);
		}

		public bool Equals(Ms2Key other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return Equals(other.massAnalyzer, massAnalyzer) && Equals(other.fragmentationType, fragmentationType) &&
				Equals(other.signalType, signalType);
		}

		public override int GetHashCode(){
			unchecked{
				if (hash == 0){
					hash = massAnalyzer.GetHashCode();
					hash = (hash*397) ^ fragmentationType.GetHashCode();
					hash = (hash*397) ^ signalType.GetHashCode();
				}
				return hash;
			}
		}
	}
}