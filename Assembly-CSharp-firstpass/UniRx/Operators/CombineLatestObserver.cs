using System;

namespace UniRx.Operators
{
	internal class CombineLatestObserver<T> : IObserver<T>
	{
		private readonly object gate;

		private readonly ICombineLatestObservable parent;

		private readonly int index;

		private T value;

		public CombineLatestObserver(object gate, ICombineLatestObservable parent, int index)
		{
			this.gate = gate;
			this.parent = parent;
			this.index = index;
		}

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public void OnNext(T value)
		{
			object obj = this.gate;
			lock (obj)
			{
				this.value = value;
				this.parent.Publish(this.index);
			}
		}

		public void OnError(Exception error)
		{
			object obj = this.gate;
			lock (obj)
			{
				this.parent.Fail(error);
			}
		}

		public void OnCompleted()
		{
			object obj = this.gate;
			lock (obj)
			{
				this.parent.Done(this.index);
			}
		}
	}
}
