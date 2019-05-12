using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerAudioClip.h")]
	[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerMovieTexture.h")]
	internal static class WebRequestWWW
	{
		[FreeFunction("UnityWebRequestCreateAudioClip")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AudioClip InternalCreateAudioClipUsingDH(DownloadHandler dh, string url, bool stream, bool compressed, AudioType audioType);
	}
}
