using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IEventHandler
	{
		void OnLostCapture();

		void HandleEvent(EventBase evt);

		bool HasCaptureHandlers();

		bool HasBubbleHandlers();
	}
}
