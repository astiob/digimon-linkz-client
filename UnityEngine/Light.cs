using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>Script interface for.</para>
	/// </summary>
	public sealed class Light : Behaviour
	{
		/// <summary>
		///   <para>The type of the light.</para>
		/// </summary>
		public extern LightType type { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The color of the light.</para>
		/// </summary>
		public Color color
		{
			get
			{
				Color result;
				this.INTERNAL_get_color(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_color(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_color(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_color(ref Color value);

		/// <summary>
		///   <para>The Intensity of a light is multiplied with the Light color.</para>
		/// </summary>
		public extern float intensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The multiplier that defines the strength of the bounce lighting.</para>
		/// </summary>
		public extern float bounceIntensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How this light casts shadows</para>
		/// </summary>
		public extern LightShadows shadows { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Strength of light's shadows.</para>
		/// </summary>
		public extern float shadowStrength { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Shadow mapping constant bias.</para>
		/// </summary>
		public extern float shadowBias { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Shadow mapping normal-based bias.</para>
		/// </summary>
		public extern float shadowNormalBias { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Shadow softness is removed in Unity 5.0+")]
		public extern float shadowSoftness { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Shadow softness is removed in Unity 5.0+")]
		public extern float shadowSoftnessFade { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The range of the light.</para>
		/// </summary>
		public extern float range { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The angle of the light's spotlight cone in degrees.</para>
		/// </summary>
		public extern float spotAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The size of a directional light's cookie.</para>
		/// </summary>
		public extern float cookieSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The cookie texture projected by the light.</para>
		/// </summary>
		public extern Texture cookie { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The to use for this light.</para>
		/// </summary>
		public extern Flare flare { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How to render the light.</para>
		/// </summary>
		public extern LightRenderMode renderMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Has the light already been lightmapped.</para>
		/// </summary>
		public extern bool alreadyLightmapped { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>This is used to light certain objects in the scene selectively.</para>
		/// </summary>
		public extern int cullingMask { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Add a command buffer to be executed at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		/// <param name="buffer">The buffer to execute.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddCommandBuffer(LightEvent evt, CommandBuffer buffer);

		/// <summary>
		///   <para>Remove command buffer from execution at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		/// <param name="buffer">The buffer to execute.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffer(LightEvent evt, CommandBuffer buffer);

		/// <summary>
		///   <para>Remove command buffers from execution at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffers(LightEvent evt);

		/// <summary>
		///   <para>Remove all command buffers set on this light.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveAllCommandBuffers();

		/// <summary>
		///   <para>Get command buffers to be executed at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		/// <returns>
		///   <para>Array of command buffers.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CommandBuffer[] GetCommandBuffers(LightEvent evt);

		/// <summary>
		///   <para>Number of command buffers set up on this light (Read Only).</para>
		/// </summary>
		public extern int commandBufferCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern int pixelLightCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Light[] GetLights(LightType type, int layer);

		[Obsolete("light.shadowConstantBias was removed, use light.shadowBias", true)]
		public float shadowConstantBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		[Obsolete("light.shadowObjectSizeBias was removed, use light.shadowBias", true)]
		public float shadowObjectSizeBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		[Obsolete("light.attenuate was removed; all lights always attenuate now", true)]
		public bool attenuate
		{
			get
			{
				return true;
			}
			set
			{
			}
		}
	}
}
