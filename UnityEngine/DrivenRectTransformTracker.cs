using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>A component can be designed drive a RectTransform. The DrivenRectTransformTracker struct is used to specify which RectTransforms it is driving.</para>
	/// </summary>
	public struct DrivenRectTransformTracker
	{
		/// <summary>
		///   <para>Add a RectTransform to be driven.</para>
		/// </summary>
		/// <param name="driver">The object to drive properties.</param>
		/// <param name="rectTransform">The RectTransform to be driven.</param>
		/// <param name="drivenProperties">The properties to be driven.</param>
		public void Add(Object driver, RectTransform rectTransform, DrivenTransformProperties drivenProperties)
		{
		}

		/// <summary>
		///   <para>Clear the list of RectTransforms being driven.</para>
		/// </summary>
		public void Clear()
		{
		}
	}
}
