using System;

namespace UnityEngine.Experimental.Director
{
	/// <summary>
	///   <para>Status of a Playable.</para>
	/// </summary>
	public enum PlayState
	{
		/// <summary>
		///   <para>The Playable has been paused. Its local time will not advance.</para>
		/// </summary>
		Paused,
		/// <summary>
		///   <para>The Playable is currently Playing.</para>
		/// </summary>
		Playing
	}
}
