using System;

namespace UniRx.Operators
{
	internal class FirstObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly bool useDefault;

		private readonly Func<T, bool> predicate;

		public FirstObservable(IObservable<T> source, bool useDefault) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.useDefault = useDefault;
		}

		public FirstObservable(IObservable<T> source, Func<T, bool> predicate, bool useDefault) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
			this.useDefault = useDefault;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.predicate == null)
			{
				return this.source.Subscribe(new FirstObservable<T>.First(this, observer, cancel));
			}
			return this.source.Subscribe(new FirstObservable<T>.First_(this, observer, cancel));
		}

		private class First : OperatorObserverBase<T, T>
		{
			private readonly FirstObservable<T> parent;

			private bool notPublished;

			public First(FirstObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.notPublished = true;
			}

			public override void OnNext(T value)
			{
				if (this.notPublished)
				{
					this.notPublished = false;
					this.observer.OnNext(value);
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					return;
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

		private class First_ : OperatorObserverBase<T, T>
		{
			private readonly FirstObservable<T> parent;

			private bool notPublished;

			public First_(FirstObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.notPublished = true;
			}

			public override void OnNext(T value)
			{
				if (this.notPublished)
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
						this.observer.OnNext(value);
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
