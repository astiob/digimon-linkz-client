using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal class ActivationPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.None;
			}
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			return Playable.Create(graph, 0);
		}
	}
}
