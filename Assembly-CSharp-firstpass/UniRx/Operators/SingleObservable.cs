using System;

namespace UniRx.Operators
{
	internal class SingleObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly bool useDefault;

		private readonly Func<T, bool> predicate;

		public SingleObservable(IObservable<T> source, bool useDefault) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.useDefault = useDefault;
		}

		public SingleObservable(IObservable<T> source, Func<T, bool> predicate, bool useDefault) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
			this.useDefault = useDefault;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.predicate == null)
			{
				return this.source.Subscribe(new SingleObservable<T>.Single(this, observer, cancel));
			}
			return this.source.Subscribe(new SingleObservable<T>.Single_(this, observer, cancel));
		}

		private class Single : OperatorObserverBase<T, T>
		{
			private readonly SingleObservable<T> parent;

			private bool seenValue;

			private T lastValue;

			public Single(SingleObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.seenValue = false;
			}

			public override void OnNext(T value)
			{
				if (this.seenValue)
				{
					try
					{
						this.observer.OnError(new InvalidOperationException("sequence is not single"));
					}
					finally
					{
						base.Dispose();
					}
				}
				else
				{
					this.seenValue = true;
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
					if (!this.seenValue)
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
				else if (!this.seenValue)
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

		private class Single_ : OperatorObserverBase<T, T>
		{
			private readonly SingleObservable<T> parent;

			private bool seenValue;

			private T lastValue;

			public Single_(SingleObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.seenValue = false;
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
					if (this.seenValue)
					{
						try
						{
							this.observer.OnError(new InvalidOperationException("sequence is not single"));
						}
						finally
						{
							base.Dispose();
						}
						return;
					}
					this.seenValue = true;
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
					if (!this.seenValue)
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
				else if (!this.seenValue)
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
