using System;

namespace UniRx
{
	public class ReactiveCommand : ReactiveCommand<Unit>
	{
		public ReactiveCommand()
		{
		}

		public ReactiveCommand(IObservable<bool> canExecuteSource, bool initialValue = true) : base(canExecuteSource, initialValue)
		{
		}

		public bool Execute()
		{
			return base.Execute(Unit.Default);
		}

		public void ForceExecute()
		{
			base.ForceExecute(Unit.Default);
		}
	}
}
