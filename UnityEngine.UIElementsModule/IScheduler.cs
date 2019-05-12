using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IScheduler
	{
		IScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs);

		IScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition = null);

		IScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs);

		void Unschedule(IScheduledItem item);

		void Schedule(IScheduledItem item);
	}
}
