using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class WhenAllObservable : OperatorObservableBase<Unit>
	{
		private readonly IObservable<Unit>[] sources;

		private readonly IEnumerable<IObservable<Unit>> sourcesEnumerable;

		public WhenAllObservable(IObservable<Unit>[] sources) : base(false)
		{
			this.sources = sources;
		}

		public WhenAllObservable(IEnumerable<IObservable<Unit>> sources) : base(false)
		{
			this.sourcesEnumerable = sources;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			if (this.sources != null)
			{
				return new WhenAllObservable.WhenAll(this.sources, observer, cancel).Run();
			}
			IList<IObservable<Unit>> list = this.sourcesEnumerable as IList<IObservable<Unit>>;
			if (list == null)
			{
				list = new List<IObservable<Unit>>(this.sourcesEnumerable);
			}
			return new WhenAllObservable.WhenAll_(list, observer, cancel).Run();
		}

		private class WhenAll : OperatorObserverBase<Unit, Unit>
		{
			private readonly IObservable<Unit>[] sources;

			private readonly object gate = new object();

			private int completedCount;

			private int length;

			public WhenAll(IObservable<Unit>[] sources, IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.sources = sources;
			}

			public IDisposable Run()
			{
				this.length = this.sources.Length;
				if (this.length == 0)
				{
					this.OnNext(Unit.Default);
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					return Disposable.Empty;
				}
				this.completedCount = 0;
				IDisposable[] array = new IDisposable[this.length];
				for (int i = 0; i < this.sources.Length; i++)
				{
					IObservable<Unit> observable = this.sources[i];
					WhenAllObservable.WhenAll.WhenAllCollectionObserver observer = new WhenAllObservable.WhenAll.WhenAllCollectionObserver(this);
					array[i] = observable.Subscribe(observer);
				}
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			public override void OnNext(Unit value)
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

			private class WhenAllCollectionObserver : IObserver<Unit>
			{
				private readonly WhenAllObservable.WhenAll parent;

				private bool isCompleted;

				public WhenAllCollectionObserver(WhenAllObservable.WhenAll parent)
				{
					this.parent = parent;
				}

				public void OnNext(Unit value)
				{
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.parent.OnError(error);
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.isCompleted = true;
							this.parent.completedCount++;
							if (this.parent.completedCount == this.parent.length)
							{
								this.parent.OnNext(Unit.Default);
								this.parent.OnCompleted();
							}
						}
					}
				}
			}
		}

		private class WhenAll_ : OperatorObserverBase<Unit, Unit>
		{
			private readonly IList<IObservable<Unit>> sources;

			private readonly object gate = new object();

			private int completedCount;

			private int length;

			public WhenAll_(IList<IObservable<Unit>> sources, IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.sources = sources;
			}

			public IDisposable Run()
			{
				this.length = this.sources.Count;
				if (this.length == 0)
				{
					this.OnNext(Unit.Default);
					try
					{
						this.observer.OnCompleted();
					}
					finally
					{
						base.Dispose();
					}
					return Disposable.Empty;
				}
				this.completedCount = 0;
				IDisposable[] array = new IDisposable[this.length];
				for (int i = 0; i < this.length; i++)
				{
					IObservable<Unit> observable = this.sources[i];
					WhenAllObservable.WhenAll_.WhenAllCollectionObserver observer = new WhenAllObservable.WhenAll_.WhenAllCollectionObserver(this);
					array[i] = observable.Subscribe(observer);
				}
				return StableCompositeDisposable.CreateUnsafe(array);
			}

			public override void OnNext(Unit value)
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

			private class WhenAllCollectionObserver : IObserver<Unit>
			{
				private readonly WhenAllObservable.WhenAll_ parent;

				private bool isCompleted;

				public WhenAllCollectionObserver(WhenAllObservable.WhenAll_ parent)
				{
					this.parent = parent;
				}

				public void OnNext(Unit value)
				{
				}

				public void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.parent.OnError(error);
						}
					}
				}

				public void OnCompleted()
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (!this.isCompleted)
						{
							this.isCompleted = true;
							this.parent.completedCount++;
							if (this.parent.completedCount == this.parent.length)
							{
								this.parent.OnNext(Unit.Default);
								this.parent.OnCompleted();
							}
						}
					}
				}
			}
		}
	}
}
