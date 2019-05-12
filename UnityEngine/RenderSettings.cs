using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Render Settings contain values for a range of visual elements in your scene, like fog and ambient light.</para>
	/// </summary>
	public sealed class RenderSettings : Object
	{
		/// <summary>
		///   <para>Is fog enabled?</para>
		/// </summary>
		public static extern bool fog { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Fog mode to use.</para>
		/// </summary>
		public static extern FogMode fogMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The color of the fog.</para>
		/// </summary>
		public static Color fogColor
		{
			get
			{
				Color result;
				RenderSettings.INTERNAL_get_fogColor(out result);
				return result;
			}
			set
			{
				RenderSettings.INTERNAL_set_fogColor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_fogColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_fogColor(ref Color value);

		/// <summary>
		///   <para>The density of the exponential fog.</para>
		/// </summary>
		public static extern float fogDensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The starting distance of linear fog.</para>
		/// </summary>
		public static extern float fogStartDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The ending distance of linear fog.</para>
		/// </summary>
		public static extern float fogEndDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Ambient lighting mode.</para>
		/// </summary>
		public static extern AmbientMode ambientMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Ambient lighting coming from above.</para>
		/// </summary>
		public static Color ambientSkyColor
		{
			get
			{
				Color result;
				RenderSettings.INTERNAL_get_ambientSkyColor(out result);
				return result;
			}
			set
			{
				RenderSettings.INTERNAL_set_ambientSkyColor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_ambientSkyColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_ambientSkyColor(ref Color value);

		/// <summary>
		///   <para>Ambient lighting coming from the sides.</para>
		/// </summary>
		public static Color ambientEquatorColor
		{
			get
			{
				Color result;
				RenderSettings.INTERNAL_get_ambientEquatorColor(out result);
				return result;
			}
			set
			{
				RenderSettings.INTERNAL_set_ambientEquatorColor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_ambientEquatorColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_ambientEquatorColor(ref Color value);

		/// <summary>
		///   <para>Ambient lighting coming from below.</para>
		/// </summary>
		public static Color ambientGroundColor
		{
			get
			{
				Color result;
				RenderSettings.INTERNAL_get_ambientGroundColor(out result);
				return result;
			}
			set
			{
				RenderSettings.INTERNAL_set_ambientGroundColor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_ambientGroundColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_ambientGroundColor(ref Color value);

		/// <summary>
		///   <para>Flat ambient lighting color.</para>
		/// </summary>
		public static Color ambientLight
		{
			get
			{
				Color result;
				RenderSettings.INTERNAL_get_ambientLight(out result);
				return result;
			}
			set
			{
				RenderSettings.INTERNAL_set_ambientLight(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_ambientLight(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_ambientLight(ref Color value);

		/// <summary>
		///   <para>How much the light from the Ambient Source affects the scene.</para>
		/// </summary>
		public static extern float ambientIntensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Custom or skybox ambient lighting data.</para>
		/// </summary>
		public static SphericalHarmonicsL2 ambientProbe
		{
			get
			{
				SphericalHarmonicsL2 result;
				RenderSettings.INTERNAL_get_ambientProbe(out result);
				return result;
			}
			set
			{
				RenderSettings.INTERNAL_set_ambientProbe(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_ambientProbe(out SphericalHarmonicsL2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_ambientProbe(ref SphericalHarmonicsL2 value);

		/// <summary>
		///   <para>How much the skybox / custom cubemap reflection affects the scene.</para>
		/// </summary>
		public static extern float reflectionIntensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The number of times a reflection includes other reflections.</para>
		/// </summary>
		public static extern int reflectionBounces { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Size of the Light halos.</para>
		/// </summary>
		public static extern float haloStrength { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The intensity of all flares in the scene.</para>
		/// </summary>
		public static extern float flareStrength { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The fade speed of all flares in the scene.</para>
		/// </summary>
		public static extern float flareFadeSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The global skybox to use.</para>
		/// </summary>
		public static extern Material skybox { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Default reflection mode.</para>
		/// </summary>
		public static extern DefaultReflectionMode defaultReflectionMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Cubemap resolution for default reflection.</para>
		/// </summary>
		public static extern int defaultReflectionResolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Custom specular reflection cubemap.</para>
		/// </summary>
		public static extern Cubemap customReflection { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Reset();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Object GetRenderSettings();
	}
}
