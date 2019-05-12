using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal class ActivationMixerPlayable : PlayableBehaviour
	{
		private ActivationTrack.PostPlaybackState m_PostPlaybackState;

		private bool m_BoundGameObjectInitialStateIsActive;

		private GameObject m_BoundGameObject;

		public GameObject boundGameObject
		{
			get
			{
				return this.m_BoundGameObject;
			}
			set
			{
				this.m_BoundGameObject = value;
				this.m_BoundGameObjectInitialStateIsActive = (value != null && value.activeSelf);
			}
		}

		public static ScriptPlayable<ActivationMixerPlayable> Create(PlayableGraph graph, int inputCount)
		{
			return ScriptPlayable<ActivationMixerPlayable>.Create(graph, inputCount);
		}

		public ActivationTrack.PostPlaybackState postPlaybackState
		{
			get
			{
				return this.m_PostPlaybackState;
			}
			set
			{
				this.m_PostPlaybackState = value;
			}
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			if (!(this.boundGameObject == null))
			{
				if (!Application.isPlaying)
				{
					this.boundGameObject.SetActive(this.m_BoundGameObjectInitialStateIsActive);
				}
				else
				{
					switch (this.m_PostPlaybackState)
					{
					case ActivationTrack.PostPlaybackState.Active:
						this.boundGameObject.SetActive(true);
						break;
					case ActivationTrack.PostPlaybackState.Inactive:
						this.boundGameObject.SetActive(false);
						break;
					case ActivationTrack.PostPlaybackState.Revert:
						this.boundGameObject.SetActive(this.m_BoundGameObjectInitialStateIsActive);
						break;
					}
				}
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (!(this.boundGameObject == null))
			{
				int inputCount = playable.GetInputCount<Playable>();
				bool active = false;
				for (int i = 0; i < inputCount; i++)
				{
					if (playable.GetInputWeight(i) > 0f)
					{
						active = true;
						break;
					}
				}
				this.boundGameObject.SetActive(active);
			}
		}
	}
}
