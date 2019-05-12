using System;
using System.Collections.Generic;

namespace UniRx
{
	public class MessageBroker : IMessageBroker, IDisposable, IMessagePublisher, IMessageReceiver
	{
		public static readonly IMessageBroker Default = new MessageBroker();

		private bool isDisposed;

		private readonly Dictionary<Type, object> notifiers = new Dictionary<Type, object>();

		public void Publish<T>(T message)
		{
			object obj = this.notifiers;
			object obj2;
			lock (obj)
			{
				if (this.isDisposed)
				{
					return;
				}
				if (!this.notifiers.TryGetValue(typeof(T), out obj2))
				{
					return;
				}
			}
			((ISubject<T>)obj2).OnNext(message);
		}

		public IObservable<T> Receive<T>()
		{
			object obj = this.notifiers;
			object obj2;
			lock (obj)
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("MessageBroker");
				}
				if (!this.notifiers.TryGetValue(typeof(T), out obj2))
				{
					ISubject<T> subject = new Subject<T>().Synchronize<T>();
					obj2 = subject;
					this.notifiers.Add(typeof(T), obj2);
				}
			}
			return ((IObservable<T>)obj2).AsObservable<T>();
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
	}
}
