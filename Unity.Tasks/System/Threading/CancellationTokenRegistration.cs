using System;

namespace System.Threading
{
	public struct CancellationTokenRegistration : IDisposable
	{
		private Action action;

		private CancellationTokenSource source;

		internal CancellationTokenRegistration(CancellationTokenSource source, Action action)
		{
			this.source = source;
			this.action = action;
		}

		public void Dispose()
		{
			if (this.source != null && this.action != null)
			{
				this.source.Unregister(this.action);
				this.action = null;
				this.source = null;
			}
		}
	}
}
