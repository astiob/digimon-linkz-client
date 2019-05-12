using System;

namespace UniRx
{
	public sealed class BooleanDisposable : IDisposable, ICancelable
	{
		public BooleanDisposable()
		{
		}

		internal BooleanDisposable(bool isDisposed)
		{
			this.IsDisposed = isDisposed;
		}

		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				this.IsDisposed = true;
			}
		}
	}
}
