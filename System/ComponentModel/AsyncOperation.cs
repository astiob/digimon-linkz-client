using System;
using System.Threading;

namespace System.ComponentModel
{
	/// <summary>Tracks the lifetime of an asynchronous operation.</summary>
	public sealed class AsyncOperation
	{
		private SynchronizationContext ctx;

		private object state;

		private bool done;

		internal AsyncOperation(SynchronizationContext ctx, object state)
		{
			this.ctx = ctx;
			this.state = state;
			ctx.OperationStarted();
		}

		~AsyncOperation()
		{
			if (!this.done && this.ctx != null)
			{
				this.ctx.OperationCompleted();
			}
		}

		/// <summary>Gets the <see cref="T:System.Threading.SynchronizationContext" /> object that was passed to the constructor.</summary>
		/// <returns>The <see cref="T:System.Threading.SynchronizationContext" /> object that was passed to the constructor.</returns>
		public SynchronizationContext SynchronizationContext
		{
			get
			{
				return this.ctx;
			}
		}

		/// <summary>Gets or sets an object used to uniquely identify an asynchronous operation.</summary>
		/// <returns>The state object passed to the asynchronous method invocation.</returns>
		public object UserSuppliedState
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>Ends the lifetime of an asynchronous operation.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.ComponentModel.AsyncOperation.OperationCompleted" /> has been called previously for this task. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void OperationCompleted()
		{
			if (this.done)
			{
				throw new InvalidOperationException("This task is already completed. Multiple call to OperationCompleted is not allowed.");
			}
			this.ctx.OperationCompleted();
			this.done = true;
		}

		/// <summary>Invokes a delegate on the thread or context appropriate for the application model.</summary>
		/// <param name="d">A <see cref="T:System.Threading.SendOrPostCallback" /> object that wraps the delegate to be called when the operation ends. </param>
		/// <param name="arg">An argument for the delegate contained in the <paramref name="d" /> parameter. </param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.ComponentModel.AsyncOperation.PostOperationCompleted(System.Threading.SendOrPostCallback,System.Object)" /> method has been called previously for this task. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		public void Post(SendOrPostCallback d, object arg)
		{
			if (this.done)
			{
				throw new InvalidOperationException("This task is already completed. Multiple call to Post is not allowed.");
			}
			this.ctx.Post(d, arg);
		}

		/// <summary>Ends the lifetime of an asynchronous operation.</summary>
		/// <param name="d">A <see cref="T:System.Threading.SendOrPostCallback" /> object that wraps the delegate to be called when the operation ends. </param>
		/// <param name="arg">An argument for the delegate contained in the <paramref name="d" /> parameter. </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.ComponentModel.AsyncOperation.OperationCompleted" /> has been called previously for this task. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="d" /> is null. </exception>
		public void PostOperationCompleted(SendOrPostCallback d, object arg)
		{
			if (this.done)
			{
				throw new InvalidOperationException("This task is already completed. Multiple call to PostOperationCompleted is not allowed.");
			}
			this.Post(d, arg);
			this.OperationCompleted();
		}
	}
}
