using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Handling of storing RenderBuffer contents after it was an active RenderTarget and another RenderTarget was set.</para>
	/// </summary>
	public enum RenderBufferStoreAction
	{
		/// <summary>
		///   <para>Make RenderBuffer to Store its contents.</para>
		/// </summary>
		Store,
		/// <summary>
		///   <para>RenderBuffer will try to skip storing its contents.</para>
		/// </summary>
		DontCare
	}
}
