using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IEventDispatcher
	{
		IEventHandler capture { get; }

		void ReleaseCapture(IEventHandler handler);

		void RemoveCapture();

		void TakeCapture(IEventHandler handler);

		void DispatchEvent(EventBase evt, IPanel panel);
	}
}
