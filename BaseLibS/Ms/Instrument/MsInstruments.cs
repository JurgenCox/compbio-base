using System;
using System.Linq;
using BaseLibS.Num;
namespace BaseLibS.Ms.Instrument{
	public static class MsInstruments{
		public static readonly MsInstrument thermoOrbi = new ThermoFtmsInstrument(0);
		public static readonly MsInstrument brukerTof = new BrukerQtof(1);
		public static readonly MsInstrument sciexTof = new SciexQtof(2);
		public static readonly MsInstrument agilentTof = new AgilentQtof(3);
		public static readonly MsInstrument brukerTims = new BrukerTims(4);
		public static readonly MsInstrument watersTof = new WatersQtof(5);
		public static readonly MsInstrument shimadzuTof = new ShimadzuQtof(6);

		private static readonly MsInstrument[] allInstrumentsRestricted = {
			thermoOrbi, brukerTof, sciexTof, agilentTof, brukerTims, watersTof
		};

		private static readonly MsInstrument[] allInstrumentsExtended =
			ArrayUtils.Concat(allInstrumentsRestricted, new[]{shimadzuTof});

		public static MsInstrument[] GetAllInstruments(bool extended){
			return extended ? allInstrumentsExtended : allInstrumentsRestricted;
		}

		public static string[] GetAllInstrumentsString(bool extended){
			MsInstrument[] ai = GetAllInstruments(extended);
			string[] result = new string[ai.Length];
			for (int i = 0; i < result.Length; i++){
				result[i] = ai[i].Name;
			}
			return result;
		}

		public static MsInstrument FromName(string name){
			if (name == null){
				return null;
			}
			foreach (MsInstrument t in allInstrumentsExtended.Where(t => t.Name.ToLower().Equals(name.ToLower()))){
				return t;
			}
			throw new Exception("Never get here.");
		}
	}
}