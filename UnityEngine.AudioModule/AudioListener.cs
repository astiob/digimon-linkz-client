using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public sealed class AudioListener : Behaviour
	{
		internal AudioListenerExtension spatializerExtension = null;

		public static extern float volume { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern bool pause { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern AudioVelocityUpdateMode velocityUpdateMode { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetOutputDataHelper(float[] samples, int channel);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window);

		[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			AudioListener.GetOutputDataHelper(array, channel);
			return array;
		}

		public static void GetOutputData(float[] samples, int channel)
		{
			AudioListener.GetOutputDataHelper(samples, channel);
		}

		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			AudioListener.GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			AudioListener.GetSpectrumDataHelper(samples, channel, window);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetNumExtensionProperties();

		internal int GetNumExtensionPropertiesForThisExtension(PropertyName extensionName)
		{
			return AudioListener.INTERNAL_CALL_GetNumExtensionPropertiesForThisExtension(this, ref extensionName);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_GetNumExtensionPropertiesForThisExtension(AudioListener self, ref PropertyName extensionName);

		internal PropertyName ReadExtensionName(int listenerIndex)
		{
			PropertyName result;
			AudioListener.INTERNAL_CALL_ReadExtensionName(this, listenerIndex, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ReadExtensionName(AudioListener self, int listenerIndex, out PropertyName value);

		internal PropertyName ReadExtensionPropertyName(int listenerIndex)
		{
			PropertyName result;
			AudioListener.INTERNAL_CALL_ReadExtensionPropertyName(this, listenerIndex, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ReadExtensionPropertyName(AudioListener self, int listenerIndex, out PropertyName value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float ReadExtensionPropertyValue(int listenerIndex);

		internal bool ReadExtensionProperty(PropertyName extensionName, PropertyName propertyName, ref float propertyValue)
		{
			return AudioListener.INTERNAL_CALL_ReadExtensionProperty(this, ref extensionName, ref propertyName, ref propertyValue);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_ReadExtensionProperty(AudioListener self, ref PropertyName extensionName, ref PropertyName propertyName, ref float propertyValue);

		internal void ClearExtensionProperties(PropertyName extensionName)
		{
			AudioListener.INTERNAL_CALL_ClearExtensionProperties(this, ref extensionName);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ClearExtensionProperties(AudioListener self, ref PropertyName extensionName);

		internal AudioListenerExtension AddExtension(Type extensionType)
		{
			if (this.spatializerExtension == null)
			{
				this.spatializerExtension = (ScriptableObject.CreateInstance(extensionType) as AudioListenerExtension);
			}
			return this.spatializerExtension;
		}
	}
}
