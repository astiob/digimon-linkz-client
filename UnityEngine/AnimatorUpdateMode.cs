using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The update mode of the Animator.</para>
	/// </summary>
	public enum AnimatorUpdateMode
	{
		/// <summary>
		///   <para>Normal update of the animator.</para>
		/// </summary>
		Normal,
		/// <summary>
		///   <para>Updates the animator during the physic loop in order to have the animation system synchronized with the physics engine.</para>
		/// </summary>
		AnimatePhysics,
		/// <summary>
		///   <para>Animator updates independently of Time.timeScale.</para>
		/// </summary>
		UnscaledTime
	}
}
