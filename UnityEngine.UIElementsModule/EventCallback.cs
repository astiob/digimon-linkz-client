using System;

namespace UnityEngine.Experimental.UIElements
{
	public delegate void EventCallback<in TEventType>(TEventType evt);
}
