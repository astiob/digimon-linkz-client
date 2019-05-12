using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	public class ActivationControlPlayable : PlayableBehaviour
	{
		public GameObject gameObject = null;

		public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

		private ActivationControlPlayable.InitialState m_InitialState;

		public static ScriptPlayable<ActivationControlPlayable> Create(PlayableGraph graph, GameObject gameObject, ActivationControlPlayable.PostPlaybackState postPlaybackState)
		{
			ScriptPlayable<ActivationControlPlayable> result;
			if (gameObject == null)
			{
				result = ScriptPlayable<ActivationControlPlayable>.Null;
			}
			else
			{
				ScriptPlayable<ActivationControlPlayable> scriptPlayable = ScriptPlayable<ActivationControlPlayable>.Create(graph, 0);
				ActivationControlPlayable behaviour = scriptPlayable.GetBehaviour();
				behaviour.gameObject = gameObject;
				behaviour.postPlayback = postPlaybackState;
				result = scriptPlayable;
			}
			return result;
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (!(this.gameObject == null))
			{
				this.gameObject.SetActive(true);
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (!(this.gameObject == null))
			{
				if (info.evaluationType == FrameData.EvaluationType.Evaluate || playable.GetGraph<Playable>().IsPlaying())
				{
					this.gameObject.SetActive(false);
				}
			}
		}

		public override void ProcessFrame(Playable playable, FrameData info, object userData)
		{
			if (this.gameObject != null)
			{
				this.gameObject.SetActive(true);
			}
		}

		public override void OnGraphStart(Playable playable)
		{
			if (this.gameObject != null)
			{
				if (this.m_InitialState == ActivationControlPlayable.InitialState.Unset)
				{
					this.m_InitialState = ((!this.gameObject.activeSelf) ? ActivationControlPlayable.InitialState.Inactive : ActivationControlPlayable.InitialState.Active);
				}
			}
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			if (!(this.gameObject == null) && this.m_InitialState != ActivationControlPlayable.InitialState.Unset)
			{
				ActivationControlPlayable.PostPlaybackState postPlaybackState = this.postPlayback;
				if (postPlaybackState != ActivationControlPlayable.PostPlaybackState.Active)
				{
					if (postPlaybackState != ActivationControlPlayable.PostPlaybackState.Inactive)
					{
						if (postPlaybackState == ActivationControlPlayable.PostPlaybackState.Revert)
						{
							this.gameObject.SetActive(this.m_InitialState == ActivationControlPlayable.InitialState.Active);
						}
					}
					else
					{
						this.gameObject.SetActive(false);
					}
				}
				else
				{
					this.gameObject.SetActive(true);
				}
			}
		}

		public enum PostPlaybackState
		{
			Active,
			Inactive,
			Revert
		}

		private enum InitialState
		{
			Unset,
			Active,
			Inactive
		}
	}
}
