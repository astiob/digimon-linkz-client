using System;

namespace UniRx
{
	public interface ISubject<T> : ISubject<T, T>, IObserver<T>, IObservable<T>
	{
	}
}
