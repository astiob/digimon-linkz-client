using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UniRx
{
	[Serializable]
	public class ReactiveCollection<T> : Collection<T>, IReactiveCollection<T>, IDisposable, IList<T>, IReadOnlyReactiveCollection<T>, IEnumerable, ICollection<T>, IEnumerable<T>
	{
		[NonSerialized]
		private bool isDisposed;

		[NonSerialized]
		private Subject<int> countChanged;

		[NonSerialized]
		private Subject<Unit> collectionReset;

		[NonSerialized]
		private Subject<CollectionAddEvent<T>> collectionAdd;

		[NonSerialized]
		private Subject<CollectionMoveEvent<T>> collectionMove;

		[NonSerialized]
		private Subject<CollectionRemoveEvent<T>> collectionRemove;

		[NonSerialized]
		private Subject<CollectionReplaceEvent<T>> collectionReplace;

		private bool disposedValue;

		public ReactiveCollection()
		{
		}

		public ReactiveCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (T item in collection)
			{
				base.Add(item);
			}
		}

		public ReactiveCollection(List<T> list) : base((list == null) ? null : new List<T>(list))
		{
		}

		protected override void ClearItems()
		{
			int count = base.Count;
			base.ClearItems();
			if (this.collectionReset != null)
			{
				this.collectionReset.OnNext(Unit.Default);
			}
			if (count > 0 && this.countChanged != null)
			{
				this.countChanged.OnNext(base.Count);
			}
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			if (this.collectionAdd != null)
			{
				this.collectionAdd.OnNext(new CollectionAddEvent<T>(index, item));
			}
			if (this.countChanged != null)
			{
				this.countChanged.OnNext(base.Count);
			}
		}

		public void Move(int oldIndex, int newIndex)
		{
			this.MoveItem(oldIndex, newIndex);
		}

		protected virtual void MoveItem(int oldIndex, int newIndex)
		{
			T t = base[oldIndex];
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, t);
			if (this.collectionMove != null)
			{
				this.collectionMove.OnNext(new CollectionMoveEvent<T>(oldIndex, newIndex, t));
			}
		}

		protected override void RemoveItem(int index)
		{
			T value = base[index];
			base.RemoveItem(index);
			if (this.collectionRemove != null)
			{
				this.collectionRemove.OnNext(new CollectionRemoveEvent<T>(index, value));
			}
			if (this.countChanged != null)
			{
				this.countChanged.OnNext(base.Count);
			}
		}

		protected override void SetItem(int index, T item)
		{
			T oldValue = base[index];
			base.SetItem(index, item);
			if (this.collectionReplace != null)
			{
				this.collectionReplace.OnNext(new CollectionReplaceEvent<T>(index, oldValue, item));
			}
		}

		public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false)
		{
			if (this.isDisposed)
			{
				return Observable.Empty<int>();
			}
			Subject<int> subject;
			if ((subject = this.countChanged) == null)
			{
				subject = (this.countChanged = new Subject<int>());
			}
			Subject<int> subject2 = subject;
			if (notifyCurrentCount)
			{
				return subject2.StartWith(new Func<int>(base.get_Count));
			}
			return subject2;
		}

		public IObservable<Unit> ObserveReset()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<Unit>();
			}
			Subject<Unit> result;
			if ((result = this.collectionReset) == null)
			{
				result = (this.collectionReset = new Subject<Unit>());
			}
			return result;
		}

		public IObservable<CollectionAddEvent<T>> ObserveAdd()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<CollectionAddEvent<T>>();
			}
			Subject<CollectionAddEvent<T>> result;
			if ((result = this.collectionAdd) == null)
			{
				result = (this.collectionAdd = new Subject<CollectionAddEvent<T>>());
			}
			return result;
		}

		public IObservable<CollectionMoveEvent<T>> ObserveMove()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<CollectionMoveEvent<T>>();
			}
			Subject<CollectionMoveEvent<T>> result;
			if ((result = this.collectionMove) == null)
			{
				result = (this.collectionMove = new Subject<CollectionMoveEvent<T>>());
			}
			return result;
		}

		public IObservable<CollectionRemoveEvent<T>> ObserveRemove()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<CollectionRemoveEvent<T>>();
			}
			Subject<CollectionRemoveEvent<T>> result;
			if ((result = this.collectionRemove) == null)
			{
				result = (this.collectionRemove = new Subject<CollectionRemoveEvent<T>>());
			}
			return result;
		}

		public IObservable<CollectionReplaceEvent<T>> ObserveReplace()
		{
			if (this.isDisposed)
			{
				return Observable.Empty<CollectionReplaceEvent<T>>();
			}
			Subject<CollectionReplaceEvent<T>> result;
			if ((result = this.collectionReplace) == null)
			{
				result = (this.collectionReplace = new Subject<CollectionReplaceEvent<T>>());
			}
			return result;
		}

		private void DisposeSubject<TSubject>(ref Subject<TSubject> subject)
		{
			if (subject != null)
			{
				try
				{
					subject.OnCompleted();
				}
				finally
				{
					subject.Dispose();
					subject = null;
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.DisposeSubject<Unit>(ref this.collectionReset);
					this.DisposeSubject<CollectionAddEvent<T>>(ref this.collectionAdd);
					this.DisposeSubject<CollectionMoveEvent<T>>(ref this.collectionMove);
					this.DisposeSubject<CollectionRemoveEvent<T>>(ref this.collectionRemove);
					this.DisposeSubject<CollectionReplaceEvent<T>>(ref this.collectionReplace);
				}
				this.disposedValue = true;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		int IReactiveCollection<T>.get_Count()
		{
			return base.Count;
		}

		T IReactiveCollection<T>.get_Item(int index)
		{
			return base[index];
		}

		void IReactiveCollection<T>.set_Item(int index, T value)
		{
			base[index] = value;
		}

		int IReadOnlyReactiveCollection<T>.get_Count()
		{
			return base.Count;
		}

		T IReadOnlyReactiveCollection<T>.get_Item(int index)
		{
			return base[index];
		}
	}
}
