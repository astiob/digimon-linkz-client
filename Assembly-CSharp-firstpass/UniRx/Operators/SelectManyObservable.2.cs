using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class SelectManyObservable<TSource, TCollection, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TSource> source;

		private readonly Func<TSource, IObservable<TCollection>> collectionSelector;

		private readonly Func<TSource, int, IObservable<TCollection>> collectionSelectorWithIndex;

		private readonly Func<TSource, IEnumerable<TCollection>> collectionSelectorEnumerable;

		private readonly Func<TSource, int, IEnumerable<TCollection>> collectionSelectorEnumerableWithIndex;

		private readonly Func<TSource, TCollection, TResult> resultSelector;

		private readonly Func<TSource, int, TCollection, int, TResult> resultSelectorWithIndex;

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.collectionSelector = collectionSelector;
			this.resultSelector = resultSelector;
		}

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.collectionSelectorWithIndex = collectionSelector;
			this.resultSelectorWithIndex = resultSelector;
		}

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.collectionSelectorEnumerable = collectionSelector;
			this.resultSelector = resultSelector;
		}

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.collectionSelectorEnumerableWithIndex = collectionSelector;
			this.resultSelectorWithIndex = resultSelector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			if (this.collectionSelector != null)
			{
				return new SelectManyObservable<TSource, TCollection, TResult>.SelectManyOuterObserver(this, observer, cancel).Run();
			}
			if (this.collectionSelectorWithIndex != null)
			{
				return new SelectManyObservable<TSource, TCollection, TResult>.SelectManyObserverWithIndex(this, observer, cancel).Run();
			}
			if (this.collectionSelectorEnumerable != null)
			{
				return new SelectManyObservable<TSource, TCollection, TResult>.SelectManyEnumerableObserver(this, observer, cancel).Run();
			}
			if (this.collectionSelectorEnumerableWithIndex != null)
			{
				return new SelectManyObservable<TSource, TCollection, TResult>.SelectManyEnumerableObserverWithIndex(this, observer, cancel).Run();
			}
			throw new InvalidOperationException();
		}

		private class SelectManyOuterObserver : OperatorObserverBase<TSource, TResult>
		{
			private readonly SelectManyObservable<TSource, TCollection, TResult> parent;

			private CompositeDisposable collectionDisposable;

			private object gate = new object();

			private bool isStopped;

			private SingleAssignmentDisposable sourceDisposable;

			public SelectManyOuterObserver(SelectManyObservable<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.collectionDisposable = new CompositeDisposable();
				this.sourceDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(this.sourceDisposable);
				this.sourceDisposable.Disposable = this.parent.source.Subscribe(this);
				return this.collectionDisposable;
			}

			public override void OnNext(TSource value)
			{
				IObservable<TCollection> observable;
				try
				{
					observable = this.parent.collectionSelector(value);
				}
				catch (Exception error)
				{
					this.OnError(error);
					return;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(singleAssignmentDisposable);
				SelectManyObservable<TSource, TCollection, TResult>.SelectManyOuterObserver.SelectMany observer = new SelectManyObservable<TSource, TCollection, TResult>.SelectManyOuterObserver.SelectMany(this, value, singleAssignmentDisposable);
				singleAssignmentDisposable.Disposable = observable.Subscribe(observer);
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
				this.isStopped = true;
				if (this.collectionDisposable.Count == 1)
				{
					object obj = this.gate;
					lock (obj)
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
				else
				{
					this.sourceDisposable.Dispose();
				}
			}

			private class SelectMany : OperatorObserverBase<TCollection, TResult>
			{
				private readonly SelectManyObservable<TSource, TCollection, TResult>.SelectManyOuterObserver parent;

				private readonly TSource sourceValue;

				private readonly IDisposable cancel;

				public SelectMany(SelectManyObservable<TSource, TCollection, TResult>.SelectManyOuterObserver parent, TSource value, IDisposable cancel) : base(parent.observer, cancel)
				{
					this.parent = parent;
					this.sourceValue = value;
					this.cancel = cancel;
				}

				public override void OnNext(TCollection value)
				{
					TResult value2;
					try
					{
						value2 = this.parent.parent.resultSelector(this.sourceValue, value);
					}
					catch (Exception error)
					{
						this.OnError(error);
						return;
					}
					object gate = this.parent.gate;
					lock (gate)
					{
						this.observer.OnNext(value2);
					}
				}

				public override void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
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
					this.parent.collectionDisposable.Remove(this.cancel);
					if (this.parent.isStopped && this.parent.collectionDisposable.Count == 1)
					{
						object gate = this.parent.gate;
						lock (gate)
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
		}

		private class SelectManyObserverWithIndex : OperatorObserverBase<TSource, TResult>
		{
			private readonly SelectManyObservable<TSource, TCollection, TResult> parent;

			private CompositeDisposable collectionDisposable;

			private object gate = new object();

			private bool isStopped;

			private SingleAssignmentDisposable sourceDisposable;

			private int index;

			public SelectManyObserverWithIndex(SelectManyObservable<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				this.collectionDisposable = new CompositeDisposable();
				this.sourceDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(this.sourceDisposable);
				this.sourceDisposable.Disposable = this.parent.source.Subscribe(this);
				return this.collectionDisposable;
			}

			public override void OnNext(TSource value)
			{
				int arg = this.index++;
				IObservable<TCollection> observable;
				try
				{
					observable = this.parent.collectionSelectorWithIndex(value, arg);
				}
				catch (Exception error)
				{
					this.OnError(error);
					return;
				}
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(singleAssignmentDisposable);
				SelectManyObservable<TSource, TCollection, TResult>.SelectManyObserverWithIndex.SelectManyObserver observer = new SelectManyObservable<TSource, TCollection, TResult>.SelectManyObserverWithIndex.SelectManyObserver(this, value, arg, singleAssignmentDisposable);
				singleAssignmentDisposable.Disposable = observable.Subscribe(observer);
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
				this.isStopped = true;
				if (this.collectionDisposable.Count == 1)
				{
					object obj = this.gate;
					lock (obj)
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
				else
				{
					this.sourceDisposable.Dispose();
				}
			}

			private class SelectManyObserver : OperatorObserverBase<TCollection, TResult>
			{
				private readonly SelectManyObservable<TSource, TCollection, TResult>.SelectManyObserverWithIndex parent;

				private readonly TSource sourceValue;

				private readonly int sourceIndex;

				private readonly IDisposable cancel;

				private int index;

				public SelectManyObserver(SelectManyObservable<TSource, TCollection, TResult>.SelectManyObserverWithIndex parent, TSource value, int index, IDisposable cancel) : base(parent.observer, cancel)
				{
					this.parent = parent;
					this.sourceValue = value;
					this.sourceIndex = index;
					this.cancel = cancel;
				}

				public override void OnNext(TCollection value)
				{
					TResult value2;
					try
					{
						value2 = this.parent.parent.resultSelectorWithIndex(this.sourceValue, this.sourceIndex, value, this.index++);
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
					object gate = this.parent.gate;
					lock (gate)
					{
						this.observer.OnNext(value2);
					}
				}

				public override void OnError(Exception error)
				{
					object gate = this.parent.gate;
					lock (gate)
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
					this.parent.collectionDisposable.Remove(this.cancel);
					if (this.parent.isStopped && this.parent.collectionDisposable.Count == 1)
					{
						object gate = this.parent.gate;
						lock (gate)
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
		}

		private class SelectManyEnumerableObserver : OperatorObserverBase<TSource, TResult>
		{
			private readonly SelectManyObservable<TSource, TCollection, TResult> parent;

			public SelectManyEnumerableObserver(SelectManyObservable<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(TSource value)
			{
				IEnumerable<TCollection> enumerable;
				try
				{
					enumerable = this.parent.collectionSelectorEnumerable(value);
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
				using (IEnumerator<TCollection> enumerator = enumerable.GetEnumerator())
				{
					bool flag = true;
					while (flag)
					{
						flag = false;
						TResult value2 = default(TResult);
						try
						{
							flag = enumerator.MoveNext();
							if (flag)
							{
								value2 = this.parent.resultSelector(value, enumerator.Current);
							}
						}
						catch (Exception error2)
						{
							try
							{
								this.observer.OnError(error2);
							}
							finally
							{
								base.Dispose();
							}
							break;
						}
						if (flag)
						{
							this.observer.OnNext(value2);
						}
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

		private class SelectManyEnumerableObserverWithIndex : OperatorObserverBase<TSource, TResult>
		{
			private readonly SelectManyObservable<TSource, TCollection, TResult> parent;

			private int index;

			public SelectManyEnumerableObserverWithIndex(SelectManyObservable<TSource, TCollection, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(TSource value)
			{
				int arg = this.index++;
				IEnumerable<TCollection> enumerable;
				try
				{
					enumerable = this.parent.collectionSelectorEnumerableWithIndex(value, arg);
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
				using (IEnumerator<TCollection> enumerator = enumerable.GetEnumerator())
				{
					int num = 0;
					bool flag = true;
					while (flag)
					{
						flag = false;
						TResult value2 = default(TResult);
						try
						{
							flag = enumerator.MoveNext();
							if (flag)
							{
								value2 = this.parent.resultSelectorWithIndex(value, arg, enumerator.Current, num++);
							}
						}
						catch (Exception error2)
						{
							try
							{
								this.observer.OnError(error2);
							}
							finally
							{
								base.Dispose();
							}
							break;
						}
						if (flag)
						{
							this.observer.OnNext(value2);
						}
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
