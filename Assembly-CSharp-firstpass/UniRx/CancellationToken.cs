using System;

namespace UniRx
{
	public struct CancellationToken
	{
		private readonly ICancelable source;

		public static readonly CancellationToken Empty = new CancellationToken(null);

		public static readonly CancellationToken None = new CancellationToken(null);

		public CancellationToken(ICancelable source)
		{
			this.source = source;
		}

		public bool IsCancellationRequested
		{
			get
			{
				return this.source != null && this.source.IsDisposed;
			}
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
