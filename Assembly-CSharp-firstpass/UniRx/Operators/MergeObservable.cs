using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class MergeObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<IObservable<T>> sources;

		private readonly int maxConcurrent;

		public MergeObservable(IObservable<IObservable<T>> sources, bool isRequiredSubscribeOnCurrentThread) : base(isRequiredSubscribeOnCurrentThread)
		{
			this.sources = sources;
		}

		public MergeObservable(IObservable<IObservable<T>> sources, int maxConcurrent, bool isRequiredSubscribeOnCurrentThread) : base(isRequiredSubscribeOnCurrentThread)
		{
			this.sources = sources;
			this.maxConcurrent = maxConcurrent;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			if (this.maxConcurrent > 0)
			{
				return new MergeObservable<T>.MergeConcurrentObserver(this, observer, cancel).Run();
			}
			return new MergeObservable<T>.MergeOuterObserver(this, observer, cancel).Run();
		}

		private class MergeOuterObserver : OperatorObserverBase<IObservable<T>, T>
		{
			private readonly MergeObservable<T> parent;

			private CompositeDisposable collectionDisposable;

			private SingleAssignmentDisposable sourceDisposable;

			private object gate = new object();

			private bool isStopped;

			public MergeOuterObserver(MergeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.collectionDisposable = new CompositeDisposable();
				this.sourceDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(this.sourceDisposable);
				this.sourceDisposable.Disposable = this.parent.sources.Subscribe(this);
				return this.collectionDisposable;
			}

			public override void OnNext(IObservable<T> value)
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(singleAssignmentDisposable);
				MergeObservable<T>.MergeOuterObserver.Merge observer = new MergeObservable<T>.MergeOuterObserver.Merge(this, singleAssignmentDisposable);
				singleAssignmentDisposable.Disposable = value.Subscribe(observer);
			}

			public override void OnError(Exception error)
			{
				object obj = this.gate;
				lock (obj)
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

			public override void OnCompleted()
			{
				this.isStopped = true;
				if (this.collectionDisposable.Count == 1)
				{
					object obj = this.gate;
					lock (obj)
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
				else
				{
					this.sourceDisposable.Dispose();
				}
			}

			private class Merge : OperatorObserverBase<T, T>
			{
				private readonly MergeObservable<T>.MergeOuterObserver parent;

				private readonly IDisposable cancel;

				public Merge(MergeObservable<T>.MergeOuterObserver parent, IDisposable cancel) : base(parent.observer, cancel)
				{
					this.parent = parent;
					this.cancel = cancel;
				}

				public override void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.observer.OnNext(value);
					}
				}

				public override void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
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

				public override void OnCompleted()
				{
					this.parent.collectionDisposable.Remove(this.cancel);
					if (this.parent.isStopped && this.parent.collectionDisposable.Count == 1)
					{
						object gate = this.parent.gate;
						lock (gate)
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

		private class MergeConcurrentObserver : OperatorObserverBase<IObservable<T>, T>
		{
			private readonly MergeObservable<T> parent;

			private CompositeDisposable collectionDisposable;

			private SingleAssignmentDisposable sourceDisposable;

			private object gate = new object();

			private bool isStopped;

			private Queue<IObservable<T>> q;

			private int activeCount;

			public MergeConcurrentObserver(MergeObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.q = new Queue<IObservable<T>>();
				this.activeCount = 0;
				this.collectionDisposable = new CompositeDisposable();
				this.sourceDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(this.sourceDisposable);
				this.sourceDisposable.Disposable = this.parent.sources.Subscribe(this);
				return this.collectionDisposable;
			}

			public override void OnNext(IObservable<T> value)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.activeCount < this.parent.maxConcurrent)
					{
						this.activeCount++;
						this.Subscribe(value);
					}
					else
					{
						this.q.Enqueue(value);
					}
				}
			}

			public override void OnError(Exception error)
			{
				object obj = this.gate;
				lock (obj)
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

			public override void OnCompleted()
			{
				object obj = this.gate;
				lock (obj)
				{
					this.isStopped = true;
					if (this.activeCount == 0)
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
					else
					{
						this.sourceDisposable.Dispose();
					}
				}
			}

			private void Subscribe(IObservable<T> innerSource)
			{
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(singleAssignmentDisposable);
				MergeObservable<T>.MergeConcurrentObserver.Merge observer = new MergeObservable<T>.MergeConcurrentObserver.Merge(this, singleAssignmentDisposable);
				singleAssignmentDisposable.Disposable = innerSource.Subscribe(observer);
			}

			private class Merge : OperatorObserverBase<T, T>
			{
				private readonly MergeObservable<T>.MergeConcurrentObserver parent;

				private readonly IDisposable cancel;

				public Merge(MergeObservable<T>.MergeConcurrentObserver parent, IDisposable cancel) : base(parent.observer, cancel)
				{
					this.parent = parent;
					this.cancel = cancel;
				}

				public override void OnNext(T value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.observer.OnNext(value);
					}
				}

				public override void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
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

				public override void OnCompleted()
				{
					this.parent.collectionDisposable.Remove(this.cancel);
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.q.Count > 0)
						{
							IObservable<T> innerSource = this.parent.q.Dequeue();
							this.parent.Subscribe(innerSource);
						}
						else
						{
							this.parent.activeCount--;
							if (this.parent.isStopped && this.parent.activeCount == 0)
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
		}
	}
}
