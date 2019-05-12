using System;

namespace UniRx.Operators
{
	internal class LastObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly bool useDefault;

		private readonly Func<T, bool> predicate;

		public LastObservable(IObservable<T> source, bool useDefault) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.useDefault = useDefault;
		}

		public LastObservable(IObservable<T> source, Func<T, bool> predicate, bool useDefault) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
			this.useDefault = useDefault;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.predicate == null)
			{
				return this.source.Subscribe(new LastObservable<T>.Last(this, observer, cancel));
			}
			return this.source.Subscribe(new LastObservable<T>.Last_(this, observer, cancel));
		}

		private class Last : OperatorObserverBase<T, T>
		{
			private readonly LastObservable<T> parent;

			private bool notPublished;

			private T lastValue;

			public Last(LastObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.notPublished = true;
			}

			public override void OnNext(T value)
			{
				this.notPublished = false;
				this.lastValue = value;
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
				if (this.parent.useDefault)
				{
					if (this.notPublished)
					{
						this.observer.OnNext(default(T));
					}
					else
					{
						this.observer.OnNext(this.lastValue);
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
				else if (this.notPublished)
				{
					try
					{
						this.observer.OnError(new InvalidOperationException("sequence is empty"));
					}
					finally
					{
						base.Dispose();
					}
				}
				else
				{
					this.observer.OnNext(this.lastValue);
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

		private class Last_ : OperatorObserverBase<T, T>
		{
			private readonly LastObservable<T> parent;

			private bool notPublished;

			private T lastValue;

			public Last_(LastObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.notPublished = true;
			}

			public override void OnNext(T value)
			{
				bool flag;
				try
				{
					flag = this.parent.predicate(value);
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
					return;
				}
				if (flag)
				{
					this.notPublished = false;
					this.lastValue = value;
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
				if (this.parent.useDefault)
				{
					if (this.notPublished)
					{
						this.observer.OnNext(default(T));
					}
					else
					{
						this.observer.OnNext(this.lastValue);
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
				else if (this.notPublished)
				{
					try
					{
						this.observer.OnError(new InvalidOperationException("sequence is empty"));
					}
					finally
					{
						base.Dispose();
					}
				}
				else
				{
					this.observer.OnNext(this.lastValue);
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
}
