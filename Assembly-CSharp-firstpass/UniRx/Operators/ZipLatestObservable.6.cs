﻿using System;

namespace UniRx.Operators
{
	internal class ZipLatestObservable<T1, T2, T3, T4, T5, T6, TR> : OperatorObservableBase<TR>
	{
		private IObservable<T1> source1;

		private IObservable<T2> source2;

		private IObservable<T3> source3;

		private IObservable<T4> source4;

		private IObservable<T5> source5;

		private IObservable<T6> source6;

		private ZipLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector;

		public ZipLatestObservable(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, ZipLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) : base(source1.IsRequiredSubscribeOnCurrentThread<T1>() || source2.IsRequiredSubscribeOnCurrentThread<T2>() || source3.IsRequiredSubscribeOnCurrentThread<T3>() || source4.IsRequiredSubscribeOnCurrentThread<T4>() || source5.IsRequiredSubscribeOnCurrentThread<T5>() || source6.IsRequiredSubscribeOnCurrentThread<T6>())
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.source6 = source6;
			this.resultSelector = resultSelector;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return new ZipLatestObservable<T1, T2, T3, T4, T5, T6, TR>.ZipLatest(6, this, observer, cancel).Run();
		}

		private class ZipLatest : NthZipLatestObserverBase<TR>
		{
			private readonly ZipLatestObservable<T1, T2, T3, T4, T5, T6, TR> parent;

			private readonly object gate = new object();

			private ZipLatestObserver<T1> c1;

			private ZipLatestObserver<T2> c2;

			private ZipLatestObserver<T3> c3;

			private ZipLatestObserver<T4> c4;

			private ZipLatestObserver<T5> c5;

			private ZipLatestObserver<T6> c6;

			public ZipLatest(int length, ZipLatestObservable<T1, T2, T3, T4, T5, T6, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(length, observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.c1 = new ZipLatestObserver<T1>(this.gate, this, 0);
				this.c2 = new ZipLatestObserver<T2>(this.gate, this, 1);
				this.c3 = new ZipLatestObserver<T3>(this.gate, this, 2);
				this.c4 = new ZipLatestObserver<T4>(this.gate, this, 3);
				this.c5 = new ZipLatestObserver<T5>(this.gate, this, 4);
				this.c6 = new ZipLatestObserver<T6>(this.gate, this, 5);
				IDisposable disposable = this.parent.source1.Subscribe(this.c1);
				IDisposable disposable2 = this.parent.source2.Subscribe(this.c2);
				IDisposable disposable3 = this.parent.source3.Subscribe(this.c3);
				IDisposable disposable4 = this.parent.source4.Subscribe(this.c4);
				IDisposable disposable5 = this.parent.source5.Subscribe(this.c5);
				IDisposable disposable6 = this.parent.source6.Subscribe(this.c6);
				return StableCompositeDisposable.Create(new IDisposable[]
				{
					disposable,
					disposable2,
					disposable3,
					disposable4,
					disposable5,
					disposable6
				});
			}

			public override TR GetResult()
			{
				return this.parent.resultSelector(this.c1.Value, this.c2.Value, this.c3.Value, this.c4.Value, this.c5.Value, this.c6.Value);
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
