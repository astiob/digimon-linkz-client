using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Audio
{
	/// <summary>
	///   <para>Object representing a snapshot in the mixer.</para>
	/// </summary>
	public class AudioMixerSnapshot : Object
	{
		internal AudioMixerSnapshot()
		{
		}

		public extern AudioMixer audioMixer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Performs an interpolated transition towards this snapshot over the time interval specified.</para>
		/// </summary>
		/// <param name="timeToReach">Relative time after which this snapshot should be reached from any current state.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TransitionTo(float timeToReach);
	}
}
