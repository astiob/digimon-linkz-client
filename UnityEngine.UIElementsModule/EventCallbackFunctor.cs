using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventCallbackFunctor<TEventType> : EventCallbackFunctorBase where TEventType : EventBase<TEventType>, new()
	{
		private EventCallback<TEventType> m_Callback;

		private long m_EventTypeId;

		public EventCallbackFunctor(EventCallback<TEventType> callback, CallbackPhase phase) : base(phase)
		{
			this.m_Callback = callback;
			this.m_EventTypeId = EventBase<TEventType>.TypeId();
		}

		public override void Invoke(EventBase evt)
		{
			if (evt == null)
			{
				throw new ArgumentNullException();
			}
			if (evt.GetEventTypeId() == this.m_EventTypeId)
			{
				if (base.PhaseMatches(evt))
				{
					this.m_Callback(evt as TEventType);
				}
			}
		}

		public override bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			return this.m_EventTypeId == eventTypeId && this.m_Callback == callback && base.phase == phase;
		}
	}
}
