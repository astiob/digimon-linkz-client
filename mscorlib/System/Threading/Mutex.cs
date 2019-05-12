using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>A synchronization primitive that can also be used for interprocess synchronization. </summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	public sealed class Mutex : WaitHandle
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with default properties.</summary>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex()
		{
			bool flag;
			this.Handle = Mutex.CreateMutex_internal(false, null, out flag);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with a Boolean value that indicates whether the calling thread should have initial ownership of the mutex.</summary>
		/// <param name="initiallyOwned">true to give the calling thread initial ownership of the mutex; otherwise, false. </param>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex(bool initiallyOwned)
		{
			bool flag;
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, null, out flag);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with a Boolean value that indicates whether the calling thread should have initial ownership of the mutex, and a string that is the name of the mutex.</summary>
		/// <param name="initiallyOwned">true to give the calling thread initial ownership of the named system mutex if the named system mutex is created as a result of this call; otherwise, false. </param>
		/// <param name="name">The name of the <see cref="T:System.Threading.Mutex" />. If the value is null, the <see cref="T:System.Threading.Mutex" /> is unnamed. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl" />.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.ApplicationException">The named mutex cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than 260 characters.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex(bool initiallyOwned, string name)
		{
			bool flag;
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, name, out flag);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with a Boolean value that indicates whether the calling thread should have initial ownership of the mutex, a string that is the name of the mutex, and a Boolean value that, when the method returns, indicates whether the calling thread was granted initial ownership of the mutex.</summary>
		/// <param name="initiallyOwned">true to give the calling thread initial ownership of the named system mutex if the named system mutex is created as a result of this call; otherwise, false. </param>
		/// <param name="name">The name of the <see cref="T:System.Threading.Mutex" />. If the value is null, the <see cref="T:System.Threading.Mutex" /> is unnamed. </param>
		/// <param name="createdNew">When this method returns, contains a Boolean that is true if a local mutex was created (that is, if <paramref name="name" /> is null or an empty string) or if the specified named system mutex was created; false if the specified named system mutex already existed. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl" />.</exception>
		/// <exception cref="T:System.IO.IOException">A Win32 error occurred.</exception>
		/// <exception cref="T:System.ApplicationException">The named mutex cannot be created, perhaps because a wait handle of a different type has the same name.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is longer than 260 characters.</exception>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Mutex(bool initiallyOwned, string name, out bool createdNew)
		{
			this.Handle = Mutex.CreateMutex_internal(initiallyOwned, name, out createdNew);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateMutex_internal(bool initiallyOwned, string name, out bool created);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ReleaseMutex_internal(IntPtr handle);

		/// <summary>Releases the <see cref="T:System.Threading.Mutex" /> once.</summary>
		/// <exception cref="T:System.ApplicationException">The calling thread does not own the mutex. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void ReleaseMutex()
		{
			if (!Mutex.ReleaseMutex_internal(this.Handle))
			{
				throw new ApplicationException("Mutex is not owned");
			}
		}
	}
}
