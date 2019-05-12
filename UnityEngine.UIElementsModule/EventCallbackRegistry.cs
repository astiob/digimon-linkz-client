using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventCallbackRegistry
	{
		private static readonly EventCallbackListPool s_ListPool = new EventCallbackListPool();

		private EventCallbackList m_Callbacks;

		private EventCallbackList m_TemporaryCallbacks;

		private int m_IsInvoking;

		public EventCallbackRegistry()
		{
			this.m_IsInvoking = 0;
		}

		private static EventCallbackList GetCallbackList(EventCallbackList initializer = null)
		{
			return EventCallbackRegistry.s_ListPool.Get(initializer);
		}

		private static void ReleaseCallbackList(EventCallbackList toRelease)
		{
			EventCallbackRegistry.s_ListPool.Release(toRelease);
		}

		private EventCallbackList GetCallbackListForWriting()
		{
			EventCallbackList result;
			if (this.m_IsInvoking > 0)
			{
				if (this.m_TemporaryCallbacks == null)
				{
					if (this.m_Callbacks != null)
					{
						this.m_TemporaryCallbacks = EventCallbackRegistry.GetCallbackList(this.m_Callbacks);
					}
					else
					{
						this.m_TemporaryCallbacks = EventCallbackRegistry.GetCallbackList(null);
					}
				}
				result = this.m_TemporaryCallbacks;
			}
			else
			{
				if (this.m_Callbacks == null)
				{
					this.m_Callbacks = EventCallbackRegistry.GetCallbackList(null);
				}
				result = this.m_Callbacks;
			}
			return result;
		}

		private EventCallbackList GetCallbackListForReading()
		{
			EventCallbackList result;
			if (this.m_TemporaryCallbacks != null)
			{
				result = this.m_TemporaryCallbacks;
			}
			else
			{
				result = this.m_Callbacks;
			}
			return result;
		}

		private bool ShouldRegisterCallback(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			bool result;
			if (callback == null)
			{
				result = false;
			}
			else
			{
				EventCallbackList callbackListForReading = this.GetCallbackListForReading();
				result = (callbackListForReading == null || !callbackListForReading.Contains(eventTypeId, callback, phase));
			}
			return result;
		}

		private bool UnregisterCallback(long eventTypeId, Delegate callback, Capture useCapture)
		{
			bool result;
			if (callback == null)
			{
				result = false;
			}
			else
			{
				EventCallbackList callbackListForWriting = this.GetCallbackListForWriting();
				CallbackPhase phase = (useCapture != Capture.Capture) ? CallbackPhase.TargetAndBubbleUp : CallbackPhase.CaptureAndTarget;
				result = callbackListForWriting.Remove(eventTypeId, callback, phase);
			}
			return result;
		}

		public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			long eventTypeId = EventBase<TEventType>.TypeId();
			CallbackPhase phase = (useCapture != Capture.Capture) ? CallbackPhase.TargetAndBubbleUp : CallbackPhase.CaptureAndTarget;
			if (this.ShouldRegisterCallback(eventTypeId, callback, phase))
			{
				EventCallbackList callbackListForWriting = this.GetCallbackListForWriting();
				callbackListForWriting.Add(new EventCallbackFunctor<TEventType>(callback, phase));
			}
		}

		public void RegisterCallback<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, TCallbackArgs userArgs, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			long eventTypeId = EventBase<TEventType>.TypeId();
			CallbackPhase phase = (useCapture != Capture.Capture) ? CallbackPhase.TargetAndBubbleUp : CallbackPhase.CaptureAndTarget;
			if (this.ShouldRegisterCallback(eventTypeId, callback, phase))
			{
				EventCallbackList callbackListForWriting = this.GetCallbackListForWriting();
				callbackListForWriting.Add(new EventCallbackFunctor<TEventType, TCallbackArgs>(callback, userArgs, phase));
			}
		}

		public bool UnregisterCallback<TEventType>(EventCallback<TEventType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			long eventTypeId = EventBase<TEventType>.TypeId();
			return this.UnregisterCallback(eventTypeId, callback, useCapture);
		}

		public bool UnregisterCallback<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			long eventTypeId = EventBase<TEventType>.TypeId();
			return this.UnregisterCallback(eventTypeId, callback, useCapture);
		}

		public void InvokeCallbacks(EventBase evt)
		{
			if (this.m_Callbacks != null)
			{
				this.m_IsInvoking++;
				for (int i = 0; i < this.m_Callbacks.Count; i++)
				{
					if (evt.isImmediatePropagationStopped)
					{
						break;
					}
					this.m_Callbacks[i].Invoke(evt);
				}
				this.m_IsInvoking--;
				if (this.m_IsInvoking == 0)
				{
					if (this.m_TemporaryCallbacks != null)
					{
						EventCallbackRegistry.ReleaseCallbackList(this.m_Callbacks);
						this.m_Callbacks = EventCallbackRegistry.GetCallbackList(this.m_TemporaryCallbacks);
						EventCallbackRegistry.ReleaseCallbackList(this.m_TemporaryCallbacks);
						this.m_TemporaryCallbacks = null;
					}
				}
			}
		}

		public bool HasCaptureHandlers()
		{
			return this.m_Callbacks != null && this.m_Callbacks.capturingCallbackCount > 0;
		}

		public bool HasBubbleHandlers()
		{
			return this.m_Callbacks != null && this.m_Callbacks.bubblingCallbackCount > 0;
		}
	}
}
