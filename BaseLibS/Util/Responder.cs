using System;
using System.Globalization;
using System.IO;

namespace BaseLibS.Util{
	public class Responder{
		private readonly string logFile;
		private readonly string commentFile;
		private readonly string progressFile;
		private StreamWriter logWriter;

		public Responder(string infoFolder, string title){
			if (string.IsNullOrEmpty(infoFolder)){
				return;
			}
			logFile = Responder.GetStatusFile(title, infoFolder) + ".log.txt";
			commentFile = Responder.GetStatusFile(title, infoFolder) + ".comment.txt";
			progressFile = Responder.GetStatusFile(title, infoFolder) + ".progress.txt";
		}

		public Responder(){ }

		public void Log(string s){
			Log(s, LogLevel.Debugging);
		}

		public void Log(string s, LogLevel level){
			if (string.IsNullOrEmpty(logFile) || string.IsNullOrEmpty(s)){
				return;
			}
			if (logWriter == null){
				logWriter = new StreamWriter(logFile){AutoFlush = true};
			}
			try{
				logWriter.WriteLine(GetLogPrefix(level) + s);
			} catch (Exception){ }
		}

		private static string GetLogPrefix(LogLevel level){
			return GetLevelString(level) + ":" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + ": ";
		}

		private static string GetLevelString(LogLevel level){
			switch (level){
				case LogLevel.Debugging:
					return "DEBUG";
				case LogLevel.Alert:
					return "ALERT";
				case LogLevel.Critical:
					return "CRITICAL";
				case LogLevel.Emergency:
					return "EMERGENCY";
				case LogLevel.Error:
					return "ERROR";
				case LogLevel.Informational:
					return "INFO";
				case LogLevel.Notification:
					return "NOTIFICATION";
				case LogLevel.Warning:
					return "WARNING";
				default:
					throw new Exception("Never get here");
			}
		}

		public void Comment(string s){
			if (string.IsNullOrEmpty(commentFile) || string.IsNullOrEmpty(s)){
				return;
			}
			try{
				File.Delete(commentFile);
				StreamWriter writer = new StreamWriter(commentFile);
				writer.WriteLine(s);
				writer.Close();
			} catch (Exception){ }
		}

		private DateTime lastProgressTime = DateTime.MinValue;
		private double lastProgressValue = -1;

		public void Progress(double x){
			if (double.IsNaN(x) || double.IsInfinity(x)){
				return;
			}
			DateTime now = DateTime.Now;
			TimeSpan diff = now - lastProgressTime;
			if (diff.TotalSeconds < 5){
				return;
			}
			lastProgressTime = now;
			if (string.IsNullOrEmpty(progressFile)){
				return;
			}
			x = Math.Min(x, 1);
			x = Math.Max(x, 0);
			if (x - lastProgressValue < 0.001){
				return;
			}
			lastProgressValue = x;
			try{
				File.Delete(progressFile);
				StreamWriter writer = new StreamWriter(progressFile);
				writer.WriteLine(x);
				writer.Close();
			} catch (Exception){ }
		}

		public static string GetStatusFile(string name, string infoFolder){
			if (string.IsNullOrEmpty(infoFolder)){
				throw new Exception("Given string for proc folder is null or empty.");
			}
			if (!Directory.Exists(infoFolder)){
				throw new Exception("Given path for proc folder does not exist. Path=" + infoFolder);
			}
			name = StringUtils.Replace(name, new[]{"\\", "(", ")", "/"}, "");
			return Path.Combine(infoFolder, name);
		}
	}
}