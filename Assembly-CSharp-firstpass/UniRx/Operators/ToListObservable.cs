using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ToListObservable<TSource> : OperatorObservableBase<IList<TSource>>
	{
		private readonly IObservable<TSource> source;

		public ToListObservable(IObservable<TSource> source) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<IList<TSource>> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new ToListObservable<TSource>.ToList(observer, cancel));
		}

		private class ToList : OperatorObserverBase<TSource, IList<TSource>>
		{
			private readonly List<TSource> list = new List<TSource>();

			public ToList(IObserver<IList<TSource>> observer, IDisposable cancel) : base(observer, cancel)
			{
			}

			public override void OnNext(TSource value)
			{
				try
				{
					this.list.Add(value);
				}
				catch (Exception error)
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
				this.observer.OnNext(this.list);
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
