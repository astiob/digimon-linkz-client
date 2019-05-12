using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	[NativeType(Header = "Runtime/Jobs/AtomicSafetyHandle.h")]
	internal struct AtomicSafetyHandle
	{
		internal IntPtr versionNode;

		internal AtomicSafetyHandleVersionMask version;

		[ThreadSafe]
		public static AtomicSafetyHandle Create()
		{
			AtomicSafetyHandle result;
			AtomicSafetyHandle.Create_Injected(out result);
			return result;
		}

		[ThreadSafe]
		public static void Release(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.Release_Injected(ref handle);
		}

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PrepareUndisposable(ref AtomicSafetyHandle handle);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UseSecondaryVersion(ref AtomicSafetyHandle handle);

		[ThreadSafe]
		public static void CheckWriteAndBumpSecondaryVersion(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion_Injected(ref handle);
		}

		[ThreadSafe]
		public static void EnforceAllBufferJobsHaveCompletedAndRelease(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.EnforceAllBufferJobsHaveCompletedAndRelease_Injected(ref handle);
		}

		[ThreadSafe]
		internal static void CheckReadAndThrowNoEarlyOut(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut_Injected(ref handle);
		}

		[ThreadSafe]
		internal static void CheckWriteAndThrowNoEarlyOut(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut_Injected(ref handle);
		}

		[ThreadSafe]
		public static void CheckDeallocateAndThrow(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.CheckDeallocateAndThrow_Injected(ref handle);
		}

		[NativeMethod(IsThreadSafe = true)]
		public static void CheckGetSecondaryDataPointerAndThrow(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandle.CheckGetSecondaryDataPointerAndThrow_Injected(ref handle);
		}

		public unsafe static void CheckReadAndThrow(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)handle.versionNode);
			if ((handle.version & AtomicSafetyHandleVersionMask.Read) == (AtomicSafetyHandleVersionMask)0 && handle.version != (*ptr & AtomicSafetyHandleVersionMask.WriteInv))
			{
				AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(handle);
			}
		}

		public unsafe static void CheckWriteAndThrow(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)handle.versionNode);
			if ((handle.version & AtomicSafetyHandleVersionMask.Write) == (AtomicSafetyHandleVersionMask)0 && handle.version != (*ptr & AtomicSafetyHandleVersionMask.ReadInv))
			{
				AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut(handle);
			}
		}

		public unsafe static void CheckExistsAndThrow(AtomicSafetyHandle handle)
		{
			AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)handle.versionNode);
			if ((handle.version & AtomicSafetyHandleVersionMask.ReadWriteAndDisposeInv) != (*ptr & AtomicSafetyHandleVersionMask.ReadWriteAndDisposeInv))
			{
				throw new InvalidOperationException("The NativeArray has been deallocated, it is not allowed to access it");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Create_Injected(out AtomicSafetyHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release_Injected(ref AtomicSafetyHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CheckWriteAndBumpSecondaryVersion_Injected(ref AtomicSafetyHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnforceAllBufferJobsHaveCompletedAndRelease_Injected(ref AtomicSafetyHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CheckReadAndThrowNoEarlyOut_Injected(ref AtomicSafetyHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CheckWriteAndThrowNoEarlyOut_Injected(ref AtomicSafetyHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CheckDeallocateAndThrow_Injected(ref AtomicSafetyHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CheckGetSecondaryDataPointerAndThrow_Injected(ref AtomicSafetyHandle handle);
	}
}
