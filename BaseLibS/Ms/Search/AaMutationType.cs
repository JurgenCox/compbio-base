namespace BaseLibS.Ms.Search{
	/// <summary>
	/// List of situations that can happen on the amino acid sequence level for a single mutation.
	/// </summary>
	public enum AaMutationType{
		/// <summary>
		/// A single aa letter is replaced by another one which is not a stop codon.
		/// </summary>
		SingleAaSubstitution,
		/// <summary>
		/// A single aa letter is removed from the sequence
		/// </summary>
		SingleAaDeletion,
		/// <summary>
		/// A single aa letter is added to the sequence
		/// </summary>
		SingleAaInsertion,
		/// <summary>
		/// A consecutive stretch of more than one aa is inserted into the sequence
		/// </summary>
		MultiAaInsertion,
		/// <summary>
		/// A consecutive stretch of more than one aa is removed from the sequence
		/// </summary>
		MultiAaDeletion,
		/// <summary>
		/// A stop codon is inserted into the sequence
		/// </summary>
		StopCodonInsertion,
		/// <summary>
		/// A consecutive stretch of more than one aa is inserted into the sequence followed by a new stop codon.
		/// </summary>
		MultiAaInsertionWithStopCodon,
		/// <summary>
		/// Remaining cases of replacement of one aa sequence by another without stop codon.
		/// </summary>
		ComplexSubstitution,
	}
}