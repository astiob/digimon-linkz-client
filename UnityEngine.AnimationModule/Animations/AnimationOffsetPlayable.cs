using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Animation/ScriptBindings/AnimationOffsetPlayable.bindings.h")]
	[StaticAccessor("AnimationOffsetPlayableBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[NativeHeader("Runtime/Animation/Director/AnimationOffsetPlayable.h")]
	internal struct AnimationOffsetPlayable : IPlayable, IEquatable<AnimationOffsetPlayable>
	{
		private PlayableHandle m_Handle;

		private static readonly AnimationOffsetPlayable m_NullPlayable = new AnimationOffsetPlayable(PlayableHandle.Null);

		internal AnimationOffsetPlayable(PlayableHandle handle)
		{
			if (handle.IsValid())
			{
				if (!handle.IsPlayableOfType<AnimationOffsetPlayable>())
				{
					throw new InvalidCastException("Can't set handle: the playable is not an AnimationOffsetPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		public static AnimationOffsetPlayable Null
		{
			get
			{
				return AnimationOffsetPlayable.m_NullPlayable;
			}
		}

		public static AnimationOffsetPlayable Create(PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
		{
			PlayableHandle handle = AnimationOffsetPlayable.CreateHandle(graph, position, rotation, inputCount);
			return new AnimationOffsetPlayable(handle);
		}

		private static PlayableHandle CreateHandle(PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationOffsetPlayable.CreateHandleInternal(graph, position, rotation, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				@null.SetInputCount(inputCount);
				result = @null;
			}
			return result;
		}

		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		public static implicit operator Playable(AnimationOffsetPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		public static explicit operator AnimationOffsetPlayable(Playable playable)
		{
			return new AnimationOffsetPlayable(playable.GetHandle());
		}

		public bool Equals(AnimationOffsetPlayable other)
		{
			return this.Equals(other.GetHandle());
		}

		public Vector3 GetPosition()
		{
			return AnimationOffsetPlayable.GetPositionInternal(ref this.m_Handle);
		}

		public void SetPosition(Vector3 value)
		{
			AnimationOffsetPlayable.SetPositionInternal(ref this.m_Handle, value);
		}

		public Quaternion GetRotation()
		{
			return AnimationOffsetPlayable.GetRotationInternal(ref this.m_Handle);
		}

		public void SetRotation(Quaternion value)
		{
			AnimationOffsetPlayable.SetRotationInternal(ref this.m_Handle, value);
		}

		private static bool CreateHandleInternal(PlayableGraph graph, Vector3 position, Quaternion rotation, ref PlayableHandle handle)
		{
			return AnimationOffsetPlayable.CreateHandleInternal_Injected(ref graph, ref position, ref rotation, ref handle);
		}

		private static Vector3 GetPositionInternal(ref PlayableHandle handle)
		{
			Vector3 result;
			AnimationOffsetPlayable.GetPositionInternal_Injected(ref handle, out result);
			return result;
		}

		private static void SetPositionInternal(ref PlayableHandle handle, Vector3 value)
		{
			AnimationOffsetPlayable.SetPositionInternal_Injected(ref handle, ref value);
		}

		private static Quaternion GetRotationInternal(ref PlayableHandle handle)
		{
			Quaternion result;
			AnimationOffsetPlayable.GetRotationInternal_Injected(ref handle, out result);
			return result;
		}

		private static void SetRotationInternal(ref PlayableHandle handle, Quaternion value)
		{
			AnimationOffsetPlayable.SetRotationInternal_Injected(ref handle, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, ref Vector3 position, ref Quaternion rotation, ref PlayableHandle handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPositionInternal_Injected(ref PlayableHandle handle, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPositionInternal_Injected(ref PlayableHandle handle, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRotationInternal_Injected(ref PlayableHandle handle, out Quaternion ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRotationInternal_Injected(ref PlayableHandle handle, ref Quaternion value);
	}
}
