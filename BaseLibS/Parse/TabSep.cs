using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Num;
using BaseLibS.Util;

namespace BaseLibS.Parse {
	public static class TabSep {
		public static string[] GetColumn(string columnName, string filename, char separator) {
			return GetColumn(columnName, filename, 0, separator);
		}

		public static string[] GetColumn(string columnName, string filename, int nskip, char separator) {
			return GetColumns(new[] {columnName}, filename, nskip, separator)[0];
		}

		public static double[][] GetDoubleColumns(string[] columnNames, string filename, char separator) {
			return GetDoubleColumns(columnNames, filename, 0, separator);
		}

		public static double[][] GetDoubleColumns(string[] columnNames, string filename, int nskip, char separator) {
			return GetDoubleColumns(columnNames, filename, double.NaN, nskip, separator);
		}

		public static double[][] GetDoubleColumns(string[] columnNames, string filename, double defaultValue, int nskip,
			char separator) {
			string[][] x = GetColumns(columnNames, filename, nskip, separator);
			double[][] d = new double[x.Length][];
			for (int i = 0; i < d.Length; i++) {
				d[i] = new double[x[i].Length];
				for (int j = 0; j < d[i].Length; j++) {
					d[i][j] = Parser.TryDouble(x[i][j], out double w)
						? w
						: defaultValue;
				}
			}
			return d;
		}

		public static double[] GetDoubleColumn(string columnName, string filename, char separator) {
			return GetDoubleColumn(columnName, filename, double.NaN, 0, separator);
		}

		public static bool[] GetBoolColumn(string columnName, string filename, char separator) {
			return GetBoolColumn(columnName, filename, false, 0, separator);
		}

		public static double[] GetDoubleColumn(string columnName, string filename, int nskip, char separator) {
			return GetDoubleColumn(columnName, filename, double.NaN, nskip, separator);
		}

		public static double[] GetDoubleColumn(string columnName, string filename, double defaultValue, int nskip,
			char separator) {
			string[] x = GetColumn(columnName, filename, nskip, separator);
			double[] d = new double[x.Length];
			for (int i = 0; i < d.Length; i++) {
				d[i] = Parser.TryDouble(x[i], out double w) ? w : defaultValue;
			}
			return d;
		}

		public static bool[] GetBoolColumn(string columnName, string filename, bool defaultValue, int nskip,
			char separator) {
			string[] x = GetColumn(columnName, filename, nskip, separator);
			bool[] d = new bool[x.Length];
			for (int i = 0; i < d.Length; i++) {
				d[i] = bool.TryParse(x[i], out bool w) ? w : defaultValue;
			}
			return d;
		}

		public static float[][] GetFloatColumns(string[] columnNames, string filename, int nskip, char separator) {
			return GetFloatColumns(columnNames, filename, float.NaN, nskip, separator);
		}

		public static float[][] GetFloatColumns(string[] columnNames, string filename, float defaultValue, int nskip,
			char separator) {
			string[][] x = GetColumns(columnNames, filename, nskip, separator);
			float[][] d = new float[x.Length][];
			for (int i = 0; i < d.Length; i++) {
				d[i] = new float[x[i].Length];
				for (int j = 0; j < d[i].Length; j++) {
					d[i][j] = Parser.TryFloat(x[i][j], out float w) ? w : defaultValue;
				}
			}
			return d;
		}

		public static float[,] GetFloatColumns2D(string[] columnNames, string filename, float defaultValue, int nskip,
			char separator) {
			string[][] x = GetColumns(columnNames, filename, nskip, separator);
			float[,] d = new float[x[0].Length, x.Length];
			for (int i = 0; i < d.GetLength(0); i++) {
				for (int j = 0; j < d.GetLength(1); j++) {
					d[i, j] = Parser.TryFloat(x[j][i], out float w) ? w : defaultValue;
				}
			}
			return d;
		}

		public static float[] GetFloatColumn(string columnName, string filename, int nskip, char separator) {
			return GetFloatColumn(columnName, filename, float.NaN, nskip, separator);
		}

		public static float[] GetFloatColumn(string columnName, string filename, char separator) {
			return GetFloatColumn(columnName, filename, float.NaN, 0, separator);
		}

		public static float[] GetFloatColumn(string columnName, string filename, float defaultValue, int nskip,
			char separator) {
			string[] x = GetColumn(columnName, filename, nskip, separator);
			float[] d = new float[x.Length];
			for (int i = 0; i < d.Length; i++) {
				d[i] = Parser.TryFloat(x[i], out float w) ? w : defaultValue;
			}
			return d;
		}

		public static int[] GetIntColumn(string columnName, string filename, char separator) {
			return GetIntColumn(columnName, filename, -1, 0, separator);
		}

