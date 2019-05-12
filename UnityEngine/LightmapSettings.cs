using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Stores lightmaps of the scene.</para>
	/// </summary>
	public sealed class LightmapSettings : Object
	{
		/// <summary>
		///   <para>Lightmap array.</para>
		/// </summary>
		public static extern LightmapData[] lightmaps { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Use lightmapsMode property")]
		public static extern LightmapsModeLegacy lightmapsModeLegacy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Non-directional, Directional or Directional Specular lightmaps rendering mode.</para>
		/// </summary>
		public static extern LightmapsMode lightmapsMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Color space of the lightmap.</para>
		/// </summary>
		[Obsolete("bakedColorSpace is no longer valid. Use QualitySettings.desiredColorSpace.", false)]
		public static ColorSpace bakedColorSpace
		{
			get
			{
				return QualitySettings.desiredColorSpace;
			}
			set
			{
			}
		}

		/// <summary>
		///   <para>Holds all data needed by the light probes.</para>
		/// </summary>
		public static extern LightProbes lightProbes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Reset();
	}
}
