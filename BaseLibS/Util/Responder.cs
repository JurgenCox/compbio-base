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
			logFile = Responder.GetStatusFile(title, infoFolder) + ".log.txt";
			commentFile = Responder.GetStatusFile(title, infoFolder) + ".comment.txt";
			progressFile = Responder.GetStatusFile(title, infoFolder) + ".progress.txt";
		}

		public Responder(){ }

		public void Log(string s){
			if (string.IsNullOrEmpty(logFile) || string.IsNullOrEmpty(s)){
				return;
			}
			if (logWriter == null){
				logWriter = new StreamWriter(logFile);
			}
			try{
				logWriter.WriteLine(GetLogPrefix() + s);
			} catch (Exception){ }
		}

		private static string GetLogPrefix(){
			return DateTime.Now.ToString(CultureInfo.InvariantCulture) + ": ";
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

		private DateTime lastProgress = DateTime.MinValue;

		public void Progress(double x){
			DateTime now = DateTime.Now;
			TimeSpan diff = now - lastProgress;
			if (diff.TotalSeconds < 5){
				return;
			}
			lastProgress = now;
			if (string.IsNullOrEmpty(progressFile) || double.IsNaN(x) || double.IsInfinity(x)){
				return;
			}
			x = Math.Min(x, 1);
			x = Math.Max(x, 0);
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