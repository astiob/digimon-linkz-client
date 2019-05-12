using System;

namespace UniRx.Operators
{
	internal class StartWithObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly T value;

		private readonly Func<T> valueFactory;

		public StartWithObservable(IObservable<T> source, T value) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.value = value;
		}

		public StartWithObservable(IObservable<T> source, Func<T> valueFactory) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.valueFactory = valueFactory;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new StartWithObservable<T>.StartWith(this, observer, cancel).Run();
		}

		private class StartWith : OperatorObserverBase<T, T>
		{
			private readonly StartWithObservable<T> parent;

			public StartWith(StartWithObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				T value;
				if (this.parent.valueFactory == null)
				{
					value = this.parent.value;
				}
				else
				{
					try
					{
						value = this.parent.valueFactory();
					}
					catch (Exception error)
					{
						try
						{
							this.observer.OnError(error);
						}
						finally
						{
							base.Dispose();
						}
						return Disposable.Empty;
					}
				}
				this.OnNext(value);
				return this.parent.source.Subscribe(this.observer);
			}

			public override void OnNext(T value)
			{
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
