using System;
using System.Collections;

namespace UniRx.Operators
{
	internal class FromMicroCoroutineObservable<T> : OperatorObservableBase<T>
	{
		private readonly Func<IObserver<T>, CancellationToken, IEnumerator> coroutine;

		private readonly FrameCountType frameCountType;

		public FromMicroCoroutineObservable(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine, FrameCountType frameCountType) : base(false)
		{
			this.coroutine = coroutine;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			FromMicroCoroutineObservable<T>.FromMicroCoroutine arg = new FromMicroCoroutineObservable<T>.FromMicroCoroutine(observer, cancel);
			BooleanDisposable booleanDisposable = new BooleanDisposable();
			CancellationToken arg2 = new CancellationToken(booleanDisposable);
			switch (this.frameCountType)
			{
			case FrameCountType.Update:
				MainThreadDispatcher.StartUpdateMicroCoroutine(this.coroutine(arg, arg2));
				break;
			case FrameCountType.FixedUpdate:
				MainThreadDispatcher.StartFixedUpdateMicroCoroutine(this.coroutine(arg, arg2));
				break;
			case FrameCountType.EndOfFrame:
				MainThreadDispatcher.StartEndOfFrameMicroCoroutine(this.coroutine(arg, arg2));
				break;
			default:
				throw new ArgumentException("Invalid FrameCountType:" + this.frameCountType);
			}
			return booleanDisposable;
		}

		private class FromMicroCoroutine : OperatorObserverBase<T, T>
		{
			public FromMicroCoroutine(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(T value)
			{
				try
				{
					this.observer.OnNext(value);
				}
				catch
				{
					base.Dispose();
					throw;
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
