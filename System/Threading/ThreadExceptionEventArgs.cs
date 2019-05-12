using System;

namespace System.Threading
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Application.ThreadException" /> event.</summary>
	/// <filterpriority>2</filterpriority>
	public class ThreadExceptionEventArgs : EventArgs
	{
		private Exception exception;

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.ThreadExceptionEventArgs" /> class.</summary>
		/// <param name="t">The <see cref="T:System.Exception" /> that occurred. </param>
		public ThreadExceptionEventArgs(Exception t)
		{
			this.exception = t;
		}

		/// <summary>Gets the <see cref="T:System.Exception" /> that occurred.</summary>
		/// <returns>The <see cref="T:System.Exception" /> that occurred.</returns>
		/// <filterpriority>1</filterpriority>
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}
	}
}
