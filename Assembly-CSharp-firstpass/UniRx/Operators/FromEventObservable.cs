using System;

namespace UniRx.Operators
{
	internal class FromEventObservable<TDelegate> : OperatorObservableBase<Unit>
	{
		private readonly Func<Action, TDelegate> conversion;

		private readonly Action<TDelegate> addHandler;

		private readonly Action<TDelegate> removeHandler;

		public FromEventObservable(Func<Action, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) : base(false)
		{
			this.conversion = conversion;
			this.addHandler = addHandler;
			this.removeHandler = removeHandler;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			FromEventObservable<TDelegate>.FromEvent fromEvent = new FromEventObservable<TDelegate>.FromEvent(this, observer);
			return (!fromEvent.Register()) ? Disposable.Empty : fromEvent;
		}

		private class FromEvent : IDisposable
		{
			private readonly FromEventObservable<TDelegate> parent;

			private readonly IObserver<Unit> observer;

			private TDelegate handler;

			public FromEvent(FromEventObservable<TDelegate> parent, IObserver<Unit> observer)
			{
				this.parent = parent;
				this.observer = observer;
			}

			public bool Register()
			{
				this.handler = this.parent.conversion(new Action(this.OnNext));
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
					this.handler = default(TDelegate);
				}
			}
		}
	}
}
