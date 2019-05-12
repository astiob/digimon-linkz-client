using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class BufferObservable<TSource, TWindowBoundary> : OperatorObservableBase<IList<TSource>>
	{
		private readonly IObservable<TSource> source;

		private readonly IObservable<TWindowBoundary> windowBoundaries;

		public BufferObservable(IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.windowBoundaries = windowBoundaries;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<TSource>> observer, IDisposable cancel)
		{
			return new BufferObservable<TSource, TWindowBoundary>.Buffer(this, observer, cancel).Run();
		}

		private class Buffer : OperatorObserverBase<TSource, IList<TSource>>
		{
			private static readonly TSource[] EmptyArray = new TSource[0];

			private readonly BufferObservable<TSource, TWindowBoundary> parent;

			private object gate = new object();

			private List<TSource> list;

			public Buffer(BufferObservable<TSource, TWindowBoundary> parent, IObserver<IList<TSource>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.list = new List<TSource>();
				IDisposable disposable = this.parent.source.Subscribe(this);
				IDisposable disposable2 = this.parent.windowBoundaries.Subscribe(new BufferObservable<TSource, TWindowBoundary>.Buffer.Buffer_(this));
				return StableCompositeDisposable.Create(disposable, disposable2);
			}

			public override void OnNext(TSource value)
			{
				object obj = this.gate;
				lock (obj)
				{
					this.list.Add(value);
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
					List<TSource> value = this.list;
					this.list = new List<TSource>();
					this.observer.OnNext(value);
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

			private class Buffer_ : IObserver<TWindowBoundary>
			{
				private readonly BufferObservable<TSource, TWindowBoundary>.Buffer parent;

				public Buffer_(BufferObservable<TSource, TWindowBoundary>.Buffer parent)
				{
					this.parent = parent;
				}

				public void OnNext(TWindowBoundary value)
				{
					bool flag = false;
					object gate = this.parent.gate;
					List<TSource> list;
					lock (gate)
					{
						list = this.parent.list;
						if (list.Count != 0)
						{
							this.parent.list = new List<TSource>();
						}
						else
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.parent.observer.OnNext(BufferObservable<TSource, TWindowBoundary>.Buffer.EmptyArray);
					}
					else
					{
						this.parent.observer.OnNext(list);
					}
				}

				public void OnError(Exception error)
				{
					this.parent.OnError(error);
				}

				public void OnCompleted()
				{
					this.parent.OnCompleted();
				}
			}
		}
	}
}
