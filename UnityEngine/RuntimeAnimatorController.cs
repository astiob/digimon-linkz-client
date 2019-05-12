using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Runtime reprentation of the AnimatorController. It can be used to change the Animator's controller during runtime.</para>
	/// </summary>
	public class RuntimeAnimatorController : Object
	{
		/// <summary>
		///   <para>Retrieves all AnimationClip used by the controller.</para>
		/// </summary>
		public extern AnimationClip[] animationClips { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
