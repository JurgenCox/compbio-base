namespace BaseLibS.Ms {
	/// <summary>
	/// Either Centroid or Profile. Centroid means that a Spectrum delivered by a spectrometer 
	/// is a list of masses at which a peak was found, together with the intensities of those peaks.
	/// Profile means that the masses are more or less evenly and closely spaced, so that the positions 
	/// and the intensities of the peaks must first be extracted.
	/// </summary>
	public enum SignalType {
		Centroid,
		Profile
	}
}