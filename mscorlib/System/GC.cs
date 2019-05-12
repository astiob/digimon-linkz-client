using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System
{
	/// <summary>Controls the system garbage collector, a service that automatically reclaims unused memory.</summary>
	/// <filterpriority>2</filterpriority>
	public static class GC
	{
		/// <summary>Gets the maximum number of generations that the system currently supports.</summary>
		/// <returns>A value that ranges from zero to the maximum number of supported generations.</returns>
		/// <filterpriority>1</filterpriority>
		public static extern int MaxGeneration { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalCollect(int generation);

		/// <summary>Forces an immediate garbage collection of all generations. </summary>
		/// <filterpriority>1</filterpriority>
		public static void Collect()
		{
			GC.InternalCollect(GC.MaxGeneration);
		}

		/// <summary>Forces an immediate garbage collection from generation zero through a specified generation.</summary>
		/// <param name="generation">The number of the oldest generation that garbage collection can be performed on. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="generation" /> is not valid. </exception>
		/// <filterpriority>1</filterpriority>
		public static void Collect(int generation)
		{
			if (generation < 0)
			{
				throw new ArgumentOutOfRangeException("generation");
			}
			GC.InternalCollect(generation);
		}

		/// <summary>Forces a garbage collection from generation zero through a specified generation, at a time specified by a <see cref="T:System.GCCollectionMode" /> value.</summary>
		/// <param name="generation">The number of the oldest generation that garbage collection can be performed on. </param>
		/// <param name="mode">One of the <see cref="T:System.GCCollectionMode" /> values.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="generation" /> is not valid.-or-<paramref name="mode" /> is not one of the <see cref="T:System.GCCollectionMode" /> values.</exception>
		[MonoDocumentationNote("mode parameter ignored")]
		public static void Collect(int generation, GCCollectionMode mode)
		{
			GC.Collect(generation);
		}

		/// <summary>Returns the current generation number of the specified object.</summary>
		/// <returns>The current generation number of <paramref name="obj" />.</returns>
		/// <param name="obj">The object that generation information is retrieved for. </param>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetGeneration(object obj);

		/// <summary>Returns the current generation number of the target of a specified weak reference.</summary>
		/// <returns>The current generation number of the target of <paramref name="wo" />.</returns>
		/// <param name="wo">A <see cref="T:System.WeakReference" /> that refers to the target object whose generation number is to be determined. </param>
		/// <exception cref="T:System.ArgumentException">Garbage collection has already been performed on <paramref name="wo" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static int GetGeneration(WeakReference wo)
		{
			object target = wo.Target;
			if (target == null)
			{
				throw new ArgumentException();
			}
			return GC.GetGeneration(target);
		}

		/// <summary>Retrieves the number of bytes currently thought to be allocated. A parameter indicates whether this method can wait a short interval before returning, to allow the system to collect garbage and finalize objects.</summary>
		/// <returns>A number that is the best available approximation of the number of bytes currently allocated in managed memory.</returns>
		/// <param name="forceFullCollection">true to indicate that this method can wait for garbage collection to occur before returning; otherwise, false.</param>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetTotalMemory(bool forceFullCollection);

		/// <summary>References the specified object, which makes it ineligible for garbage collection from the start of the current routine to the point where this method is called.</summary>
		/// <param name="obj">The object to reference. </param>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void KeepAlive(object obj);

		/// <summary>Requests that the system call the finalizer for the specified object for which <see cref="M:System.GC.SuppressFinalize(System.Object)" /> has previously been called.</summary>
		/// <param name="obj">The object that a finalizer must be called for. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="obj" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReRegisterForFinalize(object obj);

		/// <summary>Requests that the system not call the finalizer for the specified object.</summary>
		/// <param name="obj">The object that a finalizer must not be called for. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="obj" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SuppressFinalize(object obj);

		/// <summary>Suspends the current thread until the thread that is processing the queue of finalizers has emptied that queue.</summary>
		/// <filterpriority>1</filterpriority>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WaitForPendingFinalizers();

		/// <summary>Returns the number of times garbage collection has occurred for the specified generation of objects.</summary>
		/// <returns>The number of times garbage collection has occurred for the specified generation since the process was started.</returns>
		/// <param name="generation">The generation of objects for which the garbage collection count is to be determined. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="generation" /> is less than 0. </exception>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CollectionCount(int generation);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RecordPressure(long bytesAllocated);

		/// <summary>Informs the runtime of a large allocation of unmanaged memory that should be taken into account when scheduling garbage collection.</summary>
		/// <param name="bytesAllocated">The incremental amount of unmanaged memory that has been allocated. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bytesAllocated" /> is less than or equal to 0.-or-On a 32-bit computer, <paramref name="bytesAllocated" /> is larger than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void AddMemoryPressure(long bytesAllocated)
		{
			GC.RecordPressure(bytesAllocated);
		}

		/// <summary>Informs the runtime that unmanaged memory has been released and no longer needs to be taken into account when scheduling garbage collection.</summary>
		/// <param name="bytesAllocated">The amount of unmanaged memory that has been released. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bytesAllocated" /> is less than or equal to 0. -or- On a 32-bit computer, <paramref name="bytesAllocated" /> is larger than <see cref="F:System.Int32.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static void RemoveMemoryPressure(long bytesAllocated)
		{
			GC.RecordPressure(-bytesAllocated);
		}
	}
}
