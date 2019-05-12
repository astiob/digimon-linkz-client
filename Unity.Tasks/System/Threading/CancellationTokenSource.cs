using System;
using System.Collections.Generic;

namespace System.Threading
{
	public sealed class CancellationTokenSource
	{
		private object mutex = new object();

		private Action actions;

		private bool isCancellationRequested;

		internal CancellationTokenRegistration Register(Action action)
		{
			object obj = this.mutex;
			CancellationTokenRegistration result;
			lock (obj)
			{
				this.actions = (Action)Delegate.Combine(this.actions, action);
				if (this.IsCancellationRequested)
				{
					this.Cancel(false);
				}
				result = new CancellationTokenRegistration(this, action);
			}
			return result;
		}

		internal void Unregister(Action action)
		{
			object obj = this.mutex;
			lock (obj)
			{
				this.actions = (Action)Delegate.Remove(this.actions, action);
			}
		}

		internal bool IsCancellationRequested
		{
			get
			{
				object obj = this.mutex;
				bool result;
				lock (obj)
				{
					result = this.isCancellationRequested;
				}
				return result;
			}
		}

		public CancellationToken Token
		{
			get
			{
				return new CancellationToken(this);
			}
		}

		public void Cancel()
		{
			this.Cancel(false);
		}

		public void Cancel(bool throwOnFirstException)
		{
			object obj = this.mutex;
			lock (obj)
			{
				this.isCancellationRequested = true;
				if (this.actions != null)
				{
					try
					{
						if (throwOnFirstException)
						{
							this.actions();
						}
						else
						{
							foreach (Delegate @delegate in this.actions.GetInvocationList())
							{
								List<Exception> list = new List<Exception>();
								try
								{
									((Action)@delegate)();
								}
								catch (Exception item)
								{
									list.Add(item);
								}
								if (list.Count > 0)
								{
									throw new AggregateException(list);
								}
							}
						}
					}
					finally
					{
						this.actions = null;
					}
				}
			}
		}
	}
}
