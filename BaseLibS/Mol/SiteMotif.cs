using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BaseLibS.Util;

namespace BaseLibS.Mol{
	public class SiteMotif{
		private static SiteMotif[] siteMotifs;
		public static SiteMotif[] PhosphoMotifs => siteMotifs ?? (siteMotifs = ReadSiteMotif());
		private static Dictionary<char, double> aaOccurences;

		public static string GetPhosphoMotifsAsText(){
			StringBuilder result = new StringBuilder();
			foreach (SiteMotif motif in PhosphoMotifs){
				result.AppendLine(motif.ToString());
			}
			return result.ToString();
		}

		private readonly string name;
		private readonly List<string> site = new List<string>();
		private readonly List<string[]> nterm = new List<string[]>();
		private readonly List<string[]> cterm = new List<string[]>();
		private readonly List<double> prob = new List<double>();

		public SiteMotif(string name){
			this.name = name;
		}

		private static Dictionary<char, double> InitOccurences(){
			Dictionary<char, double> result = new Dictionary<char, double>{
				{'A', 0.074},
				{'R', 0.042},
				{'N', 0.044},
				{'D', 0.059},
				{'C', 0.033},
				{'E', 0.058},
				{'Q', 0.037},
				{'G', 0.074},
				{'H', 0.029},
				{'I', 0.038},
				{'L', 0.076},
				{'K', 0.072},
				{'M', 0.018},
				{'F', 0.04},
				{'P', 0.05},
				{'S', 0.081},
				{'T', 0.062},
				{'W', 0.013},
				{'Y', 0.033},
				{'V', 0.068},
				{'X', 1}
			};
			return result;
		}

		public static Dictionary<char, double> AaOccurences => aaOccurences ?? (aaOccurences = InitOccurences());

		public void Add(string[] n, string s, string[] c){
			for (int i = 0; i < n.Length; i++){
				n[i] = CompleteIl(n[i]);
			}
			for (int i = 0; i < c.Length; i++){
				c[i] = CompleteIl(c[i]);
			}
			s = CompleteIl(s);
			nterm.Add(n);
			site.Add(s);
			cterm.Add(c);
			double p = 1;
			foreach (string nn in n){
				double p1 = 0;
				foreach (char nnn in nn){
					p1 += AaOccurences[nnn];
				}
				p *= p1;
			}
			foreach (string nn in c){
				double p1 = 0;
				foreach (char nnn in nn){
					p1 += AaOccurences[nnn];
				}
				p *= p1;
			}
			prob.Add(p);
		}

		private static string CompleteIl(string s){
			if (s.Contains("I") && !s.Contains("L")){
				return s + "L";
			}
			if (s.Contains("L") && !s.Contains("I")){
				return s + "I";
			}
			return s;
		}

		private static SiteMotif[] ReadSiteMotif(){
			string filename = FileUtils.GetConfigPath() + "\\motifs.txt";
			Dictionary<string, SiteMotif> allMotifs = new Dictionary<string, SiteMotif>();
			StreamReader reader = new StreamReader(filename);
			string line;
			while ((line = reader.ReadLine()) != null){
				if (line.Length == 0){
					continue;
				}
				string[] q = line.Split('\t');
				for (int i = 0; i < q.Length; i++){
					string b = q[i];
					if (b.Length > 1){
						if ((b.StartsWith("\"") && b.EndsWith("\"")) || (b.StartsWith("'") && b.EndsWith("'"))){
							q[i] = b.Substring(1, b.Length - 2);
						}
					}
				}
				string name = q[0];
				string[] before = q[1].Length > 0 ? q[1].Split(',') : new string[0];
				string middle = q[2];
				string[] after = (q.Length > 3 && q[3].Length > 0) ? q[3].Split(',') : new string[0];
				if (!allMotifs.ContainsKey(name)){
					allMotifs.Add(name, new SiteMotif(name));
				}
				allMotifs[name].Add(before, middle, after);
			}
			SiteMotif[] phosphoMotifs = allMotifs.Values.ToArray();
			reader.Close();
			return phosphoMotifs;
		}

		public static string[] GetMatchingMotifs(string[] windows, out double prob, out string best){
			prob = 1;
			best = null;
			List<string> result = new List<string>();
			foreach (string window in windows){
				double p;
				string b;
				string[] m = GetMatchingMotifs(window, out p, out b);
				if (m.Length > 0){
					result.AddRange(m);
					if (p < prob){
						prob = p;
						best = b;
					}
				}
			}
			return result.ToArray();
		}

		private static string[] GetMatchingMotifs(string window, out double prob, out string best){
			prob = 1;
			best = null;
			List<string> result = new List<string>();
			foreach (SiteMotif motif in PhosphoMotifs){
				double p;
				if (motif.IsMatchingMotif(window, out p)){
					result.Add(motif.name);
					if (p < prob){
						prob = p;
						best = motif.name;
					}
				}
			}
			return result.ToArray();
		}

		private bool IsMatchingMotif(string window, out double p){
			p = 1;
			bool result = false;
			for (int i = 0; i < site.Count; i++){
				if (IsMatchingMotif(window, site[i], nterm[i], cterm[i])){
					result = true;
					if (prob[i] < p){
						p = prob[i];
					}
				}
			}
			return result;
		}

		private static bool IsMatchingMotif(string window, string site, IList<string> nterm, IList<string> cterm){
			if (window.Length == 0){
				return false;
			}
			if (window.Length%2 == 0){
				throw new Exception("Window length is even.");
			}
			int centerPos = window.Length/2;
			if (!IsMatch(window[centerPos], site)){
				return false;
			}
			for (int i = 0; i < nterm.Count; i++){
				int ind = centerPos - nterm.Count + i;
				if (ind < 0 || ind >= window.Length){
					return false;
				}
				if (!IsMatch(window[ind], nterm[i])){
					return false;
				}
			}
			for (int i = 0; i < cterm.Count; i++){
				int ind = centerPos + 1 + i;
				if (ind < 0 || ind >= window.Length){
					return false;
				}
				if (!IsMatch(window[ind], cterm[i])){
					return false;
				}
			}
			return true;
		}

		private static bool IsMatch(char c, string s){
			if (s.Equals("X")){
				return true;
			}
			return s.IndexOf(c) >= 0;
		}

		public override string ToString(){
			StringBuilder result = new StringBuilder();
			result.AppendLine(name);
			for (int i = 0; i < nterm.Count; i++){
				if (nterm[i].Length > 0){
					result.Append(nterm[i][0]);
				}
				for (int j = 1; j < nterm[i].Length; j++){
					result.Append("-" + nterm[i][j]);
				}
				if (nterm[i].Length > 0){
					result.Append("-");
				}
				result.Append("p" + site[i]);
				for (int j = 0; j < cterm[i].Length; j++){
					result.Append("-" + cterm[i][j]);
				}
				result.AppendLine();
			}
			return result.ToString();
		}
	}
}