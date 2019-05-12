using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[Serializable]
	public class AudioPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		[SerializeField]
		private AudioClip m_Clip;

		[SerializeField]
		private bool m_Loop;

		[SerializeField]
		[HideInInspector]
		private float m_bufferingTime = 0.1f;

		internal float bufferingTime
		{
			get
			{
				return this.m_bufferingTime;
			}
			set
			{
				this.m_bufferingTime = value;
			}
		}

		public AudioClip clip
		{
			get
			{
				return this.m_Clip;
			}
			set
			{
				this.m_Clip = value;
			}
		}

		public override double duration
		{
			get
			{
				double result;
				if (this.m_Clip == null)
				{
					result = base.duration;
				}
				else
				{
					result = (double)this.m_Clip.samples / (double)this.m_Clip.frequency;
				}
				return result;
			}
		}

		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				yield return new PlayableBinding
				{
					streamName = "audio",
					streamType = DataStreamType.Audio
				};
				yield break;
			}
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			Playable result;
			if (this.m_Clip == null)
			{
				result = Playable.Null;
			}
			else
			{
				result = AudioClipPlayable.Create(graph, this.m_Clip, this.m_Loop);
			}
			return result;
		}

		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending | ((!this.m_Loop) ? ClipCaps.None : ClipCaps.Looping);
			}
		}
	}
}
