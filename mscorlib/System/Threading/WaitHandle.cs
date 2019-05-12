using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Threading
{
	/// <summary>Encapsulates operating system–specific objects that wait for exclusive access to shared resources.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public abstract class WaitHandle : MarshalByRefObject, IDisposable
	{
		/// <summary>Indicates that a <see cref="M:System.Threading.WaitHandle.WaitAny(System.Threading.WaitHandle[],System.Int32,System.Boolean)" /> operation timed out before any of the wait handles were signaled. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const int WaitTimeout = 258;

		private SafeWaitHandle safe_wait_handle;

		/// <summary>Represents an invalid native operating system handle. This field is read-only.</summary>
		protected static readonly IntPtr InvalidHandle = (IntPtr)(-1);

		private bool disposed;

		/// <summary>Releases all resources used by the <see cref="T:System.Threading.WaitHandle" />.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool WaitAll_internal(WaitHandle[] handles, int ms, bool exitContext);

		private static void CheckArray(WaitHandle[] handles, bool waitAll)
		{
			if (handles == null)
			{
				throw new ArgumentNullException("waitHandles");
			}
			int num = handles.Length;
			if (num > 64)
			{
				throw new NotSupportedException("Too many handles");
			}
			foreach (WaitHandle waitHandle in handles)
			{
				if (waitHandle == null)
				{
					throw new ArgumentNullException("waitHandles", "null handle");
				}
				if (waitHandle.safe_wait_handle == null)
				{
					throw new ArgumentException("null element found", "waitHandle");
				}
			}
		}

		/// <summary>Waits for all the elements in the specified array to receive a signal.</summary>
		/// <returns>true when every element in <paramref name="waitHandles" /> has received a signal; otherwise the method never returns.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null. -or- One or more of the objects in the <paramref name="waitHandles" /> array are null. -or-<paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 2.0 or later. </exception>
		/// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.-or- The <see cref="T:System.STAThreadAttribute" /> attribute is applied to the thread procedure for the current thread, and <paramref name="waitHandles" /> contains more than one element. </exception>
		/// <exception cref="T:System.ApplicationException">
		///   <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 1.0 or 1.1. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait terminated because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool WaitAll(WaitHandle[] waitHandles)
		{
			WaitHandle.CheckArray(waitHandles, true);
			return WaitHandle.WaitAll_internal(waitHandles, -1, false);
		}

		/// <summary>Waits for all the elements in the specified array to receive a signal, using an <see cref="T:System.Int32" /> value to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
		/// <returns>true when every element in <paramref name="waitHandles" /> has received a signal; otherwise, false.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object (duplicates). </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely. </param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or- One or more of the objects in the <paramref name="waitHandles" /> array is null. -or-<paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 2.0 or later. </exception>
		/// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.-or- The <see cref="T:System.STAThreadAttribute" /> attribute is applied to the thread procedure for the current thread, and <paramref name="waitHandles" /> contains more than one element. </exception>
		/// <exception cref="T:System.ApplicationException">
		///   <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 1.0 or 1.1. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
		{
			WaitHandle.CheckArray(waitHandles, true);
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			bool result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				result = WaitHandle.WaitAll_internal(waitHandles, millisecondsTimeout, false);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		/// <summary>Waits for all the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> value to specify the time interval, and specifying whether to exit the synchronization domain before the wait.</summary>
		/// <returns>true when every element in <paramref name="waitHandles" /> has received a signal; otherwise false.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds, to wait indefinitely. </param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null. -or- One or more of the objects in the <paramref name="waitHandles" /> array is null. -or-<paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 2.0 or later. </exception>
		/// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.-or- The <see cref="T:System.STAThreadAttribute" /> attribute is applied to the thread procedure for the current thread, and <paramref name="waitHandles" /> contains more than one element. </exception>
		/// <exception cref="T:System.ApplicationException">
		///   <paramref name="waitHandles" /> is an array with no elements and the .NET Framework version is 1.0 or 1.1. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait terminated because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext)
		{
			WaitHandle.CheckArray(waitHandles, true);
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			bool result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				result = WaitHandle.WaitAll_internal(waitHandles, (int)num, exitContext);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int WaitAny_internal(WaitHandle[] handles, int ms, bool exitContext);

		/// <summary>Waits for any of the elements in the specified array to receive a signal.</summary>
		/// <returns>The array index of the object that satisfied the wait.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or-One or more of the objects in the <paramref name="waitHandles" /> array is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits. </exception>
		/// <exception cref="T:System.ApplicationException">
		///   <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 1.0 or 1.1. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 2.0 or later. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles)
		{
			WaitHandle.CheckArray(waitHandles, false);
			return WaitHandle.WaitAny_internal(waitHandles, -1, false);
		}

		/// <summary>Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to specify the time interval, and specifying whether to exit the synchronization domain before the wait.</summary>
		/// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="millisecondsTimeout" /> has passed.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely. </param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or-One or more of the objects in the <paramref name="waitHandles" /> array is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits. </exception>
		/// <exception cref="T:System.ApplicationException">
		///   <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 1.0 or 1.1. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 2.0 or later. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
		{
			WaitHandle.CheckArray(waitHandles, false);
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			int result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				result = WaitHandle.WaitAny_internal(waitHandles, millisecondsTimeout, exitContext);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		/// <summary>Waits for any of the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval.</summary>
		/// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="timeout" /> has passed.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or-One or more of the objects in the <paramref name="waitHandles" /> array is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="waitHandles" /> is an array with no elements. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout)
		{
			return WaitHandle.WaitAny(waitHandles, timeout, false);
		}

		/// <summary>Waits for any of the elements in the specified array to receive a signal, using a 32-bit signed integer to specify the time interval.</summary>
		/// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="millisecondsTimeout" /> has passed.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or-One or more of the objects in the <paramref name="waitHandles" /> array is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="waitHandles" /> is an array with no elements. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout)
		{
			return WaitHandle.WaitAny(waitHandles, millisecondsTimeout, false);
		}

		/// <summary>Waits for any of the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
		/// <returns>The array index of the object that satisfied the wait, or <see cref="F:System.Threading.WaitHandle.WaitTimeout" /> if no object satisfied the wait and a time interval equivalent to <paramref name="timeout" /> has passed.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely. </param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or-One or more of the objects in the <paramref name="waitHandles" /> array is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits. </exception>
		/// <exception cref="T:System.ApplicationException">
		///   <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 1.0 or 1.1. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="waitHandles" /> is an array with no elements, and the .NET Framework version is 2.0 or later. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static int WaitAny(WaitHandle[] waitHandles, TimeSpan timeout, bool exitContext)
		{
			WaitHandle.CheckArray(waitHandles, false);
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			int result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				result = WaitHandle.WaitAny_internal(waitHandles, (int)num, exitContext);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
			}
			return result;
		}

		/// <summary>When overridden in a derived class, releases all resources held by the current <see cref="T:System.Threading.WaitHandle" />.</summary>
		/// <filterpriority>2</filterpriority>
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets or sets the native operating system handle.</summary>
		/// <returns>An IntPtr representing the native operating system handle. The default is the value of the <see cref="F:System.Threading.WaitHandle.InvalidHandle" /> field.</returns>
		/// <filterpriority>2</filterpriority>
		[Obsolete("In the profiles > 2.x, use SafeHandle instead of Handle")]
		public virtual IntPtr Handle
		{
			get
			{
				return this.safe_wait_handle.DangerousGetHandle();
			}
			set
			{
				if (value == WaitHandle.InvalidHandle)
				{
					this.safe_wait_handle = new SafeWaitHandle(WaitHandle.InvalidHandle, false);
				}
				else
				{
					this.safe_wait_handle = new SafeWaitHandle(value, true);
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool WaitOne_internal(IntPtr handle, int ms, bool exitContext);

		/// <summary>When overridden in a derived class, releases the unmanaged resources used by the <see cref="T:System.Threading.WaitHandle" />, and optionally releases the managed resources.</summary>
		/// <param name="explicitDisposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool explicitDisposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (this.safe_wait_handle == null)
				{
					return;
				}
				lock (this)
				{
					if (this.safe_wait_handle != null)
					{
						this.safe_wait_handle.Dispose();
					}
				}
			}
		}

		/// <summary>Gets or sets the native operating system handle.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.SafeHandles.SafeWaitHandle" /> representing the native operating system handle.</returns>
		public SafeWaitHandle SafeWaitHandle
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				return this.safe_wait_handle;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				if (value == null)
				{
					this.safe_wait_handle = new SafeWaitHandle(WaitHandle.InvalidHandle, false);
				}
				else
				{
					this.safe_wait_handle = value;
				}
			}
		}

		/// <summary>Signals one wait handle and waits on another.</summary>
		/// <returns>true if both the signal and the wait complete successfully; if the wait does not complete, the method does not return.</returns>
		/// <param name="toSignal">The wait handle to signal.</param>
		/// <param name="toWaitOn">The wait handle to wait on.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="toSignal" /> is null.-or-<paramref name="toWaitOn" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The method was called on a thread that has <see cref="T:System.STAThreadAttribute" />. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">This method is not supported on Windows 98 or Windows Millennium Edition. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="toSignal" /> is a semaphore, and it already has a full count. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn)
		{
			return WaitHandle.SignalAndWait(toSignal, toWaitOn, -1, false);
		}

		/// <summary>Signals one wait handle and waits on another, specifying a time-out interval as a 32-bit signed integer and specifying whether to exit the synchronization domain for the context before entering the wait.</summary>
		/// <returns>true if both the signal and the wait completed successfully, or false if the signal completed but the wait timed out.</returns>
		/// <param name="toSignal">The wait handle to signal.</param>
		/// <param name="toWaitOn">The wait handle to wait on.</param>
		/// <param name="millisecondsTimeout">An integer that represents the interval to wait. If the value is <see cref="F:System.Threading.Timeout.Infinite" />, that is, -1, the wait is infinite.</param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="toSignal" /> is null.-or-<paramref name="toWaitOn" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The method is called on a thread that has <see cref="T:System.STAThreadAttribute" />. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">This method is not supported on Windows 98 or Windows Millennium Edition. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="toSignal" /> is a semaphore, and it already has a full count. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Threading.WaitHandle" /> cannot be signaled because it would exceed its maximum count.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, int millisecondsTimeout, bool exitContext)
		{
			if (toSignal == null)
			{
				throw new ArgumentNullException("toSignal");
			}
			if (toWaitOn == null)
			{
				throw new ArgumentNullException("toWaitOn");
			}
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			return WaitHandle.SignalAndWait_Internal(toSignal.Handle, toWaitOn.Handle, millisecondsTimeout, exitContext);
		}

		/// <summary>Signals one wait handle and waits on another, specifying the time-out interval as a <see cref="T:System.TimeSpan" /> and specifying whether to exit the synchronization domain for the context before entering the wait.</summary>
		/// <returns>true if both the signal and the wait completed successfully, or false if the signal completed but the wait timed out.</returns>
		/// <param name="toSignal">The wait handle to signal.</param>
		/// <param name="toWaitOn">The wait handle to wait on.</param>
		/// <param name="timeout">The interval to wait. If the value is -1, the wait is infinite.</param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="toSignal" /> is null.-or-<paramref name="toWaitOn" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The method was called on a thread that has <see cref="T:System.STAThreadAttribute" />. </exception>
		/// <exception cref="T:System.PlatformNotSupportedException">This method is not supported on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="toSignal" /> is a semaphore, and it already has a full count. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> evaluates to a negative number of milliseconds other than -1. -or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool SignalAndWait(WaitHandle toSignal, WaitHandle toWaitOn, TimeSpan timeout, bool exitContext)
		{
			double totalMilliseconds = timeout.TotalMilliseconds;
			if (totalMilliseconds > 2147483647.0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return WaitHandle.SignalAndWait(toSignal, toWaitOn, Convert.ToInt32(totalMilliseconds), false);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SignalAndWait_Internal(IntPtr toSignal, IntPtr toWaitOn, int ms, bool exitContext);

		/// <summary>Blocks the current thread until the current wait handle receives a signal.</summary>
		/// <returns>true if the current instance receives a signal. If the current instance is never signaled, <see cref="M:System.Threading.WaitHandle.WaitOne(System.Int32,System.Boolean)" /> never returns.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual bool WaitOne()
		{
			this.CheckDisposed();
			bool flag = false;
			bool result;
			try
			{
				this.safe_wait_handle.DangerousAddRef(ref flag);
				result = this.WaitOne_internal(this.safe_wait_handle.DangerousGetHandle(), -1, false);
			}
			finally
			{
				if (flag)
				{
					this.safe_wait_handle.DangerousRelease();
				}
			}
			return result;
		}

		/// <summary>Blocks the current thread until the current wait handle receives a signal, using a 32-bit signed integer to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely. </param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual bool WaitOne(int millisecondsTimeout, bool exitContext)
		{
			this.CheckDisposed();
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			bool flag = false;
			bool result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				this.safe_wait_handle.DangerousAddRef(ref flag);
				result = this.WaitOne_internal(this.safe_wait_handle.DangerousGetHandle(), millisecondsTimeout, exitContext);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
				if (flag)
				{
					this.safe_wait_handle.DangerousRelease();
				}
			}
			return result;
		}

		/// <summary>Blocks the current thread until the current wait handle receives a signal, using a 32-bit signed integer to specify the time interval.</summary>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely. </param>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		public virtual bool WaitOne(int millisecondsTimeout)
		{
			return this.WaitOne(millisecondsTimeout, false);
		}

		/// <summary>Blocks the current thread until the current instance receives a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval.</summary>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely. </param>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.-or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		public virtual bool WaitOne(TimeSpan timeout)
		{
			return this.WaitOne(timeout, false);
		}

		/// <summary>Blocks the current thread until the current instance receives a signal, using a <see cref="T:System.TimeSpan" /> to specify the time interval and specifying whether to exit the synchronization domain before the wait.</summary>
		/// <returns>true if the current instance receives a signal; otherwise, false.</returns>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely. </param>
		/// <param name="exitContext">true to exit the synchronization domain for the context before the wait (if in a synchronized context), and reacquire it afterward; otherwise, false. </param>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.-or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current instance is a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual bool WaitOne(TimeSpan timeout, bool exitContext)
		{
			this.CheckDisposed();
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			bool flag = false;
			bool result;
			try
			{
				if (exitContext)
				{
					SynchronizationAttribute.ExitContext();
				}
				this.safe_wait_handle.DangerousAddRef(ref flag);
				result = this.WaitOne_internal(this.safe_wait_handle.DangerousGetHandle(), (int)num, exitContext);
			}
			finally
			{
				if (exitContext)
				{
					SynchronizationAttribute.EnterContext();
				}
				if (flag)
				{
					this.safe_wait_handle.DangerousRelease();
				}
			}
			return result;
		}

		internal void CheckDisposed()
		{
			if (this.disposed || this.safe_wait_handle == null)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		/// <summary>Waits for all the elements in the specified array to receive a signal, using an <see cref="T:System.Int32" /> value to specify the time interval.</summary>
		/// <returns>true when every element in <paramref name="waitHandles" /> has received a signal; otherwise, false.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object (duplicates). </param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null.-or- One or more of the objects in the <paramref name="waitHandles" /> array is null. -or-<paramref name="waitHandles" /> is an array with no elements. </exception>
		/// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.-or- The <see cref="T:System.STAThreadAttribute" /> attribute is applied to the thread procedure for the current thread, and <paramref name="waitHandles" /> contains more than one element. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out. </exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait completed because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout)
		{
			return WaitHandle.WaitAll(waitHandles, millisecondsTimeout, false);
		}

		/// <summary>Waits for all the elements in the specified array to receive a signal, using a <see cref="T:System.TimeSpan" /> value to specify the time interval.</summary>
		/// <returns>true when every element in <paramref name="waitHandles" /> has received a signal; otherwise, false.</returns>
		/// <param name="waitHandles">A WaitHandle array containing the objects for which the current instance will wait. This array cannot contain multiple references to the same object. </param>
		/// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds, to wait indefinitely. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="waitHandles" /> parameter is null. -or- One or more of the objects in the <paramref name="waitHandles" /> array is null. -or-<paramref name="waitHandles" /> is an array with no elements. </exception>
		/// <exception cref="T:System.DuplicateWaitObjectException">The <paramref name="waitHandles" /> array contains elements that are duplicates. </exception>
		/// <exception cref="T:System.NotSupportedException">The number of objects in <paramref name="waitHandles" /> is greater than the system permits.-or- The <see cref="T:System.STAThreadAttribute" /> attribute is applied to the thread procedure for the current thread, and <paramref name="waitHandles" /> contains more than one element. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-<paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Threading.AbandonedMutexException">The wait terminated because a thread exited without releasing a mutex. This exception is not thrown on Windows 98 or Windows Millennium Edition.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <paramref name="waitHandles" /> array contains a transparent proxy for a <see cref="T:System.Threading.WaitHandle" /> in another application domain.</exception>
		public static bool WaitAll(WaitHandle[] waitHandles, TimeSpan timeout)
		{
			return WaitHandle.WaitAll(waitHandles, timeout, false);
		}

		/// <summary>Releases the resources held by the current instance.</summary>
		~WaitHandle()
		{
			this.Dispose(false);
		}
	}
}
