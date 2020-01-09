namespace BaseLibS.Num.Test.Univariate.NSample {
	public static class MultipleSampleTests {
		public static MultipleSampleTest[] allTests = { new OneWayAnovaTest() };
		//, new KruskalWallisTest()
		public static string[] allNames;

		static MultipleSampleTests() {
			allNames = new string[allTests.Length];
			for (int i = 0; i < allNames.Length; i++) {
				allNames[i] = allTests[i].Name;
			}
		}
	}
}
