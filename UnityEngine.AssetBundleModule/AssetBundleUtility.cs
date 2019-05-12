using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/AssetBundle/Public/AssetBundlePatching.h")]
	internal static class AssetBundleUtility
	{
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PatchAssetBundles(AssetBundle[] bundles, string[] filenames);
	}
}
