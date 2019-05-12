using System;

namespace UniRx.Operators
{
	internal class SynchronizedObserver<T> : IObserver<T>
	{
		private readonly IObserver<T> observer;

		private readonly object gate;

		public SynchronizedObserver(IObserver<T> observer, object gate)
		{
			this.observer = observer;
			this.gate = gate;
		}

		public void OnNext(T value)
		{
			object obj = this.gate;
			lock (obj)
			{
				this.observer.OnNext(value);
			}
		}

		public void OnError(Exception error)
		{
			object obj = this.gate;
			lock (obj)
			{
				this.observer.OnError(error);
			}
		}

		public void OnCompleted()
		{
			object obj = this.gate;
			lock (obj)
			{
				this.observer.OnCompleted();
			}
		}
	}
}
