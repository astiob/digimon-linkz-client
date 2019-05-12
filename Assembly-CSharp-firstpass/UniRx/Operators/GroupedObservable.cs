using System;

namespace UniRx.Operators
{
	internal class GroupedObservable<TKey, TElement> : IGroupedObservable<TKey, TElement>, IObservable<TElement>
	{
		private readonly TKey key;

		private readonly IObservable<TElement> subject;

		private readonly RefCountDisposable refCount;

		public GroupedObservable(TKey key, ISubject<TElement> subject, RefCountDisposable refCount)
		{
			this.key = key;
			this.subject = subject;
			this.refCount = refCount;
		}

		public TKey Key
		{
			get
			{
				return this.key;
			}
		}

		public IDisposable Subscribe(IObserver<TElement> observer)
		{
			IDisposable disposable = this.refCount.GetDisposable();
			IDisposable disposable2 = this.subject.Subscribe(observer);
			return StableCompositeDisposable.Create(disposable, disposable2);
		}
	}
}
