using System;

namespace UniRx
{
	public class ReactiveCommand<T> : IReactiveCommand<T>, IDisposable, IObservable<T>
	{
		private readonly Subject<T> trigger = new Subject<T>();

		private readonly IDisposable canExecuteSubscription;

		private ReactiveProperty<bool> canExecute;

		public ReactiveCommand()
		{
			this.canExecute = new ReactiveProperty<bool>(true);
			this.canExecuteSubscription = Disposable.Empty;
		}

		public ReactiveCommand(IObservable<bool> canExecuteSource, bool initialValue = true)
		{
			this.canExecute = new ReactiveProperty<bool>(initialValue);
			this.canExecuteSubscription = canExecuteSource.DistinctUntilChanged<bool>().SubscribeWithState(this.canExecute, delegate(bool b, ReactiveProperty<bool> c)
			{
				c.Value = b;
			});
		}

		public IReadOnlyReactiveProperty<bool> CanExecute
		{
			get
			{
				return this.canExecute;
			}
		}

		public bool IsDisposed { get; private set; }

		public bool Execute(T parameter)
		{
			if (this.canExecute.Value)
			{
				this.trigger.OnNext(parameter);
				return true;
			}
			return false;
		}

		public void ForceExecute(T parameter)
		{
			this.trigger.OnNext(parameter);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return this.trigger.Subscribe(observer);
		}

		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			this.IsDisposed = true;
			this.canExecute.Dispose();
			this.trigger.OnCompleted();
			this.trigger.Dispose();
			this.canExecuteSubscription.Dispose();
		}
	}
}
