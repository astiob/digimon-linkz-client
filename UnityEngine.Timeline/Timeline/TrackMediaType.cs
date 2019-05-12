using System;

namespace UnityEngine.Timeline
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TrackMediaType : Attribute
	{
		public readonly TimelineAsset.MediaType m_MediaType;

		public TrackMediaType(TimelineAsset.MediaType mt)
		{
			this.m_MediaType = mt;
		}
	}
}
