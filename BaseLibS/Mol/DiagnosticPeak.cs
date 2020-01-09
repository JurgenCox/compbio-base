using System.IO;
using System.Xml.Serialization;

namespace BaseLibS.Mol{
	public class DiagnosticPeak{
		private double mass = double.NaN;

		public DiagnosticPeak(){
			// Default Constructor for Serialization
		}

		public DiagnosticPeak(BinaryReader reader){
			Name = reader.ReadString();
			ShortName = reader.ReadString();
			Composition = reader.ReadString();
			Mass = reader.ReadDouble();
		}

		public DiagnosticPeak(string name, string shortname, string composition, double mass){
			Name = name;
			ShortName = shortname;
			Composition = composition;
			Mass = mass;
		}

		public void Write(BinaryWriter writer){
			writer.Write(Name);
			writer.Write(ShortName);
			writer.Write(Composition);
			writer.Write(Mass);
		}

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("shortname")]
		public string ShortName { get; set; }

		[XmlIgnore]
		public double Mass{
			get{
				if (double.IsNaN(mass)){
					int[] counts;
					string[] comp;
					double[] mono;
					ChemElements.DecodeComposition(Composition, ChemElements.ElementDictionary, out counts, out comp, out mono);
					mass = 0;
					for (int i = 0; i < mono.Length; i++){
						mass += mono[i]*counts[i];
					}
				}
				return mass;
			}
			set => mass = value;
		}

		[XmlAttribute("composition")]
		public string Composition { get; set; } = "";

		public object Clone(){
			return new DiagnosticPeak{Name = Name, Mass = mass, Composition = Composition, ShortName = ShortName};
		}
	}
}