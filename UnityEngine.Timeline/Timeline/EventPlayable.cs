using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal abstract class EventPlayable : PlayableBehaviour
	{
		private bool m_HasFired;

		private double m_PreviousTime;

		public double triggerTime { get; set; }

		public virtual void OnTrigger()
		{
		}

		public sealed override void PrepareFrame(Playable playable, FrameData info)
		{
			if (this.m_HasFired)
			{
				if (this.HasLooped(playable.GetTime<Playable>(), info) || this.CanRestoreEvent(playable.GetTime<Playable>(), info))
				{
					this.Restore_internal();
				}
			}
			if (!this.m_HasFired && this.CanTriggerEvent(playable.GetTime<Playable>(), info))
			{
				this.Trigger_internal();
			}
			this.m_PreviousTime = playable.GetTime<Playable>();
		}

		public sealed override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
		}

		public sealed override void OnBehaviourPlay(Playable playable, FrameData info)
		{
		}

		public sealed override void OnBehaviourPause(Playable playable, FrameData info)
		{
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			if (this.m_HasFired)
			{
				this.Restore_internal();
			}
		}

		private bool CanTriggerEvent(double playableTime, FrameData info)
		{
			bool flag = DiscreteTime.GetNearestTick(playableTime) >= DiscreteTime.GetNearestTick(this.triggerTime);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.HasLooped(playableTime, info) && this.triggerTime >= this.m_PreviousTime && this.triggerTime <= this.m_PreviousTime + (double)info.deltaTime;
				result = flag2;
			}
			return result;
		}

		private bool CanRestoreEvent(double playableTime, FrameData info)
		{
			return DiscreteTime.GetNearestTick(playableTime) < DiscreteTime.GetNearestTick(this.triggerTime);
		}

		private void Trigger_internal()
		{
			this.OnTrigger();
			this.m_HasFired = true;
		}

		private void Restore_internal()
		{
			this.m_HasFired = false;
		}

		private bool HasLooped(double playableTime, FrameData info)
		{
			return this.m_PreviousTime > playableTime && (double)info.deltaTime > playableTime;
		}
	}
}
