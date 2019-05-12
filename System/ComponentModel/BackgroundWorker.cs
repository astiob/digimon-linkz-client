using System;
using System.Threading;

namespace System.ComponentModel
{
	/// <summary>Executes an operation on a separate thread.</summary>
	public class BackgroundWorker
	{
		private AsyncOperation async;

		private bool cancel_pending;

		private bool report_progress;

		private bool support_cancel;

		/// <summary>Occurs when <see cref="M:System.ComponentModel.BackgroundWorker.RunWorkerAsync" /> is called.</summary>
		public event DoWorkEventHandler DoWork;

		/// <summary>Occurs when <see cref="M:System.ComponentModel.BackgroundWorker.ReportProgress(System.Int32)" /> is called.</summary>
		public event ProgressChangedEventHandler ProgressChanged;

		/// <summary>Occurs when the background operation has completed, has been canceled, or has raised an exception.</summary>
		public event RunWorkerCompletedEventHandler RunWorkerCompleted;

		/// <summary>Gets a value indicating whether the application has requested cancellation of a background operation.</summary>
		/// <returns>true if the application has requested cancellation of a background operation; otherwise, false. The default is false.</returns>
		public bool CancellationPending
		{
			get
			{
				return this.cancel_pending;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.ComponentModel.BackgroundWorker" /> is running an asynchronous operation.</summary>
		/// <returns>true, if the <see cref="T:System.ComponentModel.BackgroundWorker" /> is running an asynchronous operation; otherwise, false.</returns>
		public bool IsBusy
		{
			get
			{
				return this.async != null;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.ComponentModel.BackgroundWorker" /> can report progress updates.</summary>
		/// <returns>true if the <see cref="T:System.ComponentModel.BackgroundWorker" /> supports progress updates; otherwise false. The default is false.</returns>
		[DefaultValue(false)]
		public bool WorkerReportsProgress
		{
			get
			{
				return this.report_progress;
			}
			set
			{
				this.report_progress = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.ComponentModel.BackgroundWorker" /> supports asynchronous cancellation.</summary>
		/// <returns>true if the <see cref="T:System.ComponentModel.BackgroundWorker" /> supports cancellation; otherwise false. The default is false.</returns>
		[DefaultValue(false)]
		public bool WorkerSupportsCancellation
		{
			get
			{
				return this.support_cancel;
			}
			set
			{
				this.support_cancel = value;
			}
		}

		/// <summary>Requests cancellation of a pending background operation.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.ComponentModel.BackgroundWorker.WorkerSupportsCancellation" /> is false. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void CancelAsync()
		{
			if (!this.support_cancel)
			{
				throw new InvalidOperationException("This background worker does not support cancellation.");
			}
			if (!this.IsBusy)
			{
				return;
			}
			this.cancel_pending = true;
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.BackgroundWorker.ProgressChanged" /> event.</summary>
		/// <param name="percentProgress">The percentage, from 0 to 100, of the background operation that is complete. </param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.ComponentModel.BackgroundWorker.WorkerReportsProgress" /> property is set to false. </exception>
		public void ReportProgress(int percentProgress)
		{
			this.ReportProgress(percentProgress, null);
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.BackgroundWorker.ProgressChanged" /> event.</summary>
		/// <param name="percentProgress">The percentage, from 0 to 100, of the background operation that is complete.</param>
		/// <param name="userState">The state object passed to <see cref="M:System.ComponentModel.BackgroundWorker.RunWorkerAsync(System.Object)" />.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.ComponentModel.BackgroundWorker.WorkerReportsProgress" /> property is set to false. </exception>
		public void ReportProgress(int percentProgress, object userState)
		{
			if (!this.WorkerReportsProgress)
			{
				throw new InvalidOperationException("This background worker does not report progress.");
			}
			if (!this.IsBusy)
			{
				return;
			}
			this.async.Post(delegate(object o)
			{
				ProgressChangedEventArgs e = o as ProgressChangedEventArgs;
				this.OnProgressChanged(e);
			}, new ProgressChangedEventArgs(percentProgress, userState));
		}

		/// <summary>Starts execution of a background operation.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.ComponentModel.BackgroundWorker.IsBusy" /> is true.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void RunWorkerAsync()
		{
			this.RunWorkerAsync(null);
		}

		private void ProcessWorker(object argument, AsyncOperation async, SendOrPostCallback callback)
		{
			Exception error = null;
			DoWorkEventArgs doWorkEventArgs = new DoWorkEventArgs(argument);
			try
			{
				this.OnDoWork(doWorkEventArgs);
			}
			catch (Exception ex)
			{
				error = ex;
				doWorkEventArgs.Cancel = false;
			}
			callback(new object[]
			{
				new RunWorkerCompletedEventArgs(doWorkEventArgs.Result, error, doWorkEventArgs.Cancel),
				async
			});
		}

		private void CompleteWorker(object state)
		{
			object[] array = (object[])state;
			RunWorkerCompletedEventArgs arg = array[0] as RunWorkerCompletedEventArgs;
			AsyncOperation asyncOperation = array[1] as AsyncOperation;
			SendOrPostCallback d = delegate(object darg)
			{
				this.async = null;
				this.OnRunWorkerCompleted(darg as RunWorkerCompletedEventArgs);
			};
			asyncOperation.PostOperationCompleted(d, arg);
			this.cancel_pending = false;
		}

		/// <summary>Starts execution of a background operation.</summary>
		/// <param name="argument">A parameter for use by the background operation to be executed in the <see cref="E:System.ComponentModel.BackgroundWorker.DoWork" /> event handler. </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.ComponentModel.BackgroundWorker.IsBusy" /> is true. </exception>
		public void RunWorkerAsync(object argument)
		{
			if (this.IsBusy)
			{
				throw new InvalidOperationException("The background worker is busy.");
			}
			this.async = AsyncOperationManager.CreateOperation(this);
			BackgroundWorker.ProcessWorkerEventHandler processWorkerEventHandler = new BackgroundWorker.ProcessWorkerEventHandler(this.ProcessWorker);
			processWorkerEventHandler.BeginInvoke(argument, this.async, new SendOrPostCallback(this.CompleteWorker), null, null);
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.BackgroundWorker.DoWork" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected virtual void OnDoWork(DoWorkEventArgs e)
		{
			if (this.DoWork != null)
			{
				this.DoWork(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.BackgroundWorker.ProgressChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
		{
			if (this.ProgressChanged != null)
			{
				this.ProgressChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.ComponentModel.BackgroundWorker.RunWorkerCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
		{
			if (this.RunWorkerCompleted != null)
			{
				this.RunWorkerCompleted(this, e);
			}
		}

		private delegate void ProcessWorkerEventHandler(object argument, AsyncOperation async, SendOrPostCallback callback);
	}
}
