using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Compute Shader asset.</para>
	/// </summary>
	public sealed class ComputeShader : Object
	{
		/// <summary>
		///   <para>Find ComputeShader kernel index.</para>
		/// </summary>
		/// <param name="name">Name of kernel function.</param>
		/// <returns>
		///   <para>Kernel index, or -1 if not found.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int FindKernel(string name);

		/// <summary>
		///   <para>Set a float parameter.</para>
		/// </summary>
		/// <param name="name">Variable name in shader code.</param>
		/// <param name="val">Value to set.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(string name, float val);

		/// <summary>
		///   <para>Set an integer parameter.</para>
		/// </summary>
		/// <param name="name">Variable name in shader code.</param>
		/// <param name="val">Value to set.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInt(string name, int val);

		/// <summary>
		///   <para>Set a vector parameter.</para>
		/// </summary>
		/// <param name="name">Variable name in shader code.</param>
		/// <param name="val">Value to set.</param>
		public void SetVector(string name, Vector4 val)
		{
			ComputeShader.INTERNAL_CALL_SetVector(this, name, ref val);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetVector(ComputeShader self, string name, ref Vector4 val);

		/// <summary>
		///   <para>Set multiple consecutive float parameters at once.</para>
		/// </summary>
		/// <param name="name">Array variable name in the shader code.</param>
		/// <param name="values">Value array to set.</param>
		public void SetFloats(string name, params float[] values)
		{
			this.Internal_SetFloats(name, values);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetFloats(string name, float[] values);

		/// <summary>
		///   <para>Set multiple consecutive integer parameters at once.</para>
		/// </summary>
		/// <param name="name">Array variable name in the shader code.</param>
		/// <param name="values">Value array to set.</param>
		public void SetInts(string name, params int[] values)
		{
			this.Internal_SetInts(name, values);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetInts(string name, int[] values);

		/// <summary>
		///   <para>Set a texture parameter.</para>
		/// </summary>
		/// <param name="kernelIndex">For which kernel the texture is being set. See FindKernel.</param>
		/// <param name="name">Name of the buffer variable in shader code.</param>
		/// <param name="texture">Texture to set.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTexture(int kernelIndex, string name, Texture texture);

		/// <summary>
		///   <para>Sets an input or output compute buffer.</para>
		/// </summary>
		/// <param name="kernelIndex">For which kernel the buffer is being set. See FindKernel.</param>
		/// <param name="name">Name of the buffer variable in shader code.</param>
		/// <param name="buffer">Buffer to set.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBuffer(int kernelIndex, string name, ComputeBuffer buffer);

		/// <summary>
		///   <para>Execute a compute shader.</para>
		/// </summary>
		/// <param name="kernelIndex">Which kernel to execute. A single compute shader asset can have multiple kernel entry points.</param>
		/// <param name="threadsX">Work size in X dimension.</param>
		/// <param name="threadsY">Work size in Y dimension.</param>
		/// <param name="threadsZ">Work size in Z dimension.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispatch(int kernelIndex, int threadsX, int threadsY, int threadsZ);
	}
}
