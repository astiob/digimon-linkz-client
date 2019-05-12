using System;
using System.Collections.Generic;
using System.Threading;

namespace Firebase
{
	internal class Dispatcher
	{
		private int ownerThreadId;

		private Queue<Action> queue = new Queue<Action>();

		public Dispatcher()
		{
			this.ownerThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		public TResult Run<TResult>(Func<TResult> callback)
		{
			if (this.ManagesThisThread())
			{
				return callback();
			}
			EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
			Dispatcher.CallbackStorage<TResult> result = new Dispatcher.CallbackStorage<TResult>();
			object obj = this.queue;
			lock (obj)
			{
				this.queue.Enqueue(delegate
				{
					try
					{
						result.Result = callback();
					}
					catch (Exception exception)
					{
						result.Exception = exception;
					}
					finally
					{
						waitHandle.Set();
					}
				});
			}
			waitHandle.WaitOne();
			if (result.Exception != null)
			{
				throw result.Exception;
			}
			return result.Result;
		}

		internal bool ManagesThisThread()
		{
			return Thread.CurrentThread.ManagedThreadId == this.ownerThreadId;
		}

		public void PollJobs()
		{
			for (;;)
			{
				object obj = this.queue;
				Action action;
				lock (obj)
				{
					if (this.queue.Count <= 0)
					{
						break;
					}
					action = this.queue.Dequeue();
				}
				action();
			}
		}

		private class CallbackStorage<TResult>
		{
			public TResult Result { get; set; }

			public Exception Exception { get; set; }
		}
	}
}
