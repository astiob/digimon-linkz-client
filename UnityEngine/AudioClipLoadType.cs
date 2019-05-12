using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Determines how the audio clip is loaded in.</para>
	/// </summary>
	public enum AudioClipLoadType
	{
		/// <summary>
		///   <para>The audio data is decompressed when the audio clip is loaded.</para>
		/// </summary>
		DecompressOnLoad,
		/// <summary>
		///   <para>The audio data of the clip will be kept in memory in compressed form.</para>
		/// </summary>
		CompressedInMemory,
		/// <summary>
		///   <para>Streams audio data from disk.</para>
		/// </summary>
		Streaming
	}
}
