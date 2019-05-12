using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Bindings;
using UnityEngine.Collections;

namespace UnityEngine
{
	[NativeType(Header = "Modules/Audio/Public/ScriptBindings/AudioRenderer.bindings.h")]
	public class AudioRenderer
	{
		public static bool Start()
		{
			return AudioRenderer.Internal_AudioRenderer_Start();
		}

		public static bool Stop()
		{
			return AudioRenderer.Internal_AudioRenderer_Stop();
		}

		public static int GetSampleCountForCaptureFrame()
		{
			return AudioRenderer.Internal_AudioRenderer_GetSampleCountForCaptureFrame();
		}

		internal static bool AddMixerGroupSink(AudioMixerGroup mixerGroup, NativeArray<float> buffer, bool excludeFromMix)
		{
			return AudioRenderer.Internal_AudioRenderer_AddMixerGroupSink(mixerGroup, buffer.UnsafePtr, buffer.Length, excludeFromMix);
		}

		public static bool Render(NativeArray<float> buffer)
		{
			return AudioRenderer.Internal_AudioRenderer_Render(buffer.UnsafePtr, buffer.Length);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_AudioRenderer_Start();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_AudioRenderer_Stop();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_AudioRenderer_GetSampleCountForCaptureFrame();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_AudioRenderer_AddMixerGroupSink(AudioMixerGroup mixerGroup, IntPtr ptr, int length, bool excludeFromMix);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Internal_AudioRenderer_Render(IntPtr ptr, int length);
	}
}
