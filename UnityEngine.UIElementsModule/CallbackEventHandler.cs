using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class CallbackEventHandler : IEventHandler
	{
		private EventCallbackRegistry m_CallbackRegistry;

		public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			if (this.m_CallbackRegistry == null)
			{
				this.m_CallbackRegistry = new EventCallbackRegistry();
			}
			this.m_CallbackRegistry.RegisterCallback<TEventType>(callback, useCapture);
		}

		public void RegisterCallback<TEventType, TUserArgsType>(EventCallback<TEventType, TUserArgsType> callback, TUserArgsType userArgs, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			if (this.m_CallbackRegistry == null)
			{
				this.m_CallbackRegistry = new EventCallbackRegistry();
			}
			this.m_CallbackRegistry.RegisterCallback<TEventType, TUserArgsType>(callback, userArgs, useCapture);
		}

		public void UnregisterCallback<TEventType>(EventCallback<TEventType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			if (this.m_CallbackRegistry != null)
			{
				this.m_CallbackRegistry.UnregisterCallback<TEventType>(callback, useCapture);
			}
		}

		public void UnregisterCallback<TEventType, TUserArgsType>(EventCallback<TEventType, TUserArgsType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
		{
			if (this.m_CallbackRegistry != null)
			{
				this.m_CallbackRegistry.UnregisterCallback<TEventType, TUserArgsType>(callback, useCapture);
			}
		}

		public virtual void HandleEvent(EventBase evt)
		{
			if (evt.propagationPhase != PropagationPhase.DefaultAction)
			{
				if (this.m_CallbackRegistry != null)
				{
					this.m_CallbackRegistry.InvokeCallbacks(evt);
				}
			}
			else
			{
				this.ExecuteDefaultAction(evt);
			}
		}

		public bool HasCaptureHandlers()
		{
			return this.m_CallbackRegistry != null && this.m_CallbackRegistry.HasCaptureHandlers();
		}

		public bool HasBubbleHandlers()
		{
			return this.m_CallbackRegistry != null && this.m_CallbackRegistry.HasBubbleHandlers();
		}

		protected internal virtual void ExecuteDefaultAction(EventBase evt)
		{
		}

		public virtual void OnLostCapture()
		{
		}
	}
}
