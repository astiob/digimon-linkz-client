using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>A DownloadHandler subclass specialized for downloading AssetBundles.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerAssetBundle : DownloadHandler
	{
		/// <summary>
		///   <para>Not implemented. Throws &lt;a href="http:msdn.microsoft.comen-uslibrarysystem.notsupportedexception"&gt;NotSupportedException&lt;a&gt;.</para>
		/// </summary>
		/// <returns>
		///   <para>Not implemented.</para>
		/// </returns>
		protected override byte[] GetData()
		{
			throw new NotSupportedException("Raw data access is not supported for asset bundles");
		}

		/// <summary>
		///   <para>Not implemented. Throws &lt;a href="http:msdn.microsoft.comen-uslibrarysystem.notsupportedexception"&gt;NotSupportedException&lt;a&gt;.</para>
		/// </summary>
		/// <returns>
		///   <para>Not implemented.</para>
		/// </returns>
		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for asset bundles");
		}
	}
}
