using System;

namespace System.IO
{
	/// <summary>Provides data for the <see cref="E:System.IO.FileSystemWatcher.Error" /> event.</summary>
	/// <filterpriority>2</filterpriority>
	public class ErrorEventArgs : EventArgs
	{
		private Exception exception;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.ErrorEventArgs" /> class.</summary>
		/// <param name="exception">An <see cref="T:System.Exception" /> that represents the error that occurred. </param>
		public ErrorEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		/// <summary>Gets the <see cref="T:System.Exception" /> that represents the error that occurred.</summary>
		/// <returns>An <see cref="T:System.Exception" /> that represents the error that occurred.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual Exception GetException()
		{
			return this.exception;
		}
	}
}
