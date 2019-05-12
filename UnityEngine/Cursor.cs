using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Cursor API for setting the cursor that is used for rendering.</para>
	/// </summary>
	public sealed class Cursor
	{
		private static void SetCursor(Texture2D texture, CursorMode cursorMode)
		{
			Cursor.SetCursor(texture, Vector2.zero, cursorMode);
		}

		/// <summary>
		///   <para>Specify a custom cursor that you wish to use as a cursor.</para>
		/// </summary>
		/// <param name="texture">The texture to use for the cursor or null to set the default cursor. Note that a texture needs to be imported with "Read/Write enabled" in the texture importer (or using the "Cursor" defaults), in order to be used as a cursor.</param>
		/// <param name="hotspot">The offset from the top left of the texture to use as the target point (must be within the bounds of the cursor).</param>
		/// <param name="cursorMode">Allow this cursor to render as a hardware cursor on supported platforms, or force software cursor.</param>
		public static void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
		{
			Cursor.INTERNAL_CALL_SetCursor(texture, ref hotspot, cursorMode);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetCursor(Texture2D texture, ref Vector2 hotspot, CursorMode cursorMode);

		/// <summary>
		///   <para>Should the cursor be visible?</para>
		/// </summary>
		public static extern bool visible { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How should the cursor be handled?</para>
		/// </summary>
		public static extern CursorLockMode lockState { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
