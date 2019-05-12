using System;

namespace UniRx.Operators
{
	internal class SkipWhileObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly Func<T, bool> predicate;

		private readonly Func<T, int, bool> predicateWithIndex;

		public SkipWhileObservable(IObservable<T> source, Func<T, bool> predicate) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicate = predicate;
		}

		public SkipWhileObservable(IObservable<T> source, Func<T, int, bool> predicateWithIndex) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.predicateWithIndex = predicateWithIndex;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.predicate != null)
			{
				return new SkipWhileObservable<T>.SkipWhile(this, observer, cancel).Run();
			}
			return new SkipWhileObservable<T>.SkipWhile_(this, observer, cancel).Run();
		}

		private class SkipWhile : OperatorObserverBase<T, T>
		{
			private readonly SkipWhileObservable<T> parent;

			private bool endSkip;

			public SkipWhile(SkipWhileObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				if (!this.endSkip)
				{
					try
					{
						this.endSkip = !this.parent.predicate(value);
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
					if (!this.endSkip)
					{
						return;
					}
				}
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

		private class SkipWhile_ : OperatorObserverBase<T, T>
		{
			private readonly SkipWhileObservable<T> parent;

			private bool endSkip;

			private int index;

			public SkipWhile_(SkipWhileObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(T value)
			{
				if (!this.endSkip)
				{
					try
					{
						this.endSkip = !this.parent.predicateWithIndex(value, this.index++);
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
					if (!this.endSkip)
					{
						return;
					}
				}
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
