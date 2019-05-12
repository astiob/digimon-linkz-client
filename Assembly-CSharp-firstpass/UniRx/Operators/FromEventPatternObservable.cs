using System;

namespace UniRx.Operators
{
	internal class FromEventPatternObservable<TDelegate, TEventArgs> : OperatorObservableBase<EventPattern<TEventArgs>> where TEventArgs : EventArgs
	{
		private readonly Func<EventHandler<TEventArgs>, TDelegate> conversion;

		private readonly Action<TDelegate> addHandler;

		private readonly Action<TDelegate> removeHandler;

		public FromEventPatternObservable(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) : base(false)
		{
			this.conversion = conversion;
			this.addHandler = addHandler;
			this.removeHandler = removeHandler;
		}

		protected override IDisposable SubscribeCore(IObserver<EventPattern<TEventArgs>> observer, IDisposable cancel)
		{
			FromEventPatternObservable<TDelegate, TEventArgs>.FromEventPattern fromEventPattern = new FromEventPatternObservable<TDelegate, TEventArgs>.FromEventPattern(this, observer);
			return (!fromEventPattern.Register()) ? Disposable.Empty : fromEventPattern;
		}

		private class FromEventPattern : IDisposable
		{
			private readonly FromEventPatternObservable<TDelegate, TEventArgs> parent;

			private readonly IObserver<EventPattern<TEventArgs>> observer;

			private TDelegate handler;

			public FromEventPattern(FromEventPatternObservable<TDelegate, TEventArgs> parent, IObserver<EventPattern<TEventArgs>> observer)
			{
				this.parent = parent;
				this.observer = observer;
			}

			public bool Register()
			{
				this.handler = this.parent.conversion(new EventHandler<TEventArgs>(this.OnNext));
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

			private void OnNext(object sender, TEventArgs eventArgs)
			{
				this.observer.OnNext(new EventPattern<TEventArgs>(sender, eventArgs));
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
