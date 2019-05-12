using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class ToArrayObservable<TSource> : OperatorObservableBase<TSource[]>
	{
		private readonly IObservable<TSource> source;

		public ToArrayObservable(IObservable<TSource> source) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
		}

		protected override IDisposable SubscribeCore(IObserver<TSource[]> observer, IDisposable cancel)
		{
			return this.source.Subscribe(new ToArrayObservable<TSource>.ToArray(observer, cancel));
		}

		private class ToArray : OperatorObserverBase<TSource, TSource[]>
		{
			private readonly List<TSource> list = new List<TSource>();

			public ToArray(IObserver<TSource[]> observer, IDisposable cancel) : base(observer, cancel)
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
				TSource[] value;
				try
				{
					value = this.list.ToArray();
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
					return;
				}
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
	}
}
