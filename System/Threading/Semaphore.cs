using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Threading
{
	/// <summary>Limits the number of threads that can access a resource or pool of resources concurrently. </summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(false)]
	public sealed class Semaphore : WaitHandle
	{
		private Semaphore(IntPtr handle)
		{
			this.Handle = handle;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the maximum number of concurrent entries and optionally reserving some entries.</summary>
		/// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="maximumCount" /> is less than 1.-or-<paramref name="initialCount" /> is less than 0.</exception>
		public Semaphore(int initialCount, int maximumCount) : this(initialCount, maximumCount, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the maximum number of concurrent entries, optionally reserving some entries for the calling thread, and optionally specifying the name of a system semaphore object.</summary>
		/// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently. </param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
		/// <param name="name">The name of a named system semaphore object.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />.-or-<paramref name="name" /> is longer than 260 characters.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="maximumCount" /> is less than 1.-or-<paramref name="initialCount" /> is less than 0.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists and has access control security, and the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.FullControl" />.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named semaphore cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		public Semaphore(int initialCount, int maximumCount, string name)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", "< 0");
			}
			if (maximumCount < 1)
			{
				throw new ArgumentOutOfRangeException("maximumCount", "< 1");
			}
			if (initialCount > maximumCount)
			{
				throw new ArgumentException("initialCount > maximumCount");
			}
			bool flag;
			this.Handle = Semaphore.CreateSemaphore_internal(initialCount, maximumCount, name, out flag);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the maximum number of concurrent entries, optionally reserving some entries for the calling thread, optionally specifying the name of a system semaphore object, and specifying a variable that receives a value indicating whether a new system semaphore was created.</summary>
		/// <param name="initialCount">The initial number of requests for the semaphore that can be satisfied concurrently. </param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="name">The name of a named system semaphore object.</param>
		/// <param name="createdNew">When this method returns, contains true if a local semaphore was created (that is, if <paramref name="name" /> is null or an empty string) or if the specified named system semaphore was created; false if the specified named system semaphore already existed. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />. -or-<paramref name="name" /> is longer than 260 characters.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="maximumCount" /> is less than 1.-or-<paramref name="initialCount" /> is less than 0.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists and has access control security, and the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.FullControl" />.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named semaphore cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew) : this(initialCount, maximumCount, name, out createdNew, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the maximum number of concurrent entries, optionally reserving some entries for the calling thread, optionally specifying the name of a system semaphore object, specifying a variable that receives a value indicating whether a new system semaphore was created, and specifying security access control for the system semaphore.</summary>
		/// <param name="initialCount">The initial number of requests for the semaphore that can be satisfied concurrently. </param>
		/// <param name="maximumCount">The maximum number of requests for the semaphore that can be satisfied concurrently.</param>
		/// <param name="name">The name of a named system semaphore object.</param>
		/// <param name="createdNew">When this method returns, contains true if a local semaphore was created (that is, if <paramref name="name" /> is null or an empty string) or if the specified named system semaphore was created; false if the specified named system semaphore already existed. This parameter is passed uninitialized.</param>
		/// <param name="semaphoreSecurity">A <see cref="T:System.Security.AccessControl.SemaphoreSecurity" />  object that represents the access control security to be applied to the named system semaphore.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />.-or-<paramref name="name" /> is longer than 260 characters.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="maximumCount" /> is less than 1.-or-<paramref name="initialCount" /> is less than 0.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists and has access control security, and the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.FullControl" />.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named semaphore cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		[MonoTODO("Does not support access control, semaphoreSecurity is ignored")]
		public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew, System.Security.AccessControl.SemaphoreSecurity semaphoreSecurity)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount", "< 0");
			}
			if (maximumCount < 1)
			{
				throw new ArgumentOutOfRangeException("maximumCount", "< 1");
			}
			if (initialCount > maximumCount)
			{
				throw new ArgumentException("initialCount > maximumCount");
			}
			this.Handle = Semaphore.CreateSemaphore_internal(initialCount, maximumCount, name, out createdNew);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateSemaphore_internal(int initialCount, int maximumCount, string name, out bool createdNew);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ReleaseSemaphore_internal(IntPtr handle, int releaseCount, out bool fail);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr OpenSemaphore_internal(string name, System.Security.AccessControl.SemaphoreRights rights, out MonoIOError error);

		/// <summary>Gets the access control security for a named system semaphore.</summary>
		/// <returns>A <see cref="T:System.Security.AccessControl.SemaphoreSecurity" /> object that represents the access control security for the named system semaphore.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">The current <see cref="T:System.Threading.Semaphore" /> object represents a named system semaphore, and the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.ReadPermissions" /> rights.-or-The current <see cref="T:System.Threading.Semaphore" /> object represents a named system semaphore and was not opened with <see cref="F:System.Security.AccessControl.SemaphoreRights.ReadPermissions" /> rights.</exception>
		/// <exception cref="T:System.NotSupportedException">Not supported for Windows 98 or Windows Millennium Edition.</exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO]
		public System.Security.AccessControl.SemaphoreSecurity GetAccessControl()
		{
			throw new NotImplementedException();
		}

		/// <summary>Exits the semaphore and returns the previous count.</summary>
		/// <returns>The count on the semaphore before the <see cref="Overload:System.Threading.Semaphore.Release" /> method was called. </returns>
		/// <exception cref="T:System.Threading.SemaphoreFullException">The semaphore count is already at the maximum value.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred with a named semaphore.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The current semaphore represents a named system semaphore, but the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" />.-or-The current semaphore represents a named system semaphore, but it was not opened with <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[PrePrepareMethod]
		public int Release()
		{
			return this.Release(1);
		}

		/// <summary>Exits the semaphore a specified number of times and returns the previous count.</summary>
		/// <returns>The count on the semaphore before the <see cref="Overload:System.Threading.Semaphore.Release" /> method was called. </returns>
		/// <param name="releaseCount">The number of times to exit the semaphore.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="releaseCount" /> is less than 1.</exception>
		/// <exception cref="T:System.Threading.SemaphoreFullException">The semaphore count is already at the maximum value.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred with a named semaphore.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The current semaphore represents a named system semaphore, but the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" /> rights.-or-The current semaphore represents a named system semaphore, but it was not opened with <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" /> rights.</exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int Release(int releaseCount)
		{
			if (releaseCount < 1)
			{
				throw new ArgumentOutOfRangeException("releaseCount");
			}
			bool flag;
			int result = Semaphore.ReleaseSemaphore_internal(this.Handle, releaseCount, out flag);
			if (flag)
			{
				throw new SemaphoreFullException();
			}
			return result;
		}

		/// <summary>Sets the access control security for a named system semaphore.</summary>
		/// <param name="semaphoreSecurity">A <see cref="T:System.Security.AccessControl.SemaphoreSecurity" />  object that represents the access control security to be applied to the named system semaphore.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="semaphoreSecurity" /> is null.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.ChangePermissions" /> rights.-or-The semaphore was not opened with <see cref="F:System.Security.AccessControl.SemaphoreRights.ChangePermissions" /> rights.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see cref="T:System.Threading.Semaphore" /> object does not represent a named system semaphore.</exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO]
		public void SetAccessControl(System.Security.AccessControl.SemaphoreSecurity semaphoreSecurity)
		{
			if (semaphoreSecurity == null)
			{
				throw new ArgumentNullException("semaphoreSecurity");
			}
			throw new NotImplementedException();
		}

		/// <summary>Opens an existing named semaphore.</summary>
		/// <returns>A <see cref="T:System.Threading.Semaphore" /> object that represents a named system semaphore.</returns>
		/// <param name="name">The name of a named system semaphore.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is an empty string.-or-<paramref name="name" /> is longer than 260 characters.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named semaphore does not exist.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists, but the user does not have the security access required to use it.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static Semaphore OpenExisting(string name)
		{
			return Semaphore.OpenExisting(name, System.Security.AccessControl.SemaphoreRights.Modify | System.Security.AccessControl.SemaphoreRights.Synchronize);
		}

		/// <summary>Opens an existing named semaphore, specifying the desired security access rights.</summary>
		/// <returns>A <see cref="T:System.Threading.Semaphore" /> object that represents the named system semaphore.</returns>
		/// <param name="name">The name of a system semaphore.</param>
		/// <param name="rights">A bitwise combination of the <see cref="T:System.Security.AccessControl.SemaphoreRights" />  values that represent the desired security access rights.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is an empty string.-or-<paramref name="name" /> is longer than 260 characters.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">The named semaphore does not exist.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists, but the user does not have the desired security access rights.</exception>
		/// <filterpriority>1</filterpriority>
		public static Semaphore OpenExisting(string name, System.Security.AccessControl.SemaphoreRights rights)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0 || name.Length > 260)
			{
				throw new ArgumentException("name", Locale.GetText("Invalid length [1-260]."));
			}
			MonoIOError monoIOError;
			IntPtr intPtr = Semaphore.OpenSemaphore_internal(name, rights, out monoIOError);
			if (!(intPtr == (IntPtr)null))
			{
				return new Semaphore(intPtr);
			}
			if (monoIOError == MonoIOError.ERROR_FILE_NOT_FOUND)
			{
				throw new WaitHandleCannotBeOpenedException(Locale.GetText("Named Semaphore handle does not exist: ") + name);
			}
			if (monoIOError == MonoIOError.ERROR_ACCESS_DENIED)
			{
				throw new UnauthorizedAccessException();
			}
			throw new IOException(Locale.GetText("Win32 IO error: ") + monoIOError.ToString());
		}
	}
}
