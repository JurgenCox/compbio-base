using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Util;

namespace BaseLibS.Parse.Uniprot{
	[Serializable]
	public class UniprotFeature{
		public string FeatureId { get; set; }
		public string FeatureDescription { get; set; }
		public string FeatureStatus { get; set; }
		public string FeatureBegin { get; set; }
		public string FeatureEnd { get; set; }
		public readonly List<string> originals = new List<string>();
		public readonly List<string> variations = new List<string>();

		public UniprotFeature(string featureDescription, string featureStatus, string featureId){
			FeatureDescription = featureDescription;
			FeatureStatus = featureStatus;
			FeatureId = featureId;
		}

		public UniprotFeature(BinaryReader reader){
			FeatureId = reader.ReadString();
			FeatureDescription = reader.ReadString();
			FeatureStatus = reader.ReadString();
			FeatureBegin = reader.ReadString();
			FeatureEnd = reader.ReadString();
			int len = reader.ReadInt32();
			originals = new List<string>();
			for (int i = 0; i < len; i++){
				originals.Add(reader.ReadString());
			}
			len = reader.ReadInt32();
			variations = new List<string>();
			for (int i = 0; i < len; i++){
				variations.Add(reader.ReadString());
			}
		}

		public void Write(BinaryWriter writer){
			writer.Write(FeatureId);
			writer.Write(FeatureDescription);
			writer.Write(FeatureStatus);
			writer.Write(FeatureBegin);
			writer.Write(FeatureEnd);
			writer.Write(originals.Count);
			foreach (string t in originals){
				writer.Write(t);
			}
			writer.Write(variations.Count);
			foreach (string t in variations){
				writer.Write(t);
			}
		}

		public UniprotFeature(string[] u){
			FeatureId = u[0];
			FeatureDescription = u[1];
			FeatureStatus = u[2];
			FeatureBegin = u[3];
			FeatureEnd = u[4];
			originals = new List<string>();
			if (u.Length > 5){
				originals.Add(u[5]);
			}
			variations = new List<string>();
			if (u.Length > 6){
				for (int i = 6; i < u.Length; i++){
					variations.Add(u[i]);
				}
			}
		}

		public override string ToString(){
			List<string> x = new List<string>{
				Convert1(FeatureId),
				Convert1(FeatureDescription),
				Convert1(FeatureStatus),
				Convert1(FeatureBegin),
				Convert1(FeatureEnd),
				Convert1(originals.Count > 0 ? originals[0] : "")
			};
			foreach (string s in variations){
				x.Add(Convert1(s));
			}
			return StringUtils.Concat(",", x);
		}

		private static string Convert1(string s){
			s = s.Replace(';', '|');
			s = s.Replace(',', '|');
			return s;
		}

		public override bool Equals(object obj){
			if (ReferenceEquals(null, obj)){
				return false;
			}
			if (ReferenceEquals(this, obj)){
				return true;
			}
			return obj.GetType() == typeof (UniprotFeature) && Equals((UniprotFeature) obj);
		}

		public bool Equals(UniprotFeature other){
			if (ReferenceEquals(null, other)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return other.FeatureDescription != null && other.FeatureStatus != null && other.FeatureId != null &&
					other.FeatureDescription.Equals(FeatureDescription) && other.FeatureStatus.Equals(FeatureStatus) &&
					other.FeatureId.Equals(FeatureId);
		}

		public override int GetHashCode(){
			unchecked{
				int result = FeatureDescription?.GetHashCode() ?? 0;
				result = (result*397) ^ (FeatureStatus?.GetHashCode() ?? 0);
				result = (result*397) ^ (FeatureId?.GetHashCode() ?? 0);
				return result;
			}
		}

		public void AddFeatureLocation(string featureBegin1, string featureEnd1){
			FeatureBegin = featureBegin1;
			FeatureEnd = featureEnd1;
		}

		public void AddFeatureVariation(string variation){
			variations.Add(variation);
		}

		public void AddFeatureOriginal(string original){
			originals.Add(original);
		}
	}
}