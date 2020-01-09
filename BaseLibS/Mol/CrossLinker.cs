using System.Xml.Serialization;

namespace BaseLibS.Mol{
	public class CrossLinker : StorableItem{
		private double saturated = double.NaN;
		private double unsaturated = double.NaN;

		[XmlAttribute("saturated_composition")]
		public string SaturatedComposition { get; set; }

		[XmlAttribute("unsaturated_composition")]
		public string UnsaturatedComposition { get; set; }

		[XmlIgnore]
		public double SaturatedMass{
			get{
				if (double.IsNaN(saturated)){
					saturated = ChemElements.GetMassFromComposition(SaturatedComposition);
				}
				return saturated;
			}
		}

		[XmlIgnore]
		public double UnsaturatedMass{
			get{
				if (double.IsNaN(unsaturated)){
					unsaturated = ChemElements.GetMassFromComposition(UnsaturatedComposition);
				}
				return unsaturated;
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

        public override bool Equals(object obj){
			if (this == obj){
				return true;
			}
			if (obj is Modification){
				return (((Modification) obj).Name != Name);
			}
			return false;
		}

		public override int GetHashCode() { return Name.GetHashCode(); }
	}
}