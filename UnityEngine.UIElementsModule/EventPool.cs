using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventPool<T> where T : EventBase<T>, new()
	{
		private readonly Stack<T> m_Stack = new Stack<T>();

		public T Get()
		{
			return (this.m_Stack.Count != 0) ? this.m_Stack.Pop() : Activator.CreateInstance<T>();
		}

		public void Release(T element)
		{
			if (this.m_Stack.Contains(element))
			{
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
			}
			this.m_Stack.Push(element);
		}
	}
}
