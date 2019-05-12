using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Audio Echo Filter repeats a sound after a given Delay, attenuating.</para>
	/// </summary>
	public sealed class AudioEchoFilter : Behaviour
	{
		/// <summary>
		///   <para>Echo delay in ms. 10 to 5000. Default = 500.</para>
		/// </summary>
		public extern float delay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (i.e. simple 1 line delay). Default = 0.5.</para>
		/// </summary>
		public extern float decayRatio { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Volume of original signal to pass to output. 0.0 to 1.0. Default = 1.0.</para>
		/// </summary>
		public extern float dryMix { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Volume of echo signal to pass to output. 0.0 to 1.0. Default = 1.0.</para>
		/// </summary>
		public extern float wetMix { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
