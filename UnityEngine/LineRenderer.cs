using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The line renderer is used to draw free-floating lines in 3D space.</para>
	/// </summary>
	public sealed class LineRenderer : Renderer
	{
		/// <summary>
		///   <para>Set the line width at the start and at the end.</para>
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void SetWidth(float start, float end)
		{
			LineRenderer.INTERNAL_CALL_SetWidth(this, start, end);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetWidth(LineRenderer self, float start, float end);

		/// <summary>
		///   <para>Set the line color at the start and at the end.</para>
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void SetColors(Color start, Color end)
		{
			LineRenderer.INTERNAL_CALL_SetColors(this, ref start, ref end);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetColors(LineRenderer self, ref Color start, ref Color end);

		/// <summary>
		///   <para>Set the number of line segments.</para>
		/// </summary>
		/// <param name="count"></param>
		public void SetVertexCount(int count)
		{
			LineRenderer.INTERNAL_CALL_SetVertexCount(this, count);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetVertexCount(LineRenderer self, int count);

		/// <summary>
		///   <para>Set the position of the vertex in the line.</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="position"></param>
		public void SetPosition(int index, Vector3 position)
		{
			LineRenderer.INTERNAL_CALL_SetPosition(this, index, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetPosition(LineRenderer self, int index, ref Vector3 position);

		/// <summary>
		///   <para>If enabled, the lines are defined in world space.</para>
		/// </summary>
		public extern bool useWorldSpace { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
