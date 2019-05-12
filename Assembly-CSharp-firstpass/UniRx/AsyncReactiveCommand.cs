using System;

namespace UniRx
{
	public class AsyncReactiveCommand : AsyncReactiveCommand<Unit>
	{
		public AsyncReactiveCommand()
		{
		}

		public AsyncReactiveCommand(IObservable<bool> canExecuteSource) : base(canExecuteSource)
		{
		}

		public AsyncReactiveCommand(IReactiveProperty<bool> sharedCanExecute) : base(sharedCanExecute)
		{
		}

		public IDisposable Execute()
		{
			return base.Execute(Unit.Default);
		}
	}
}
