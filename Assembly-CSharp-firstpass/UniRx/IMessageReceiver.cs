using System;

namespace UniRx
{
	public interface IMessageReceiver
	{
		IObservable<T> Receive<T>();
	}
}
