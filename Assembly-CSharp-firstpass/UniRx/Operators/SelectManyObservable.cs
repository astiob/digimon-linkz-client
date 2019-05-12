using System;
using System.Collections.Generic;

namespace UniRx.Operators
{
	internal class SelectManyObservable<TSource, TResult> : OperatorObservableBase<TResult>
	{
		private readonly IObservable<TSource> source;

		private readonly Func<TSource, IObservable<TResult>> selector;

		private readonly Func<TSource, int, IObservable<TResult>> selectorWithIndex;

		private readonly Func<TSource, IEnumerable<TResult>> selectorEnumerable;

		private readonly Func<TSource, int, IEnumerable<TResult>> selectorEnumerableWithIndex;

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.selector = selector;
		}

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.selectorWithIndex = selector;
		}

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.selectorEnumerable = selector;
		}

		public SelectManyObservable(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector) : base(source.IsRequiredSubscribeOnCurrentThread<TSource>())
		{
			this.source = source;
			this.selectorEnumerableWithIndex = selector;
		}

		protected override IDisposable SubscribeCore(IObserver<TResult> observer, IDisposable cancel)
		{
			if (this.selector != null)
			{
				return new SelectManyObservable<TSource, TResult>.SelectManyOuterObserver(this, observer, cancel).Run();
			}
			if (this.selectorWithIndex != null)
			{
				return new SelectManyObservable<TSource, TResult>.SelectManyObserverWithIndex(this, observer, cancel).Run();
			}
			if (this.selectorEnumerable != null)
			{
				return new SelectManyObservable<TSource, TResult>.SelectManyEnumerableObserver(this, observer, cancel).Run();
			}
			if (this.selectorEnumerableWithIndex != null)
			{
				return new SelectManyObservable<TSource, TResult>.SelectManyEnumerableObserverWithIndex(this, observer, cancel).Run();
			}
			throw new InvalidOperationException();
		}

		private class SelectManyOuterObserver : OperatorObserverBase<TSource, TResult>
		{
			private readonly SelectManyObservable<TSource, TResult> parent;

			private CompositeDisposable collectionDisposable;

			private SingleAssignmentDisposable sourceDisposable;

			private object gate = new object();

			private bool isStopped;

			public SelectManyOuterObserver(SelectManyObservable<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
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
				IObservable<TResult> observable;
				try
				{
					observable = this.parent.selector(value);
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
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(singleAssignmentDisposable);
				SelectManyObservable<TSource, TResult>.SelectManyOuterObserver.SelectMany observer = new SelectManyObservable<TSource, TResult>.SelectManyOuterObserver.SelectMany(this, singleAssignmentDisposable);
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

			private class SelectMany : OperatorObserverBase<TResult, TResult>
			{
				private readonly SelectManyObservable<TSource, TResult>.SelectManyOuterObserver parent;

				private readonly IDisposable cancel;

				public SelectMany(SelectManyObservable<TSource, TResult>.SelectManyOuterObserver parent, IDisposable cancel) : base(parent.observer, cancel)
				{
					this.parent = parent;
					this.cancel = cancel;
				}

				public override void OnNext(TResult value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.observer.OnNext(value);
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
			private readonly SelectManyObservable<TSource, TResult> parent;

			private CompositeDisposable collectionDisposable;

			private int index;

			private object gate = new object();

			private bool isStopped;

			private SingleAssignmentDisposable sourceDisposable;

			public SelectManyObserverWithIndex(SelectManyObservable<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
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
				IObservable<TResult> observable;
				try
				{
					observable = this.parent.selectorWithIndex(value, this.index++);
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
				SingleAssignmentDisposable singleAssignmentDisposable = new SingleAssignmentDisposable();
				this.collectionDisposable.Add(singleAssignmentDisposable);
				SelectManyObservable<TSource, TResult>.SelectManyObserverWithIndex.SelectMany observer = new SelectManyObservable<TSource, TResult>.SelectManyObserverWithIndex.SelectMany(this, singleAssignmentDisposable);
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

			private class SelectMany : OperatorObserverBase<TResult, TResult>
			{
				private readonly SelectManyObservable<TSource, TResult>.SelectManyObserverWithIndex parent;

				private readonly IDisposable cancel;

				public SelectMany(SelectManyObservable<TSource, TResult>.SelectManyObserverWithIndex parent, IDisposable cancel) : base(parent.observer, cancel)
				{
					this.parent = parent;
					this.cancel = cancel;
				}

				public override void OnNext(TResult value)
				{
					object gate = this.parent.gate;
					lock (gate)
					{
						this.observer.OnNext(value);
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
			private readonly SelectManyObservable<TSource, TResult> parent;

			public SelectManyEnumerableObserver(SelectManyObservable<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(TSource value)
			{
				IEnumerable<TResult> enumerable;
				try
				{
					enumerable = this.parent.selectorEnumerable(value);
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
				using (IEnumerator<TResult> enumerator = enumerable.GetEnumerator())
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
								value2 = enumerator.Current;
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
			private readonly SelectManyObservable<TSource, TResult> parent;

			private int index;

			public SelectManyEnumerableObserverWithIndex(SelectManyObservable<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel) : base(observer, cancel)
			{
				this.parent = parent;
			}

			public IDisposable Run()
			{
				return this.parent.source.Subscribe(this);
			}

			public override void OnNext(TSource value)
			{
				IEnumerable<TResult> enumerable;
				try
				{
					enumerable = this.parent.selectorEnumerableWithIndex(value, this.index++);
				}
				catch (Exception error)
				{
					this.OnError(error);
					return;
				}
				using (IEnumerator<TResult> enumerator = enumerable.GetEnumerator())
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
								value2 = enumerator.Current;
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
