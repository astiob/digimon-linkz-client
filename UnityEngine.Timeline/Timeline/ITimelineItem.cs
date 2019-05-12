using System;

namespace UnityEngine.Timeline
{
	internal interface ITimelineItem
	{
		int Hash();

		TrackAsset parentTrack { get; }

		double start { get; }
	}
}
