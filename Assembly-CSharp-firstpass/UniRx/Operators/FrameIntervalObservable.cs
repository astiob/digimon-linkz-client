using System;
using UnityEngine;

namespace UniRx.Operators
{
	internal class FrameIntervalObservable<T> : OperatorObservableBase<FrameInterval<T>>
	{
		private readonly IObservable<T> source;

		public FrameIntervalObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<FrameInterval<T>> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new FrameIntervalObservable<T>.FrameInterval(observer, cancel));
		}

		private class FrameInterval : OperatorObserverBase<T, FrameInterval<T>>
		{
			private int lastFrame;

			public FrameInterval(IObserver<FrameInterval<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.lastFrame = Time.frameCount;
			}

			public override void OnNext(T value)
			{
				int frameCount = Time.frameCount;
				int interval = frameCount - this.lastFrame;
				this.lastFrame = frameCount;
				this.observer.OnNext(new FrameInterval<T>(value, interval));
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
