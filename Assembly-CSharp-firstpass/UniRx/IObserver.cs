using System;

namespace UniRx
{
	public interface IObserver<TValue, TResult>
	{
		TResult OnNext(TValue value);

		TResult OnError(Exception exception);

		TResult OnCompleted();
	}
}
