using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[NotKeyable]
	[Serializable]
	public class AnimationPlayableAsset : PlayableAsset, ITimelineClipAsset, IPropertyPreview
	{
		[SerializeField]
		private AnimationClip m_Clip;

		[SerializeField]
		private Vector3 m_Position = Vector3.zero;

		[SerializeField]
		private Quaternion m_Rotation = Quaternion.identity;

		[SerializeField]
		private bool m_UseTrackMatchFields = false;

		[SerializeField]
		private MatchTargetFields m_MatchTargetFields = MatchTargetFieldConstants.All;

		[SerializeField]
		private bool m_RemoveStartOffset = true;

		private AnimationClipPlayable m_AnimationClipPlayable;

		public Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				this.m_Rotation = value;
			}
		}

		public bool useTrackMatchFields
		{
			get
			{
				return this.m_UseTrackMatchFields;
			}
			set
			{
				this.m_UseTrackMatchFields = value;
			}
		}

		public MatchTargetFields matchTargetFields
		{
			get
			{
				return this.m_MatchTargetFields;
			}
			set
			{
				this.m_MatchTargetFields = value;
			}
		}

		internal bool removeStartOffset
		{
			get
			{
				return this.m_RemoveStartOffset;
			}
			set
			{
				this.m_RemoveStartOffset = value;
			}
		}

		public AnimationClip clip
		{
			get
			{
				return this.m_Clip;
			}
			set
			{
				if (value != null)
				{
					base.name = "AnimationPlayableAsset of " + value.name;
				}
				this.m_Clip = value;
			}
		}

		public override double duration
		{
			get
			{
				double result;
				if (this.clip == null)
				{
					result = double.MaxValue;
				}
				else
				{
					double num = (double)this.clip.length;
					if (this.clip.frameRate > 0f)
					{
						double num2 = (double)Mathf.Round(this.clip.length * this.clip.frameRate);
						num = num2 / (double)this.clip.frameRate;
					}
					result = num;
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
					streamType = DataStreamType.Animation,
					streamName = "Animation"
				};
				yield break;
			}
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			Playable result;
			if (this.clip == null || this.clip.legacy)
			{
				result = Playable.Null;
			}
			else
			{
				AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(graph, this.m_Clip);
				this.m_AnimationClipPlayable = animationClipPlayable;
				this.m_AnimationClipPlayable.SetRemoveStartOffset(this.removeStartOffset);
				Playable playable = animationClipPlayable;
				if (this.applyRootMotion)
				{
					AnimationOffsetPlayable animationOffsetPlayable = AnimationOffsetPlayable.Create(graph, this.m_Position, this.m_Rotation, 1);
					graph.Connect<AnimationClipPlayable, AnimationOffsetPlayable>(animationClipPlayable, 0, animationOffsetPlayable, 0);
					animationOffsetPlayable.SetInputWeight(0, 1f);
					playable = animationOffsetPlayable;
				}
				this.LiveLink();
				result = playable;
			}
			return result;
		}

		private bool applyRootMotion
		{
			get
			{
				return this.m_Position != Vector3.zero || this.m_Rotation != Quaternion.identity || (this.m_Clip != null && this.m_Clip.hasRootMotion);
			}
		}

		public void LiveLink()
		{
		}

		public ClipCaps clipCaps
		{
			get
			{
				ClipCaps clipCaps = ClipCaps.All;
				if (this.m_Clip == null || !this.m_Clip.isLooping)
				{
					clipCaps &= ~ClipCaps.Looping;
				}
				return clipCaps;
			}
		}

		public void ResetOffsets()
		{
			this.position = Vector3.zero;
			this.rotation = Quaternion.identity;
		}

		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			driver.AddFromClip(this.m_Clip);
		}
	}
}
