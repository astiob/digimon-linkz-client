using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IUIElementDataWatch
	{
		IUIElementDataWatchRequest RegisterWatch(Object toWatch, Action<Object> watchNotification);

		void UnregisterWatch(IUIElementDataWatchRequest requested);
	}
}
