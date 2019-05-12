using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>These are speaker types defined for use with AudioSettings.speakerMode.</para>
	/// </summary>
	public enum AudioSpeakerMode
	{
		/// <summary>
		///   <para>Channel count is unaffected.</para>
		/// </summary>
		Raw,
		/// <summary>
		///   <para>Channel count is set to 1. The speakers are monaural.</para>
		/// </summary>
		Mono,
		/// <summary>
		///   <para>Channel count is set to 2. The speakers are stereo. This is the editor default.</para>
		/// </summary>
		Stereo,
		/// <summary>
		///   <para>Channel count is set to 4. 4 speaker setup. This includes front left, front right, rear left, rear right.</para>
		/// </summary>
		Quad,
		/// <summary>
		///   <para>Channel count is set to 5. 5 speaker setup. This includes front left, front right, center, rear left, rear right.</para>
		/// </summary>
		Surround,
		/// <summary>
		///   <para>Channel count is set to 6. 5.1 speaker setup. This includes front left, front right, center, rear left, rear right and a subwoofer.</para>
		/// </summary>
		Mode5point1,
		/// <summary>
		///   <para>Channel count is set to 8. 7.1 speaker setup. This includes front left, front right, center, rear left, rear right, side left, side right and a subwoofer.</para>
		/// </summary>
		Mode7point1,
		/// <summary>
		///   <para>Channel count is set to 2. Stereo output, but data is encoded in a way that is picked up by a Prologic/Prologic2 decoder and split into a 5.1 speaker setup.</para>
		/// </summary>
		Prologic
	}
}
