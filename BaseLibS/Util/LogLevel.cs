namespace BaseLibS.Util{
	public enum LogLevel{
		Emergency = 0, //System unusable
		Alert = 1, //Immediate action needed
		Critical = 2, //Critical condition—default level
		Error = 3, //Error condition
		Warning = 4, //Warning condition
		Notification = 5, //Normal but significant condition
		Informational = 6, //Informational message only
		Debugging = 7 //Appears during debugging only
	}
}