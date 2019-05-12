using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>An enumeration of transform properties that can be driven on a RectTransform by an object.</para>
	/// </summary>
	[Flags]
	public enum DrivenTransformProperties
	{
		/// <summary>
		///   <para>Deselects all driven properties.</para>
		/// </summary>
		None = 0,
		/// <summary>
		///   <para>Selects all driven properties.</para>
		/// </summary>
		All = -1,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchoredPosition.x.</para>
		/// </summary>
		AnchoredPositionX = 2,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchoredPosition.y.</para>
		/// </summary>
		AnchoredPositionY = 4,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchoredPosition3D.z.</para>
		/// </summary>
		AnchoredPositionZ = 8,
		/// <summary>
		///   <para>Selects driven property Transform.localRotation.</para>
		/// </summary>
		Rotation = 16,
		/// <summary>
		///   <para>Selects driven property Transform.localScale.x.</para>
		/// </summary>
		ScaleX = 32,
		/// <summary>
		///   <para>Selects driven property Transform.localScale.y.</para>
		/// </summary>
		ScaleY = 64,
		/// <summary>
		///   <para>Selects driven property Transform.localScale.z.</para>
		/// </summary>
		ScaleZ = 128,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchorMin.x.</para>
		/// </summary>
		AnchorMinX = 256,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchorMin.y.</para>
		/// </summary>
		AnchorMinY = 512,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchorMax.x.</para>
		/// </summary>
		AnchorMaxX = 1024,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchorMax.y.</para>
		/// </summary>
		AnchorMaxY = 2048,
		/// <summary>
		///   <para>Selects driven property RectTransform.sizeDelta.x.</para>
		/// </summary>
		SizeDeltaX = 4096,
		/// <summary>
		///   <para>Selects driven property RectTransform.sizeDelta.y.</para>
		/// </summary>
		SizeDeltaY = 8192,
		/// <summary>
		///   <para>Selects driven property RectTransform.pivot.x.</para>
		/// </summary>
		PivotX = 16384,
		/// <summary>
		///   <para>Selects driven property RectTransform.pivot.y.</para>
		/// </summary>
		PivotY = 32768,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchoredPosition.</para>
		/// </summary>
		AnchoredPosition = 6,
		/// <summary>
		///   <para>Selects driven property RectTransform.anchoredPosition3D.</para>
		/// </summary>
		AnchoredPosition3D = 14,
		/// <summary>
		///   <para>Selects driven property combining ScaleX, ScaleY &amp;&amp; ScaleZ.</para>
		/// </summary>
		Scale = 224,
		/// <summary>
		///   <para>Selects driven property combining AnchorMinX and AnchorMinY.</para>
		/// </summary>
		AnchorMin = 768,
		/// <summary>
		///   <para>Selects driven property combining AnchorMaxX and AnchorMaxY.</para>
		/// </summary>
		AnchorMax = 3072,
		/// <summary>
		///   <para>Selects driven property combining AnchorMinX, AnchorMinY, AnchorMaxX and AnchorMaxY.</para>
		/// </summary>
		Anchors = 3840,
		/// <summary>
		///   <para>Selects driven property combining SizeDeltaX and SizeDeltaY.</para>
		/// </summary>
		SizeDelta = 12288,
		/// <summary>
		///   <para>Selects driven property combining PivotX and PivotY.</para>
		/// </summary>
		Pivot = 49152
	}
}
