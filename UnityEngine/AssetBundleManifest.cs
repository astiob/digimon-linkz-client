using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Manifest for all the assetBundle in the build.</para>
	/// </summary>
	public sealed class AssetBundleManifest : Object
	{
		/// <summary>
		///   <para>Get all the AssetBundles in the manifest.</para>
		/// </summary>
		/// <returns>
		///   <para>An array of asset bundle names.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string[] GetAllAssetBundles();

		/// <summary>
		///   <para>Get all the AssetBundles with variant in the manifest.</para>
		/// </summary>
		/// <returns>
		///   <para>An array of asset bundle names.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string[] GetAllAssetBundlesWithVariant();

		/// <summary>
		///   <para>Get the hash for the given AssetBundle.</para>
		/// </summary>
		/// <param name="assetBundleName">Name of the asset bundle.</param>
		/// <returns>
		///   <para>The 128-bit hash for the asset bundle.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Hash128 GetAssetBundleHash(string assetBundleName);

		/// <summary>
		///   <para>Get the direct dependent AssetBundles for the given AssetBundle.</para>
		/// </summary>
		/// <param name="assetBundleName">Name of the asset bundle.</param>
		/// <returns>
		///   <para>Array of asset bundle names this asset bundle depends on.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string[] GetDirectDependencies(string assetBundleName);

		/// <summary>
		///   <para>Get all the dependent AssetBundles for the given AssetBundle.</para>
		/// </summary>
		/// <param name="assetBundleName">Name of the asset bundle.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string[] GetAllDependencies(string assetBundleName);
	}
}
