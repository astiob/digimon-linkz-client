using System;

namespace UniRx.Operators
{
	internal class SampleFrameObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public SampleFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			return new SampleFrameObservable<T>.SampleFrame(this, observer, cancel).Run();
		}

		private class SampleFrame : OperatorObserverBase<T, T>
		{
			private readonly SampleFrameObservable<T> parent;

			private readonly object gate = new object();

			private T latestValue = default(T);

			private bool isUpdated;

			private bool isCompleted;

			private SingleAssignmentDisposable sourceSubscription;

			public SampleFrame(SampleFrameObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.sourceSubscription = new SingleAssignmentDisposable();
				this.sourceSubscription.Disposable = this.parent.source.Subscribe(this);
				IDisposable disposable = Observable.IntervalFrame(this.parent.frameCount, this.parent.frameCountType).Subscribe(new SampleFrameObservable<T>.SampleFrame.SampleFrameTick(this));
				return StableCompositeDisposable.Create(this.sourceSubscription, disposable);
			}

			private void OnNextTick(long _)
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.isUpdated)
					{
						T value = this.latestValue;
						this.isUpdated = false;
						this.observer.OnNext(value);
					}
					if (this.isCompleted)
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

			public override void OnNext(T value)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.latestValue = value;
					this.isUpdated = true;
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
					this.isCompleted = true;
					this.sourceSubscription.Dispose();
				}
			}

			private class SampleFrameTick : IObserver<long>
			{
				private readonly SampleFrameObservable<T>.SampleFrame parent;

				public SampleFrameTick(SampleFrameObservable<T>.SampleFrame parent)
				{
					this.parent = parent;
				}

				public void OnCompleted()
				{
				}

				public void OnError(Exception error)
				{
				}

				public void OnNext(long _)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						if (this.parent.isUpdated)
						{
							T latestValue = this.parent.latestValue;
							this.parent.isUpdated = false;
							this.parent.observer.OnNext(latestValue);
						}
						if (this.parent.isCompleted)
						{
							try
							{
								this.parent.observer.OnCompleted();
							}
							finally
							{
								this.parent.Dispose();
							}
						}
					}
				}
			}
		}
	}
}
