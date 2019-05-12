using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Used by Animation.Play function.</para>
	/// </summary>
	public enum QueueMode
	{
		/// <summary>
		///   <para>Will start playing after all other animations have stopped playing.</para>
		/// </summary>
		CompleteOthers,
		/// <summary>
		///   <para>Starts playing immediately. This can be used if you just want to quickly create a duplicate animation.</para>
		/// </summary>
		PlayNow = 2
	}
}
