using System;

namespace UniRx.Operators
{
	internal class SubscribeOnMainThreadObservable<T> : OperatorObservableBase<T>
	{
		private readonly IObservable<T> source;

		private readonly IObservable<long> subscribeTrigger;

		public SubscribeOnMainThreadObservable(IObservable<T> source, IObservable<long> subscribeTrigger) : base(source.IsRequiredSubscribeOnCurrentThread<T>())
		{
			this.source = source;
			this.subscribeTrigger = subscribeTrigger;
		}

		protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
		{
			SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
			SerialDisposable serialDisposable = new SerialDisposable();
			serialDisposable.Disposable = singleAssignmentDisposable;
			singleAssignmentDisposable.Disposable = this.subscribeTrigger.SubscribeWithState3(observer, serialDisposable, this.source, delegate(long _, IObserver<T> o, SerialDisposable disp, IObservable<T> s)
			{
				disp.Disposable = s.Subscribe(o);
			});
			return serialDisposable;
		}
	}
}