		public static int[] GetIntColumn(string columnName, string filename, int defaultValue, int nskip, char separator) {
			string[] x = GetColumn(columnName, filename, nskip, separator);
			int[] d = new int[x.Length];
			for (int i = 0; i < d.Length; i++) {
				d[i] = Parser.TryInt(x[i], out int w) ? w : defaultValue;
			}
			return d;
		}

		public static string[][] GetColumns(string[] columnNames, string filename, int nskip, char separator) {
			return GetColumns(columnNames, filename, nskip, null, null, separator);
		}

		public static string[][] GetColumns(string[] columnNames, string filename, int nskip, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions, char separator) {
			StreamReader reader = FileUtils.GetReader(filename);
			for (int i = 0; i < nskip; i++) {
				reader.ReadLine();
			}
			string line = reader.ReadLine();
			if (commentPrefix != null) {
				while (IsCommentLine(line, commentPrefix, commentPrefixExceptions)) {
					line = reader.ReadLine();
				}
			}
			string[] titles = line.Split(separator);
			int[] colIndices = new int[columnNames.Length];
			for (int i = 0; i < columnNames.Length; i++) {
				colIndices[i] = -1;
				for (int j = 0; j < titles.Length; j++) {
					if (titles[j].Trim().Equals(columnNames[i])) {
						colIndices[i] = j;
						break;
					}
				}
				if (colIndices[i] == -1) {
					throw new ArgumentException("Column " + columnNames[i] + " does not exist.");
				}
			}
			List<string[]> x = new List<string[]>();
			while ((line = reader.ReadLine()) != null) {
				if (line.Trim().Length == 0) {
					continue;
				}
				string[] w = line.Split(separator);
				string[] z;
				try {
					z = ArrayUtils.SubArray(w, colIndices);
				} catch (Exception) {
					continue;
				}
				for (int i = 0; i < z.Length; i++) {
					if (z[i].StartsWith("\"") && z[i].EndsWith("\"")) {
						z[i] = z[i].Substring(1, z[i].Length - 2);
					}
				}
				x.Add(z);
			}
			reader.Close();
			string[][] result = new string[columnNames.Length][];
			for (int i = 0; i < columnNames.Length; i++) {
				result[i] = new string[x.Count];
			}
			for (int i = 0; i < x.Count; i++) {
				for (int j = 0; j < columnNames.Length; j++) {
					result[j][i] = x[i][j];
				}
			}
			return result;
		}

		public static bool HasColumn(string columnName, string filename, char separator) {
			return HasColumn(columnName, filename, 0, separator);
		}

		public static bool HasColumn(string columnName, string filename, int nskip, char separator) {
			return HasColumn(columnName, filename, nskip, null, null, separator);
		}

		public static bool HasColumn(string columnName, string filename, int nskip, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions, char separator) {
			StreamReader reader = FileUtils.GetReader(filename);
			for (int i = 0; i < nskip; i++) {
				reader.ReadLine();
			}
			string line = reader.ReadLine();
			if (commentPrefix != null) {
				while (IsCommentLine(line, commentPrefix, commentPrefixExceptions)) {
					line = reader.ReadLine();
				}
			}
			reader.Close();
			string[] titles = line.Split(separator);
			foreach (string t in titles) {
				if (t.Trim().Equals(columnName)) {
					return true;
				}
			}
			return false;
		}

		public static string[][] GetColumnsIfContains(string[] columnNames, string controlColumn, string controlValue,
			string filename, bool inverse, int nskip, char separator) {
			string[] allNames = new string[columnNames.Length + 1];
			for (int i = 0; i < columnNames.Length; i++) {
				allNames[i] = columnNames[i];
			}
			allNames[columnNames.Length] = controlColumn;
			string[][] x = GetColumns(allNames, filename, nskip, separator);
			string[] controlColumnValues = x[columnNames.Length];
			List<int> valids = new List<int>();
			for (int i = 0; i < controlColumnValues.Length; i++) {
				bool contains = controlColumnValues[i].Contains(controlValue);
				if ((contains && !inverse) || (!contains && inverse)) {
					valids.Add(i);
				}
			}
			int[] v = valids.ToArray();
			string[][] result = new string[columnNames.Length][];
			for (int i = 0; i < columnNames.Length; i++) {
				result[i] = ArrayUtils.SubArray(x[i], v);
				for (int j = 0; j < result[i].Length; j++) {
					if (result[i][j].StartsWith("\"") && result[i][j].EndsWith("\"")) {
						result[i][j] = result[i][j].Substring(1, result[i][j].Length - 2);
					}
				}
			}
			return result;
		}

		public static string[] GetColumnIfContains(string columnName, string controlColumn, string controlValue,
			string filename, bool inverse, int nskip, char separator) {
			return GetColumnsIfContains(new[] {columnName}, controlColumn, controlValue, filename, inverse, nskip, separator)[0];
		}

