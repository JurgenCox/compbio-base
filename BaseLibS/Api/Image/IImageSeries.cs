using BaseLibS.Api.Generic;

namespace BaseLibS.Api.Image{
	public interface IImageSeries : IAtom{
		int LengthT{ get; }
		int LengthX{ get; }
		int LengthY{ get; }
		int LengthZ{ get; }
		string Name { get; set; }
		IImageMetadata Metadata { get; set; }
		float GetValueAt(int t, int x, int y, int z);
		float GetWeightAt(int c, int x, int y, int z);
		bool GetIndicatorAt(int c, int x, int y, int z);
		int IndicatorCount { get; }
		float MinValue { get; }
		float MaxValue { get; }
		bool HasTime{ get; }
		bool IsFlat{ get; }
		int FlatDimension{ get; }
		bool HasWeights{ get; }
		int NumComponents{ get; }
		bool IsTwoSided{ get; }
		void SetWeights(float[][,,] weights, bool isTwoSided);
		void SetData(float[][,,] data);
		float RepetitionTimeSeconds{ get; }
		float VoxelSizeXmm { get; }
		float VoxelSizeYmm { get; }
		float VoxelSizeZmm { get; }
	}
}