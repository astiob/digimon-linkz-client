using System;

namespace System.Runtime.ExceptionServices
{
	public class ExceptionDispatchInfo
	{
		private ExceptionDispatchInfo(Exception ex)
		{
			this.SourceException = ex;
		}

		public static ExceptionDispatchInfo Capture(Exception ex)
		{
			return new ExceptionDispatchInfo(ex);
		}

		public Exception SourceException { get; private set; }

		public void Throw()
		{
			throw this.SourceException;
		}
	}
}
