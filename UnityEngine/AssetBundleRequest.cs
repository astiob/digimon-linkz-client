using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Asynchronous load request from an AssetBundle.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AssetBundleRequest : AsyncOperation
	{
		internal AssetBundle m_AssetBundle;

		internal string m_Path;

		internal Type m_Type;

		/// <summary>
		///   <para>Asset object being loaded (Read Only).</para>
		/// </summary>
		public Object asset
		{
			get
			{
				return this.m_AssetBundle.LoadAsset(this.m_Path, this.m_Type);
			}
		}

		/// <summary>
		///   <para>Asset objects with sub assets being loaded. (Read Only)</para>
		/// </summary>
		public Object[] allAssets
		{
			get
			{
				return this.m_AssetBundle.LoadAssetWithSubAssets_Internal(this.m_Path, this.m_Type);
			}
		}
	}
}
