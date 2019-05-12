using System;

namespace UnityEngine.Experimental.UIElements
{
	public class IMGUIEvent : EventBase<IMGUIEvent>
	{
		public IMGUIEvent()
		{
			this.Init();
		}

		public static IMGUIEvent GetPooled(Event systemEvent)
		{
			IMGUIEvent pooled = EventBase<IMGUIEvent>.GetPooled();
			pooled.imguiEvent = systemEvent;
			return pooled;
		}

		protected override void Init()
		{
			base.Init();
			this.flags = (EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable | EventBase.EventFlags.Cancellable);
		}
	}
}
