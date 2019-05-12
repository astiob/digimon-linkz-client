using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class EventBase<T> : EventBase where T : EventBase<T>, new()
	{
		private static readonly long s_TypeId = EventBase.RegisterEventType();

		private static readonly EventPool<T> s_Pool = new EventPool<T>();

		public static long TypeId()
		{
			return EventBase<T>.s_TypeId;
		}

		public static T GetPooled()
		{
			T result = EventBase<T>.s_Pool.Get();
			result.Init();
			return result;
		}

		public static void ReleasePooled(T evt)
		{
			EventBase<T>.s_Pool.Release(evt);
			evt.target = null;
		}

		public override long GetEventTypeId()
		{
			return EventBase<T>.s_TypeId;
		}
	}
}
