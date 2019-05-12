using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Constants to pass to Application.RequestUserAuthorization.</para>
	/// </summary>
	public enum UserAuthorization
	{
		/// <summary>
		///   <para>Request permission to use any video input sources attached to the computer.</para>
		/// </summary>
		WebCam = 1,
		/// <summary>
		///   <para>Request permission to use any audio input sources attached to the computer.</para>
		/// </summary>
		Microphone
	}
}
