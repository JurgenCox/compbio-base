using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BaseLibS.Mol{
	public static class ChemElements{
		private static ChemElement[] elements;
		public static ChemElement[] Elements => elements ?? (elements = InitElements());
		private static Dictionary<string, ChemElement> elementDictionary;

		public static Dictionary<string, ChemElement> ElementDictionary{
			get{
				if (elementDictionary == null){
					Dictionary<string, ChemElement> elementDictionary1 = new Dictionary<string, ChemElement>();
					foreach (ChemElement t in Elements){
						elementDictionary1.Add(t.Symbol, t);
					}
					elementDictionary = elementDictionary1;
				}
				return elementDictionary;
			}
		}

		private static Dictionary<string, int> elementIndex;

		public static Dictionary<string, int> ElementIndex{
			get{
				if (elementIndex == null){
					Dictionary<string, int> elementIndex1 = new Dictionary<string, int>();
					for (int i = 0; i < Elements.Length; i++){
						ChemElement t = Elements[i];
						elementIndex1.Add(t.Symbol, i);
					}
					elementIndex = elementIndex1;
				}
				return elementIndex;
			}
		}

		private static int indexC = -1;

		public static int IndexC{
			get{
				if (indexC == -1){
					indexC = ElementIndex["C"];
				}
				return indexC;
			}
		}

		private static int indexO = -1;

		public static int IndexO{
			get{
				if (indexO == -1){
					indexO = ElementIndex["O"];
				}
				return indexO;
			}
		}

		private static int indexS = -1;

		public static int IndexS{
			get{
				if (indexS == -1){
					indexS = ElementIndex["S"];
				}
				return indexS;
			}
		}

		private static int indexCl = -1;

		public static int IndexCl{
			get{
				if (indexCl == -1){
					indexCl = ElementIndex["Cl"];
				}
				return indexCl;
			}
		}

		private static int indexH = -1;

		public static int IndexH{
			get{
				if (indexH == -1){
					indexH = ElementIndex["H"];
				}
				return indexH;
			}
		}

		private static ChemElement[] InitElements(){
			ChemElement[] e = {
				new ChemElement(1, "H", new string[0], "Hydrogen", new[]{1.0078250321, 2.014101778},
					new[]{99.9885, 0.0115}, 1.00794, ChemElementType.Nonmetal, new[]{1}, "1333-74-0", false, 200),
				new ChemElement(1, "Hx", new string[0], "Deuterium", new[]{1.0078250321, 2.014101778},
					new[]{0.0, 100.0}, 2.014101778, ChemElementType.Nonmetal, new[]{1}, "7782-39-0", true),
				new ChemElement(1, "T", new string[0], "Tritium", new[]{1.0078250321, 2.014101778, 3.0160492777},
					new[]{0.0, 0.0, 100.0}, 3.0160492777, ChemElementType.Nonmetal, new[]{1}, "10028-17-8", true),
				new ChemElement(2, "He", new string[0], "Helium", new[]{3.0160293191, 4.00260325415},
					new[]{0.000134, 99.999866}, 4.002602, ChemElementType.NobleGas, new[]{0}),
				new ChemElement(3, "Li", new string[0], "Lithium", new[]{6.0151223, 7.0160040}, new[]{7.59, 92.41},
					6.941, ChemElementType.AlkaliMetal, new[]{1}),
				new ChemElement(5, "B", new string[0], "Boron", new[]{10.0129370, 11.0093055}, new[]{19.9, 80.1},
					10.811, ChemElementType.Semimetal, new[]{-3, 3}),
				new ChemElement(4, "Be", new string[0], "Beryllium", new[]{9.0121822}, new[]{100.0}, 9.0121822,
					ChemElementType.AlkalineEarth, new[]{2}),
				new ChemElement(6, "C", new string[0], "Carbon", new[]{12.0, 13.0033548378}, new[]{98.93, 1.07},
					12.0107, ChemElementType.Nonmetal, new[]{4, 2}, "7440-44-0", false, 200),
				new ChemElement(6, "Cx", new []{"[13C]"}, "Carbon-13", new[]{12.0, 13.0033548378}, new[]{0.0, 100.0},
					13.0033548378, ChemElementType.Nonmetal, new[]{4, 2}, "", true),
				new ChemElement(7, "N", new string[0], "Nitrogen", new[]{14.0030740052, 15.0001088984},
					new[]{99.632, 0.368}, 14.0067, ChemElementType.Nonmetal, new[]{-3, -2, -1, +2, +3, +4, +5}, "",
					false, 100),
				new ChemElement(7, "Nx", new []{"[15N]"}, "Nitrogen", new[]{14.0030740052, 15.0001088984},
					new[]{0.0, 100.0}, 15.0001088984, ChemElementType.Nonmetal, new[]{-3, -2, -1, +2, +3, +4, +5}, "",
					true),
				new ChemElement(8, "O", new string[0], "Oxygen", new[]{15.9949146221, 16.9991315, 17.9991604},
					new[]{99.757, 0.038, 0.205}, 15.9994, ChemElementType.Nonmetal, new[]{-2}, "", false, 100),
				new ChemElement(8, "Ox", new string[0], "Oxygen", new[]{15.9949146221, 17.9991604}, new[]{0.0, 100.0},
					17.9991604, ChemElementType.Nonmetal, new[]{-2}, "", true),
				new ChemElement(8, "Oy", new string[0], "Oxygen", new[]{15.9949146221, 16.9991315}, new[]{0.0, 100.0},
					16.9991315, ChemElementType.Nonmetal, new[]{-2}, "", true),
				new ChemElement(9, "F", new string[0], "Fluorine", new[]{18.99840320}, new[]{100.0}, 18.9984032,
					ChemElementType.Halogen, new[]{-1}),
				new ChemElement(10, "Ne", new string[0], "Neon", new[]{19.9924401754, 20.99384668, 21.991385114},
					new[]{90.48, 0.27, 9.25}, 20.1797, ChemElementType.NobleGas, new[]{0}),
				new ChemElement(11, "Na", new string[0], "Sodium", new[]{22.98976967}, new[]{100.0}, 22.989770,
					ChemElementType.AlkaliMetal, new[]{1}),
				new ChemElement(12, "Mg", new string[0], "Magnesium", new[]{23.985041700, 24.98583692, 25.982592929},
					new[]{78.99, 10.00, 11.01}, 24.3050, ChemElementType.AlkalineEarth, new[]{2}),
				new ChemElement(13, "Al", new string[0], "Aluminum", new[]{26.98153863}, new[]{100.0}, 26.98153863,
					ChemElementType.BasicMetal, new[]{3}),
				new ChemElement(15, "P", new string[0], "Phosphorus", new[]{30.97376151}, new[]{100.0}, 30.973761,
					ChemElementType.Nonmetal, new[]{-3, 1, 3, 5}),
				new ChemElement(16, "S", new string[0], "Sulfur",
					new[]{31.97207069, 32.9714585, 33.96786683, 35.96708088}, new[]{94.93, 0.76, 4.29, 0.02}, 32.065,
					ChemElementType.Nonmetal, new[]{-2, 2, 4, 6}),
				new ChemElement(16, "Sx", new string[0], "Sulfur", new[]{31.97207069, 33.96786683}, new[]{0.0, 100.0},
					33.96786683, ChemElementType.Nonmetal, new[]{-2, 2, 4, 6}, "", true),
				new ChemElement(16, "Sy", new string[0], "Sulfur", new[]{31.97207069, 32.9714585}, new[]{0.0, 100.0},
					32.9714585, ChemElementType.Nonmetal, new[]{-2, 2, 4, 6}, "", true),
				new ChemElement(14, "Si", new string[0], "Silicon", new[]{27.9769265325, 28.976494700, 29.97377017},
					new[]{92.223, 4.685, 3.092}, 28.0855, ChemElementType.Semimetal, new[]{4, -4}),
				new ChemElement(17, "Cl", new string[0], "Chlorine", new[]{34.96885271, 36.9659026},
					new[]{75.78, 24.22}, 35.453, ChemElementType.Halogen, new[]{-1, +1, +3, +5, +7}),
				new ChemElement(17, "Clx", new string[0], "Chlorine", new[]{34.96885271, 36.9659026}, new[]{0.0, 100.0},
					36.9659026, ChemElementType.Halogen, new[]{-1, +1, +3, +5, +7}, "", true),
				new ChemElement(18, "Ar", new string[0], "Argon", new[]{35.967545106, 37.9627324, 39.9623831225},
					new[]{0.3365, 0.0632, 99.6003}, 39.948, ChemElementType.NobleGas, new[]{0}),
				new ChemElement(19, "K", new string[0], "Potassium", new[]{38.9637069, 39.96399867, 40.96182597},
					new[]{93.2581, 0.0117, 6.7302}, 39.0983, ChemElementType.AlkaliMetal, new[]{1}),
				new ChemElement(19, "Kx", new string[0], "Potassium", new[]{38.9637069, 39.96399867, 40.96182597},
					new[]{0.0, 0.0, 100.0}, 40.96182597, ChemElementType.AlkaliMetal, new[]{1}, "", true),
				new ChemElement(21, "Sc", new string[0], "Scandium", new[]{44.9559119}, new[]{100.0}, 44.9559119,
					ChemElementType.TransitionMetal, new[]{3}),
				new ChemElement(22, "Ti", new string[0], "Titanium",
					new[]{45.9526316, 46.9517631, 47.9479463, 48.9478700, 49.9447912},
					new[]{8.25, 7.44, 73.72, 5.41, 5.18}, 47.867, ChemElementType.TransitionMetal, new[]{4, 3, 2}),
				new ChemElement(20, "Ca", new string[0], "Calcium",
					new[]{39.9625912, 41.9586183, 42.9587668, 43.9554811, 45.9536928, 47.952534},
					new[]{96.941, 0.647, 0.135, 2.086, 0.004, 0.187}, 40.078, ChemElementType.AlkalineEarth, new[]{2}),
				new ChemElement(23, "V", new string[0], "Vanadium", new[]{49.9471585, 50.9439595}, new[]{0.250, 99.750},
					50.9415, ChemElementType.TransitionMetal, new[]{2, 3, 4, 5}),
				new ChemElement(24, "Cr", new string[0], "Chromium",
					new[]{49.9460442, 51.9405075, 52.9406494, 53.9388804}, new[]{4.345, 83.789, 9.501, 2.365}, 51.9961,
					ChemElementType.TransitionMetal, new[]{2, 3, 6}),
				new ChemElement(25, "Mn", new string[0], "Manganese", new[]{54.9380451}, new[]{100.0}, 54.938045,
					ChemElementType.TransitionMetal, new[]{2, 4, 7}, "7439-96-5"),
				new ChemElement(26, "Fe", new string[0], "Iron", new[]{53.9396148, 55.9349421, 56.9353987, 57.9332805},
					new[]{5.845, 91.754, 2.119, 0.282}, 55.845, ChemElementType.TransitionMetal, new[]{2, 3}),
				new ChemElement(26, "Fex", new string[0], "Iron", new[]{53.9396148, 55.9349421}, new[]{0.0, 100.0},
					55.9349421, ChemElementType.TransitionMetal, new[]{2, 3}, "", true),
				new ChemElement(26, "Fey", new string[0], "Iron", new[]{53.9396148, 56.9353987}, new[]{0.0, 100.0},
					56.9353987, ChemElementType.TransitionMetal, new[]{2, 3}, "", true),
				new ChemElement(28, "Ni", new string[0], "Nickel",
					new[]{57.9353479, 59.9307906, 60.9310604, 61.9283488, 63.9279696},
					new[]{68.0769, 26.2231, 1.1399, 3.6345, 0.9256}, 58.6934, ChemElementType.TransitionMetal,
					new[]{2}),
				new ChemElement(27, "Co", new string[0], "Cobalt", new[]{58.9331950}, new[]{100.0}, 58.933195,
					ChemElementType.TransitionMetal, new[]{2, 3}),
				new ChemElement(29, "Cu", new string[0], "Copper", new[]{62.9296011, 64.9277937}, new[]{69.17, 30.83},
					63.546, ChemElementType.TransitionMetal, new[]{1, 2}),
				new ChemElement(30, "Zn", new string[0], "Zinc",
					new[]{63.9291466, 65.9260368, 66.9271309, 67.9248476, 69.9253250},
					new[]{48.63, 27.90, 4.10, 18.75, 0.62}, 65.409, ChemElementType.TransitionMetal, new[]{2}),
				new ChemElement(31, "Ga", new string[0], "Gallium", new[]{68.9255736, 70.9247013},
					new[]{60.108, 39.892}, 69.723, ChemElementType.BasicMetal, new[]{3}),
				new ChemElement(32, "Ge", new string[0], "Germanium",
					new[]{69.9242474, 71.9220758, 72.9234589, 73.9211778, 75.9214026},
					new[]{20.38, 27.31, 7.76, 36.72, 7.83}, 72.64, ChemElementType.Semimetal, new[]{-4, 2, 4}),
				new ChemElement(33, "As", new string[0], "Arsenic", new[]{74.9215964}, new[]{100.0}, 74.92160,
					ChemElementType.Semimetal, new[]{-3, 3, 5}),
				new ChemElement(34, "Se", new string[0], "Selenium",
					new[]{73.9224766, 75.9192141, 76.9199146, 77.9173095, 79.9165218, 81.9167000},
					new[]{0.89, 9.37, 7.63, 23.77, 49.61, 8.73}, 78.96, ChemElementType.Nonmetal, new[]{-2, 4, 6}),
				new ChemElement(35, "Br", new string[0], "Bromine", new[]{78.9183376, 80.916291}, new[]{50.69, 49.31},
					79.904, ChemElementType.Halogen, new[]{-1, 1, 5}),
				new ChemElement(36, "Kr", new string[0], "Krypton",
					new[]{77.9203648, 79.9163790, 81.9134836, 82.914136, 83.911507, 85.91061073},
					new[]{0.355, 2.286, 11.593, 11.500, 56.987, 17.279}, 83.798, ChemElementType.NobleGas, new[]{0}),
				new ChemElement(37, "Rb", new string[0], "Rubidium", new[]{84.911789738, 86.909180527},
					new[]{72.17, 27.83}, 85.4678, ChemElementType.AlkaliMetal, new[]{1}),
				new ChemElement(38, "Sr", new string[0], "Strontium",
					new[]{83.913425, 85.9092602, 86.9088771, 87.9056121}, new[]{0.56, 9.86, 7.00, 82.58}, 87.62,
					ChemElementType.AlkalineEarth, new[]{2}, "7440-24-6"),
				new ChemElement(39, "Y", new string[0], "Yttrium", new[]{88.9058483}, new[]{100.0}, 88.9058483,
					ChemElementType.TransitionMetal, new[]{3}),
				new ChemElement(40, "Zr", new string[0], "Zirconium",
					new[]{89.9047044, 90.9056458, 91.9050408, 93.9063152, 95.9082734},
					new[]{51.45, 11.22, 17.15, 17.38, 2.80}, 91.224, ChemElementType.TransitionMetal, new[]{4}),
				new ChemElement(41, "Nb", new string[0], "Niobium", new[]{92.9063781}, new[]{100.0}, 92.90638,
					ChemElementType.TransitionMetal, new[]{3, 5}, "7440-03-1"),
				new ChemElement(45, "Rh", new string[0], "Rhodium", new[]{102.905504}, new[]{100.0}, 102.905504,
					ChemElementType.TransitionMetal, new[]{4}),
				new ChemElement(47, "Ag", new string[0], "Silver", new[]{106.905093, 108.904756}, new[]{51.839, 48.161},
					107.8682, ChemElementType.TransitionMetal, new[]{1}),
				new ChemElement(42, "Mo", new string[0], "Molybdenum",
					new[]{91.906810, 93.9050876, 94.9058415, 95.9046789, 96.9060210, 97.9054078, 99.907477},
					new[]{14.84, 9.25, 15.92, 16.68, 9.55, 24.13, 9.63}, 95.94, ChemElementType.TransitionMetal,
					new[]{3, 6}, "7439-98-7"),
				new ChemElement(43, "Tc", new string[0], "Technetium", new[]{98.9062547}, new[]{100.0}, 98.9062547,
					ChemElementType.TransitionMetal, new[]{6}, "7440-26-8"),
				new ChemElement(44, "Ru", new string[0], "Ruthenium",
					new[]{95.907598, 97.905287, 98.9059393, 99.9042195, 100.9055821, 101.9043493, 103.905433},
					new[]{5.54, 1.87, 12.76, 12.60, 17.06, 31.55, 18.62}, 101.07, ChemElementType.TransitionMetal,
					new[]{3, 4, 8}, "7440-18-8"),
				new ChemElement(46, "Pd", new string[0], "Palladium",
					new[]{101.905609, 103.904036, 104.905085, 105.903486, 107.903892, 109.905153},
					new[]{1.02, 11.14, 22.33, 27.33, 26.46, 11.72}, 106.42, ChemElementType.TransitionMetal,
					new[]{2, 4}),
				new ChemElement(48, "Cd", new string[0], "Cadmium",
					new[]{
						105.906459, 107.904184, 109.9030021, 110.9041781, 111.9027578, 112.9044017, 113.9033585,
						115.904756
					}, new[]{1.25, 0.89, 12.49, 12.80, 24.13, 12.22, 28.73, 7.49}, 112.411,
					ChemElementType.TransitionMetal, new[]{2}),
				new ChemElement(51, "Sb", new string[0], "Antimony", new[]{120.9038157, 122.9042140},
					new[]{57.21, 42.79}, 121.760, ChemElementType.Semimetal, new[]{-3, 3, 5}),
				new ChemElement(50, "Sn", new string[0], "Tin",
					new[]{
						111.904818, 113.902779, 114.903342, 115.901741, 116.902952, 117.901603, 118.903308, 119.9021947,
						121.9034390, 123.9052739
					}, new[]{0.97, 0.66, 0.34, 14.54, 7.68, 24.22, 8.59, 32.58, 4.63, 5.79}, 118.710,
					ChemElementType.BasicMetal, new[]{2, 4}),
				new ChemElement(53, "I", new string[0], "Iodine", new[]{126.904468}, new[]{100.0}, 126.90447,
					ChemElementType.Halogen, new[]{-1, 1, 5, 7}, "7553-56-2"),
				new ChemElement(49, "In", new string[0], "Indium", new[]{112.904058, 114.903878}, new[]{4.29, 95.71},
					114.818, ChemElementType.BasicMetal, new[]{3}, "7440-74-6"),
				new ChemElement(52, "Te", new string[0], "Tellurium",
					new[]{
						119.904020, 121.9030439, 122.9042700, 123.9028179, 124.9044307, 125.9033117, 127.904631,
						129.9062244
					}, new[]{0.09, 2.55, 0.89, 4.74, 7.07, 18.84, 31.74, 34.08}, 127.60, ChemElementType.Semimetal,
					new[]{-2, 4, 6}, "13494-80-9"),
				new ChemElement(57, "La", new string[0], "Lanthanum", new[]{137.907112, 138.9063533},
					new[]{0.09, 99.91}, 138.90547, ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(58, "Ce", new string[0], "Cerium",
					new[]{135.907172, 137.905991, 139.9054387, 141.909244}, new[]{0.185, 0.251, 88.450, 11.114},
					140.116, ChemElementType.Lanthanide, new[]{3, 4}),
				new ChemElement(54, "Xe", new string[0], "Xenon",
					new[]{
						123.9058930, 125.904274, 127.9035313, 128.9047794, 129.9035080, 130.9050824, 131.9041535,
						133.9053945, 135.907219
					}, new[]{0.0952, 0.0890, 1.9102, 26.4006, 4.0710, 21.2324, 26.9086, 10.4357, 8.8573}, 131.293,
					ChemElementType.NobleGas, new[]{0}, "7440-63-3"),
				new ChemElement(56, "Ba", new string[0], "Barium",
					new[]{129.9063208, 131.9050613, 133.9045084, 134.9056886, 135.9045759, 136.9058274, 137.9052472},
					new[]{0.106, 0.101, 2.417, 6.592, 7.854, 11.232, 71.698}, 137.327, ChemElementType.AlkalineEarth,
					new[]{2}),
				new ChemElement(55, "Cs", new string[0], "Cesium", new[]{132.905451933}, new[]{100.0}, 132.905451933,
					ChemElementType.AlkaliMetal, new[]{1}),
				new ChemElement(59, "Pr", new string[0], "Praseodymium", new[]{140.9076528}, new[]{100.0}, 140.90765,
					ChemElementType.Lanthanide, new[]{3}, "7440-10-0"),
				new ChemElement(60, "Nd", new string[0], "Neodymium",
					new[]{141.9077233, 142.9098143, 143.9100873, 144.9125736, 145.9131169, 147.916893, 149.920891},
					new[]{27.2, 12.2, 23.8, 8.3, 17.2, 5.7, 5.6}, 144.242, ChemElementType.Lanthanide, new[]{3, 4},
					"7440-00-8"),
				new ChemElement(61, "Pm", new string[0], "Promethium", new[]{144.912749}, new[]{100.0}, 144.912749,
					ChemElementType.Lanthanide, new[]{3}, "7440-12-2"),
				new ChemElement(63, "Eu", new string[0], "Europium", new[]{150.9198502, 152.9212303},
					new[]{47.81, 52.19}, 151.964, ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(62, "Sm", new string[0], "Samarium",
					new[]{143.911999, 146.9148979, 147.9148227, 148.9171847, 149.9172755, 151.9197324, 153.9222093},
					new[]{3.07, 14.99, 11.24, 13.82, 7.38, 26.75, 22.75}, 150.36, ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(64, "Gd", new string[0], "Gadolinium",
					new[]{151.9197910, 153.9208656, 154.9226220, 155.9221227, 156.9239601, 157.9241039, 159.9270541},
					new[]{0.20, 2.18, 14.80, 20.47, 15.65, 24.84, 21.86}, 157.25, ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(65, "Tb", new string[0], "Terbium", new[]{158.9253468}, new[]{100.0}, 158.92535,
					ChemElementType.Lanthanide, new[]{3, 4}),
				new ChemElement(66, "Dy", new string[0], "Dysprosium",
					new[]{155.924283, 157.924409, 159.9251975, 160.9269334, 161.9267984, 162.9287312, 163.9291748},
					new[]{0.056, 0.095, 2.329, 18.889, 25.475, 24.896, 28.260}, 162.500, ChemElementType.Lanthanide,
					new[]{3}),
				new ChemElement(67, "Ho", new string[0], "Holmium", new[]{164.9303221}, new[]{100.0}, 164.93032,
					ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(68, "Er", new string[0], "Erbium",
					new[]{161.928778, 163.929200, 165.9302931, 166.9320482, 167.9323702, 169.9354643},
					new[]{0.139, 1.601, 33.503, 22.869, 26.978, 14.910}, 167.259, ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(69, "Tm", new string[0], "Thulium", new[]{168.9342133}, new[]{100.0}, 168.93421,
					ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(70, "Yb", new string[0], "Ytterbium",
					new[]{167.933897, 169.9347618, 170.9363258, 171.9363815, 172.9382108, 173.9388621, 175.9425717},
					new[]{0.13, 3.04, 14.28, 21.83, 16.13, 31.83, 12.76}, 173.054, ChemElementType.Lanthanide,
					new[]{3}),
				new ChemElement(71, "Lu", new string[0], "Lutetium", new[]{174.9407718, 175.9426863},
					new[]{97.41, 2.59}, 174.9668, ChemElementType.Lanthanide, new[]{3}),
				new ChemElement(72, "Hf", new string[0], "Hafnium",
					new[]{173.940046, 175.9414086, 176.9432207, 177.9436988, 178.9458161, 179.9465500},
					new[]{0.16, 5.26, 18.60, 27.28, 13.62, 35.08}, 178.49, ChemElementType.TransitionMetal, new[]{4}),
				new ChemElement(73, "Ta", new string[0], "Tantalum", new[]{179.9474648, 180.9479958},
					new[]{0.012, 99.988}, 180.94788, ChemElementType.TransitionMetal, new[]{5}),
				new ChemElement(75, "Re", new string[0], "Rhenium", new[]{184.9529550, 186.9557531},
					new[]{37.40, 62.60}, 186.207, ChemElementType.TransitionMetal, new[]{2, 4, 6, 7}),
				new ChemElement(77, "Ir", new string[0], "Iridium", new[]{190.9605940, 192.9629264}, new[]{37.3, 62.7},
					192.217, ChemElementType.TransitionMetal, new[]{3, 4, 6}),
				new ChemElement(74, "W", new string[0], "Tungsten",
					new[]{179.946704, 181.9482042, 182.9502230, 183.9509312, 185.9543641},
					new[]{0.12, 26.50, 14.31, 30.64, 28.43}, 183.84, ChemElementType.TransitionMetal, new[]{6}),
				new ChemElement(76, "Os", new string[0], "Osmium",
					new[]{183.9524891, 185.9538382, 186.9557505, 187.9558382, 188.9581475, 189.9584470, 191.9614807},
					new[]{0.02, 1.59, 1.96, 13.24, 16.15, 26.26, 40.78}, 190.23, ChemElementType.TransitionMetal,
					new[]{3, 4, 6, 8}),
				new ChemElement(78, "Pt", new string[0], "Platinum",
					new[]{189.959932, 191.9610380, 193.9626803, 194.9647911, 195.9649515, 197.967893},
					new[]{0.014, 0.782, 32.967, 33.832, 25.242, 7.163}, 195.084, ChemElementType.TransitionMetal,
					new[]{2, 4, 6}),
				new ChemElement(79, "Au", new string[0], "Gold", new[]{196.966552}, new[]{100.0}, 196.96655,
					ChemElementType.TransitionMetal, new[]{1, 3}),
				new ChemElement(80, "Hg", new string[0], "Mercury",
					new[]{195.965815, 197.966752, 198.968262, 199.968309, 200.970285, 201.970626, 203.973476},
					new[]{0.15, 9.97, 16.87, 23.10, 13.18, 29.86, 6.87}, 200.59, ChemElementType.TransitionMetal,
					new[]{1, 2}),
				new ChemElement(82, "Pb", new string[0], "Lead",
					new[]{203.9730436, 205.9744653, 206.9758969, 207.9766521}, new[]{1.4, 24.1, 22.1, 52.4}, 207.2,
					ChemElementType.BasicMetal, new[]{2, 4}),
				new ChemElement(81, "Tl", new string[0], "Thallium", new[]{202.9723442, 204.9744275},
					new[]{29.52, 70.48}, 204.3833, ChemElementType.BasicMetal, new[]{1, 3}),
				new ChemElement(83, "Bi", new string[0], "Bismuth", new[]{208.9803987}, new[]{100.0}, 208.98040,
					ChemElementType.BasicMetal, new[]{3}),
				new ChemElement(84, "Po", new string[0], "Polonium", new[]{208.9824304}, new[]{100.0}, 208.9824304,
					ChemElementType.Semimetal, new[]{2, 4}),
				new ChemElement(85, "At", new string[0], "Astatine", new[]{209.987148}, new[]{100.0}, 209.987148,
					ChemElementType.Halogen),
				new ChemElement(86, "Rn", new string[0], "Radon", new[]{222.0175777}, new[]{100.0}, 222.0175777,
					ChemElementType.NobleGas, new[]{0}),
				new ChemElement(87, "Fr", new string[0], "Francium", new[]{223.0197359}, new[]{100.0}, 223.0197359,
					ChemElementType.AlkaliMetal),
				new ChemElement(88, "Ra", new string[0], "Radium", new[]{226.0254098}, new[]{100.0}, 226.0254098,
					ChemElementType.AlkalineEarth, new[]{0}),
				new ChemElement(89, "Ac", new string[0], "Actinium", new[]{227.0277521}, new[]{100.0}, 227.0277521,
					ChemElementType.Actinide, new[]{3}),
				new ChemElement(90, "Th", new string[0], "Thorium", new[]{232.0380553}, new[]{100.0}, 232.0380553,
					ChemElementType.Actinide, new[]{4}),
				new ChemElement(91, "Pa", new string[0], "Protactinium", new[]{231.0358840}, new[]{100.0}, 231.0358840,
					ChemElementType.Actinide, new[]{5}),
				new ChemElement(92, "U", new string[0], "Uranium", new[]{234.0409521, 235.0439299, 238.0507882},
					new[]{0.0054, 0.7204, 99.2742}, 238.02891, ChemElementType.Actinide, new[]{3, 4, 6}),
				new ChemElement(93, "Np", new string[0], "Neptunium", new[]{237.0481734}, new[]{100.0}, 237.0481734,
					ChemElementType.Actinide),
				new ChemElement(94, "Pu", new string[0], "Plutonium", new[]{244.064204}, new[]{100.0}, 244.064204,
					ChemElementType.Actinide),
				new ChemElement(95, "Am", new string[0], "Americium", new[]{243.0613811}, new[]{100.0}, 243.0613811,
					ChemElementType.Actinide),
				new ChemElement(96, "Cm", new string[0], "Curium", new[]{247.070354}, new[]{100.0}, 247.070354,
					ChemElementType.Actinide),
				new ChemElement(97, "Bk", new string[0], "Berkelium", new[]{247.070307}, new[]{100.0}, 247.070307,
					ChemElementType.Actinide),
				new ChemElement(98, "Cf", new string[0], "Californium", new[]{251.079587}, new[]{100.0}, 251.079587,
					ChemElementType.Actinide),
				new ChemElement(99, "Es", new string[0], "Einsteinium", new[]{252.082980}, new[]{100.0}, 252.082980,
					ChemElementType.Actinide),
				new ChemElement(100, "Fm", new string[0], "Fermium", new[]{257.095105}, new[]{100.0}, 257.095105,
					ChemElementType.Actinide),
				new ChemElement(101, "Md", new string[0], "Mendelevium", new[]{258.098431}, new[]{100.0}, 258.098431,
					ChemElementType.Actinide),
				new ChemElement(102, "No", new string[0], "Nobelium", new[]{259.10103}, new[]{100.0}, 259.10103,
					ChemElementType.Actinide),
				new ChemElement(103, "Lr", new string[0], "Lawrencium", new[]{262.10963}, new[]{100.0}, 262.10963,
					ChemElementType.Actinide, null, "22537-19-5"),
				new ChemElement(104, "Rf", new string[0], "Rutherfordium", new[]{265.11670}, new[]{100.0}, 265.11670,
					ChemElementType.TransitionMetal, null, "53850-36-5"),
				new ChemElement(105, "Db", new string[0], "Dubnium", new[]{268.12545}, new[]{100.0}, 268.12545,
					ChemElementType.TransitionMetal, null, "53850-35-4"),
				new ChemElement(106, "Sg", new string[0], "Seaborgium", new[]{271.13347}, new[]{100.0}, 271.13347,
					ChemElementType.TransitionMetal, null, "54038-81-2"),
				new ChemElement(107, "Bh", new string[0], "Bohrium", new[]{272.13803}, new[]{100.0}, 272.13803,
					ChemElementType.TransitionMetal, null, "54037-14-8"),
				new ChemElement(108, "Hs", new string[0], "Hassium", new[]{270.13465}, new[]{100.0}, 270.13465,
					ChemElementType.TransitionMetal, null, "54037-57-9"),
				new ChemElement(109, "Mt", new string[0], "Meitnerium", new[]{276.15116}, new[]{100.0}, 276.15116,
					ChemElementType.TransitionMetal, null, "54038-01-6"),
				new ChemElement(110, "Ds", new string[0], "Darmstadtium", new[]{281.16206}, new[]{100.0}, 281.16206,
					ChemElementType.TransitionMetal, null, "54083-77-1"),
				new ChemElement(111, "Rg", new string[0], "Roentgenium", new[]{280.16447}, new[]{100.0}, 280.16447,
					ChemElementType.TransitionMetal, null, "54386-24-2"),
				new ChemElement(112, "Cn", new string[0], "Copernicium", new[]{285.17411}, new[]{100.0}, 285.17411,
					ChemElementType.TransitionMetal, null, "54084-26-3")
			};
			foreach (ChemElement ce in e.Where(ce => ce.IsIsotopicLabel)){
				SetNaturalVersion(ce, e);
			}
			return e;
		}

		private static void SetNaturalVersion(ChemElement ce, IList<ChemElement> chemElements){
			string label = ce.Symbol;
			if (label.Equals("D")){
				ce.NaturalVersion = GetElementIndexByName("H", chemElements);
				return;
			}
			if (label.Equals("T")){
				ce.NaturalVersion = GetElementIndexByName("H", chemElements);
				return;
			}
			if (label.Equals("Fey")){
				ce.NaturalVersion = GetElementIndexByName("Fe", chemElements);
				return;
			}
			if (label.Equals("Oy")){
				ce.NaturalVersion = GetElementIndexByName("O", chemElements);
				return;
			}
			if (label.Equals("Sy")){
				ce.NaturalVersion = GetElementIndexByName("S", chemElements);
				return;
			}
			if (!label.EndsWith("x")){
				throw new Exception("Never get here.");
			}
			label = label.Substring(0, label.Length - 1);
			ce.NaturalVersion = GetElementIndexByName(label, chemElements);
		}

		private static int GetElementIndexByName(string label, IList<ChemElement> chemElements){
			for (int i = 0; i < chemElements.Count; i++){
				ChemElement element = chemElements[i];
				if (element.Symbol.Equals(label)){
					return i;
				}
			}
			throw new Exception("Never get here.");
		}

		public static string[] GetSymbols(){
			string[] result = new string[Elements.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = elements[i].Symbol;
			}
			return result;
		}

		public static void DecodeComposition(string composition, Dictionary<string, ChemElement> elements1,
			out int[] counts, out string[] comp, out double[] monoMasses){
			string[] matches = composition.Split(new[]{" "}, StringSplitOptions.RemoveEmptyEntries);
			counts = new int[matches.Length];
			comp = new string[matches.Length];
			monoMasses = new double[matches.Length];
			Regex pattern2 = new Regex("\\(([-]*[0-9]+)\\)");
			Regex pattern3 = new Regex("[0-9]+");
			for (int i = 0; i < matches.Length; i++){
				string key;
				if (pattern2.IsMatch(matches[i])){
					Match match = pattern2.Match(matches[i]);
					string temp = match.Value;
					key = matches[i].Replace(temp, "");
					counts[i] = Convert.ToInt32(temp.Replace("(", "").Replace(")", "").Trim());
				} else{
					key = matches[i];
					counts[i] = 1;
				}
				if (elements1.ContainsKey(key)){
					ChemElement element = elements1[key];
					comp[i] = element.Symbol;
					monoMasses[i] = element.MonoIsotopicMass;
				} else if (pattern3.IsMatch(key)){
					Match match = pattern3.Match(key);
					string key2 = key.Replace(match.Value, "");
					if (elements1.ContainsKey(key2 + "x")){
						ChemElement element = elements1[key2 + "x"];
						monoMasses[i] = element.MonoIsotopicMass;
						comp[i] = element.Symbol;
					} else{
						throw new Exception();
					}
				} else{
					throw new Exception();
				}
			}
		}

		public static double GetMassFromComposition(string composition){
			if (composition == null){
				return 0;
			}
			DecodeComposition(composition, ElementDictionary, out int[] counts, out _, out double[] mono);
			double deltaMass = 0;
			for (int i = 0; i < mono.Length; i++){
				deltaMass += mono[i] * counts[i];
			}
			return deltaMass;
		}
	}
}