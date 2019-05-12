using System;

namespace UniRx.Operators
{
	internal class FromEventObservable : OperatorObservableBase<Unit>
	{
		private readonly Action<Action> addHandler;

		private readonly Action<Action> removeHandler;

		public FromEventObservable(Action<Action> addHandler, Action<Action> removeHandler) : base(false)
		{
			this.addHandler = addHandler;
			this.removeHandler = removeHandler;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			FromEventObservable.FromEvent fromEvent = new FromEventObservable.FromEvent(this, observer);
			return (!fromEvent.Register()) ? Disposable.Empty : fromEvent;
		}

		private class FromEvent : IDisposable
		{
			private readonly FromEventObservable parent;

			private readonly IObserver<Unit> observer;

			private Action handler;

			public FromEvent(FromEventObservable parent, IObserver<Unit> observer)
			{
				this.parent = parent;
				this.observer = observer;
				this.handler = new Action(this.OnNext);
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

			private void OnNext()
			{
				this.observer.OnNext(Unit.Default);
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
