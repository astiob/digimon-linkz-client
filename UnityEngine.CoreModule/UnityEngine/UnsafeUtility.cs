using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Collections;

namespace UnityEngine
{
	[NativeHeader("Runtime/Export/Unsafe/UnsafeUtility.bindings.h")]
	[StaticAccessor("UnsafeUtility", StaticAccessorType.DoubleColon)]
	internal static class UnsafeUtility
	{
		public unsafe static void CopyPtrToStructure<T>(IntPtr ptr, out T output) where T : struct
		{
			output = *ptr;
		}

		public unsafe static void CopyStructureToPtr<T>(ref T output, IntPtr ptr) where T : struct
		{
			*ptr = output;
		}

		public unsafe static T ReadArrayElement<T>(IntPtr source, int index)
		{
			return *(source + (IntPtr)(index * sizeof(T)));
		}

		public unsafe static T ReadArrayElementWithStride<T>(IntPtr source, int index, int stride)
		{
			return *(source + (IntPtr)(index * stride));
		}

		public unsafe static void WriteArrayElement<T>(IntPtr destination, int index, T value)
		{
			*(destination + (IntPtr)(index * sizeof(T))) = value;
		}

		public unsafe static void WriteArrayElementWithStride<T>(IntPtr destination, int index, int stride, T value)
		{
			*(destination + (IntPtr)(index * stride)) = value;
		}

		public static IntPtr AddressOf<T>(ref T output) where T : struct
		{
			return ref output;
		}

		public static int SizeOf<T>() where T : struct
		{
			return sizeof(T);
		}

		public static int AlignOf<T>() where T : struct
		{
			return 4;
		}

		public static int OffsetOf<T>(string name) where T : struct
		{
			return (int)Marshal.OffsetOf(typeof(T), name);
		}

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr Malloc(int size, int alignment, Allocator label);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Free(IntPtr memory, Allocator label);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemCpy(IntPtr destination, IntPtr source, int size);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemMove(IntPtr destination, IntPtr source, int size);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemClear(IntPtr destination, int size);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int SizeOfStruct(Type type);

		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogError(string msg, string filename, int linenumber);
	}
}
