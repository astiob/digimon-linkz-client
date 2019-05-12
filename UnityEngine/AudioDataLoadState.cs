using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Value describing the current load state of the audio data associated with an AudioClip.</para>
	/// </summary>
	public enum AudioDataLoadState
	{
		/// <summary>
		///   <para>Value returned by AudioClip.loadState for an AudioClip that has no audio data loaded and where loading has not been initiated yet.</para>
		/// </summary>
		Unloaded,
		/// <summary>
		///   <para>Value returned by AudioClip.loadState for an AudioClip that is currently loading audio data.</para>
		/// </summary>
		Loading,
		/// <summary>
		///   <para>Value returned by AudioClip.loadState for an AudioClip that has succeeded loading its audio data.</para>
		/// </summary>
		Loaded,
		/// <summary>
		///   <para>Value returned by AudioClip.loadState for an AudioClip that has failed loading its audio data.</para>
		/// </summary>
		Failed
	}
}
