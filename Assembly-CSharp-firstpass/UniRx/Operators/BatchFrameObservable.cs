using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class BatchFrameObservable<T> : OperatorObservableBase<IList<T>>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public BatchFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<T>> observer, IDisposable cancel)
		{
			return new BatchFrameObservable<T>.BatchFrame(this, observer, cancel).Run();
		}

		private class BatchFrame : OperatorObserverBase<T, IList<T>>
		{
			private readonly BatchFrameObservable<T> parent;

			private readonly object gate = new object();

			private readonly BooleanDisposable cancellationToken = new BooleanDisposable();

			private readonly IEnumerator timer;

			private bool isRunning;

			private bool isCompleted;

			private List<T> list;

			public BatchFrame(BatchFrameObservable<T> parent, IObserver<IList<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.timer = new BatchFrameObservable<T>.BatchFrame.ReusableEnumerator(this);
			}

			public IDisposable Run()
			{
				this.list = new List<T>();
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, this.cancellationToken);
			}

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (!this.isCompleted)
					{
						this.list.Add(value);
						if (!this.isRunning)
						{
							this.isRunning = true;
							this.timer.Reset();
							switch (this.parent.frameCountType)
							{
							case FrameCountType.Update:
								MainThreadDispatcher.StartUpdateMicroCoroutine(this.timer);
								break;
							case FrameCountType.FixedUpdate:
								MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.timer);
								break;
							case FrameCountType.EndOfFrame:
								MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.timer);
								break;
							}
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
				object obj = this.gate;
				List<T> list;
				lock (obj)
				{
					this.isCompleted = true;
					list = this.list;
				}
				if (list.Count != 0)
				{
					this.observer.OnNext(list);
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

			private class ReusableEnumerator : IEnumerator
			{
				private readonly BatchFrameObservable<T>.BatchFrame parent;

				private int currentFrame;

				public ReusableEnumerator(BatchFrameObservable<T>.BatchFrame parent)
				{
					this.parent = parent;
				}

				public object Current
				{
					get
					{
						return null;
					}
				}

				public bool MoveNext()
				{
					if (this.parent.cancellationToken.IsDisposed)
					{
						return false;
					}
					object gate = this.parent.gate;
					List<T> list;
					lock (gate)
					{
						if (this.currentFrame++ != this.parent.parent.frameCount)
						{
							return true;
						}
						if (this.parent.isCompleted)
						{
							return false;
						}
						list = this.parent.list;
						this.parent.list = new List<T>();
						this.parent.isRunning = false;
					}
					this.parent.observer.OnNext(list);
					return false;
				}

				public void Reset()
				{
					this.currentFrame = 0;
				}
			}
		}
	}
}
