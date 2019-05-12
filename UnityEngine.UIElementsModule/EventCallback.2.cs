using System;

namespace UnityEngine.Experimental.UIElements
{
	public delegate void EventCallback<in TEventType, in TCallbackArgs>(TEventType evt, TCallbackArgs userArgs);
}
