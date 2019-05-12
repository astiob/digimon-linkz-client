using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Component added to a camera to make it render 2D GUI elements.</para>
	/// </summary>
	public sealed class GUILayer : Behaviour
	{
		/// <summary>
		///   <para>Get the GUI element at a specific screen position.</para>
		/// </summary>
		/// <param name="screenPosition"></param>
		public GUIElement HitTest(Vector3 screenPosition)
		{
			return GUILayer.INTERNAL_CALL_HitTest(this, ref screenPosition);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GUIElement INTERNAL_CALL_HitTest(GUILayer self, ref Vector3 screenPosition);
	}
}
