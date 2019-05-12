using System;

namespace UnityEngine.Experimental.UIElements
{
	internal abstract class EventCallbackFunctorBase
	{
		protected EventCallbackFunctorBase(CallbackPhase phase)
		{
			this.phase = phase;
		}

		public CallbackPhase phase { get; private set; }

		public abstract void Invoke(EventBase evt);

		public abstract bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase);

		protected bool PhaseMatches(EventBase evt)
		{
			CallbackPhase phase = this.phase;
			if (phase != CallbackPhase.CaptureAndTarget)
			{
				if (phase == CallbackPhase.TargetAndBubbleUp)
				{
					if (evt.propagationPhase != PropagationPhase.AtTarget && evt.propagationPhase != PropagationPhase.BubbleUp)
					{
						return false;
					}
				}
			}
			else if (evt.propagationPhase != PropagationPhase.Capture && evt.propagationPhase != PropagationPhase.AtTarget)
			{
				return false;
			}
			return true;
		}
	}
}
