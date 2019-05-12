using System;
using System.Threading;

namespace System
{
	public class Progress<T> : IProgress<T> where T : EventArgs
	{
		private SynchronizationContext synchronizationContext;

		private SendOrPostCallback synchronizationCallback;

		private Action<T> eventHandler;

		public Progress()
		{
			this.synchronizationContext = (SynchronizationContext.Current ?? ProgressSynchronizationContext.SharedContext);
			this.synchronizationCallback = new SendOrPostCallback(this.NotifyDelegates);
		}

		public Progress(Action<T> handler) : this()
		{
			this.eventHandler = handler;
		}

		void IProgress<T>.Report(T value)
		{
			this.OnReport(value);
		}

		protected virtual void OnReport(T value)
		{
			this.synchronizationContext.Post(this.synchronizationCallback, value);
		}

		public event EventHandler<T> ProgressChanged;

		private void NotifyDelegates(object newValue)
		{
			T t = (T)((object)newValue);
			Action<T> action = this.eventHandler;
			EventHandler<T> progressChanged = this.ProgressChanged;
			if (action != null)
			{
				action(t);
			}
			if (progressChanged != null)
			{
				progressChanged(this, t);
			}
		}
	}
}
