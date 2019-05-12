using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes scaling modes for displaying movies.</para>
	/// </summary>
	public enum FullScreenMovieScalingMode
	{
		/// <summary>
		///   <para>Do not scale the movie.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Scale the movie until one dimension fits on the screen exactly.</para>
		/// </summary>
		AspectFit,
		/// <summary>
		///   <para>Scale the movie until the movie fills the entire screen.</para>
		/// </summary>
		AspectFill,
		/// <summary>
		///   <para>Scale the movie until both dimensions fit the screen exactly.</para>
		/// </summary>
		Fill
	}
}
