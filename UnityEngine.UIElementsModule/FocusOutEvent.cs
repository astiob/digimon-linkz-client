using System;

namespace UnityEngine.Experimental.UIElements
{
	public class FocusOutEvent : FocusEventBase<FocusOutEvent>
	{
		public FocusOutEvent()
		{
			this.Init();
		}

		protected override void Init()
		{
			base.Init();
			this.flags = (EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable);
		}
	}
}
