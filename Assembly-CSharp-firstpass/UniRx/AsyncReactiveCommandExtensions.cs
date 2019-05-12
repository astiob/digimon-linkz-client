using System;
using UnityEngine.UI;

namespace UniRx
{
	public static class AsyncReactiveCommandExtensions
	{
		public static AsyncReactiveCommand ToAsyncReactiveCommand(this IReactiveProperty<bool> sharedCanExecuteSource)
		{
			return new AsyncReactiveCommand(sharedCanExecuteSource);
		}

		public static AsyncReactiveCommand<T> ToAsyncReactiveCommand<T>(this IReactiveProperty<bool> sharedCanExecuteSource)
		{
			return new AsyncReactiveCommand<T>(sharedCanExecuteSource);
		}

		public static IDisposable BindTo(this AsyncReactiveCommand<Unit> command, Button button)
		{
			IDisposable disposable = command.CanExecute.SubscribeToInteractable(button);
			IDisposable disposable2 = button.OnClickAsObservable().SubscribeWithState(command, delegate(Unit x, AsyncReactiveCommand<Unit> c)
			{
				c.Execute(x);
			});
			return StableCompositeDisposable.Create(disposable, disposable2);
		}

		public static IDisposable BindToOnClick(this AsyncReactiveCommand<Unit> command, Button button, Func<Unit, IObservable<Unit>> asyncOnClick)
		{
			IDisposable disposable = command.CanExecute.SubscribeToInteractable(button);
			IDisposable disposable2 = button.OnClickAsObservable().SubscribeWithState(command, delegate(Unit x, AsyncReactiveCommand<Unit> c)
			{
				c.Execute(x);
			});
			IDisposable disposable3 = command.Subscribe(asyncOnClick);
			return StableCompositeDisposable.Create(disposable, disposable2, disposable3);
		}

		public static IDisposable BindToOnClick(this Button button, Func<Unit, IObservable<Unit>> asyncOnClick)
		{
			return new AsyncReactiveCommand().BindToOnClick(button, asyncOnClick);
		}

		public static IDisposable BindToOnClick(this Button button, IReactiveProperty<bool> sharedCanExecuteSource, Func<Unit, IObservable<Unit>> asyncOnClick)
		{
			return sharedCanExecuteSource.ToAsyncReactiveCommand().BindToOnClick(button, asyncOnClick);
		}
	}
}
