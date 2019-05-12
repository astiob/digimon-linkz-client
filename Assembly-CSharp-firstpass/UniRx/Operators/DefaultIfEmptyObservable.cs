using System;

namespace UniRx.Operators
{
	internal class DefaultIfEmptyObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly T defaultValue;

		public DefaultIfEmptyObservable(IObservable<T> source, T defaultValue) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.defaultValue = defaultValue;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new DefaultIfEmptyObservable<T>.DefaultIfEmpty(this, observer, cancel));
		}

		private class DefaultIfEmpty : OperatorObserverBase<T, T>
		{
			private readonly DefaultIfEmptyObservable<T> parent;

			private bool hasValue;

			public DefaultIfEmpty(DefaultIfEmptyObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.hasValue = false;
			}

			public override void OnNext(T value)
			{
				this.hasValue = true;
				this.observer.OnNext(value);
			}

			public override void OnError(Exception error)
			{
				try
				{
					this.observer.OnError(error);
				}
				finally
				{
					base.Dispose();
				}
			}

			public override void OnCompleted()
			{
				if (!this.hasValue)
				{
					this.observer.OnNext(this.parent.defaultValue);
				}
				try
				{
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
			}
		}
	}
}
