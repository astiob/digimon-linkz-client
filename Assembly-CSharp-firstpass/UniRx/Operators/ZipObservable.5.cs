using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ZipObservable<T1, T2, T3, T4, T5, TR> : OperatorObservableBase<TR>
	{
		private IObservable<T1> source1;

		private IObservable<T2> source2;

		private IObservable<T3> source3;

		private IObservable<T4> source4;

		private IObservable<T5> source5;

		private ZipFunc<T1, T2, T3, T4, T5, TR> resultSelector;

		public ZipObservable(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, ZipFunc<T1, T2, T3, T4, T5, TR> resultSelector) : base(source1.IsRequiredSubscribeOnCurrentThread<T1>() || source2.IsRequiredSubscribeOnCurrentThread<T2>() || source3.IsRequiredSubscribeOnCurrentThread<T3>() || source4.IsRequiredSubscribeOnCurrentThread<T4>() || source5.IsRequiredSubscribeOnCurrentThread<T5>())
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.resultSelector = resultSelector;
		}

		protected override IDisposable SubscribeCore(IObserver<TR> observer, IDisposable cancel)
		{
			return new ZipObservable<T1, T2, T3, T4, T5, TR>.Zip(this, observer, cancel).Run();
		}

		private class Zip : NthZipObserverBase<TR>
		{
			private readonly ZipObservable<T1, T2, T3, T4, T5, TR> parent;

			private readonly object gate = new object();

			private readonly Queue<T1> q1 = new Queue<T1>();

			private readonly Queue<T2> q2 = new Queue<T2>();

			private readonly Queue<T3> q3 = new Queue<T3>();

			private readonly Queue<T4> q4 = new Queue<T4>();

			private readonly Queue<T5> q5 = new Queue<T5>();

			public Zip(ZipObservable<T1, T2, T3, T4, T5, TR> parent, IObserver<TR> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				base.SetQueue(new ICollection[]
				{
					this.q1,
					this.q2,
					this.q3,
					this.q4,
					this.q5
				});
				IDisposable disposable = this.parent.source1.Subscribe(new ZipObserver<T1>(this.gate, this, 0, this.q1));
				IDisposable disposable2 = this.parent.source2.Subscribe(new ZipObserver<T2>(this.gate, this, 1, this.q2));
				IDisposable disposable3 = this.parent.source3.Subscribe(new ZipObserver<T3>(this.gate, this, 2, this.q3));
				IDisposable disposable4 = this.parent.source4.Subscribe(new ZipObserver<T4>(this.gate, this, 3, this.q4));
				IDisposable disposable5 = this.parent.source5.Subscribe(new ZipObserver<T5>(this.gate, this, 4, this.q5));
				return StableCompositeDisposable.Create(new IDisposable[]
				{
					disposable,
					disposable2,
					disposable3,
					disposable4,
					disposable5,
					Disposable.Create(delegate
					{
						object obj = this.gate;
						lock (obj)
						{
							this.q1.Clear();
							this.q2.Clear();
							this.q3.Clear();
							this.q4.Clear();
							this.q5.Clear();
						}
					})
				});
			}

			public override TR GetResult()
			{
				return this.parent.resultSelector(this.q1.Dequeue(), this.q2.Dequeue(), this.q3.Dequeue(), this.q4.Dequeue(), this.q5.Dequeue());
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
