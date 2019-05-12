using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx.Toolkit
{
	public abstract class ObjectPool<T> : IDisposable where T : Component
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

		protected abstract T CreateInstance();

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

		public T Rent()
		{
			if (this.disposedValue)
			{
				throw new ObjectDisposedException("ObjectPool was already disposed.");
			}
			if (this.q == null)
			{
				this.q = new Queue<T>();
			}
			T t = (this.q.Count <= 0) ? this.CreateInstance() : this.q.Dequeue();
			this.OnBeforeRent(t);
			return t;
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
				for (int i = 0; i < createCount; i++)
				{
					try
					{
						T instance = this.CreateInstance();
						this.Return(instance);
					}
					catch (Exception error)
					{
						observer.OnError(error);
						yield break;
					}
				}
				yield return null;
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
