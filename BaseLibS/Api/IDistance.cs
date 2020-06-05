using System;
using System.Collections.Generic;
using System.IO;
using BaseLibS.Num.Vector;
using BaseLibS.Param;

namespace BaseLibS.Api{
	/// <summary>
	/// Ancestor class of all distances. Distances are needed e.g. for hierarchical clustering
	/// or k-nearest neighbor classification. 
	/// </summary>
	public interface IDistance : ICloneable, INamedListItem{
		Parameters Parameters { get; set; }

		/// <summary>
		/// Calculates the distance between two vectors. The two vectors must have the same length.
		/// </summary>
		/// <param name="x">The first vector.</param>
		/// <param name="y">The second vector.</param>
		/// <returns></returns>
		double Get(IList<float> x, IList<float> y);

		/// <summary>
		/// Calculates the distance between two vectors. The two vectors must have the same length.
		/// </summary>
		/// <param name="x">The first vector.</param>
		/// <param name="y">The second vector.</param>
		/// <returns></returns>
		double Get(IList<double> x, IList<double> y);

		/// <summary>
		/// Calculates the distance between two vectors. The two vectors must have the same length.
		/// </summary>
		/// <param name="x">The first vector.</param>
		/// <param name="y">The second vector.</param>
		/// <returns></returns>
		double Get(BaseVector x, BaseVector y);

		/// <summary>
		/// This method returns the distance between two row or column vectors in two matrices.
		/// The matrices must have the same number of rows or columns (depending on the access type 
		/// being column or row). 
		/// </summary>
		/// <param name="data1">The matrix from which the first row/column is taken.</param>
		/// <param name="data2">The matrix from which the second row/column is taken.</param>
		/// <param name="index1">The row/column index in the first matrix.</param>
		/// <param name="index2">The row/column index in the second matrix.</param>
		/// <param name="access1">Specifies whether the distance is evaluated on row or column vectors of the first matrix.</param>
		/// <param name="access2">Specifies whether the distance is evaluated on row or column vectors of the second matrix.</param>
		/// <returns></returns>
		double Get(float[,] data1, float[,] data2, int index1, int index2, MatrixAccess access1, MatrixAccess access2);

		/// <summary>
		/// This method returns the distance between two row or column vectors in two matrices.
		/// The matrices must have the same number of rows or columns (depending on the access type 
		/// being column or row). 
		/// </summary>
		/// <param name="data1">The matrix from which the first row/column is taken.</param>
		/// <param name="data2">The matrix from which the second row/column is taken.</param>
		/// <param name="index1">The row/column index in the first matrix.</param>
		/// <param name="index2">The row/column index in the second matrix.</param>
		/// <param name="access1">Specifies whether the distance is evaluated on row or column vectors of the first matrix.</param>
		/// <param name="access2">Specifies whether the distance is evaluated on row or column vectors of the second matrix.</param>
		/// <returns></returns>
		double Get(double[,] data1, double[,] data2, int index1, int index2, MatrixAccess access1, MatrixAccess access2);

		bool IsAngular { get; }
		void Write(BinaryWriter writer);
		void Read(BinaryReader reader);
		DistanceType GetDistanceType();

	}
}