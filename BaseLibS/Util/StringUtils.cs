using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BaseLibS.Util {
	public static class StringUtils {
		/// <summary>
		/// The digits 0 to 9 as subscripts.
		/// </summary>
		private static readonly char[] subscripts =
			{'\u2080', '\u2081', '\u2082', '\u2083', '\u2084', '\u2085', '\u2086', '\u2087', '\u2088', '\u2089'};

		/// <summary>
		/// The digits 0 to 9 as superscripts.
		/// </summary>
		private static readonly char[] superscripts =
			{'\u2070', '\u00b9', '\u00b2', '\u00b3', '\u2074', '\u2075', '\u2076', '\u2077', '\u2078', '\u2079'};

		/// <summary>
		/// Returns a string containing a representation of the given integer as superscript.
		/// </summary>
		/// <param name="n">The integer to be converted to superscript.</param>
		/// <param name="explicitPlus">Whether or not a '+' is added in front of positive numbers.</param>
		/// <param name="explicitBracket"></param>
		/// <returns>Representation of the given integer as superscript string.</returns>
		public static string ToSuperscript(int n, bool explicitPlus, bool explicitBracket) {
			bool isNegative = n < 0;
			bool isPositive = n > 0;
			StringBuilder result = new StringBuilder();
			try {
				n = Math.Abs(n);
				string nn = Parser.ToString(n);
				if (explicitBracket) {
					result.Append("\u207D");
				}
				if (isNegative) {
					result.Append('\u207B');
				}
				char[] nnn = nn.ToCharArray();
				foreach (char t in nnn) {
					result.Append(superscripts[t - '0']);
				}
				if (isPositive && explicitPlus) {
					result.Append('\u207A');
				}
				if (explicitBracket) {
					result.Append("\u207E");
				}
			} catch (OverflowException) {
				Console.Error.WriteLine("Could not calculate the absolute value of n=" + n);
			}
			return result.ToString();
		}

		/// <summary>
		/// Returns a string containing a representation of the given integer as subscript.
		/// </summary>
		/// <param name="n">The integer to be converted to subscript.</param>
		/// <param name="explicitPlus">Whether or not a '+' is added in front of positive numbers.</param>
		/// <returns>Representation of the given integer as subscript string.</returns>
		public static string ToSubscript(int n, bool explicitPlus) {
			bool isNegative = n < 0;
			bool isPositive = n > 0;
			n = Math.Abs(n);
			string nn = Parser.ToString(n);
			StringBuilder result = new StringBuilder();
			if (isNegative) {
				result.Append('\u208B');
			}
			if (isPositive && explicitPlus) {
				result.Append('\u208A');
			}
			char[] nnn = nn.ToCharArray();
			foreach (char t in nnn) {
				result.Append(subscripts[t - '0']);
			}
			return result.ToString();
		}

		/// <summary>
		/// Concatenates the string representations of the objects in the given array using the specified separator.
		/// </summary>
		/// <typeparam name="T">Type of objects to be concatenated as strings.</typeparam>
		/// <param name="separator">A string used to separate the array members.</param>
		/// <param name="o">The list of objects to be concatenated.</param>
		/// <returns>The concatenated string of all string representations of the array members.</returns>
		public static string Concat<T>(string separator, T[] o) {
			return Concat(separator, o, int.MaxValue);
		}

		/// <summary>
		/// Concatenates the string representations of the objects in the given array using the specified separator.
		/// </summary>
		/// <typeparam name="T">Type of objects to be concatenated as strings.</typeparam>
		/// <param name="separator">A string used to separate the array members.</param>
		/// <param name="o">The list of objects to be concatenated.</param>
		/// <param name="maxLen">The convatenation is terminated such that the length of the resulting string will not exceed this value.</param>
		/// <returns>The concatenated string of all string representations of the array members.</returns>
		public static string Concat<T>(string separator, T[] o, int maxLen) {
			if (o == null || o.Length == 0) {
				return "";
			}
			if (o.Length == 1) {
				return Parser.ToString(o[0]);
			}
			StringBuilder s = new StringBuilder(Parser.ToString(o[0]));
			for (int i = 1; i < o.Length; i++) {
				string w = separator + Parser.ToString(o[i]);
				if (s.Length + w.Length > maxLen) {
					break;
				}
				s.Append(w);
			}
			return s.ToString();
		}

		public static int[][] SplitToInt(char separator1, char separator2, string s) {
			string[][] x = Split(separator1, separator2, s);
			int[][] result = new int[x.Length][];
			for (int i = 0; i < x.Length; i++) {
				result[i] = new int[x[i].Length];
				for (int j = 0; j < x[i].Length; j++) {
					result[i][j] = Parser.Int(x[i][j]);
				}
			}
			return result;
		}

		public static string[][] Split(char separator1, char separator2, string s) {
			if (s == null || s.ToLower().Equals("null")) {
				return null;
			}
			if (s.Length == 0) {
				return new string[0][];
			}
			string[] q1 = s.Length > 0 ? s.Split(separator1) : new string[0];
			string[][] result = new string[q1.Length][];
			for (int i = 0; i < result.Length; i++) {
				string q = q1[i];
				result[i] = q.Length > 0 ? q.Split(separator2) : new string[0];
			}
			return result;
		}

		public static string Concat<T>(string separator1, string separator2, T[][] o) {
			return Concat(separator1, separator2, o, int.MaxValue);
		}

		public static string Concat<T>(string separator1, string separator2, T[][] o, int maxLen) {
			if (o == null) {
				return "";
			}
			if (o.Length == 0) {
				return "";
			}
			if (o.Length == 1) {
				return Concat(separator2, o[0], maxLen);
			}
			StringBuilder s = new StringBuilder(Concat(separator2, o[0], maxLen));
			for (int i = 1; i < o.Length; i++) {
				string w = separator1 + Concat(separator2, o[i], maxLen - s.Length);
				if (s.Length + w.Length > maxLen) {
					break;
				}
				s.Append(w);
			}
			return s.ToString();
		}

		/// <summary>
		/// Concatenates the string representations of the objects in the given array using the specified separator.
		/// </summary>
		/// <typeparam name="T">Type of objects to be concatenated as strings.</typeparam>
		/// <param name="separator">A string used to separate the array members.</param>
		/// <param name="o">The list of objects to be concatenated.</param>
		/// <returns>The concatenated string of all string representations of the array members.</returns>
		public static string Concat<T>(string separator, IList<T> o) {
			if (o == null) {
				return "";
			}
			if (o.Count == 0) {
				return "";
			}
			if (o.Count == 1) {
				return Parser.ToString(o[0]);
			}
			StringBuilder s = new StringBuilder(Parser.ToString(o[0]));
			for (int i = 1; i < o.Count; i++) {
				string w = separator + Parser.ToString(o[i]);
				s.Append(w);
			}
			return s.ToString();
		}

		/// <summary>
		/// Concatenates the string representations of the objects in the given array using the specified separator.
		/// </summary>
		/// <typeparam name="T">Type of objects to be concatenated as strings.</typeparam>
		/// <param name="separator">A string used to separate the array members.</param>
		/// <param name="o">The list of objects to be concatenated.</param>
		/// <returns>The concatenated string of all string representations of the array members.</returns>
		public static string Concat<T>(string separator, IEnumerable<T> o) {
			if (o == null) {
				return "";
			}
			StringBuilder s = new StringBuilder();
			string separator1 = ""; // empty separator for first element
			foreach (T i in o) {
				string w = separator1 + Parser.ToString(i);
				s.Append(w);
				separator1 = separator;
			}
			return s.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string[] Wrap(string text, int maxLength) {
			if (text == null) {
				return new string[0];
			}
			string[] words = text.Split(' ');
			int currentLineLength = 0;
			ArrayList lines = new ArrayList(text.Length / maxLength);
			bool inTag = false;
			string currentLine = "";
			foreach (string currentWord in words) {
				//ignore html
				if (currentWord.Length > 0) {
					if (currentWord.Substring(0, 1) == "<") {
						inTag = true;
					}
					if (inTag) {
						//handle filenames inside html tags
						if (currentLine.EndsWith(".")) {
							currentLine += currentWord;
						} else {
							currentLine += " " + currentWord;
						}
						if (currentWord.IndexOf(">", StringComparison.InvariantCulture) > -1) {
							inTag = false;
						}
					} else {
						if (currentLineLength + currentWord.Length + 1 < maxLength) {
							currentLine += (currentLineLength == 0 ? "" : " ") + currentWord;
							currentLineLength += (currentWord.Length + 1);
						} else {
							if (!string.IsNullOrEmpty(currentLine)) {
								lines.Add(currentLine);
							}
							currentLine = currentWord;
							currentLineLength = currentWord.Length;
						}
					}
				}
			}
			if (currentLine != "") {
				lines.Add(currentLine);
			}
			string[] textLinesStr = new string[lines.Count];
			lines.CopyTo(textLinesStr, 0);
			return textLinesStr;
		}

		public static bool ContainsAll(string x, IEnumerable<string> strings) {
			x = x.ToLower();
			foreach (string s in strings) {
				if (x.IndexOf(s.ToLower(), StringComparison.InvariantCulture) == -1) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns a string that is the same as the input string, except that all whitespace characters are removed.
		/// </summary>
		public static string RemoveWhitespace(string str) {
			StringBuilder s = new StringBuilder();
			int len = str.Length;
			for (int i = 0; i < len; i++) {
				char c = str[i];
				if (!char.IsWhiteSpace(c)) {
					s.Append(c);
				}
			}
			return s.ToString();
		}

		/// <summary>
		/// Returns a string that is the same as the input string, except that all whitespace characters are replaced.
		/// </summary>
		public static string ReplaceWhitespace(string str, string replaceBy) {
			StringBuilder s = new StringBuilder();
			int len = str.Length;
			for (int i = 0; i < len; i++) {
				char c = str[i];
				if (!char.IsWhiteSpace(c)) {
					s.Append(c);
				} else {
					s.Append(replaceBy);
				}
			}
			return s.ToString();
		}

		/// <summary>
		/// Returns a string that is the same as the input string, except that all consecutive sets of whitespace 
		/// characters are replaced by a single blank character.
		/// </summary>
		public static string ReduceWhitespace(string str) {
			StringBuilder s = new StringBuilder();
			int len = str.Length;
			bool previousWasWhiteSpace = false;
			for (int i = 0; i < len; i++) {
				char c = str[i];
				if (!char.IsWhiteSpace(c)) {
					s.Append(c);
					previousWasWhiteSpace = false;
				} else {
					if (!previousWasWhiteSpace) {
						s.Append(' ');
					}
					previousWasWhiteSpace = true;
				}
			}
			return s.ToString().Trim();
		}

		public static string Replace(string x, string[] oldChar, string newChar) {
			if (string.IsNullOrEmpty(x)) {
				return x;
			}
			string result = x;
			foreach (string t in oldChar) {
				if (!string.IsNullOrEmpty(x)) {
					result = result.Replace(t, newChar);
				}
			}
			return result;
		}

		public static string Replace(string x, string oldWord, string newWord) {
			if (string.IsNullOrEmpty(x)) {
				return x;
			}
			int ind = x.IndexOf(oldWord, StringComparison.InvariantCulture);
			if (ind < 0) {
				return x;
			}
			StringBuilder result = new StringBuilder();
			result.Append(x.Substring(0, ind));
			result.Append(newWord);
			result.Append(x.Substring(ind + oldWord.Length));
			return result.ToString();
		}

		public static int OccurenceCount(string s, char c) {
			if (s == null) {
				return 0;
			}
			int count = 0;
			foreach (char w in s) {
				if (w == c) {
					count++;
				}
			}
			return count;
		}

		public static string JoinQuotedCsv(string[] x) {
			if (x.Length == 0) {
				return "";
			}
			if (x[0].StartsWith("\"")) {
				return Concat(",", x);
			}
			return "\"" + Concat("\",\"", x) + "\"";
		}

		public static string[] SplitQuotedCsv(string line) {
			line = line.Trim();
			if (line.StartsWith("\"") && line.EndsWith("\"")) {
				line = line.Substring(1, line.Length - 2);
				return line.Split(new[] {"\",\""}, StringSplitOptions.None);
			}
			return line.Split(',');
		}

		public static string[] Split(string seq, int n) {
			if (seq.Length <= n) {
				return new[] {seq};
			}
			int q = (int) Math.Ceiling(seq.Length / (double) n);
			string[] result = new string[q];
			for (int i = 0; i < q - 1; i++) {
				result[i] = seq.Substring(i * n, n);
			}
			result[q - 1] = seq.Substring((q - 1) * n);
			return result;
		}

		public static string ToString(IDictionary d) {
			StringBuilder result = new StringBuilder();
			foreach (string key in d.Keys) {
				result.AppendLine(key + "\t" + d[key]);
			}
			return result.ToString();
		}

		public static int[] AllIndicesOf(string str, string word) {
			List<int> result = new List<int>();
			int found = str.IndexOf(word, StringComparison.InvariantCulture);
			while (found != -1) {
				result.Add(found);
				found = str.IndexOf(word, found + 1, StringComparison.InvariantCulture);
			}
			return result.ToArray();
		}

		public static string Repeat(string s, int n) {
			if (n == 0) {
				return "";
			}
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < n; i++) {
				result.Append(s);
			}
			return result.ToString();
		}

		public static string Repeat(char c, int n) {
			if (n == 0) {
				return "";
			}
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < n; i++) {
				result.Append(c);
			}
			return result.ToString();
		}

		public static string[] SplitCsv(string line) {
			bool inQuote = false;
			List<int> indices = new List<int>();
			for (int i = 0; i < line.Length; i++) {
				if (line[i] == '\"') {
					inQuote = !inQuote;
				} else if (!inQuote && line[i] == ',') {
					indices.Add(i);
				}
			}
			string[] result = SplitAtIndices(line, indices);
			for (int i = 0; i < result.Length; i++) {
				if (result[i].Length > 1) {
					if (result[i][0] == '\"' && result[i][result[i].Length - 1] == '\"') {
						result[i] = result[i].Substring(1, result[i].Length - 2);
					}
				}
			}
			return result;
		}

		public static string[] SplitAtIndices(string line, IList<int> indices) {
			if (indices.Count == 0) {
				return new[] {line};
			}
			string[] result = new string[indices.Count + 1];
			result[0] = line.Substring(0, indices[0]);
			for (int i = 1; i < indices.Count; i++) {
				result[i] = line.Substring(indices[i - 1] + 1, indices[i] - indices[i - 1] - 1);
			}
			if (indices[indices.Count - 1] + 1 < line.Length) {
				result[indices.Count] = line.Substring(indices[indices.Count - 1] + 1,
					line.Length - indices[indices.Count - 1] - 1);
			} else {
				result[indices.Count] = "";
			}
			return result;
		}

		private static readonly HashSet<char> notInFilenames =
			new HashSet<char>(new[] {'\\', '/', ':', '*', '?', '\"', '<', '>', '|'});

		/// <summary>
		/// Replaces occurences of characters that are problematic in file names or paths with an underscore.
		/// </summary>
		public static string ReplaceCharactersForFilename(string str) {
			return ReplaceCharactersForFilename(str, '_');
		}

		public static string ReplaceCharactersForFilename(string str, char replaceBy) {
			StringBuilder s = new StringBuilder();
			int len = str.Length;
			for (int i = 0; i < len; i++) {
				char c = str[i];
				s.Append(notInFilenames.Contains(c) ? replaceBy : c);
			}
			return s.ToString();
		}

		public static string GetFileSizeString(string filename) {
			FileInfo info = new FileInfo(filename);
			// ReSharper disable PossibleLossOfFraction
			double len = info.Length / 1024;
			// ReSharper restore PossibleLossOfFraction
			if (len < 1024) {
				return "" + Parser.ToString((int) (10 * len) / 10.0) + " KB";
			}
			len /= 1024;
			if (len < 1024) {
				return "" + Parser.ToString((int) (10 * len) / 10.0) + " MB";
			}
			len /= 1024;
			return "" + Parser.ToString((int) (10 * len) / 10.0) + " GB";
		}

		public static string[] SplitWithBrackets(string line, char separator) {
			bool inQuote = false;
			List<int> indices = new List<int>();
			for (int i = 0; i < line.Length; i++) {
				switch (line[i]) {
					case '(':
						inQuote = true;
						break;
					case ')':
						inQuote = false;
						break;
					default:
						if (!inQuote && line[i] == separator) {
							indices.Add(i);
						}
						break;
				}
			}
			return SplitAtIndices(line, indices);
		}

		/// <summary>
		/// Contains all the invalid characters of strings for R. These can be replaced
		/// with the appropriate values with the function <see cref="Replace(string,string[][])"/>.
		/// </summary>
		public static readonly string[][] invalidR = {new[] {"\t", ";"}, new[] {"\"", ""}, new[] {"'", ""}, new[] {"?", ""}};

		/// <summary>
		/// Replaces the occurances in the given string of the chs[][0] vector with the chs[][1]. Predefined
		/// values can be found in <see cref="invalidR"/> and <see cref="notInFilenames"/>.
		/// </summary>
		/// <param name="str">The string to be converted</param>
		/// <param name="chs">The mapping.</param>
		/// <returns>The new string.</returns>
		public static string Replace(string str, string[][] chs) {
			string s = str;
			foreach (string[] t in chs) {
				s = s.Replace(t[0], t[1]);
			}
			return s;
		}

		public static string GetTimeString(double t) {
			long sec = (long) Math.Round(t / 1000.0);
			long min = sec / 60;
			sec -= min * 60;
			long hrs = min / 60;
			min -= hrs * 60;
			if (hrs == 0) {
				return "" + min + ":" + (sec < 10 ? "0" : "") + sec;
			}
			return "" + hrs + ":" + (min < 10 ? "0" : "") + min + ":" + (sec < 10 ? "0" : "") + sec;
		}

		public static string IntToStringLeadingZeroes(int n, int len) {
			if (n == 0) {
				return Repeat('0', n);
			}
			bool negative = n < 0;
			n = Math.Abs(n);
			int ndigits = (int) Math.Log10(n) + 1;
			if (ndigits >= len) {
				return (negative ? "-" : "") + n;
			}
			return (negative ? "-" : "") + Repeat('0', len - ndigits) + n;
		}

		public static string ReturnAtWhitespace(string s) {
			return ReturnAtWhitespace(s, 80);
		}

		public static string ReturnAtWhitespace(string s, int len) {
			return Concat("\n", SplitLinesAtWhitespace(s, len));
		}

		public static string[] SplitLinesAtWhitespace(string s, int len) {
			if (s == null) {
				return new string[0];
			}
			List<string> result = new List<string>();
			StringBuilder line = new StringBuilder();
			foreach (char c in s) {
				if (line.Length == 0 && char.IsWhiteSpace(c)) {
					continue;
				}
				if (line.Length < len) {
					line.Append(c);
				} else {
					if (char.IsWhiteSpace(c)) {
						result.Add(line.ToString());
						line.Clear();
					} else {
						line.Append(c);
					}
				}
			}
			if (line.Length > 0) {
				result.Add(line.ToString());
			}
			return result.ToArray();
		}

		public static string[] SplitAtWhitespace(string s) {
			if (string.IsNullOrEmpty(s)) {
				return new string[0];
			}
			s = s.Trim();
			if (string.IsNullOrEmpty(s)) {
				return new string[0];
			}
			StringBuilder current = new StringBuilder();
			List<string> result = new List<string>();
			foreach (char c in s) {
				if (char.IsWhiteSpace(c)) {
					if (current.Length > 0) {
						result.Add(current.ToString());
						current.Clear();
					}
				} else {
					current.Append(c);
				}
			}
			if (current.Length > 0) {
				result.Add(current.ToString());
				current.Clear();
			}
			return result.ToArray();
		}

		public static string[] RemoveCommonSubstrings(string[] s, bool ensureNonempty) {
			if (s.Length < 2) {
				return s;
			}
			int prefixLen = GetCommonPrefixLength(s);
			string prefix = prefixLen > 0 ? s[0].Substring(0, prefixLen) : "";
			string[] result = new string[s.Length];
			for (int i = 0; i < result.Length; i++) {
				result[i] = prefixLen > 0 ? s[i].Substring(prefixLen) : s[i];
			}
			int suffixLen = GetCommonSuffixLength(result);
			string suffix = suffixLen > 0 ? s[0].Substring(s[0].Length - suffixLen, suffixLen) : "";
			if (suffixLen > 0) {
				for (int i = 0; i < result.Length; i++) {
					result[i] = result[i].Substring(0, result[i].Length - suffixLen);
				}
			}
			if (!ensureNonempty) {
				return result;
			}
			bool anyIsEmpty = false;
			foreach (string s1 in result) {
				if (s1.Length == 0) {
					anyIsEmpty = true;
					break;
				}
			}
			if (!anyIsEmpty) {
				return result;
			}
			if (prefixLen > 0) {
				for (int i = 0; i < result.Length; i++) {
					result[i] = prefix[prefixLen - 1] + result[i];
				}
				return result;
			}
			if (suffixLen > 0) {
				for (int i = 0; i < result.Length; i++) {
					result[i] = result[i] + suffix[0];
				}
				return result;
			}
			return result;
		}

		public static int GetCommonSuffixLength(IList<string> s) {
			string[] x = new string[s.Count];
			for (int i = 0; i < s.Count; i++) {
				char[] c = s[i].ToCharArray();
				Array.Reverse(c);
				x[i] = new string(c);
			}
			return GetCommonPrefixLength(x);
		}

		public static int GetCommonPrefixLength(IList<string> s) {
			string prefix = s[0];
			for (int i = 1; i < s.Count; i++) {
				string file = s[i];
				int index = -1;
				for (int j = 0; j < Math.Min(prefix.Length, s[i].Length); j++) {
					if (prefix[j] != file[j]) {
						index = j;
						break;
					}
				}
				if (index >= 0) {
					prefix = prefix.Substring(0, index);
				} else {
					if (file.Length < prefix.Length) {
						prefix = prefix.Substring(0, file.Length);
					}
				}
			}
			return prefix.Length;
		}

		public static string WithDecimalSeparators(long a) {
			if (a < 999 && a > -999) {
				return "" + a;
			}
			string s = "";
			if (a < 0) {
				s = "-";
				a = -a;
			}
			long r = a;
			List<string> t = new List<string>();
			while (r > 0) {
				long x = r % 1000;
				t.Add("" + x);
				r = r / 1000;
			}
			t.Reverse();
			for (int i = 1; i < t.Count; i++) {
				if (t[i].Length < 3) {
					switch (t[i].Length) {
						case 0:
							t[i] = "000";
							break;
						case 1:
							t[i] = "00" + t[i];
							break;
						default:
							t[i] = "0" + t[i];
							break;
					}
				}
			}
			return s + Concat(",", t);
		}

		public static bool EqualsIgnoreCase(string str1, string str2) {
			if (str1 == null || str2 == null) {
				return false;
			}
			return str1.ToLower().Equals(str2.ToLower());
		}

		public static string GetNextAvailableName(string s, ICollection<string> taken) {
			if (!taken.Contains(s)) {
				return s;
			}
			while (true) {
				s = GetNext(s);
				if (!taken.Contains(s)) {
					return s;
				}
			}
		}

		public static bool IsDigit(char c) {
			return c >= '0' && c <= '9';
		}

		public static bool AreDigits(string s) {
			foreach (char c in s) {
				if (!IsDigit(c)) {
					return false;
				}
			}
			return true;
		}

		private static string GetNext(string s) {
			if (!HasNumberExtension(s)) {
				return s + "_1";
			}
			int x = s.LastIndexOf('_');
			string s1 = s.Substring(x + 1);
			int num = Parser.Int(s1);
			return s.Substring(0, x + 1) + (num + 1);
		}

		private static bool HasNumberExtension(string s) {
			int x = s.LastIndexOf('_');
			if (x < 0) {
				return false;
			}
			string s1 = s.Substring(x + 1);
			bool succ = Parser.TryInt(s1, out int _);
			return succ;
		}

		public static readonly HashSet<string> categoricalColDefaultNames = new HashSet<string>(new[] {
			"pfam names", "gocc names", "gomf names", "gobp names", "kegg pathway names", "chromosome", "strand",
			"interpro name", "prints name", "prosite name", "smart name", "sequence motifs", "reactome", "transcription factors",
			"microrna", "scop class", "scop fold", "scop superfamily", "scop family", "phospho motifs", "mim", "pdb", "intact",
			"corum", "motifs", "best motif", "reverse", "contaminant", "potential contaminant", "only identified by site",
			"type", "amino acid", "raw file", "experiment", "charge", "modifications", "md modification", "dp aa", "dp decoy",
			"dp modification", "fraction", "dp cluster index", "authors", "publication", "year", "publisher", "geography",
			"geography id", "identified", "fragmentation", "mass analyzer", "labeling state", "ion mode", "mode", "composition",
			"isotope cluster index", "flagged", "from chebi", "completed"
		});

		public static readonly HashSet<string> textualColDefaultNames = new HashSet<string>(new[] {
			"protein ids", "protein", "majority protein ids", "protein names", "gene names", "uniprot", "ensembl", "ensg",
			"ensp", "enst", "mgi", "kegg ortholog", "dip", "hprd interactors", "sequence window", "sequence", "orf name",
			"names", "proteins", "positions within proteins", "leading proteins", "leading razor protein", "md sequence",
			"md proteins", "md gene names", "md protein names", "dp base sequence", "dp probabilities", "dp proteins",
			"dp gene names", "dp protein names", "name", "dn sequence", "title", "volume", "number", "pages",
			"modified sequence", "formula", "formula2", "geneid", "chr"
		});

		public static readonly HashSet<string> numericColDefaultNames = new HashSet<string>(new[] {
			"length", "position", "total position", "peptides (seq)", "razor peptides (seq)", "unique peptides (seq)",
			"localization prob", "size", "p value", "benj. hoch. fdr", "score", "delta score", "combinatorics", "intensity",
			"score for localization", "pep", "m/z", "mass", "resolution", "uncalibrated - calibrated m/z [ppm]",
			"mass error [ppm]", "uncalibrated mass error [ppm]", "uncalibrated - calibrated m/z [da]", "mass error [da]",
			"uncalibrated mass error [da]", "max intensity m/z 0", "retention length", "retention time",
			"calibrated retention time", "calibrated retention time start", "calibrated retention time finish",
			"retention time calibration", "match time difference", "match q-value", "match score", "number of data points",
			"number of scans", "number of isotopic peaks", "pif", "fraction of total spectrum", "base peak fraction",
			"ms/ms count", "ms/ms m/z", "md base scan number", "md mass error", "md time difference", "dp mass difference",
			"dp time difference", "dp score", "dp pep", "dp positional probability", "dp base scan number", "dp mod scan number",
			"dp cluster mass", "dp cluster mass sd", "dp cluster size total", "dp cluster size forward",
			"dp cluster size reverse", "dp peptide length difference", "dn score", "dn normalized score", "dn nterm mass",
			"dn cterm mass", "dn missing mass", "dn score diff", "views", "estimated minutes watched", "average view duration",
			"average percentage viewed", "subscriber views", "subscriber minutes watched", "clicks", "clickable impressions",
			"click through rate", "closes", "closable impressions", "close rate", "impressions", "likes", "likes added",
			"likes removed", "dislikes", "dislikes added", "dislikes removed", "shares", "comments", "favorites",
			"favorites added", "favorites removed", "subscribers", "subscribers gained", "subscribers lost",
			"average view duration (minutes)", "scan number", "ion injection time", "total ion current", "base peak intensity",
			"elapsed time", "precursor full scan number", "precursor intensity", "precursor apex fraction",
			"precursor apex offset", "precursor apex offset time", "scan event number", "scan index", "ms scan index",
			"ms scan number", "agc fill", "parent intensity fraction", "intens comp factor", "ctcd comp", "rawovftt",
			"cycle time", "dead time", "basepeak intensity", "mass calibration", "peak length", "isotope pattern length",
			"multiplet length", "peaks / s", "single peaks / s", "isotope patterns / s", "single isotope patterns / s",
			"multiplets / s", "identified multiplets / s", "multiplet identification rate [%]", "ms/ms / s",
			"identified ms/ms / s", "ms/ms identification rate [%]", "mass fractional part", "mass deficit",
			"mass precision [ppm]", "max intensity m/z 1", "retention length (fwhm)", "min scan number", "max scan number",
			"lys count", "arg count", "intensity", "intensity h", "intensity m", "intensity l", "r count", "k count", "jitter",
			"closest known m/z", "delta [ppm]", "delta [mda]", "uncalibrated delta [ppm]", "uncalibrated delta [mda]",
			"recalibration curve [ppm]", "recalibration curve [mda]", "q-value", "number of frames", "min frame number",
			"max frame number", "ion mobility index", "ion mobility index length", "ion mobility index length (fwhm)",
			"isotope correlation", "peptides", "razor + unique peptides", "unique peptides", "sequence coverage [%]",
			"unique sequence coverage [%]", "unique + razor sequence coverage [%]", "mol. weight [kda]", "dm [mda]", "dm [ppm]",
			"time [sec]", "du", "isotope index", "start", "end"
		});

		public static readonly HashSet<string> multiNumericColDefaultNames = new HashSet<string>(new[]
			{"mass deviations [da]", "mass deviations [ppm]", "number of phospho (sty)", "protein group ids"});
	}
}