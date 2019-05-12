using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[Obsolete("For best performance use PlayableAsset and PlayableBehaviour.")]
	[Serializable]
	public class BasicPlayableBehaviour : ScriptableObject, IPlayableAsset, IPlayableBehaviour
	{
		public virtual double duration
		{
			get
			{
				return PlayableBinding.DefaultDuration;
			}
		}

		public virtual IEnumerable<PlayableBinding> outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}

		public virtual void OnGraphStart(Playable playable)
		{
		}

		public virtual void OnGraphStop(Playable playable)
		{
		}

		public virtual void OnPlayableCreate(Playable playable)
		{
		}

		public virtual void OnPlayableDestroy(Playable playable)
		{
		}

		public virtual void OnBehaviourPlay(Playable playable, FrameData info)
		{
		}

		public virtual void OnBehaviourPause(Playable playable, FrameData info)
		{
		}

		public virtual void PrepareFrame(Playable playable, FrameData info)
		{
		}

		public virtual void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
		}

		public virtual Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<BasicPlayableBehaviour>.Create(graph, this, 0);
		}
	}
}
