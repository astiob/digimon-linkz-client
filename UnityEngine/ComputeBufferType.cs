using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>ComputeBuffer type.</para>
	/// </summary>
	[Flags]
	public enum ComputeBufferType
	{
		/// <summary>
		///   <para>Default ComputeBuffer type.</para>
		/// </summary>
		Default = 0,
		/// <summary>
		///   <para>Raw ComputeBuffer type.</para>
		/// </summary>
		Raw = 1,
		/// <summary>
		///   <para>Append-consume ComputeBuffer type.</para>
		/// </summary>
		Append = 2,
		/// <summary>
		///   <para>ComputeBuffer with a counter.</para>
		/// </summary>
		Counter = 4,
		/// <summary>
		///   <para>ComputeBuffer used for Graphics.DrawProceduralIndirect.</para>
		/// </summary>
		DrawIndirect = 256
	}
}
