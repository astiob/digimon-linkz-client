using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Audio High Pass Filter passes high frequencies of an AudioSource and.</para>
	/// </summary>
	public sealed class AudioHighPassFilter : Behaviour
	{
		/// <summary>
		///   <para>Highpass cutoff frequency in hz. 10.0 to 22000.0. Default = 5000.0.</para>
		/// </summary>
		public extern float cutoffFrequency { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Determines how much the filter's self-resonance isdampened.</para>
		/// </summary>
		public extern float highpassResonanceQ { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
