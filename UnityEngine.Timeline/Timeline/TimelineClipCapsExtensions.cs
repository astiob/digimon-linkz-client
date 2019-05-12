using System;

namespace UnityEngine.Timeline
{
	internal static class TimelineClipCapsExtensions
	{
		public static bool SupportsLooping(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Looping) != ClipCaps.None;
		}

		public static bool SupportsExtrapolation(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Extrapolation) != ClipCaps.None;
		}

		public static bool SupportsClipIn(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.ClipIn) != ClipCaps.None;
		}

		public static bool SupportsSpeedMultiplier(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.SpeedMultiplier) != ClipCaps.None;
		}

		public static bool SupportsBlending(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Blending) != ClipCaps.None;
		}

		public static bool HasAll(this ClipCaps caps, ClipCaps flags)
		{
			return (caps & flags) == flags;
		}

		public static bool HasAny(this ClipCaps caps, ClipCaps flags)
		{
			return (caps & flags) != ClipCaps.None;
		}
	}
}
