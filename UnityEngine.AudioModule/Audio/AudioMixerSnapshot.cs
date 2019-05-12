using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	public class AudioMixerSnapshot : Object
	{
		internal AudioMixerSnapshot()
		{
		}

		public extern AudioMixer audioMixer { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TransitionTo(float timeToReach);
	}
}
