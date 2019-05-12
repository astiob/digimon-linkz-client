using System;

namespace UnityEngine
{
	public interface ICanvasRaycastFilter
	{
		/// <summary>
		///   <para>Given a point and a camera is the raycast valid.</para>
		/// </summary>
		/// <param name="sp">Screen position.</param>
		/// <param name="eventCamera">Raycast camera.</param>
		/// <returns>
		///   <para>Valid.</para>
		/// </returns>
		bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera);
	}
}
