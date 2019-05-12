using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Base class for images &amp; text strings displayed in a GUI.</para>
	/// </summary>
	public class GUIElement : Behaviour
	{
		/// <summary>
		///   <para>Is a point on screen inside the element?</para>
		/// </summary>
		/// <param name="screenPosition"></param>
		/// <param name="camera"></param>
		public bool HitTest(Vector3 screenPosition, [DefaultValue("null")] Camera camera)
		{
			return GUIElement.INTERNAL_CALL_HitTest(this, ref screenPosition, camera);
		}

		/// <summary>
		///   <para>Is a point on screen inside the element?</para>
		/// </summary>
		/// <param name="screenPosition"></param>
		/// <param name="camera"></param>
		[ExcludeFromDocs]
		public bool HitTest(Vector3 screenPosition)
		{
			Camera camera = null;
			return GUIElement.INTERNAL_CALL_HitTest(this, ref screenPosition, camera);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_HitTest(GUIElement self, ref Vector3 screenPosition, Camera camera);

		/// <summary>
		///   <para>Returns bounding rectangle of GUIElement in screen coordinates.</para>
		/// </summary>
		/// <param name="camera"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Rect GetScreenRect([DefaultValue("null")] Camera camera);

		[ExcludeFromDocs]
		public Rect GetScreenRect()
		{
			Camera camera = null;
			return this.GetScreenRect(camera);
		}
	}
}
