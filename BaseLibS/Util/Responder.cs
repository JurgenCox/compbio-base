namespace BaseLibS.Util {
	public class Responder {
		private string infoFolder;
		private string title;
		public Responder(string infoFolder, string title) {
			this.infoFolder = infoFolder;
			this.title = title;
		}
		public void Comment(string s) { }
		public void DoneFraction(double x) { }
	}
}