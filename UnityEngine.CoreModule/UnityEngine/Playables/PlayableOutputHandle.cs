using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	[NativeHeader("Runtime/Director/Core/HPlayableOutput.h")]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[UsedByNativeCode]
	public struct PlayableOutputHandle
	{
		internal IntPtr m_Handle;

		internal int m_Version;

		internal Object GetUserData()
		{
			return PlayableOutputHandle.GetInternalUserData(ref this);
		}

		internal void SetUserData(Object value)
		{
			PlayableOutputHandle.SetInternalUserData(ref this, value);
		}

		internal static Object GetInternalUserData(ref PlayableOutputHandle handle)
		{
			return PlayableOutputHandle.INTERNAL_CALL_GetInternalUserData(ref handle);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object INTERNAL_CALL_GetInternalUserData(ref PlayableOutputHandle handle);

		internal static void SetInternalUserData(ref PlayableOutputHandle handle, [Writable] Object target)
		{
			PlayableOutputHandle.INTERNAL_CALL_SetInternalUserData(ref handle, target);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetInternalUserData(ref PlayableOutputHandle handle, [Writable] Object target);

		public static PlayableOutputHandle Null
		{
			get
			{
				return new PlayableOutputHandle
				{
					m_Version = int.MaxValue
				};
			}
		}

		internal bool IsPlayableOutputOfType<T>()
		{
			return this.GetPlayableOutputType() == typeof(T);
		}

		public override int GetHashCode()
		{
			return this.m_Handle.GetHashCode() ^ this.m_Version.GetHashCode();
		}

		public static bool operator ==(PlayableOutputHandle lhs, PlayableOutputHandle rhs)
		{
			return PlayableOutputHandle.CompareVersion(lhs, rhs);
		}

		public static bool operator !=(PlayableOutputHandle lhs, PlayableOutputHandle rhs)
		{
			return !PlayableOutputHandle.CompareVersion(lhs, rhs);
		}

		public override bool Equals(object p)
		{
			return p is PlayableOutputHandle && PlayableOutputHandle.CompareVersion(this, (PlayableOutputHandle)p);
		}

		internal static bool CompareVersion(PlayableOutputHandle lhs, PlayableOutputHandle rhs)
		{
			return lhs.m_Handle == rhs.m_Handle && lhs.m_Version == rhs.m_Version;
		}

		internal bool IsValid()
		{
			return PlayableOutputHandle.IsValid_Injected(ref this);
		}

		internal Type GetPlayableOutputType()
		{
			return PlayableOutputHandle.GetPlayableOutputType_Injected(ref this);
		}

		internal Object GetReferenceObject()
		{
			return PlayableOutputHandle.GetReferenceObject_Injected(ref this);
		}

		internal void SetReferenceObject(Object target)
		{
			PlayableOutputHandle.SetReferenceObject_Injected(ref this, target);
		}

		internal PlayableHandle GetSourcePlayable()
		{
			PlayableHandle result;
			PlayableOutputHandle.GetSourcePlayable_Injected(ref this, out result);
			return result;
		}

		internal void SetSourcePlayable(PlayableHandle target)
		{
			PlayableOutputHandle.SetSourcePlayable_Injected(ref this, ref target);
		}

		internal int GetSourceInputPort()
		{
			return PlayableOutputHandle.GetSourceInputPort_Injected(ref this);
		}

		internal void SetSourceInputPort(int port)
		{
			PlayableOutputHandle.SetSourceInputPort_Injected(ref this, port);
		}

		internal float GetWeight()
		{
			return PlayableOutputHandle.GetWeight_Injected(ref this);
		}

		internal void SetWeight(float weight)
		{
			PlayableOutputHandle.SetWeight_Injected(ref this, weight);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValid_Injected(ref PlayableOutputHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetPlayableOutputType_Injected(ref PlayableOutputHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object GetReferenceObject_Injected(ref PlayableOutputHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetReferenceObject_Injected(ref PlayableOutputHandle _unity_self, Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSourcePlayable_Injected(ref PlayableOutputHandle _unity_self, out PlayableHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSourcePlayable_Injected(ref PlayableOutputHandle _unity_self, ref PlayableHandle target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetSourceInputPort_Injected(ref PlayableOutputHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSourceInputPort_Injected(ref PlayableOutputHandle _unity_self, int port);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetWeight_Injected(ref PlayableOutputHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetWeight_Injected(ref PlayableOutputHandle _unity_self, float weight);
	}
}
