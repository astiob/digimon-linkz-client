using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
	[NativeHeader("Runtime/Animation/Director/AnimationMotionXToDeltaPlayable.h")]
	[RequiredByNativeCode]
	internal struct AnimationMotionXToDeltaPlayable : IPlayable, IEquatable<AnimationMotionXToDeltaPlayable>
	{
		private PlayableHandle m_Handle;

		private AnimationMotionXToDeltaPlayable(PlayableHandle handle)
		{
			if (handle.IsValid())
			{
				if (!handle.IsPlayableOfType<AnimationMotionXToDeltaPlayable>())
				{
					throw new InvalidCastException("Can't set handle: the playable is not an AnimationMotionXToDeltaPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		public static AnimationMotionXToDeltaPlayable Create(PlayableGraph graph)
		{
			PlayableHandle handle = AnimationMotionXToDeltaPlayable.CreateHandle(graph);
			return new AnimationMotionXToDeltaPlayable(handle);
		}

		private static PlayableHandle CreateHandle(PlayableGraph graph)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationMotionXToDeltaPlayable.CreateHandleInternal(graph, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				@null.SetInputCount(1);
				result = @null;
			}
			return result;
		}

		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		public static implicit operator Playable(AnimationMotionXToDeltaPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		public static explicit operator AnimationMotionXToDeltaPlayable(Playable playable)
		{
			return new AnimationMotionXToDeltaPlayable(playable.GetHandle());
		}

		public bool Equals(AnimationMotionXToDeltaPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		private static bool CreateHandleInternal(PlayableGraph graph, ref PlayableHandle handle)
		{
			return AnimationMotionXToDeltaPlayable.CreateHandleInternal_Injected(ref graph, ref handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, ref PlayableHandle handle);
	}
}
