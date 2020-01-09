namespace BaseLibS.Graph{
	//
	// Summary:
	//     Specifies the type of point in a System.Drawing.Drawing2D.GraphicsPath object.
	public enum PathPointType2{
		//
		// Summary:
		//     The starting point of a System.Drawing.Drawing2D.GraphicsPath object.
		Start = 0,
		//
		// Summary:
		//     A line segment.
		Line = 1,
		//
		// Summary:
		//     A default Bézier curve.
		Bezier = 3,
		//
		// Summary:
		//     A cubic Bézier curve.
		Bezier3 = 3,
		//
		// Summary:
		//     A mask point.
		PathTypeMask = 7,
		//
		// Summary:
		//     The corresponding segment is dashed.
		DashMode = 16,
		//
		// Summary:
		//     A path marker.
		PathMarker = 32,
		//
		// Summary:
		//     The endpoint of a subpath.
		CloseSubpath = 128
	}
}