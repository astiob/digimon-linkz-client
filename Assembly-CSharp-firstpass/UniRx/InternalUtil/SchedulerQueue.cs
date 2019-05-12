using System;

namespace UniRx.InternalUtil
{
	internal class SchedulerQueue
	{
		private readonly PriorityQueue<ScheduledItem> _queue;

		public SchedulerQueue() : this(1024)
		{
		}

		public SchedulerQueue(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this._queue = new PriorityQueue<ScheduledItem>(capacity);
		}

		public int Count
		{
			get
			{
				return this._queue.Count;
			}
		}

		public void Enqueue(ScheduledItem scheduledItem)
		{
			this._queue.Enqueue(scheduledItem);
		}

		public bool Remove(ScheduledItem scheduledItem)
		{
			return this._queue.Remove(scheduledItem);
		}

		public ScheduledItem Dequeue()
		{
			return this._queue.Dequeue();
		}

		public ScheduledItem Peek()
		{
			return this._queue.Peek();
		}
	}
}
