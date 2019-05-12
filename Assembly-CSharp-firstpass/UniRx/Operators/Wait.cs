using System;
using System.Threading;

namespace UniRx.Operators
{
	internal class Wait<T> : IObserver<T>
	{
		private static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1);

		private readonly IObservable<T> source;

		private readonly TimeSpan timeout;

		private ManualResetEvent semaphore;

		private bool seenValue;

		private T value = default(T);

		private Exception ex;

		public Wait(IObservable<T> source, TimeSpan timeout)
		{
			this.source = source;
			this.timeout = timeout;
		}

		public T Run()
		{
			this.semaphore = new ManualResetEvent(false);
			using (this.source.Subscribe(this))
			{
				if (!((!(this.timeout == Wait<T>.InfiniteTimeSpan)) ? this.semaphore.WaitOne(this.timeout) : this.semaphore.WaitOne()))
				{
					throw new TimeoutException("OnCompleted not fired.");
				}
			}
			if (this.ex != null)
			{
				throw this.ex;
			}
			if (!this.seenValue)
			{
				throw new InvalidOperationException("No Elements.");
			}
			return this.value;
		}

		public void OnNext(T value)
		{
			this.seenValue = true;
			this.value = value;
		}

		public void OnError(Exception error)
		{
			this.ex = error;
			this.semaphore.Set();
		}

		public void OnCompleted()
		{
			this.semaphore.Set();
		}
	}
}
