using System;

namespace UnityEngine.Timeline
{
	internal static class MatchTargetFieldConstants
	{
		public static MatchTargetFields All = MatchTargetFields.PositionX | MatchTargetFields.PositionY | MatchTargetFields.PositionZ | MatchTargetFields.RotationX | MatchTargetFields.RotationY | MatchTargetFields.RotationZ;

		public static MatchTargetFields None = (MatchTargetFields)0;

		public static MatchTargetFields Position = MatchTargetFields.PositionX | MatchTargetFields.PositionY | MatchTargetFields.PositionZ;

		public static MatchTargetFields Rotation = MatchTargetFields.RotationX | MatchTargetFields.RotationY | MatchTargetFields.RotationZ;

		public static bool HasAny(this MatchTargetFields me, MatchTargetFields fields)
		{
			return (me & fields) != MatchTargetFieldConstants.None;
		}

		public static MatchTargetFields Toggle(this MatchTargetFields me, MatchTargetFields flag)
		{
			return me ^ flag;
		}
	}
}
