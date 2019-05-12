using System;
using UniRx.InternalUtil;

namespace UniRx
{
	public class AsyncReactiveCommand<T> : IAsyncReactiveCommand<T>
	{
		private ImmutableList<Func<T, IObservable<Unit>>> asyncActions = ImmutableList<Func<T, IObservable<Unit>>>.Empty;

		private readonly object gate = new object();

		private readonly IReactiveProperty<bool> canExecuteSource;

		private readonly IReadOnlyReactiveProperty<bool> canExecute;

		public AsyncReactiveCommand()
		{
			this.canExecuteSource = new ReactiveProperty<bool>(true);
			this.canExecute = this.canExecuteSource;
		}

		public AsyncReactiveCommand(IObservable<bool> canExecuteSource)
		{
			this.canExecuteSource = new ReactiveProperty<bool>(true);
			this.canExecute = this.canExecute.CombineLatest(canExecuteSource, (bool x, bool y) => x && y).ToReactiveProperty<bool>();
		}

		public AsyncReactiveCommand(IReactiveProperty<bool> sharedCanExecute)
		{
			this.canExecuteSource = sharedCanExecute;
			this.canExecute = sharedCanExecute;
		}

		public IReadOnlyReactiveProperty<bool> CanExecute
		{
			get
			{
				return this.canExecute;
			}
		}

		public bool IsDisposed { get; private set; }

		public IDisposable Execute(T parameter)
		{
			if (this.canExecute.Value)
			{
				this.canExecuteSource.Value = false;
				Func<T, IObservable<Unit>>[] data = this.asyncActions.Data;
				if (data.Length == 1)
				{
					try
					{
						IObservable<Unit> source = data[0](parameter) ?? Observable.ReturnUnit();
						return source.Finally(delegate
						{
							this.canExecuteSource.Value = true;
						}).Subscribe<Unit>();
					}
					catch
					{
						this.canExecuteSource.Value = true;
						throw;
					}
				}
				IObservable<Unit>[] array = new IObservable<Unit>[data.Length];
				try
				{
					for (int i = 0; i < data.Length; i++)
					{
						array[i] = (data[i](parameter) ?? Observable.ReturnUnit());
					}
				}
				catch
				{
					this.canExecuteSource.Value = true;
					throw;
				}
				return Observable.WhenAll(array).Finally(delegate
				{
					this.canExecuteSource.Value = true;
				}).Subscribe<Unit>();
			}
			return Disposable.Empty;
		}

		public IDisposable Subscribe(Func<T, IObservable<Unit>> asyncAction)
		{
			object obj = this.gate;
			lock (obj)
			{
				this.asyncActions = this.asyncActions.Add(asyncAction);
			}
			return new AsyncReactiveCommand<T>.Subscription(this, asyncAction);
		}

		private class Subscription : IDisposable
		{
			private readonly AsyncReactiveCommand<T> parent;

			private readonly Func<T, IObservable<Unit>> asyncAction;

			public Subscription(AsyncReactiveCommand<T> parent, Func<T, IObservable<Unit>> asyncAction)
			{
				this.parent = parent;
				this.asyncAction = asyncAction;
			}

			public void Dispose()
			{
				object gate = this.parent.gate;
				lock (gate)
				{
					this.parent.asyncActions = this.parent.asyncActions.Remove(this.asyncAction);
				}
			}
		}
	}
}
