using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerAssetBundle.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerAssetBundle : DownloadHandler
	{
		public DownloadHandlerAssetBundle(string url, uint crc)
		{
			this.InternalCreateAssetBundle(url, crc);
		}

		public DownloadHandlerAssetBundle(string url, uint version, uint crc)
		{
			this.InternalCreateAssetBundleCached(url, "", new Hash128(0u, 0u, 0u, version), crc);
		}

		public DownloadHandlerAssetBundle(string url, Hash128 hash, uint crc)
		{
			this.InternalCreateAssetBundleCached(url, "", hash, crc);
		}

		public DownloadHandlerAssetBundle(string url, string name, Hash128 hash, uint crc)
		{
			this.InternalCreateAssetBundleCached(url, name, hash, crc);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerAssetBundle obj, string url, uint crc);

		private static IntPtr CreateCached(DownloadHandlerAssetBundle obj, string url, string name, Hash128 hash, uint crc)
		{
			return DownloadHandlerAssetBundle.CreateCached_Injected(obj, url, name, ref hash, crc);
		}

		private void InternalCreateAssetBundle(string url, uint crc)
		{
			this.m_Ptr = DownloadHandlerAssetBundle.Create(this, url, crc);
		}

		private void InternalCreateAssetBundleCached(string url, string name, Hash128 hash, uint crc)
		{
			this.m_Ptr = DownloadHandlerAssetBundle.CreateCached(this, url, name, hash, crc);
		}

		protected override byte[] GetData()
		{
			throw new NotSupportedException("Raw data access is not supported for asset bundles");
		}

		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for asset bundles");
		}

		public extern AssetBundle assetBundle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static AssetBundle GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerAssetBundle>(www).assetBundle;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateCached_Injected(DownloadHandlerAssetBundle obj, string url, string name, ref Hash128 hash, uint crc);
	}
}
