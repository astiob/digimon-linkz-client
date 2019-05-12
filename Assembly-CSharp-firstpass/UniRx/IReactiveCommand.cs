using System;

namespace UniRx
{
	public interface IReactiveCommand<T> : IObservable<T>
	{
		IReadOnlyReactiveProperty<bool> CanExecute { get; }

		bool Execute(T parameter);
	}
}
