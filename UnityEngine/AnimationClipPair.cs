using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>This class defines a pair of clips used by AnimatorOverrideController.</para>
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AnimationClipPair
	{
		/// <summary>
		///   <para>The original clip from the controller.</para>
		/// </summary>
		public AnimationClip originalClip;

		/// <summary>
		///   <para>The override animation clip.</para>
		/// </summary>
		public AnimationClip overrideClip;
	}
}
