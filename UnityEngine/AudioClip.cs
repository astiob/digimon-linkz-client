using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A container for audio data.</para>
	/// </summary>
	public sealed class AudioClip : Object
	{
		private event AudioClip.PCMReaderCallback m_PCMReaderCallback;

		private event AudioClip.PCMSetPositionCallback m_PCMSetPositionCallback;

		/// <summary>
		///   <para>The length of the audio clip in seconds. (Read Only)</para>
		/// </summary>
		public extern float length { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The length of the audio clip in samples. (Read Only)</para>
		/// </summary>
		public extern int samples { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of channels in the audio clip. (Read Only)</para>
		/// </summary>
		public extern int channels { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The sample frequency of the clip in Hertz. (Read Only)</para>
		/// </summary>
		public extern int frequency { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns true if the AudioClip is ready to play (read-only).</para>
		/// </summary>
		[Obsolete("Use AudioClip.loadState instead to get more detailed information about the loading process.")]
		public extern bool isReadyToPlay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The load type of the clip (read-only).</para>
		/// </summary>
		public extern AudioClipLoadType loadType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Loads the audio data of a clip. Clips that have "Preload Audio Data" set will load the audio data automatically.</para>
		/// </summary>
		/// <returns>
		///   <para>Returns true if loading succeeded.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool LoadAudioData();

		/// <summary>
		///   <para>Unloads the audio data associated with the clip. This works only for AudioClips that are based on actual sound file assets.</para>
		/// </summary>
		/// <returns>
		///   <para>Returns false if unloading failed.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool UnloadAudioData();

		/// <summary>
		///   <para>Preloads audio data of the clip when the clip asset is loaded. When this flag is off, scripts have to call AudioClip.LoadAudioData() to load the data before the clip can be played. Properties like length, channels and format are available before the audio data has been loaded.</para>
		/// </summary>
		public extern bool preloadAudioData { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the current load state of the audio data associated with an AudioClip.</para>
		/// </summary>
		public extern AudioDataLoadState loadState { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Corresponding to the "Load In Background" flag in the inspector, when this flag is set, the loading will happen delayed without blocking the main thread.</para>
		/// </summary>
		public extern bool loadInBackground { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Fills an array with sample data from the clip.</para>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="offsetSamples"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetData(float[] data, int offsetSamples);

		/// <summary>
		///   <para>Set sample data in a clip.</para>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="offsetSamples"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetData(float[] data, int offsetSamples);

		/// <summary>
		///   <para>Creates a user AudioClip with a name and with the given length in samples, channels and frequency.</para>
		/// </summary>
		/// <param name="name">Name of clip.</param>
		/// <param name="lengthSamples">Number of sample frames.</param>
		/// <param name="channels">Number of channels per frame.</param>
		/// <param name="frequency">Sample frequency of clip.</param>
		/// <param name="_3D">Audio clip is played back in 3D.</param>
		/// <param name="stream">True if clip is streamed, that is if the pcmreadercallback generates data on the fly.</param>
		/// <param name="pcmreadercallback">This callback is invoked to generate a block of sample data. Non-streamed clips call this only once at creation time while streamed clips call this continuously.</param>
		/// <param name="pcmsetpositioncallback">This callback is invoked whenever the clip loops or changes playback position.</param>
		/// <returns>
		///   <para>A reference to the created AudioClip.</para>
		/// </returns>
		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream);
		}

		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, AudioClip.PCMReaderCallback pcmreadercallback)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		}

		[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, AudioClip.PCMReaderCallback pcmreadercallback, AudioClip.PCMSetPositionCallback pcmsetpositioncallback)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback);
		}

		/// <summary>
		///   <para>Creates a user AudioClip with a name and with the given length in samples, channels and frequency.</para>
		/// </summary>
		/// <param name="name">Name of clip.</param>
		/// <param name="lengthSamples">Number of sample frames.</param>
		/// <param name="channels">Number of channels per frame.</param>
		/// <param name="frequency">Sample frequency of clip.</param>
		/// <param name="_3D">Audio clip is played back in 3D.</param>
		/// <param name="stream">True if clip is streamed, that is if the pcmreadercallback generates data on the fly.</param>
		/// <param name="pcmreadercallback">This callback is invoked to generate a block of sample data. Non-streamed clips call this only once at creation time while streamed clips call this continuously.</param>
		/// <param name="pcmsetpositioncallback">This callback is invoked whenever the clip loops or changes playback position.</param>
		/// <returns>
		///   <para>A reference to the created AudioClip.</para>
		/// </returns>
		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, null, null);
		}

		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, AudioClip.PCMReaderCallback pcmreadercallback)
		{
			return AudioClip.Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
		}

		public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, AudioClip.PCMReaderCallback pcmreadercallback, AudioClip.PCMSetPositionCallback pcmsetpositioncallback)
		{
			if (name == null)
			{
				throw new NullReferenceException();
			}
			if (lengthSamples <= 0)
			{
				throw new ArgumentException("Length of created clip must be larger than 0");
			}
			if (channels <= 0)
			{
				throw new ArgumentException("Number of channels in created clip must be greater than 0");
			}
			if (frequency <= 0)
			{
				throw new ArgumentException("Frequency in created clip must be greater than 0");
			}
			AudioClip audioClip = AudioClip.Construct_Internal();
			if (pcmreadercallback != null)
			{
				AudioClip audioClip2 = audioClip;
				audioClip2.m_PCMReaderCallback = (AudioClip.PCMReaderCallback)Delegate.Combine(audioClip2.m_PCMReaderCallback, pcmreadercallback);
			}
			if (pcmsetpositioncallback != null)
			{
				AudioClip audioClip3 = audioClip;
				audioClip3.m_PCMSetPositionCallback = (AudioClip.PCMSetPositionCallback)Delegate.Combine(audioClip3.m_PCMSetPositionCallback, pcmsetpositioncallback);
			}
			audioClip.Init_Internal(name, lengthSamples, channels, frequency, stream);
			return audioClip;
		}

		private void InvokePCMReaderCallback_Internal(float[] data)
		{
			if (this.m_PCMReaderCallback != null)
			{
				this.m_PCMReaderCallback(data);
			}
		}

		private void InvokePCMSetPositionCallback_Internal(int position)
		{
			if (this.m_PCMSetPositionCallback != null)
			{
				this.m_PCMSetPositionCallback(position);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioClip Construct_Internal();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init_Internal(string name, int lengthSamples, int channels, int frequency, bool stream);

		/// <summary>
		///   <para>Delegate called each time AudioClip reads data.</para>
		/// </summary>
		/// <param name="data">Array of floats containing data read from the clip.</param>
		public delegate void PCMReaderCallback(float[] data);

		/// <summary>
		///   <para>Delegate called each time AudioClip changes read position.</para>
		/// </summary>
		/// <param name="position">New position in the audio clip.</param>
		public delegate void PCMSetPositionCallback(int position);
	}
}
