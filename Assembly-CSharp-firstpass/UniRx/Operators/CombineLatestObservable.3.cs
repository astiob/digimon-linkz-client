using System;

namespace UniRx.Operators
{
	internal class CombineLatestObservable<T1, T2, T3, TR> : OperatorObservableBase<TR>
	{
		private IObservable<T1> source1;

		private IObservable<T2> source2;

		private IObservable<T3> source3;

		private CombineLatestFunc<T1, T2, T3, TR> resultSelector;

		public CombineLatestObservable(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, CombineLatestFunc<T1, T2, T3, TR> resultSelector) : base(source1.IsRequiredSubscribeOnCurrentThread<T1>() || source2.IsRequiredSubscribeOnCurrentThread<T2>() || source3.IsRequiredSubscribeOnCurrentThread<T3>())
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.resultSelector = resultSelector;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return new CombineLatestObservable<T1, T2, T3, TR>.CombineLatest(3, this, observer, cancel).Run();
		}

		private class CombineLatest : NthCombineLatestObserverBase<TR>
		{
			private readonly CombineLatestObservable<T1, T2, T3, TR> parent;

			private readonly object gate = new object();

			private CombineLatestObserver<T1> c1;

			private CombineLatestObserver<T2> c2;

			private CombineLatestObserver<T3> c3;

			public CombineLatest(int length, CombineLatestObservable<T1, T2, T3, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(length, observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.c1 = new CombineLatestObserver<T1>(this.gate, this, 0);
				this.c2 = new CombineLatestObserver<T2>(this.gate, this, 1);
				this.c3 = new CombineLatestObserver<T3>(this.gate, this, 2);
				IDisposable disposable = this.parent.source1.Subscribe(this.c1);
				IDisposable disposable2 = this.parent.source2.Subscribe(this.c2);
				IDisposable disposable3 = this.parent.source3.Subscribe(this.c3);
				return StableCompositeDisposable.Create(disposable, disposable2, disposable3);
			}

			public override TR GetResult()
			{
				return this.parent.resultSelector(this.c1.Value, this.c2.Value, this.c3.Value);
			}

			public override void OnNext(TR value)
			{
				this.observer.OnNext(value);
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
