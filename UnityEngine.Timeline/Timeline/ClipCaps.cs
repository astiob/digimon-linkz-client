using System;

namespace UnityEngine.Timeline
{
	[Flags]
	public enum ClipCaps
	{
		None = 0,
		Looping = 1,
		Extrapolation = 2,
		ClipIn = 4,
		SpeedMultiplier = 8,
		Blending = 16,
		All = -1
	}
}
