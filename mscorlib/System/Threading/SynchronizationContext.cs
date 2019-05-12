using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	/// <summary>Provides the basic functionality for propagating a synchronization context in various synchronization models. </summary>
	/// <filterpriority>2</filterpriority>
	public class SynchronizationContext
	{
		private bool notification_required;

		[ThreadStatic]
		private static SynchronizationContext currentContext;

		/// <summary>Creates a new instance of the <see cref="T:System.Threading.SynchronizationContext" /> class.</summary>
		public SynchronizationContext()
		{
		}

		internal SynchronizationContext(SynchronizationContext context)
		{
			SynchronizationContext.currentContext = context;
		}

		/// <summary>Gets the synchronization context for the current thread.</summary>
		/// <returns>A <see cref="T:System.Threading.SynchronizationContext" /> object representing the current synchronization context.</returns>
		/// <filterpriority>1</filterpriority>
		public static SynchronizationContext Current
		{
			get
			{
				return SynchronizationContext.currentContext;
			}
		}

		/// <summary>When overridden in a derived class, creates a copy of the synchronization context.  </summary>
		/// <returns>A new <see cref="T:System.Threading.SynchronizationContext" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual SynchronizationContext CreateCopy()
		{
			return new SynchronizationContext(this);
		}

		/// <summary>Determines if wait notification is required.</summary>
		/// <returns>true if wait notification is required; otherwise, false. </returns>
		public bool IsWaitNotificationRequired()
		{
			return this.notification_required;
		}

		/// <summary>When overridden in a derived class, responds to the notification that an operation has completed.</summary>
		public virtual void OperationCompleted()
		{
		}

		/// <summary>When overridden in a derived class, responds to the notification that an operation has started.</summary>
		public virtual void OperationStarted()
		{
		}

		/// <summary>When overridden in a derived class, dispatches an asynchronous message to a synchronization context.</summary>
		/// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <filterpriority>2</filterpriority>
		public virtual void Post(SendOrPostCallback d, object state)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(d.Invoke), state);
		}

		/// <summary>When overridden in a derived class, dispatches a synchronous message to a synchronization context.</summary>
		/// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <filterpriority>2</filterpriority>
		public virtual void Send(SendOrPostCallback d, object state)
		{
			d(state);
		}

		/// <summary>Sets the current synchronization context.</summary>
		/// <param name="syncContext">The <see cref="T:System.Threading.SynchronizationContext" /> object to be set.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public static void SetSynchronizationContext(SynchronizationContext syncContext)
		{
			SynchronizationContext.currentContext = syncContext;
		}

		[Obsolete]
		public static void SetThreadStaticContext(SynchronizationContext syncContext)
		{
			SynchronizationContext.currentContext = syncContext;
		}

		/// <summary>Sets notification that wait notification is required and prepares the callback method so it can be called more reliably when a wait occurs.</summary>
		[MonoTODO]
		protected void SetWaitNotificationRequired()
		{
			this.notification_required = true;
			throw new NotImplementedException();
		}

		/// <summary>Waits for any or all the elements in the specified array to receive a signal.</summary>
		/// <returns>The array index of the object that satisfied the wait.</returns>
		/// <param name="waitHandles">An array of type <see cref="T:System.IntPtr" /> that contains the native operating system handles.</param>
		/// <param name="waitAll">true to wait for all handles; false to wait for any handle. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		[CLSCompliant(false)]
		[PrePrepareMethod]
		public virtual int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			return SynchronizationContext.WaitHelper(waitHandles, waitAll, millisecondsTimeout);
		}

		/// <summary>Helper function that waits for any or all the elements in the specified array to receive a signal.</summary>
		/// <returns>The array index of the object that satisfied the wait.</returns>
		/// <param name="waitHandles">An array of type <see cref="T:System.IntPtr" /> that contains the native operating system handles.</param>
		/// <param name="waitAll">true to wait for all handles;  false to wait for any handle. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[PrePrepareMethod]
		[CLSCompliant(false)]
		[MonoTODO]
		protected static int WaitHelper(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			throw new NotImplementedException();
		}
	}
}
