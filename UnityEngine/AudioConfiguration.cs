using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Specifies the current properties or desired properties to be set for the audio system.</para>
	/// </summary>
	public struct AudioConfiguration
	{
		/// <summary>
		///   <para>The current speaker mode used by the audio output device.</para>
		/// </summary>
		public AudioSpeakerMode speakerMode;

		/// <summary>
		///   <para>The length of the DSP buffer in samples determining the latency of sounds by the audio output device.</para>
		/// </summary>
		public int dspBufferSize;

		/// <summary>
		///   <para>The current sample rate of the audio output device used.</para>
		/// </summary>
		public int sampleRate;

		/// <summary>
		///   <para>The current maximum number of simultaneously audible sounds in the game.</para>
		/// </summary>
		public int numRealVoices;

		/// <summary>
		///   <para>The  maximum number of managed sounds in the game. Beyond this limit sounds will simply stop playing.</para>
		/// </summary>
		public int numVirtualVoices;
	}
}
