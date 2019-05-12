using System;
using System.Reflection;

namespace System.ComponentModel
{
	/// <summary>Provides data for the MethodNameCompleted event.</summary>
	public class AsyncCompletedEventArgs : EventArgs
	{
		private Exception _error;

		private bool _cancelled;

		private object _userState;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> class. </summary>
		/// <param name="error">Any error that occurred during the asynchronous operation.</param>
		/// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
		/// <param name="userState">The optional user-supplied state object passed to the <see cref="M:System.ComponentModel.BackgroundWorker.RunWorkerAsync(System.Object)" /> method.</param>
		public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
		{
			this._error = error;
			this._cancelled = cancelled;
			this._userState = userState;
		}

		/// <summary>Raises a user-supplied exception if an asynchronous operation failed.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.ComponentModel.AsyncCompletedEventArgs.Cancelled" /> property is true. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The <see cref="P:System.ComponentModel.AsyncCompletedEventArgs.Error" /> property has been set by the asynchronous operation. The <see cref="P:System.Exception.InnerException" /> property holds a reference to <see cref="P:System.ComponentModel.AsyncCompletedEventArgs.Error" />. </exception>
		protected void RaiseExceptionIfNecessary()
		{
			if (this._error != null)
			{
				throw new TargetInvocationException(this._error);
			}
			if (this._cancelled)
			{
				throw new InvalidOperationException("The operation was cancelled");
			}
		}

		/// <summary>Gets a value indicating whether an asynchronous operation has been canceled.</summary>
		/// <returns>true if the background operation has been canceled; otherwise false. The default is false.</returns>
		public bool Cancelled
		{
			get
			{
				return this._cancelled;
			}
		}

		/// <summary>Gets a value indicating which error occurred during an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Exception" /> instance, if an error occurred during an asynchronous operation; otherwise null.</returns>
		public Exception Error
		{
			get
			{
				return this._error;
			}
		}

		/// <summary>Gets the unique identifier for the asynchronous task.</summary>
		/// <returns>An object reference that uniquely identifies the asynchronous task; otherwise, null if no value has been set.</returns>
		public object UserState
		{
			get
			{
				return this._userState;
			}
		}
	}
}
