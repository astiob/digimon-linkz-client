using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>This enum controlls culling of Animation component.</para>
	/// </summary>
	public enum AnimationCullingType
	{
		/// <summary>
		///   <para>Animation culling is disabled - object is animated even when offscreen.</para>
		/// </summary>
		AlwaysAnimate,
		/// <summary>
		///   <para>Animation is disabled when renderers are not visible.</para>
		/// </summary>
		BasedOnRenderers,
		[Obsolete("Enum member AnimatorCullingMode.BasedOnClipBounds has been deprecated. Use AnimationCullingType.AlwaysAnimate or AnimationCullingType.BasedOnRenderers instead")]
		BasedOnClipBounds,
		[Obsolete("Enum member AnimatorCullingMode.BasedOnUserBounds has been deprecated. Use AnimationCullingType.AlwaysAnimate or AnimationCullingType.BasedOnRenderers instead")]
		BasedOnUserBounds
	}
}
