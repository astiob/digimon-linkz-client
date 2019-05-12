using System;

namespace UniRx.Operators
{
	internal class FromEventObservable_<T> : OperatorObservableBase<T>
	{
		private readonly Action<Action<T>> addHandler;

		private readonly Action<Action<T>> removeHandler;

		public FromEventObservable_(Action<Action<T>> addHandler, Action<Action<T>> removeHandler) : base(false)
		{
			this.addHandler = addHandler;
			this.removeHandler = removeHandler;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			FromEventObservable_<T>.FromEvent fromEvent = new FromEventObservable_<T>.FromEvent(this, observer);
			return (!fromEvent.Register()) ? Disposable.Empty : fromEvent;
		}

		private class FromEvent : IDisposable
		{
			private readonly FromEventObservable_<T> parent;

			private readonly IObserver<T> observer;

			private Action<T> handler;

			public FromEvent(FromEventObservable_<T> parent, IObserver<T> observer)
			{
				this.parent = parent;
				this.observer = observer;
				this.handler = new Action<T>(this.OnNext);
			}

			public bool Register()
			{
				try
				{
					this.parent.addHandler(this.handler);
				}
				catch (Exception error)
				{
					this.observer.OnError(error);
					return false;
				}
				return true;
			}

			private void OnNext(T value)
			{
				this.observer.OnNext(value);
			}

			public void Dispose()
			{
				if (this.handler != null)
				{
					this.parent.removeHandler(this.handler);
					this.handler = null;
				}
			}
		}
	}
}
