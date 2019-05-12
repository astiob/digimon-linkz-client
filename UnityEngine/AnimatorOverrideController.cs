using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interface to control AnimatorOverrideController.</para>
	/// </summary>
	public sealed class AnimatorOverrideController : RuntimeAnimatorController
	{
		public AnimatorOverrideController()
		{
			AnimatorOverrideController.Internal_CreateAnimationSet(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateAnimationSet([Writable] AnimatorOverrideController self);

		/// <summary>
		///   <para>The Controller that the AnimatorOverrideController overrides.</para>
		/// </summary>
		public extern RuntimeAnimatorController runtimeAnimatorController { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public AnimationClip this[string name]
		{
			get
			{
				return this.Internal_GetClipByName(name, true);
			}
			set
			{
				this.Internal_SetClipByName(name, value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationClip Internal_GetClipByName(string name, bool returnEffectiveClip);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetClipByName(string name, AnimationClip clip);

		public AnimationClip this[AnimationClip clip]
		{
			get
			{
				return this.Internal_GetClip(clip, true);
			}
			set
			{
				this.Internal_SetClip(clip, value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationClip Internal_GetClip(AnimationClip originalClip, bool returnEffectiveClip);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip);

		/// <summary>
		///   <para>Returns the list of orignal clip from the controller and their override clip.</para>
		/// </summary>
		public AnimationClipPair[] clips
		{
			get
			{
				AnimationClip[] array = this.GetOriginalClips();
				Dictionary<AnimationClip, bool> dictionary = new Dictionary<AnimationClip, bool>(array.Length);
				foreach (AnimationClip key in array)
				{
					dictionary[key] = true;
				}
				array = new AnimationClip[dictionary.Count];
				dictionary.Keys.CopyTo(array, 0);
				AnimationClipPair[] array3 = new AnimationClipPair[array.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array3[j] = new AnimationClipPair();
					array3[j].originalClip = array[j];
					array3[j].overrideClip = this.Internal_GetClip(array[j], false);
				}
				return array3;
			}
			set
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.Internal_SetClip(value[i].originalClip, value[i].overrideClip);
				}
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationClip[] GetOriginalClips();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationClip[] GetOverrideClips();
	}
}
