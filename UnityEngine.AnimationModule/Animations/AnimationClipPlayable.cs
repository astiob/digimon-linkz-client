using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
	[RequiredByNativeCode]
	[StaticAccessor("AnimationClipPlayableBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Animation/Director/AnimationClipPlayable.h")]
	[NativeHeader("Runtime/Animation/ScriptBindings/AnimationClipPlayable.bindings.h")]
	public struct AnimationClipPlayable : IPlayable, IEquatable<AnimationClipPlayable>
	{
		private PlayableHandle m_Handle;

		internal AnimationClipPlayable(PlayableHandle handle)
		{
			if (handle.IsValid())
			{
				if (!handle.IsPlayableOfType<AnimationClipPlayable>())
				{
					throw new InvalidCastException("Can't set handle: the playable is not an AnimationClipPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		public static AnimationClipPlayable Create(PlayableGraph graph, AnimationClip clip)
		{
			PlayableHandle handle = AnimationClipPlayable.CreateHandle(graph, clip);
			return new AnimationClipPlayable(handle);
		}

		private static PlayableHandle CreateHandle(PlayableGraph graph, AnimationClip clip)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationClipPlayable.CreateHandleInternal(graph, clip, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		public static implicit operator Playable(AnimationClipPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		public static explicit operator AnimationClipPlayable(Playable playable)
		{
			return new AnimationClipPlayable(playable.GetHandle());
		}

		public bool Equals(AnimationClipPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		public AnimationClip GetAnimationClip()
		{
			return AnimationClipPlayable.GetAnimationClipInternal(ref this.m_Handle);
		}

		public bool GetApplyFootIK()
		{
			return AnimationClipPlayable.GetApplyFootIKInternal(ref this.m_Handle);
		}

		public void SetApplyFootIK(bool value)
		{
			AnimationClipPlayable.SetApplyFootIKInternal(ref this.m_Handle, value);
		}

		internal bool GetRemoveStartOffset()
		{
			return AnimationClipPlayable.GetRemoveStartOffsetInternal(ref this.m_Handle);
		}

		internal void SetRemoveStartOffset(bool value)
		{
			AnimationClipPlayable.SetRemoveStartOffsetInternal(ref this.m_Handle, value);
		}

		private static bool CreateHandleInternal(PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle)
		{
			return AnimationClipPlayable.CreateHandleInternal_Injected(ref graph, clip, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationClip GetAnimationClipInternal(ref PlayableHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetApplyFootIKInternal(ref PlayableHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetApplyFootIKInternal(ref PlayableHandle handle, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetRemoveStartOffsetInternal(ref PlayableHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRemoveStartOffsetInternal(ref PlayableHandle handle, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle);
	}
}
