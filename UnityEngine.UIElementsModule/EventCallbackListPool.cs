using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventCallbackListPool
	{
		private readonly Stack<EventCallbackList> m_Stack = new Stack<EventCallbackList>();

		public EventCallbackList Get(EventCallbackList initializer)
		{
			EventCallbackList eventCallbackList;
			if (this.m_Stack.Count == 0)
			{
				if (initializer != null)
				{
					eventCallbackList = new EventCallbackList(initializer);
				}
				else
				{
					eventCallbackList = new EventCallbackList();
				}
			}
			else
			{
				eventCallbackList = this.m_Stack.Pop();
				if (initializer != null)
				{
					eventCallbackList.AddRange(initializer);
				}
			}
			return eventCallbackList;
		}

		public void Release(EventCallbackList element)
		{
			element.Clear();
			this.m_Stack.Push(element);
		}
	}
}
