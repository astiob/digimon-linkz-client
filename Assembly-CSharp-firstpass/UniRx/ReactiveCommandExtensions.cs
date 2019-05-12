using System;
using UnityEngine.UI;

namespace UniRx
{
	public static class ReactiveCommandExtensions
	{
		public static ReactiveCommand ToReactiveCommand(this IObservable<bool> canExecuteSource, bool initialValue = true)
		{
			return new ReactiveCommand(canExecuteSource, initialValue);
		}

		public static ReactiveCommand<T> ToReactiveCommand<T>(this IObservable<bool> canExecuteSource, bool initialValue = true)
		{
			return new ReactiveCommand<T>(canExecuteSource, initialValue);
		}

		public static IDisposable BindTo(this ReactiveCommand<Unit> command, Button button)
		{
			IDisposable disposable = command.CanExecute.SubscribeToInteractable(button);
			IDisposable disposable2 = button.OnClickAsObservable().SubscribeWithState(command, delegate(Unit x, ReactiveCommand<Unit> c)
			{
				c.Execute(x);
			});
			return StableCompositeDisposable.Create(disposable, disposable2);
		}

		public static IDisposable BindToOnClick(this ReactiveCommand<Unit> command, Button button, Action<Unit> onClick)
		{
			IDisposable disposable = command.CanExecute.SubscribeToInteractable(button);
			IDisposable disposable2 = button.OnClickAsObservable().SubscribeWithState(command, delegate(Unit x, ReactiveCommand<Unit> c)
			{
				c.Execute(x);
			});
			IDisposable disposable3 = command.Subscribe(onClick);
			return StableCompositeDisposable.Create(disposable, disposable2, disposable3);
		}

		public static IDisposable BindToButtonOnClick(this IObservable<bool> canExecuteSource, Button button, Action<Unit> onClick, bool initialValue = true)
		{
			return canExecuteSource.ToReactiveCommand(initialValue).BindToOnClick(button, onClick);
		}
	}
}
