using System;
using UnityEngine;

namespace UniRx.Operators
{
	internal class FrameTimeIntervalObservable<T> : OperatorObservableBase<TimeInterval<T>>
	{
		private readonly IObservable<T> source;

		private readonly bool ignoreTimeScale;

		public FrameTimeIntervalObservable(IObservable<T> source, bool ignoreTimeScale) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.ignoreTimeScale = ignoreTimeScale;
		}

		protected override IDisposable SubscribeCore(IObserver<TimeInterval<T>> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new FrameTimeIntervalObservable<T>.FrameTimeInterval(this, observer, cancel));
		}

		private class FrameTimeInterval : OperatorObserverBase<T, TimeInterval<T>>
		{
			private readonly FrameTimeIntervalObservable<T> parent;

			private float lastTime;

			public FrameTimeInterval(FrameTimeIntervalObservable<T> parent, IObserver<TimeInterval<T>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				this.lastTime = ((!parent.ignoreTimeScale) ? Time.time : Time.unscaledTime);
			}

			public override void OnNext(T value)
			{
				float num = (!this.parent.ignoreTimeScale) ? Time.time : Time.unscaledTime;
				float num2 = num - this.lastTime;
				this.lastTime = num;
				this.observer.OnNext(new TimeInterval<T>(value, TimeSpan.FromSeconds((double)num2)));
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
