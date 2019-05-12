using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class GroupByObservable<TSource, TKey, TElement> : OperatorObservableBase<IGroupedObservable<TKey, TElement>>
	{
		private readonly IObservable<TSource> source;

		private readonly Func<TSource, TKey> keySelector;

		private readonly Func<TSource, TElement> elementSelector;

		private readonly int? capacity;

		private readonly IEqualityComparer<TKey> comparer;

		public GroupByObservable(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int? capacity, IEqualityComparer<TKey> comparer) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.capacity = capacity;
			this.comparer = comparer;
		}

		protected override IDisposable SubscribeCore(IObserver<IGroupedObservable<TKey, TElement>> observer, IDisposable cancel)
		{
			return new GroupByObservable<TSource, TKey, TElement>.GroupBy(this, observer, cancel).Run();
		}

		private class GroupBy : OperatorObserverBase<TSource, IGroupedObservable<TKey, TElement>>
		{
			private readonly GroupByObservable<TSource, TKey, TElement> parent;

			private readonly Dictionary<TKey, ISubject<TElement>> map;

			private ISubject<TElement> nullKeySubject;

			private CompositeDisposable groupDisposable;

			private RefCountDisposable refCountDisposable;

			public GroupBy(GroupByObservable<TSource, TKey, TElement> parent, IObserver<IGroupedObservable<TKey, TElement>> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
				if (parent.capacity != null)
				{
					this.map = new Dictionary<TKey, ISubject<TElement>>(parent.capacity.Value, parent.comparer);
				}
				else
				{
					this.map = new Dictionary<TKey, ISubject<TElement>>(parent.comparer);
				}
			}

			public IDisposable Run()
			{
				this.groupDisposable = new CompositeDisposable();
				this.refCountDisposable = new RefCountDisposable(this.groupDisposable);
				this.groupDisposable.Add(this.parent.source.Subscribe(this));
				return this.refCountDisposable;
			}

			public override void OnNext(TSource value)
			{
				TKey tkey = default(TKey);
				try
				{
					tkey = this.parent.keySelector(value);
				}
				catch (Exception exception)
				{
					this.Error(exception);
					return;
				}
				bool flag = false;
				ISubject<TElement> subject = null;
				try
				{
					if (tkey == null)
					{
						if (this.nullKeySubject == null)
						{
							this.nullKeySubject = new Subject<TElement>();
							flag = true;
						}
						subject = this.nullKeySubject;
					}
					else if (!this.map.TryGetValue(tkey, out subject))
					{
						subject = new Subject<TElement>();
						this.map.Add(tkey, subject);
						flag = true;
					}
				}
				catch (Exception exception2)
				{
					this.Error(exception2);
					return;
				}
				if (flag)
				{
					GroupedObservable<TKey, TElement> value2 = new GroupedObservable<TKey, TElement>(tkey, subject, this.refCountDisposable);
					this.observer.OnNext(value2);
				}
				TElement value3 = default(TElement);
				try
				{
					value3 = this.parent.elementSelector(value);
				}
				catch (Exception exception3)
				{
					this.Error(exception3);
					return;
				}
				subject.OnNext(value3);
			}

			public override void OnError(Exception error)
			{
				this.Error(error);
			}

			public override void OnCompleted()
			{
				try
				{
					if (this.nullKeySubject != null)
					{
						this.nullKeySubject.OnCompleted();
					}
					foreach (ISubject<TElement> subject in this.map.Values)
					{
						subject.OnCompleted();
					}
					this.observer.OnCompleted();
				}
				finally
				{
					base.Dispose();
				}
			}

			private void Error(Exception exception)
			{
				try
				{
					if (this.nullKeySubject != null)
					{
						this.nullKeySubject.OnError(exception);
					}
					foreach (ISubject<TElement> subject in this.map.Values)
					{
						subject.OnError(exception);
					}
					this.observer.OnError(exception);
				}
				finally
				{
					base.Dispose();
				}
			}
		}
	}
}
