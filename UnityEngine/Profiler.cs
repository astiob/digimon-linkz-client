using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Controls the from script.</para>
	/// </summary>
	public sealed class Profiler
	{
		public static extern bool supported { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Sets profiler output file in built players.</para>
		/// </summary>
		public static extern string logFile { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets profiler output file in built players.</para>
		/// </summary>
		public static extern bool enableBinaryLog { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Enables the Profiler.</para>
		/// </summary>
		public static extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Displays the recorded profiledata in the profiler.</para>
		/// </summary>
		/// <param name="file"></param>
		[Conditional("ENABLE_PROFILER")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void AddFramesFromFile(string file);

		/// <summary>
		///   <para>Begin profiling a piece of code with a custom label.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="targetObject"></param>
		[Conditional("ENABLE_PROFILER")]
		public static void BeginSample(string name)
		{
			Profiler.BeginSampleOnly(name);
		}

		/// <summary>
		///   <para>Begin profiling a piece of code with a custom label.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="targetObject"></param>
		[WrapperlessIcall]
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginSample(string name, Object targetObject);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BeginSampleOnly(string name);

		/// <summary>
		///   <para>End profiling a piece of code with a custom label.</para>
		/// </summary>
		[Conditional("ENABLE_PROFILER")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndSample();

		/// <summary>
		///   <para>Resize the profiler sample buffers to allow the desired amount of samples per thread.</para>
		/// </summary>
		public static extern int maxNumberOfSamplesPerFrame { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Heap size used by the program.</para>
		/// </summary>
		/// <returns>
		///   <para>Size of the used heap in bytes, (or 0 if the profiler is disabled).</para>
		/// </returns>
		public static extern uint usedHeapSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the runtime memory usage of the resource.</para>
		/// </summary>
		/// <param name="o"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetRuntimeMemorySize(Object o);

		/// <summary>
		///   <para>Returns the size of the mono heap.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetMonoHeapSize();

		/// <summary>
		///   <para>Returns the used size from mono.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetMonoUsedSize();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetTotalAllocatedMemory();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetTotalUnusedReservedMemory();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetTotalReservedMemory();
	}
}
