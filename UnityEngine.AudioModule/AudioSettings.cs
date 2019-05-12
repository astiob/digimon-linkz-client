using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class AudioSettings
	{
		public static extern AudioSpeakerMode driverCapabilities { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern AudioSpeakerMode speakerMode { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal static extern int profilerCaptureFlags { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[ThreadAndSerializationSafe]
		public static extern double dspTime { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern int outputSampleRate { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetDSPBufferSize(out int bufferLength, out int numBuffers);

		[Obsolete("AudioSettings.SetDSPBufferSize is deprecated and has been replaced by audio project settings and the AudioSettings.GetConfiguration/AudioSettings.Reset API.")]
		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetDSPBufferSize(int bufferLength, int numBuffers);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetSpatializerPluginName();

		public static AudioConfiguration GetConfiguration()
		{
			AudioConfiguration result;
			AudioSettings.INTERNAL_CALL_GetConfiguration(out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetConfiguration(out AudioConfiguration value);

		public static bool Reset(AudioConfiguration config)
		{
			return AudioSettings.INTERNAL_CALL_Reset(ref config);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_Reset(ref AudioConfiguration config);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event AudioSettings.AudioConfigurationChangeHandler OnAudioConfigurationChanged;

		[RequiredByNativeCode]
		internal static void InvokeOnAudioConfigurationChanged(bool deviceWasChanged)
		{
			if (AudioSettings.OnAudioConfigurationChanged != null)
			{
				AudioSettings.OnAudioConfigurationChanged(deviceWasChanged);
			}
		}

		[RequiredByNativeCode]
		internal static void InvokeOnAudioManagerUpdate()
		{
			AudioExtensionManager.Update();
		}

		[RequiredByNativeCode]
		internal static void InvokeOnAudioSourcePlay(AudioSource source)
		{
			AudioSourceExtension audioSourceExtension = AudioExtensionManager.AddSpatializerExtension(source);
			if (audioSourceExtension != null)
			{
				AudioExtensionManager.GetReadyToPlay(audioSourceExtension);
			}
			if (source.clip != null && source.clip.ambisonic)
			{
				AudioSourceExtension audioSourceExtension2 = AudioExtensionManager.AddAmbisonicDecoderExtension(source);
				if (audioSourceExtension2 != null)
				{
					AudioExtensionManager.GetReadyToPlay(audioSourceExtension2);
				}
			}
		}

		internal static extern bool unityAudioDisabled { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetAmbisonicDecoderPluginName();

		public delegate void AudioConfigurationChangeHandler(bool deviceWasChanged);
	}
}
