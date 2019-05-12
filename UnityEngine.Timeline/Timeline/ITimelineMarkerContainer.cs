using System;

namespace UnityEngine.Timeline
{
	internal interface ITimelineMarkerContainer
	{
		TimelineMarker[] GetMarkers();

		TimelineMarker CreateMarker(string key, double time);

		void RemoveMarker(TimelineMarker marker);
	}
}
