using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public sealed class AudioSource : Behaviour
	{
		internal AudioSourceExtension spatializerExtension = null;

		internal AudioSourceExtension ambisonicExtension = null;

		public extern float volume { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float pitch { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float time { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[ThreadAndSerializationSafe]
		public extern int timeSamples { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[ThreadAndSerializationSafe]
		public extern AudioClip clip { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern AudioMixerGroup outputAudioMixerGroup { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Play([DefaultValue("0")] ulong delay);

		[ExcludeFromDocs]
		public void Play()
		{
			ulong delay = 0UL;
			this.Play(delay);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void PlayDelayed(float delay);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void PlayScheduled(double time);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetScheduledStartTime(double time);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetScheduledEndTime(double time);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();

		public void Pause()
		{
			AudioSource.INTERNAL_CALL_Pause(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Pause(AudioSource self);

		public void UnPause()
		{
			AudioSource.INTERNAL_CALL_UnPause(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_UnPause(AudioSource self);

		public extern bool isPlaying { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool isVirtual { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[ExcludeFromDocs]
		public void PlayOneShot(AudioClip clip)
		{
			float volumeScale = 1f;
			this.PlayOneShot(clip, volumeScale);
		}

		public void PlayOneShot(AudioClip clip, [DefaultValue("1.0F")] float volumeScale)
		{
			if (clip != null && clip.ambisonic)
			{
				AudioSourceExtension audioSourceExtension = AudioExtensionManager.AddAmbisonicDecoderExtension(this);
				if (audioSourceExtension != null)
				{
					AudioExtensionManager.GetReadyToPlay(audioSourceExtension);
				}
			}
			this.PlayOneShotHelper(clip, volumeScale);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PlayOneShotHelper(AudioClip clip, [DefaultValue("1.0F")] float volumeScale);

		[ExcludeFromDocs]
		private void PlayOneShotHelper(AudioClip clip)
		{
			float volumeScale = 1f;
			this.PlayOneShotHelper(clip, volumeScale);
		}

		[ExcludeFromDocs]
		public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
		{
			float volume = 1f;
			AudioSource.PlayClipAtPoint(clip, position, volume);
		}

		public static void PlayClipAtPoint(AudioClip clip, Vector3 position, [DefaultValue("1.0F")] float volume)
		{
			GameObject gameObject = new GameObject("One shot audio");
			gameObject.transform.position = position;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.clip = clip;
			audioSource.spatialBlend = 1f;
			audioSource.volume = volume;
			audioSource.Play();
			Object.Destroy(gameObject, clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f));
		}

		public extern bool loop { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool ignoreListenerVolume { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool playOnAwake { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool ignoreListenerPause { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern AudioVelocityUpdateMode velocityUpdateMode { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float panStereo { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float spatialBlend { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal extern bool spatializeInternal { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public bool spatialize
		{
			get
			{
				return this.spatializeInternal;
			}
			set
			{
				if (this.spatializeInternal != value)
				{
					this.spatializeInternal = value;
					if (value)
					{
						AudioSourceExtension audioSourceExtension = AudioExtensionManager.AddSpatializerExtension(this);
						if (audioSourceExtension != null && this.isPlaying)
						{
							AudioExtensionManager.GetReadyToPlay(audioSourceExtension);
						}
					}
				}
			}
		}

		public extern bool spatializePostEffects { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimationCurve GetCustomCurve(AudioSourceCurveType type);

		public extern float reverbZoneMix { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool bypassEffects { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool bypassListenerEffects { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool bypassReverbZones { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float dopplerLevel { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float spread { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern int priority { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool mute { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float minDistance { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float maxDistance { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern AudioRolloffMode rolloffMode { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetOutputDataHelper(float[] samples, int channel);

		[Obsolete("GetOutputData return a float[] is deprecated, use GetOutputData passing a pre allocated array instead.")]
		public float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			this.GetOutputDataHelper(array, channel);
			return array;
		}

		public void GetOutputData(float[] samples, int channel)
		{
			this.GetOutputDataHelper(samples, channel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window);

		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetSpectrumData passing a pre allocated array instead.")]
		public float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			this.GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			this.GetSpectrumDataHelper(samples, channel, window);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetNumExtensionProperties();

		internal int GetNumExtensionPropertiesForThisExtension(PropertyName extensionName)
		{
			return AudioSource.INTERNAL_CALL_GetNumExtensionPropertiesForThisExtension(this, ref extensionName);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetNumExtensionPropertiesForThisExtension(AudioSource self, ref PropertyName extensionName);

		internal PropertyName ReadExtensionName(int sourceIndex)
		{
			PropertyName result;
			AudioSource.INTERNAL_CALL_ReadExtensionName(this, sourceIndex, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ReadExtensionName(AudioSource self, int sourceIndex, out PropertyName value);

		internal PropertyName ReadExtensionPropertyName(int sourceIndex)
		{
			PropertyName result;
			AudioSource.INTERNAL_CALL_ReadExtensionPropertyName(this, sourceIndex, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ReadExtensionPropertyName(AudioSource self, int sourceIndex, out PropertyName value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float ReadExtensionPropertyValue(int sourceIndex);

		internal bool ReadExtensionProperty(PropertyName extensionName, PropertyName propertyName, ref float propertyValue)
		{
			return AudioSource.INTERNAL_CALL_ReadExtensionProperty(this, ref extensionName, ref propertyName, ref propertyValue);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_ReadExtensionProperty(AudioSource self, ref PropertyName extensionName, ref PropertyName propertyName, ref float propertyValue);

		internal void ClearExtensionProperties(PropertyName extensionName)
		{
			AudioSource.INTERNAL_CALL_ClearExtensionProperties(this, ref extensionName);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ClearExtensionProperties(AudioSource self, ref PropertyName extensionName);

		internal AudioSourceExtension AddSpatializerExtension(Type extensionType)
		{
			if (this.spatializerExtension == null)
			{
				this.spatializerExtension = (ScriptableObject.CreateInstance(extensionType) as AudioSourceExtension);
			}
			return this.spatializerExtension;
		}

		internal AudioSourceExtension AddAmbisonicExtension(Type extensionType)
		{
			if (this.ambisonicExtension == null)
			{
				this.ambisonicExtension = (ScriptableObject.CreateInstance(extensionType) as AudioSourceExtension);
			}
			return this.ambisonicExtension;
		}

		[Obsolete("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public extern float minVolume { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public extern float maxVolume { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
		public extern float rolloffFactor { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetSpatializerFloat(int index, float value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetSpatializerFloat(int index, out float value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetAmbisonicDecoderFloat(int index, float value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAmbisonicDecoderFloat(int index, out float value);
	}
}
