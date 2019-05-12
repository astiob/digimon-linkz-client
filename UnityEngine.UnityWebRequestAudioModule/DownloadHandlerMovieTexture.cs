using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	[NativeHeader("Runtime/Video/MovieTexture.h")]
	[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerMovieTexture.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerMovieTexture : DownloadHandler
	{
	}
}
