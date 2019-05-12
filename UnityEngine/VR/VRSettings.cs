using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.VR
{
	/// <summary>
	///   <para>Global VR related settings.</para>
	/// </summary>
	public sealed class VRSettings
	{
		/// <summary>
		///   <para>Globally enables or disables VR for the application.</para>
		/// </summary>
		public static extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Mirror what is shown on the device to the main display, if possible.</para>
		/// </summary>
		public static extern bool showDeviceView { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Controls the texel:pixel ratio before lens correction, trading performance for sharpness.</para>
		/// </summary>
		public static extern float renderScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Type of VR device that is currently in use.</para>
		/// </summary>
		public static extern VRDeviceType loadedDevice { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
