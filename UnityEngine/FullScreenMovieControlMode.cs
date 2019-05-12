using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes options for displaying movie playback controls.</para>
	/// </summary>
	public enum FullScreenMovieControlMode
	{
		/// <summary>
		///   <para>Display the standard controls for controlling movie playback.</para>
		/// </summary>
		Full,
		/// <summary>
		///   <para>Display minimal set of controls controlling movie playback.</para>
		/// </summary>
		Minimal,
		/// <summary>
		///   <para>Do not display any controls, but cancel movie playback if input occurs.</para>
		/// </summary>
		CancelOnInput,
		/// <summary>
		///   <para>Do not display any controls.</para>
		/// </summary>
		Hidden
	}
}
