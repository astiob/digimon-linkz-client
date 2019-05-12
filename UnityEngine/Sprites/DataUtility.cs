using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Sprites
{
	/// <summary>
	///   <para>Helper utilities for accessing Sprite data.</para>
	/// </summary>
	public sealed class DataUtility
	{
		/// <summary>
		///   <para>Inner UV's of the Sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector4 GetInnerUV(Sprite sprite);

		/// <summary>
		///   <para>Outer UV's of the Sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector4 GetOuterUV(Sprite sprite);

		/// <summary>
		///   <para>Return the padding on the sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector4 GetPadding(Sprite sprite);

		/// <summary>
		///   <para>Minimum width and height of the Sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		public static Vector2 GetMinSize(Sprite sprite)
		{
			Vector2 result;
			DataUtility.Internal_GetMinSize(sprite, out result);
			return result;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetMinSize(Sprite sprite, out Vector2 output);
	}
}
