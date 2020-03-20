using System.Xml.Serialization;

namespace BaseLibS.Mol{

	// For NHS-ester crosslinkers, amidation is important to screen for links with Serine and Threonine (but smaller portions of identified crosslinks).
	// Currently amidated crosslinker setting is ignored but could be added later, not the priority now. 
	public class CrossLinker : StorableItem{
		private double linkedMass = double.NaN;
		private double hydrolyzedMass = double.NaN;
		private double crossFragmentLongMass = double.NaN;
		private double crossFragmentShortMass = double.NaN;

		[XmlAttribute("linkedComposition")]
		public string LinkedComposition { get; set; }

		[XmlIgnore]
		public double LinkedMass {
			get {
				if (double.IsNaN(linkedMass)) {
					linkedMass = ChemElements.GetMassFromComposition(LinkedComposition);
				}

				return linkedMass;
			}
		}

		[XmlAttribute("hydrolyzedComposition")]
		public string HydrolyzedComposition { get; set; }
		
		[XmlIgnore]
		public double HydrolyzedMass {
			get{
				if (double.IsNaN(hydrolyzedMass)){
					hydrolyzedMass = ChemElements.GetMassFromComposition(HydrolyzedComposition);
				}

				return hydrolyzedMass;
			}
		}

		[XmlAttribute("crosslinkFragmentLongComposition")]
		public string CrossFragmentLongComposition { get; set; }

		[XmlIgnore]
		public double CrossFragmentLongMass {
			get {
				if (double.IsNaN(crossFragmentLongMass)) {
					crossFragmentLongMass = ChemElements.GetMassFromComposition(CrossFragmentLongComposition);
				}

				return crossFragmentLongMass;
			}
		}

		[XmlAttribute("crosslinkFragmentShortComposition")]
		public string CrossFragmentShortComposition { get; set; }

		[XmlIgnore]
		public double CrossFragmentShortMass {
			get {
				if (double.IsNaN(crossFragmentShortMass)) {
					crossFragmentShortMass = ChemElements.GetMassFromComposition(CrossFragmentShortComposition);
				}

				return crossFragmentShortMass;
			}
		}

		[XmlAttribute("specificity1")]
		public string Specificity1 { get; set; }

		[XmlAttribute("proteinNterm1")]
		public bool DoesCrosslinkProteinNterm1 { get; set; }

		[XmlAttribute("proteinCterm1")]
		public bool DoesCrosslinkProteinCterm1 { get; set; }

	    [XmlElement("position1", typeof(ModificationPosition))]
	    public ModificationPosition Position1 { get; set; } = ModificationPosition.anywhere;

        [XmlAttribute("specificity2")]
	    public string Specificity2 { get; set; }

	    [XmlAttribute("proteinNterm2")]
	    public bool DoesCrosslinkProteinNterm2 { get; set; }

	    [XmlAttribute("proteinCterm2")]
	    public bool DoesCrosslinkProteinCterm2 { get; set; }

	    [XmlElement("position2", typeof(ModificationPosition))]
	    public ModificationPosition Position2 { get; set; } = ModificationPosition.anywhere;

	    [XmlAttribute("mscleavable")]
	    public bool IsMsCleavableCrosslinker { get; set; }

	    protected bool Equals(CrossLinker other) {
		    return LinkedComposition == other.LinkedComposition &&
		           HydrolyzedComposition == other.HydrolyzedComposition &&
		           CrossFragmentLongComposition == other.CrossFragmentLongComposition &&
		           CrossFragmentShortComposition == other.CrossFragmentShortComposition &&
		           Specificity1 == other.Specificity1 &&
		           DoesCrosslinkProteinNterm1 == other.DoesCrosslinkProteinNterm1 &&
		           DoesCrosslinkProteinCterm1 == other.DoesCrosslinkProteinCterm1 &&
		           Specificity2 == other.Specificity2 &&
		           DoesCrosslinkProteinNterm2 == other.DoesCrosslinkProteinNterm2 &&
		           DoesCrosslinkProteinCterm2 == other.DoesCrosslinkProteinCterm2 && Position2 == other.Position2 &&
		           IsMsCleavableCrosslinker == other.IsMsCleavableCrosslinker;
	    }

	    public override bool Equals(object obj){
		    if (ReferenceEquals(null, obj)) return false;
		    if (ReferenceEquals(this, obj)) return true;
		    if (obj.GetType() != this.GetType()) return false;
		    return Equals((CrossLinker) obj);
	    }

	    public override int GetHashCode() {
			unchecked {
				var hashCode = (LinkedComposition != null ? LinkedComposition.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (HydrolyzedComposition != null ? HydrolyzedComposition.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (CrossFragmentLongComposition != null ? CrossFragmentLongComposition.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (CrossFragmentShortComposition != null ? CrossFragmentShortComposition.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Specificity1 != null ? Specificity1.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ DoesCrosslinkProteinNterm1.GetHashCode();
				hashCode = (hashCode * 397) ^ DoesCrosslinkProteinCterm1.GetHashCode();
				hashCode = (hashCode * 397) ^ (Specificity2 != null ? Specificity2.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ DoesCrosslinkProteinNterm2.GetHashCode();
				hashCode = (hashCode * 397) ^ DoesCrosslinkProteinCterm2.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) Position2;
				hashCode = (hashCode * 397) ^ IsMsCleavableCrosslinker.GetHashCode();
				return hashCode;
			}
		}
	}
}