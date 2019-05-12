using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[TrackClipType(typeof(IPlayableAsset))]
	[TrackMediaType(TimelineAsset.MediaType.Script)]
	[Serializable]
	public class PlayableTrack : TrackAsset
	{
		internal override void OnCreateClipFromAsset(Object asset, TimelineClip newClip)
		{
			base.OnCreateClipFromAsset(asset, newClip);
			if (newClip != null)
			{
				newClip.displayName = asset.GetType().Name;
			}
		}
	}
}
