using System;

namespace UniRx.Operators
{
	internal class ForEachAsyncObservable<T> : OperatorObservableBase<Unit>
	{
		private readonly IObservable<T> source;

		private readonly Action<T> onNext;

		private readonly Action<T, int> onNextWithIndex;

		public ForEachAsyncObservable(IObservable<T> source, Action<T> onNext) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onNext = onNext;
		}

		public ForEachAsyncObservable(IObservable<T> source, Action<T, int> onNext) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.onNextWithIndex = onNext;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			if (this.onNext != null)
			{
				return this.source.Subscribe(new ForEachAsyncObservable<T>.ForEachAsync(this, observer, cancel));
			}
			return this.source.Subscribe(new ForEachAsyncObservable<T>.ForEachAsync_(this, observer, cancel));
		}

		private class ForEachAsync : OperatorObserverBase<T, Unit>
		{
			private readonly ForEachAsyncObservable<T> parent;

			public ForEachAsync(ForEachAsyncObservable<T> parent, IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				try
				{
					this.parent.onNext(value);
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
				}
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
				this.observer.OnNext(Unit.Default);
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

		private class ForEachAsync_ : OperatorObserverBase<T, Unit>
		{
			private readonly ForEachAsyncObservable<T> parent;

			private int index;

			public ForEachAsync_(ForEachAsyncObservable<T> parent, IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public override void OnNext(T value)
			{
				try
				{
					this.parent.onNextWithIndex(value, this.index++);
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
				}
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
				this.observer.OnNext(Unit.Default);
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
