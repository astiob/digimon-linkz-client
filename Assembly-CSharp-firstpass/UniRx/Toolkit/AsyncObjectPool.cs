using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx.Toolkit
{
	public abstract class AsyncObjectPool<T> : IDisposable where T : Component
	{
		private bool disposedValue;

		private Queue<T> q;

		protected int MaxPoolCount
		{
			get
			{
				return int.MaxValue;
			}
		}

		protected abstract IObservable<T> CreateInstanceAsync();

		protected virtual void OnBeforeRent(T instance)
		{
			instance.gameObject.SetActive(true);
		}

		protected virtual void OnBeforeReturn(T instance)
		{
			instance.gameObject.SetActive(false);
		}

		protected virtual void OnClear(T instance)
		{
			if (instance == null)
			{
				return;
			}
			GameObject gameObject = instance.gameObject;
			if (gameObject == null)
			{
				return;
			}
			UnityEngine.Object.Destroy(gameObject);
		}

		public int Count
		{
			get
			{
				if (this.q == null)
				{
					return 0;
				}
				return this.q.Count;
			}
		}

		public IObservable<T> RentAsync()
		{
			if (this.disposedValue)
			{
				throw new ObjectDisposedException("ObjectPool was already disposed.");
			}
			if (this.q == null)
			{
				this.q = new Queue<T>();
			}
			if (this.q.Count > 0)
			{
				T t = this.q.Dequeue();
				this.OnBeforeRent(t);
				return Observable.Return<T>(t);
			}
			IObservable<T> source = this.CreateInstanceAsync();
			return source.Do(delegate(T x)
			{
				this.OnBeforeRent(x);
			});
		}

		public void Return(T instance)
		{
			if (this.disposedValue)
			{
				throw new ObjectDisposedException("ObjectPool was already disposed.");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (this.q == null)
			{
				this.q = new Queue<T>();
			}
			if (this.q.Count + 1 == this.MaxPoolCount)
			{
				throw new InvalidOperationException("Reached Max PoolSize");
			}
			this.OnBeforeReturn(instance);
			this.q.Enqueue(instance);
		}

		public void Shrink(float instanceCountRatio, int minSize, bool callOnBeforeRent = false)
		{
			if (this.q == null)
			{
				return;
			}
			if (instanceCountRatio <= 0f)
			{
				instanceCountRatio = 0f;
			}
			if (instanceCountRatio >= 1f)
			{
				instanceCountRatio = 1f;
			}
			int num = (int)((float)this.q.Count * instanceCountRatio);
			num = Math.Max(minSize, num);
			while (this.q.Count > num)
			{
				T instance = this.q.Dequeue();
				if (callOnBeforeRent)
				{
					this.OnBeforeRent(instance);
				}
				this.OnClear(instance);
			}
		}

		public IDisposable StartShrinkTimer(TimeSpan checkInterval, float instanceCountRatio, int minSize, bool callOnBeforeRent = false)
		{
			return Observable.Interval(checkInterval).TakeWhile((long _) => this.disposedValue).Subscribe(delegate(long _)
			{
				this.Shrink(instanceCountRatio, minSize, callOnBeforeRent);
			});
		}

		public void Clear(bool callOnBeforeRent = false)
		{
			if (this.q == null)
			{
				return;
			}
			while (this.q.Count != 0)
			{
				T instance = this.q.Dequeue();
				if (callOnBeforeRent)
				{
					this.OnBeforeRent(instance);
				}
				this.OnClear(instance);
			}
		}

		public IObservable<Unit> PreloadAsync(int preloadCount, int threshold)
		{
			if (this.q == null)
			{
				this.q = new Queue<T>(preloadCount);
			}
			return Observable.FromMicroCoroutine<Unit>((IObserver<Unit> observer, CancellationToken cancel) => this.PreloadCore(preloadCount, threshold, observer, cancel), FrameCountType.Update);
		}

		private IEnumerator PreloadCore(int preloadCount, int threshold, IObserver<Unit> observer, CancellationToken cancellationToken)
		{
			while (this.Count < preloadCount && !cancellationToken.IsCancellationRequested)
			{
				int requireCount = preloadCount - this.Count;
				if (requireCount <= 0)
				{
					break;
				}
				int createCount = Math.Min(requireCount, threshold);
				IObservable<Unit>[] loaders = new IObservable<Unit>[createCount];
				for (int i = 0; i < createCount; i++)
				{
					IObservable<T> source = this.CreateInstanceAsync();
					loaders[i] = source.ForEachAsync(delegate(T x)
					{
						this.Return(x);
					});
				}
				ObservableYieldInstruction<Unit> awaiter = Observable.WhenAll(loaders).ToYieldInstruction(false, cancellationToken);
				while (!awaiter.HasResult && !awaiter.IsCanceled && !awaiter.HasError)
				{
					yield return null;
				}
				if (awaiter.HasError)
				{
					observer.OnError(awaiter.Error);
					yield break;
				}
				if (awaiter.IsCanceled)
				{
					yield break;
				}
			}
			observer.OnNext(Unit.Default);
			observer.OnCompleted();
			yield break;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.Clear(false);
				}
				this.disposedValue = true;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
		}
	}
}
