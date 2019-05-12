using System;

namespace UnityEngine.Experimental.UIElements
{
	public static class EventHandlerExtensions
	{
		public static void TakeCapture(this IEventHandler handler)
		{
			UIElementsUtility.eventDispatcher.TakeCapture(handler);
		}

		public static bool HasCapture(this IEventHandler handler)
		{
			return UIElementsUtility.eventDispatcher.capture == handler;
		}

		public static void ReleaseCapture(this IEventHandler handler)
		{
			UIElementsUtility.eventDispatcher.ReleaseCapture(handler);
		}

		public static void RemoveCapture(this IEventHandler handler)
		{
			UIElementsUtility.eventDispatcher.RemoveCapture();
		}
	}
}
