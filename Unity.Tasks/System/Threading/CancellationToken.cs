using System;

namespace System.Threading
{
	public struct CancellationToken
	{
		private CancellationTokenSource source;

		internal CancellationToken(CancellationTokenSource source)
		{
			this.source = source;
		}

		public static CancellationToken None
		{
			get
			{
				return default(CancellationToken);
			}
		}

		public bool IsCancellationRequested
		{
			get
			{
				return this.source != null && this.source.IsCancellationRequested;
			}
		}

		public CancellationTokenRegistration Register(Action callback)
		{
			if (this.source != null)
			{
				return this.source.Register(callback);
			}
			return default(CancellationTokenRegistration);
		}

		public void ThrowIfCancellationRequested()
		{
			if (this.IsCancellationRequested)
			{
				throw new OperationCanceledException();
			}
		}
	}
}
