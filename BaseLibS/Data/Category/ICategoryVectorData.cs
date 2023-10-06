using System.IO;
namespace BaseLibS.Data.Category{
	internal interface ICategoryVectorData{
		/// <summary>
		/// Get all categories at index i
		/// </summary>
		string[] this[int i] { get; }
		/// <summary>
		/// Gets the length of the vector.
		/// </summary>
		int Length { get; }
		/// <summary>
		/// Create subset of the vector data given the indices
		/// </summary>
		ICategoryVectorData GetSubVector(int[] indices);
		/// <summary>
		/// Return the unique categories which occur in the vector data in alphabetical order.
		/// </summary>
		string[] Values { get; }
		/// <summary>
		/// Creates a deep copy of itself.
		/// </summary>
		ICategoryVectorData Copy();
		void Write(BinaryWriter writer);
	}
}