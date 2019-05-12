using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Structure for building a LOD for passing to the SetLODs function.</para>
	/// </summary>
	public struct LOD
	{
		/// <summary>
		///   <para>The screen relative height to use for the transition [0-1].</para>
		/// </summary>
		public float screenRelativeTransitionHeight;

		/// <summary>
		///   <para>Width of the cross-fade transition zone (proportion to the current LOD's whole length) [0-1]. Only used if it's not animated.</para>
		/// </summary>
		public float fadeTransitionWidth;

		/// <summary>
		///   <para>List of renderers for this LOD level.</para>
		/// </summary>
		public Renderer[] renderers;

		/// <summary>
		///   <para>Construct a LOD.</para>
		/// </summary>
		/// <param name="screenRelativeTransitionHeight">The screen relative height to use for the transition [0-1].</param>
		/// <param name="renderers">An array of renderers to use for this LOD level.</param>
		public LOD(float screenRelativeTransitionHeight, Renderer[] renderers)
		{
			this.screenRelativeTransitionHeight = screenRelativeTransitionHeight;
			this.fadeTransitionWidth = 0f;
			this.renderers = renderers;
		}
	}
}
