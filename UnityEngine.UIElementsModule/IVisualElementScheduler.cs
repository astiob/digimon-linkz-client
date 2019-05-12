using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IVisualElementScheduler
	{
		IVisualElementScheduledItem Execute(Action<TimerState> timerUpdateEvent);

		IVisualElementScheduledItem Execute(Action updateEvent);
	}
}
