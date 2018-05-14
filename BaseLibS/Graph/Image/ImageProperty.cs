using System;

namespace BaseLibS.Graph.Image{
	public class ImageProperty : IEquatable<ImageProperty>{
		public ImageProperty(string name, string value){
			if (string.IsNullOrEmpty(name)){
				throw new ArgumentNullException();
			}
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public string Value { get; }

		public static bool operator ==(ImageProperty left, ImageProperty right){
			return left.Equals(right);
		}

		public static bool operator !=(ImageProperty left, ImageProperty right){
			return !left.Equals(right);
		}

		public override bool Equals(object obj){
			ImageProperty other = obj as ImageProperty;
			return Equals(other);
		}

		public override int GetHashCode(){
			unchecked{
				int hashCode = Name.GetHashCode();
				if (Value != null){
					hashCode = (hashCode*397) ^ Value.GetHashCode();
				}
				return hashCode;
			}
		}

		public override string ToString(){
			return $"ImageProperty [ Name={Name}, Value={Value} ]";
		}

		public bool Equals(ImageProperty other){
			if (ReferenceEquals(other, null)){
				return false;
			}
			if (ReferenceEquals(this, other)){
				return true;
			}
			return Name.Equals(other.Name) && Value != null && Value.Equals(other.Value);
		}
	}
}