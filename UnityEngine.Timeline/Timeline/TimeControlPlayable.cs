using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	public class TimeControlPlayable : PlayableBehaviour
	{
		private ITimeControl m_timeControl;

		private bool m_started;

		public static ScriptPlayable<TimeControlPlayable> Create(PlayableGraph graph, ITimeControl timeControl)
		{
			ScriptPlayable<TimeControlPlayable> result;
			if (timeControl == null)
			{
				result = ScriptPlayable<TimeControlPlayable>.Null;
			}
			else
			{
				ScriptPlayable<TimeControlPlayable> scriptPlayable = ScriptPlayable<TimeControlPlayable>.Create(graph, 0);
				scriptPlayable.GetBehaviour().Initialize(timeControl);
				result = scriptPlayable;
			}
			return result;
		}

		public void Initialize(ITimeControl timeControl)
		{
			this.m_timeControl = timeControl;
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (this.m_timeControl != null)
			{
				this.m_timeControl.SetTime(playable.GetTime<Playable>());
			}
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (this.m_timeControl != null)
			{
				if (!this.m_started)
				{
					this.m_timeControl.OnControlTimeStart();
					this.m_started = true;
				}
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (this.m_timeControl != null)
			{
				if (this.m_started)
				{
					this.m_timeControl.OnControlTimeStop();
					this.m_started = false;
				}
			}
		}
	}
}
