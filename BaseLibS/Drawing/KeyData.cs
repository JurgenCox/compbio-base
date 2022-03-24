namespace BaseLibS.Drawing{
	public static class KeyData{
		public static char GetChar(Keys2 keyData, int keyboardId){
			switch (keyData){
				case Keys2.A | Keys2.Shift:
				case Keys2.B | Keys2.Shift:
				case Keys2.C | Keys2.Shift:
				case Keys2.D | Keys2.Shift:
				case Keys2.E | Keys2.Shift:
				case Keys2.F | Keys2.Shift:
				case Keys2.G | Keys2.Shift:
				case Keys2.H | Keys2.Shift:
				case Keys2.I | Keys2.Shift:
				case Keys2.J | Keys2.Shift:
				case Keys2.K | Keys2.Shift:
				case Keys2.L | Keys2.Shift:
				case Keys2.M | Keys2.Shift:
				case Keys2.N | Keys2.Shift:
				case Keys2.O | Keys2.Shift:
				case Keys2.P | Keys2.Shift:
				case Keys2.Q | Keys2.Shift:
				case Keys2.R | Keys2.Shift:
				case Keys2.S | Keys2.Shift:
				case Keys2.T | Keys2.Shift:
				case Keys2.U | Keys2.Shift:
				case Keys2.V | Keys2.Shift:
				case Keys2.W | Keys2.Shift:
				case Keys2.X | Keys2.Shift:
				case Keys2.Y | Keys2.Shift:
				case Keys2.Z | Keys2.Shift:
				case Keys2.D0:
				case Keys2.D1:
				case Keys2.D2:
				case Keys2.D3:
				case Keys2.D4:
				case Keys2.D5:
				case Keys2.D6:
				case Keys2.D7:
				case Keys2.D8:
				case Keys2.D9:
					return (char) keyData;
				case Keys2.A:
				case Keys2.B:
				case Keys2.C:
				case Keys2.D:
				case Keys2.E:
				case Keys2.F:
				case Keys2.G:
				case Keys2.H:
				case Keys2.I:
				case Keys2.J:
				case Keys2.K:
				case Keys2.L:
				case Keys2.M:
				case Keys2.N:
				case Keys2.O:
				case Keys2.P:
				case Keys2.Q:
				case Keys2.R:
				case Keys2.S:
				case Keys2.T:
				case Keys2.U:
				case Keys2.V:
				case Keys2.W:
				case Keys2.X:
				case Keys2.Y:
				case Keys2.Z:
					return char.ToLower((char) keyData);
				case Keys2.NumPad0:
					return '0';
				case Keys2.NumPad1:
					return '1';
				case Keys2.NumPad2:
					return '2';
				case Keys2.NumPad3:
					return '3';
				case Keys2.NumPad4:
					return '4';
				case Keys2.NumPad5:
					return '5';
				case Keys2.NumPad6:
					return '6';
				case Keys2.NumPad7:
					return '7';
				case Keys2.NumPad8:
					return '8';
				case Keys2.NumPad9:
					return '9';
				default:
					switch (keyboardId){
						case 0x00000407:
							return GetCharLocaleGerman(keyData);
						default:
							return GetCharLocaleUnitedStatesEnglish(keyData);
					}
			}
		}

		//TODO
		public static char GetCharLocaleUnitedStatesEnglish(Keys2 keyData) {
			switch (keyData) {
				case Keys2.D1 | Keys2.Shift:
					return '!';
				case Keys2.D2 | Keys2.Shift:
					return '@';
				case Keys2.D3 | Keys2.Shift:
					return '#';
				case Keys2.D4 | Keys2.Shift:
					return '$';
				case Keys2.D5 | Keys2.Shift:
					return '%';
				case Keys2.D6 | Keys2.Shift:
					return '^';
				case Keys2.D7 | Keys2.Shift:
					return '&';
				case Keys2.D8 | Keys2.Shift:
					return '*';
				case Keys2.D9 | Keys2.Shift:
					return '(';
				case Keys2.D0 | Keys2.Shift:
					return ')';
				case Keys2.Oemplus:
					return '=';
				case Keys2.Oemplus | Keys2.Shift:
					return '+';
				case Keys2.OemMinus:
					return '-';
				case Keys2.OemMinus | Keys2.Shift:
					return '_';
				case Keys2.OemPeriod:
					return '.';
				case Keys2.OemPeriod | Keys2.Shift:
					return '>';
				case Keys2.Oemcomma:
					return ',';
				case Keys2.Oemcomma | Keys2.Shift:
					return '<';
				case Keys2.OemQuestion:
					return '/';
				case Keys2.OemQuestion | Keys2.Shift:
					return '?';
				default:
					return (char)0;
			}
		}

		public static char GetCharLocaleGerman(Keys2 keyData) {
			switch (keyData) {
				case Keys2.D1 | Keys2.Shift:
					return '!';
				case Keys2.D2 | Keys2.Shift:
					return '"';
				case Keys2.D3 | Keys2.Shift:
					return '§';
				case Keys2.D4 | Keys2.Shift:
					return '$';
				case Keys2.D5 | Keys2.Shift:
					return '%';
				case Keys2.D6 | Keys2.Shift:
					return '&';
				case Keys2.D7 | Keys2.Shift:
					return '/';
				case Keys2.D8 | Keys2.Shift:
					return '(';
				case Keys2.D9 | Keys2.Shift:
					return ')';
				case Keys2.D0 | Keys2.Shift:
					return '=';
				case Keys2.Oemplus:
					return '+';
				case Keys2.Oemplus | Keys2.Shift:
					return '*';
				case Keys2.OemMinus:
					return '-';
				case Keys2.OemMinus | Keys2.Shift:
					return '_';
				case Keys2.OemPeriod:
					return '.';
				case Keys2.OemPeriod | Keys2.Shift:
					return ':';
				case Keys2.Oemcomma:
					return ',';
				case Keys2.Oemcomma | Keys2.Shift:
					return ',';
				case Keys2.OemQuestion:
					return 'ß';
				case Keys2.OemQuestion | Keys2.Shift:
					return '?';
				default:
					return (char)0;
			}
		}


	}
}