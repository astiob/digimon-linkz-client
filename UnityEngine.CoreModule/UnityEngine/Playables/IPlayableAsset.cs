using System;
using System.Collections.Generic;

namespace UnityEngine.Playables
{
	public interface IPlayableAsset
	{
		Playable CreatePlayable(PlayableGraph graph, GameObject owner);

		double duration { get; }

		IEnumerable<PlayableBinding> outputs { get; }
	}
}
