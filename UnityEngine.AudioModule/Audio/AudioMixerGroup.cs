﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	public class AudioMixerGroup : Object
	{
		internal AudioMixerGroup()
		{
		}

		public extern AudioMixer audioMixer { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
