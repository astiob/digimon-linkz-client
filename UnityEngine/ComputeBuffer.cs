using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine
{
	/// <summary>
	///   <para>Data buffer to hold data for compute shaders.</para>
	/// </summary>
	public sealed class ComputeBuffer : IDisposable
	{
		internal IntPtr m_Ptr;

		/// <summary>
		///   <para>Create a Compute Buffer.</para>
		/// </summary>
		/// <param name="count">Number of elements in the buffer.</param>
		/// <param name="stride">Size of one element in the buffer. Has to match size of buffer type in the shader. See for cross-platform compatibility information.</param>
		/// <param name="type">Type of the buffer, default is ComputeBufferType.Default.</param>
		public ComputeBuffer(int count, int stride) : this(count, stride, ComputeBufferType.Default)
		{
		}

		/// <summary>
		///   <para>Create a Compute Buffer.</para>
		/// </summary>
		/// <param name="count">Number of elements in the buffer.</param>
		/// <param name="stride">Size of one element in the buffer. Has to match size of buffer type in the shader. See for cross-platform compatibility information.</param>
		/// <param name="type">Type of the buffer, default is ComputeBufferType.Default.</param>
		public ComputeBuffer(int count, int stride, ComputeBufferType type)
		{
			this.m_Ptr = IntPtr.Zero;
			ComputeBuffer.InitBuffer(this, count, stride, type);
		}

		~ComputeBuffer()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			ComputeBuffer.DestroyBuffer(this);
			this.m_Ptr = IntPtr.Zero;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitBuffer(ComputeBuffer buf, int count, int stride, ComputeBufferType type);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyBuffer(ComputeBuffer buf);

		/// <summary>
		///   <para>Release a Compute Buffer.</para>
		/// </summary>
		public void Release()
		{
			this.Dispose();
		}

		/// <summary>
		///   <para>Number of elements in the buffer (Read Only).</para>
		/// </summary>
		public extern int count { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Size of one element in the buffer (Read Only).</para>
		/// </summary>
		public extern int stride { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Set the buffer with values from an array.</para>
		/// </summary>
		/// <param name="data">Array of values to fill the buffer.</param>
		[SecuritySafeCritical]
		public void SetData(Array data)
		{
			this.InternalSetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		[SecurityCritical]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetData(Array data, int elemSize);

		/// <summary>
		///   <para>Read data values from the buffer into an array.</para>
		/// </summary>
		/// <param name="data">An array to receive the data.</param>
		[SecuritySafeCritical]
		public void GetData(Array data)
		{
			this.InternalGetData(data, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		[SecurityCritical]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalGetData(Array data, int elemSize);

		/// <summary>
		///   <para>Copy counter value of append/consume buffer into another buffer.</para>
		/// </summary>
		/// <param name="src">Append/consume buffer to copy the counter from.</param>
		/// <param name="dst">A buffer to copy the counter to.</param>
		/// <param name="dstOffset">Target byte offset in dst.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CopyCount(ComputeBuffer src, ComputeBuffer dst, int dstOffset);
	}
}
