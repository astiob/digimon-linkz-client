using System;

namespace UniRx
{
	public interface ISubject<TSource, TResult> : IObserver<TSource>, IObservable<TResult>
	{
	}
}
