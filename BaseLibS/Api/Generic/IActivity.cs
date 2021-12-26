using BaseLibS.Drawing;
using BaseLibS.Graph;
using BaseLibS.Param;

namespace BaseLibS.Api.Generic{
	/// <summary>
	/// This interface is the base from which all other activities are derived. 
	/// It provides properties that are common to all activities. 
	/// </summary>
	public interface IActivity : INamedListItem{
		/// <summary>
		/// A shortcut button will be displayed in the top button row. This also requires that an image is returned by <code>ButtonImage</code>>. 
		/// </summary>
		bool HasButton { get; }

		/// <summary>
		/// Specifies the maximal number of threads that this activity can make use of simultaneously.
		/// </summary>
		/// <param name="parameters">The parameters of the activity. The maximal usable number of threads might depend on the parameter settings.</param>
		/// <returns></returns>
		int GetMaxThreads(Parameters parameters);

		/// <summary>
		/// Link to a URL providing further information, documentation, advice about this activity. 
		/// </summary>
		string Url { get; }

		/// <summary>
		/// The image for the menu entry and the shortcut button when applicable.
		/// </summary>
		Bitmap2 DisplayImage { get; }
	}
}