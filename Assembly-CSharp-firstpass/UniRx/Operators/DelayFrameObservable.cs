using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class DelayFrameObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public DelayFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new DelayFrameObservable<T>.DelayFrame(this, observer, cancel).Run();
		}

		private class DelayFrame : OperatorObserverBase<T, T>
		{
			private readonly DelayFrameObservable<T> parent;

			private readonly object gate = new object();

			private readonly DelayFrameObservable<T>.QueuePool pool = new DelayFrameObservable<T>.QueuePool();

			private int runningEnumeratorCount;

			private bool readyDrainEnumerator;

			private bool running;

			private IDisposable sourceSubscription;

			private Queue<T> currentQueueReference;

			private bool calledCompleted;

			private bool hasError;

			private Exception error;

			private BooleanDisposable cancelationToken;

			public DelayFrame(DelayFrameObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.cancelationToken = new BooleanDisposable();
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.sourceSubscription = singleAssignmentDisposable;
				singleAssignmentDisposable.Disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(this.cancelationToken, this.sourceSubscription);
			}

			private IEnumerator DrainQueue(Queue<T> q, int frameCount)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.readyDrainEnumerator = false;
					this.running = false;
				}
				while (!this.cancelationToken.IsDisposed)
				{
					int num;
					frameCount = (num = frameCount) - 1;
					if (num == 0)
					{
						break;
					}
					yield return null;
				}
				try
				{
					if (q != null)
					{
						while (q.Count > 0 && !this.hasError)
						{
							if (this.cancelationToken.IsDisposed)
							{
								break;
							}
							object obj2 = this.gate;
							lock (obj2)
							{
								this.running = true;
							}
							T value = q.Dequeue();
							this.observer.OnNext(value);
							object obj3 = this.gate;
							lock (obj3)
							{
								this.running = false;
							}
						}
						if (q.Count == 0)
						{
							this.pool.Return(q);
						}
					}
					if (this.hasError)
					{
						if (!this.cancelationToken.IsDisposed)
						{
							this.cancelationToken.Dispose();
							try
							{
								this.observer.OnError(this.error);
							}
							finally
							{
								base.Dispose();
							}
						}
					}
					else if (this.calledCompleted)
					{
						object obj4 = this.gate;
						lock (obj4)
						{
							if (this.runningEnumeratorCount != 1)
							{
								yield break;
							}
						}
						if (!this.cancelationToken.IsDisposed)
						{
							this.cancelationToken.Dispose();
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
				finally
				{
					object obj5 = this.gate;
					lock (obj5)
					{
						this.runningEnumeratorCount--;
					}
				}
				yield break;
			}

			public override void OnNext(T value)
			{
				if (this.cancelationToken.IsDisposed)
				{
					return;
				}
				Queue<T> queue = null;
				object obj = this.gate;
				lock (obj)
				{
					if (this.readyDrainEnumerator)
					{
						if (this.currentQueueReference != null)
						{
							this.currentQueueReference.Enqueue(value);
						}
						return;
					}
					this.readyDrainEnumerator = true;
					this.runningEnumeratorCount++;
					queue = (this.currentQueueReference = this.pool.Get());
					queue.Enqueue(value);
				}
				switch (this.parent.frameCountType)
				{
				case FrameCountType.Update:
					MainThreadDispatcher.StartUpdateMicroCoroutine(this.DrainQueue(queue, this.parent.frameCount));
					break;
				case FrameCountType.FixedUpdate:
					MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.DrainQueue(queue, this.parent.frameCount));
					break;
				case FrameCountType.EndOfFrame:
					MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.DrainQueue(queue, this.parent.frameCount));
					break;
				default:
					throw new ArgumentException("Invalid FrameCountType:" + this.parent.frameCountType);
				}
			}

			public override void OnError(Exception error)
			{
				this.sourceSubscription.Dispose();
				if (this.cancelationToken.IsDisposed)
				{
					return;
				}
				object obj = this.gate;
				lock (obj)
				{
					if (this.running)
					{
						this.hasError = true;
						this.error = error;
						return;
					}
				}
				this.cancelationToken.Dispose();
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
				this.sourceSubscription.Dispose();
				if (this.cancelationToken.IsDisposed)
				{
					return;
				}
				object obj = this.gate;
				lock (obj)
				{
					this.calledCompleted = true;
					if (this.readyDrainEnumerator)
					{
						return;
					}
					this.readyDrainEnumerator = true;
					this.runningEnumeratorCount++;
				}
				switch (this.parent.frameCountType)
				{
				case FrameCountType.Update:
					MainThreadDispatcher.StartUpdateMicroCoroutine(this.DrainQueue(null, this.parent.frameCount));
					break;
				case FrameCountType.FixedUpdate:
					MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.DrainQueue(null, this.parent.frameCount));
					break;
				case FrameCountType.EndOfFrame:
					MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.DrainQueue(null, this.parent.frameCount));
					break;
				default:
					throw new ArgumentException("Invalid FrameCountType:" + this.parent.frameCountType);
				}
			}
		}

		private class QueuePool
		{
			private readonly object gate = new object();

			private readonly Queue<Queue<T>> pool = new Queue<Queue<T>>(2);

			public Queue<T> Get()
			{
				object obj = this.gate;
				Queue<T> result;
				lock (obj)
				{
					if (this.pool.Count == 0)
					{
						result = new Queue<T>(2);
					}
					else
					{
						result = this.pool.Dequeue();
					}
				}
				return result;
			}

			public void Return(Queue<T> q)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.pool.Enqueue(q);
				}
			}
		}
	}
}
