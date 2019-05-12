using System;

namespace UniRx.Operators
{
	internal class CombineLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR> : OperatorObservableBase<TR>
	{
		private IObservable<T1> source1;

		private IObservable<T2> source2;

		private IObservable<T3> source3;

		private IObservable<T4> source4;

		private IObservable<T5> source5;

		private IObservable<T6> source6;

		private IObservable<T7> source7;

		private CombineLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector;

		public CombineLatestObservable(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, CombineLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) : base(source1.IsRequiredSubscribeOnCurrentThread<T1>() || source2.IsRequiredSubscribeOnCurrentThread<T2>() || source3.IsRequiredSubscribeOnCurrentThread<T3>() || source4.IsRequiredSubscribeOnCurrentThread<T4>() || source5.IsRequiredSubscribeOnCurrentThread<T5>() || source6.IsRequiredSubscribeOnCurrentThread<T6>() || source7.IsRequiredSubscribeOnCurrentThread<T7>())
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.source6 = source6;
			this.source7 = source7;
			this.resultSelector = resultSelector;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return new CombineLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR>.CombineLatest(7, this, observer, cancel).Run();
		}

		private class CombineLatest : NthCombineLatestObserverBase<TR>
		{
			private readonly CombineLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR> parent;

			private readonly object gate = new object();

			private CombineLatestObserver<T1> c1;

			private CombineLatestObserver<T2> c2;

			private CombineLatestObserver<T3> c3;

			private CombineLatestObserver<T4> c4;

			private CombineLatestObserver<T5> c5;

			private CombineLatestObserver<T6> c6;

			private CombineLatestObserver<T7> c7;

			public CombineLatest(int length, CombineLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(length, observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.c1 = new CombineLatestObserver<T1>(this.gate, this, 0);
				this.c2 = new CombineLatestObserver<T2>(this.gate, this, 1);
				this.c3 = new CombineLatestObserver<T3>(this.gate, this, 2);
				this.c4 = new CombineLatestObserver<T4>(this.gate, this, 3);
				this.c5 = new CombineLatestObserver<T5>(this.gate, this, 4);
				this.c6 = new CombineLatestObserver<T6>(this.gate, this, 5);
				this.c7 = new CombineLatestObserver<T7>(this.gate, this, 6);
				IDisposable disposable = this.parent.source1.Subscribe(this.c1);
				IDisposable disposable2 = this.parent.source2.Subscribe(this.c2);
				IDisposable disposable3 = this.parent.source3.Subscribe(this.c3);
				IDisposable disposable4 = this.parent.source4.Subscribe(this.c4);
				IDisposable disposable5 = this.parent.source5.Subscribe(this.c5);
				IDisposable disposable6 = this.parent.source6.Subscribe(this.c6);
				IDisposable disposable7 = this.parent.source7.Subscribe(this.c7);
				return StableCompositeDisposable.Create(new IDisposable[]
				{
					disposable,
					disposable2,
					disposable3,
					disposable4,
					disposable5,
					disposable6,
					disposable7
				});
			}

			public override TR GetResult()
			{
				return this.parent.resultSelector(this.c1.Value, this.c2.Value, this.c3.Value, this.c4.Value, this.c5.Value, this.c6.Value, this.c7.Value);
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
