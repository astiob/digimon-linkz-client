using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ZipObserver<T> : IObserver<T>
	{
		private readonly object gate;

		private readonly IZipObservable parent;

		private readonly int index;

		private readonly Queue<T> queue;

		public ZipObserver(object gate, IZipObservable parent, int index, Queue<T> queue)
		{
			this.gate = gate;
			this.parent = parent;
			this.index = index;
			this.queue = queue;
		}

		public void OnNext(T value)
		{
			object obj = this.gate;
			lock (obj)
			{
				this.queue.Enqueue(value);
				this.parent.Dequeue(this.index);
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
