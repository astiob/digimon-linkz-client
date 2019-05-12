using System;

namespace UniRx
{
	public interface IAsyncReactiveCommand<T>
	{
		IReadOnlyReactiveProperty<bool> CanExecute { get; }

		IDisposable Execute(T parameter);

		IDisposable Subscribe(Func<T, IObservable<Unit>> asyncAction);
	}
}
