using System;
using System.Collections;

namespace UniRx.Operators
{
	internal class BatchFrameObservable : OperatorObservableBase<Unit>
	{
		private readonly IObservable<Unit> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public BatchFrameObservable(IObservable<Unit> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<Unit>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<Unit> observer, IDisposable cancel)
		{
			return new BatchFrameObservable.BatchFrame(this, observer, cancel).Run();
		}

		private class BatchFrame : OperatorObserverBase<Unit, Unit>
		{
			private readonly BatchFrameObservable parent;

			private readonly object gate = new object();

			private readonly BooleanDisposable cancellationToken = new BooleanDisposable();

			private readonly IEnumerator timer;

			private bool isRunning;

			private bool isCompleted;

			public BatchFrame(BatchFrameObservable parent, IObserver<Unit> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.timer = new BatchFrameObservable.BatchFrame.ReusableEnumerator(this);
			}

			public IDisposable Run()
			{
				IDisposable disposable = this.parent.source.Subscribe(this);
				return StableCompositeDisposable.Create(disposable, this.cancellationToken);
			}

			public override void OnNext(Unit value)
			{
				object obj = this.gate;
				lock (obj)
				{
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
				bool flag;
				lock (obj)
				{
					flag = this.isRunning;
					this.isCompleted = true;
				}
				if (flag)
				{
					this.observer.OnNext(Unit.Default);
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
				private readonly BatchFrameObservable.BatchFrame parent;

				private int currentFrame;

				public ReusableEnumerator(BatchFrameObservable.BatchFrame parent)
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
						this.parent.isRunning = false;
					}
					this.parent.observer.OnNext(Unit.Default);
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
