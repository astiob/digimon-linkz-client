using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Audio
{
	/// <summary>
	///   <para>Object representing a group in the mixer.</para>
	/// </summary>
	public class AudioMixerGroup : Object
	{
		internal AudioMixerGroup()
		{
		}

		public extern AudioMixer audioMixer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
