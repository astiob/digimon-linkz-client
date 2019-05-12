using System;
using System.Collections.Generic;
using UniRx.InternalUtil;

namespace UniRx
{
	public class AsyncMessageBroker : IAsyncMessageBroker, IDisposable, IAsyncMessagePublisher, IAsyncMessageReceiver
	{
		public static readonly IAsyncMessageBroker Default = new AsyncMessageBroker();

		private bool isDisposed;

		private readonly Dictionary<Type, object> notifiers = new Dictionary<Type, object>();

		public IObservable<Unit> PublishAsync<T>(T message)
		{
			object obj = this.notifiers;
			ImmutableList<Func<T, IObservable<Unit>>> immutableList;
			lock (obj)
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("AsyncMessageBroker");
				}
				object obj2;
				if (!this.notifiers.TryGetValue(typeof(T), out obj2))
				{
					return Observable.ReturnUnit();
				}
				immutableList = (ImmutableList<Func<T, IObservable<Unit>>>)obj2;
			}
			Func<T, IObservable<Unit>>[] data = immutableList.Data;
			IObservable<Unit>[] array = new IObservable<Unit>[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = data[i](message);
			}
			return Observable.WhenAll(array);
		}

		public IDisposable Subscribe<T>(Func<T, IObservable<Unit>> asyncMessageReceiver)
		{
			object obj = this.notifiers;
			lock (obj)
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("AsyncMessageBroker");
				}
				object obj2;
				if (!this.notifiers.TryGetValue(typeof(T), out obj2))
				{
					ImmutableList<Func<T, IObservable<Unit>>> immutableList = ImmutableList<Func<T, IObservable<Unit>>>.Empty;
					immutableList = immutableList.Add(asyncMessageReceiver);
					this.notifiers.Add(typeof(T), immutableList);
				}
				else
				{
					ImmutableList<Func<T, IObservable<Unit>>> immutableList2 = (ImmutableList<Func<T, IObservable<Unit>>>)obj2;
					immutableList2 = immutableList2.Add(asyncMessageReceiver);
					this.notifiers[typeof(T)] = immutableList2;
				}
			}
			return new AsyncMessageBroker.Subscription<T>(this, asyncMessageReceiver);
		}

		public void Dispose()
		{
			object obj = this.notifiers;
			lock (obj)
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.notifiers.Clear();
				}
			}
		}

		private class Subscription<T> : IDisposable
		{
			private readonly AsyncMessageBroker parent;

			private readonly Func<T, IObservable<Unit>> asyncMessageReceiver;

			public Subscription(AsyncMessageBroker parent, Func<T, IObservable<Unit>> asyncMessageReceiver)
			{
				this.parent = parent;
				this.asyncMessageReceiver = asyncMessageReceiver;
			}

			public void Dispose()
			{
				object notifiers = this.parent.notifiers;
				lock (notifiers)
				{
					object obj;
					if (this.parent.notifiers.TryGetValue(typeof(T), out obj))
					{
						ImmutableList<Func<T, IObservable<Unit>>> immutableList = (ImmutableList<Func<T, IObservable<Unit>>>)obj;
						immutableList = immutableList.Remove(this.asyncMessageReceiver);
						this.parent.notifiers[typeof(T)] = immutableList;
					}
				}
			}
		}
	}
}
