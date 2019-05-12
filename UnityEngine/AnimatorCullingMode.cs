using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Culling mode for the Animator.</para>
	/// </summary>
	public enum AnimatorCullingMode
	{
		/// <summary>
		///   <para>Always animate the entire character. Object is animated even when offscreen.</para>
		/// </summary>
		AlwaysAnimate,
		/// <summary>
		///   <para>Retarget, IK and write of Transforms are disabled when renderers are not visible.</para>
		/// </summary>
		CullUpdateTransforms,
		/// <summary>
		///   <para>Animation is completely disabled when renderers are not visible.</para>
		/// </summary>
		CullCompletely
	}
}
