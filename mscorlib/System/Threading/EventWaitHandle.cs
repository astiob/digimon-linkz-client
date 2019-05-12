using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>Represents a thread synchronization event.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public class EventWaitHandle : WaitHandle
	{
		private EventWaitHandle(IntPtr handle)
		{
			this.Handle = handle;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.EventWaitHandle" /> class, specifying whether the wait handle is initially signaled, and whether it resets automatically or manually.</summary>
		/// <param name="initialState">true to set the initial state to signaled; false to set it to nonsignaled.</param>
		/// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
		public EventWaitHandle(bool initialState, EventResetMode mode)
		{
			bool manual = this.IsManualReset(mode);
			bool flag;
			this.Handle = NativeEventCalls.CreateEvent_internal(manual, initialState, null, out flag);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.EventWaitHandle" /> class, specifying whether the wait handle is initially signaled if created as a result of this call, whether it resets automatically or manually, and the name of a system synchronization event.</summary>
		/// <param name="initialState">true to set the initial state to signaled if the named event is created as a result of this call; false to set it to nonsignaled.</param>
		/// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
		/// <param name="name">The name of a system-wide synchronization event.</param>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named event exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.EventWaitHandleRights.FullControl" />.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named event cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than 260 characters.</exception>
		public EventWaitHandle(bool initialState, EventResetMode mode, string name)
		{
			bool manual = this.IsManualReset(mode);
			bool flag;
			this.Handle = NativeEventCalls.CreateEvent_internal(manual, initialState, name, out flag);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.EventWaitHandle" /> class, specifying whether the wait handle is initially signaled if created as a result of this call, whether it resets automatically or manually, the name of a system synchronization event, and a Boolean variable whose value after the call indicates whether the named system event was created.</summary>
		/// <param name="initialState">true to set the initial state to signaled if the named event is created as a result of this call; false to set it to nonsignaled.</param>
		/// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
		/// <param name="name">The name of a system-wide synchronization event.</param>
		/// <param name="createdNew">When this method returns, contains true if a local event was created (that is, if <paramref name="name" /> is null or an empty string) or if the specified named system event was created; false if the specified named system event already existed. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named event exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.EventWaitHandleRights.FullControl" />.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named event cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than 260 characters.</exception>
		public EventWaitHandle(bool initialState, EventResetMode mode, string name, out bool createdNew)
		{
			bool manual = this.IsManualReset(mode);
			this.Handle = NativeEventCalls.CreateEvent_internal(manual, initialState, name, out createdNew);
		}

		private bool IsManualReset(EventResetMode mode)
		{
			if (mode < EventResetMode.AutoReset || mode > EventResetMode.ManualReset)
			{
				throw new ArgumentException("mode");
			}
			return mode == EventResetMode.ManualReset;
		}

		/// <summary>Sets the state of the event to nonsignaled, causing threads to block.</summary>
		/// <returns>true if the operation succeeds; otherwise, false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="M:System.Threading.EventWaitHandle.Close" /> method was previously called on this <see cref="T:System.Threading.EventWaitHandle" />.</exception>
		/// <filterpriority>2</filterpriority>
		public bool Reset()
		{
			base.CheckDisposed();
			return NativeEventCalls.ResetEvent_internal(this.Handle);
		}

		/// <summary>Sets the state of the event to signaled, allowing one or more waiting threads to proceed.</summary>
		/// <returns>true if the operation succeeds; otherwise, false.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="M:System.Threading.EventWaitHandle.Close" /> method was previously called on this <see cref="T:System.Threading.EventWaitHandle" />.</exception>
		/// <filterpriority>2</filterpriority>
		public bool Set()
		{
			base.CheckDisposed();
			return NativeEventCalls.SetEvent_internal(this.Handle);
		}
	}
}
