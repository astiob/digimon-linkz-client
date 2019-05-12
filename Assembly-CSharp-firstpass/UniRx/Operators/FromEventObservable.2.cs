using System;

namespace UniRx.Operators
{
	internal class FromEventObservable<TDelegate, TEventArgs> : OperatorObservableBase<TEventArgs>
	{
		private readonly Func<Action<TEventArgs>, TDelegate> conversion;

		private readonly Action<TDelegate> addHandler;

		private readonly Action<TDelegate> removeHandler;

		public FromEventObservable(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) : base(false)
		{
			this.conversion = conversion;
			this.addHandler = addHandler;
			this.removeHandler = removeHandler;
		}

		protected override IDisposable SubscribeCore(IObserver<TEventArgs> observer, IDisposable cancel)
		{
			FromEventObservable<TDelegate, TEventArgs>.FromEvent fromEvent = new FromEventObservable<TDelegate, TEventArgs>.FromEvent(this, observer);
			return (!fromEvent.Register()) ? Disposable.Empty : fromEvent;
		}

		private class FromEvent : IDisposable
		{
			private readonly FromEventObservable<TDelegate, TEventArgs> parent;

			private readonly IObserver<TEventArgs> observer;

			private TDelegate handler;

			public FromEvent(FromEventObservable<TDelegate, TEventArgs> parent, IObserver<TEventArgs> observer)
			{
				this.parent = parent;
				this.observer = observer;
			}

			public bool Register()
			{
				this.handler = this.parent.conversion(new Action<TEventArgs>(this.OnNext));
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

			private void OnNext(TEventArgs args)
			{
				this.observer.OnNext(args);
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
