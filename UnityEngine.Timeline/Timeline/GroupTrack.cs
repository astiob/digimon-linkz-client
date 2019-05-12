using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[TrackClipType(typeof(TrackAsset))]
	[TrackMediaType(TimelineAsset.MediaType.Group)]
	[SupportsChildTracks(null, 2147483647)]
	[Serializable]
	public class GroupTrack : TrackAsset
	{
		internal override bool compilable
		{
			get
			{
				return false;
			}
		}

		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}
	}
}
