using System;

namespace UniRx
{
	public interface IAsyncMessageReceiver
	{
		IDisposable Subscribe<T>(Func<T, IObservable<Unit>> asyncMessageReceiver);
	}
}
