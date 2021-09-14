using System;

namespace BaseLibS.Api.Image{
	public interface IImageSeries : ICloneable{
		int LengthT{ get; }
		int LengthX{ get; }
		int LengthY{ get; }
		int LengthZ{ get; }
		string Name { get; set; }
		IImageMetadata Metadata { get; set; }
		float GetValueAt(int t, int x, int y, int z);
		float GetWeightAt(int c, int x, int y, int z);
		bool GetROIAt(int r, int x, int y, int z);
		bool[][,,] GetROIs();
		string[] GetROINames();
		bool[,,] GetROI(string name);
		bool GetIndicatorAt(int c, int x, int y, int z);
		float[,,] GetImageAtTimestep(int t);
		int IndicatorCount { get; }
		float MinValue { get; }
		float MaxValue { get; }
		bool HasTime{ get; }
		bool IsFlat{ get; }
		int FlatDimension{ get; }
		bool HasWeights{ get; }
		bool HasROIs { get; }
		int NumComponents{ get; }
		bool IsTwoSided{ get; }
		void SetWeights(float[][,,] weights, bool isTwoSided);
		void SetROIs(bool[][,,] rois, string[] names);
		void AddROI(bool[,,] roi, string name);
		void SetData(float[][,,] data);
		float RepetitionTimeSeconds{ get; }
		float VoxelSizeXmm { get; }
		float VoxelSizeYmm { get; }
		float VoxelSizeZmm { get; }
	}
}