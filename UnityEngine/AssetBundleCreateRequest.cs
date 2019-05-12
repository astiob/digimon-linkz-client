using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Asynchronous create request for an AssetBundle.</para>
	/// </summary>
	public sealed class AssetBundleCreateRequest : AsyncOperation
	{
		/// <summary>
		///   <para>Asset object being loaded (Read Only).</para>
		/// </summary>
		public extern AssetBundle assetBundle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void DisableCompatibilityChecks();
	}
}