		public static double[] GetDoubleColumnIfContains(string columnName, string controlColumn, string controlValue,
			string filename, bool inverse, int nskip, char separator) {
			string[] x = GetColumnIfContains(columnName, controlColumn, controlValue, filename, inverse, nskip, separator);
			double[] d = new double[x.Length];
			for (int i = 0; i < d.Length; i++) {
				d[i] = Parser.TryDouble(x[i], out double w) ? w : double.NaN;
			}
			return d;
		}

		public static string[] GetColumnNames(string filename, int nskip, char separator) {
			return GetColumnNames(filename, nskip, null, null, null, separator);
		}

		public static string[] GetColumnNames(string filename, char separator) {
			return GetColumnNames(filename, 0, null, null, null, separator);
		}

		public static string[] GetColumnNames(string filename, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions, Dictionary<string, string[]> annotationRows, char separator) {
			return GetColumnNames(filename, 0, commentPrefix, commentPrefixExceptions, annotationRows, separator);
		}

		public static string[] GetColumnNames(string filename, int nskip, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions, Dictionary<string, string[]> annotationRows, char separator) {
			StreamReader reader = FileUtils.GetReader(filename);
			string[] titles = GetColumnNames(reader, nskip, commentPrefix, commentPrefixExceptions, annotationRows, separator);
			reader.Close();
			return titles;
		}

		public static string[] GetColumnNames(StreamReader reader, int nskip, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions, Dictionary<string, string[]> annotationRows, char separator) {
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			for (int i = 0; i < nskip; i++) {
				reader.ReadLine();
			}
			string line = reader.ReadLine();
			if (line == null) {
				return new string[0];
			}
			if (commentPrefix != null) {
				while (IsCommentLine(line, commentPrefix, commentPrefixExceptions)) {
					line = reader.ReadLine();
				}
			}
			string[] titles = line.Split(separator);
			if (annotationRows != null) {
				while ((line = reader.ReadLine()) != null) {
					if (!line.StartsWith("#!{")) {
						break;
					}
					int end = line.IndexOf('}');
					if (end == -1) {
						continue;
					}
					string name = line.Substring(3, end - 3);
					string w = line.Substring(end + 1);
					string[] terms = w.Split(separator);
					annotationRows.Add(name, terms);
				}
			}
			return titles;
		}

		public static bool IsCommentLine(string line, IEnumerable<string> prefix, HashSet<string> prefixExceptions) {
		    if (line == null)
		    {
		        return false;
		    }
			if (string.IsNullOrEmpty(line)) {
				return true;
			}
			foreach (string s in prefixExceptions) {
				if (line.StartsWith(s)) {
					return false;
				}
			}
			foreach (string s in prefix) {
				if (line.StartsWith(s)) {
					return true;
				}
			}
			return false;
		}

		public static string CanOpen(string filename) {
			try {
				StreamReader s = new StreamReader(filename);
				s.Close();
			} catch (Exception e) {
				return e.Message;
			}
			return null;
		}

		public static int GetRowCount(string filename) {
			return GetRowCount(filename, 0);
		}

		public static int GetRowCount(string filename, int nskip) {
			return GetRowCount(filename, nskip, null, null);
		}

		public static int GetRowCount(string filename, int nskip, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions) {
			StreamReader reader = FileUtils.GetReader(filename);
			int count = GetRowCount(reader, nskip, commentPrefix, commentPrefixExceptions);
			reader.Close();
			return count;
		}

		public static int GetRowCount(StreamReader reader, int nskip, HashSet<string> commentPrefix,
			HashSet<string> commentPrefixExceptions) {
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			reader.ReadLine();
			for (int i = 0; i < nskip; i++) {
				reader.ReadLine();
			}
			int count = 0;
			string line;
			while ((line = reader.ReadLine()) != null) {
				if (commentPrefix != null) {
					while (IsCommentLine(line, commentPrefix, commentPrefixExceptions)) {
						line = reader.ReadLine();
					}
				}
				count++;
			}
			reader.Close();
			return count;
		}

		public static void Write(string filename, string[] columnNames, string[][] columns) {
			if (columnNames == null || columnNames.Length == 0) {
				return;
			}
			StreamWriter writer = new StreamWriter(filename);
			writer.Write(columnNames[0]);
			for (int i = 1; i < columnNames.Length; i++) {
				writer.Write("\t" + columnNames[i]);
			}
			writer.WriteLine();
			for (int i = 0; i < columns[0].Length; i++) {
				writer.Write(columns[0][i]);
				for (int j = 1; j < columnNames.Length; j++) {
					writer.Write("\t" + columns[j][i]);
				}
				writer.WriteLine();
			}
			writer.Close();
		}
	}
}