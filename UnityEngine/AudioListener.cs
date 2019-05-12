using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Representation of a listener in 3D space.</para>
	/// </summary>
	public sealed class AudioListener : Behaviour
	{
		/// <summary>
		///   <para>Controls the game sound volume (0.0 to 1.0).</para>
		/// </summary>
		public static extern float volume { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The paused state of the audio system.</para>
		/// </summary>
		public static extern bool pause { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>This lets you set whether the Audio Listener should be updated in the fixed or dynamic update.</para>
		/// </summary>
		public extern AudioVelocityUpdateMode velocityUpdateMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetOutputDataHelper(float[] samples, int channel);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window);

		/// <summary>
		///   <para>Returns a block of the listener (master)'s output data.</para>
		/// </summary>
		/// <param name="numSamples"></param>
		/// <param name="channel"></param>
		[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetOutputData(int numSamples, int channel)
		{
			float[] array = new float[numSamples];
			AudioListener.GetOutputDataHelper(array, channel);
			return array;
		}

		/// <summary>
		///   <para>Returns a block of the listener (master)'s output data.</para>
		/// </summary>
		/// <param name="samples"></param>
		/// <param name="channel"></param>
		public static void GetOutputData(float[] samples, int channel)
		{
			AudioListener.GetOutputDataHelper(samples, channel);
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="numSamples"></param>
		/// <param name="channel"></param>
		/// <param name="window"></param>
		[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
		public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
		{
			float[] array = new float[numSamples];
			AudioListener.GetSpectrumDataHelper(array, channel, window);
			return array;
		}

		/// <summary>
		///   <para>Returns a block of the listener (master)'s spectrum data.</para>
		/// </summary>
		/// <param name="samples"></param>
		/// <param name="channel"></param>
		/// <param name="window"></param>
		public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
		{
			AudioListener.GetSpectrumDataHelper(samples, channel, window);
		}
	}
}
