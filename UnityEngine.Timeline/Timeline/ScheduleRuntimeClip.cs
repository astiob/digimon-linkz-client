using System;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal class ScheduleRuntimeClip : RuntimeClipBase
	{
		private TimelineClip m_Clip;

		private Playable m_Playable;

		private Playable m_ParentMixer;

		private double m_StartDelay;

		private double m_FinishTail;

		private bool m_Started = false;

		public ScheduleRuntimeClip(TimelineClip clip, Playable clipPlayable, Playable parentMixer, double startDelay = 0.2, double finishTail = 0.1)
		{
			this.Create(clip, clipPlayable, parentMixer, startDelay, finishTail);
		}

		public override double start
		{
			get
			{
				return Math.Max(0.0, this.m_Clip.start - this.m_StartDelay);
			}
		}

		public override double duration
		{
			get
			{
				return this.m_Clip.duration + this.m_FinishTail + this.m_Clip.start - this.start;
			}
		}

		public void SetTime(double time)
		{
			this.m_Playable.SetTime(time);
		}

		public TimelineClip clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		public Playable mixer
		{
			get
			{
				return this.m_ParentMixer;
			}
		}

		public Playable playable
		{
			get
			{
				return this.m_Playable;
			}
		}

		private void Create(TimelineClip clip, Playable clipPlayable, Playable parentMixer, double startDelay, double finishTail)
		{
			this.m_Clip = clip;
			this.m_Playable = clipPlayable;
			this.m_ParentMixer = parentMixer;
			this.m_StartDelay = startDelay;
			this.m_FinishTail = finishTail;
			clipPlayable.Pause<Playable>();
		}

		public override bool enable
		{
			set
			{
				if (value && this.m_Playable.GetPlayState<Playable>() != PlayState.Playing)
				{
					this.m_Playable.Play<Playable>();
				}
				else if (!value && this.m_Playable.GetPlayState<Playable>() != PlayState.Paused)
				{
					this.m_Playable.Pause<Playable>();
					if (this.m_ParentMixer.IsValid<Playable>())
					{
						this.m_ParentMixer.SetInputWeight(this.m_Playable, 0f);
					}
				}
				this.m_Started = (this.m_Started && value);
			}
		}

		public override void EvaluateAt(double localTime, FrameData frameData)
		{
			if (frameData.timeHeld)
			{
				this.enable = false;
			}
			else
			{
				bool flag = frameData.seekOccurred || frameData.timeLooped || frameData.evaluationType == FrameData.EvaluationType.Evaluate;
				if (localTime <= this.start + this.duration - this.m_FinishTail)
				{
					float weight = this.clip.EvaluateMixIn(localTime) * this.clip.EvaluateMixOut(localTime);
					if (this.mixer.IsValid<Playable>())
					{
						this.mixer.SetInputWeight(this.playable, weight);
					}
					if (!this.m_Started || flag)
					{
						double startTime = this.clip.ToLocalTime(Math.Max(localTime, this.clip.start));
						double startDelay = Math.Max(this.clip.start - localTime, 0.0) * this.clip.timeScale;
						double duration = this.m_Clip.duration * this.clip.timeScale;
						if (this.m_Playable.IsPlayableOfType<AudioClipPlayable>())
						{
							((AudioClipPlayable)this.m_Playable).Seek(startTime, startDelay, duration);
						}
						this.m_Started = true;
					}
				}
			}
		}
	}
}
