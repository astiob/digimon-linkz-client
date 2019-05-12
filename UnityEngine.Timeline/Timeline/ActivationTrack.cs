using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[TrackClipType(typeof(ActivationPlayableAsset))]
	[TrackMediaType(TimelineAsset.MediaType.Script)]
	[TrackBindingType(typeof(GameObject))]
	[Serializable]
	public class ActivationTrack : TrackAsset
	{
		[SerializeField]
		private ActivationTrack.PostPlaybackState m_PostPlaybackState = ActivationTrack.PostPlaybackState.LeaveAsIs;

		private ActivationMixerPlayable m_ActivationMixer;

		internal override bool compilable
		{
			get
			{
				return this.isEmpty || base.compilable;
			}
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
				this.UpdateTrackMode();
			}
		}

		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			ScriptPlayable<ActivationMixerPlayable> playable = ActivationMixerPlayable.Create(graph, inputCount);
			this.m_ActivationMixer = playable.GetBehaviour();
			PlayableDirector component = go.GetComponent<PlayableDirector>();
			this.UpdateBoundGameObject(component);
			this.UpdateTrackMode();
			return playable;
		}

		private void UpdateBoundGameObject(PlayableDirector director)
		{
			if (director != null)
			{
				GameObject gameObject = director.GetGenericBinding(this) as GameObject;
				if (gameObject != null && this.m_ActivationMixer != null)
				{
					this.m_ActivationMixer.boundGameObject = gameObject;
				}
			}
		}

		internal void UpdateTrackMode()
		{
			if (this.m_ActivationMixer != null)
			{
				this.m_ActivationMixer.postPlaybackState = this.m_PostPlaybackState;
			}
		}

		public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			GameObject gameObjectBinding = base.GetGameObjectBinding(director);
			if (gameObjectBinding != null)
			{
				driver.AddFromName(gameObjectBinding, "m_IsActive");
			}
		}

		public enum PostPlaybackState
		{
			Active,
			Inactive,
			Revert,
			LeaveAsIs
		}
	}
}
