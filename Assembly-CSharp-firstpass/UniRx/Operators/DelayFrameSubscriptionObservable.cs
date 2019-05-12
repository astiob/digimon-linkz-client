using System;

namespace UniRx.Operators
{
	internal class DelayFrameSubscriptionObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly int frameCount;

		private readonly FrameCountType frameCountType;

		public DelayFrameSubscriptionObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.frameCount = frameCount;
			this.frameCountType = frameCountType;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			MultipleAssignmentDisposable multipleAssignmentDisposable = new MultipleAssignmentDisposable();
			multipleAssignmentDisposable.Disposable = Observable.TimerFrame(this.frameCount, this.frameCountType).SubscribeWithState3(observer, multipleAssignmentDisposable, this.source, delegate(long _, IObserver<T> o, MultipleAssignmentDisposable disp, IObservable<T> s)
			{
				disp.Disposable = s.Subscribe(o);
			});
			return multipleAssignmentDisposable;
		}
	}
}
