using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Audio Distortion Filter distorts the sound from an AudioSource or.</para>
	/// </summary>
	public sealed class AudioDistortionFilter : Behaviour
	{
		/// <summary>
		///   <para>Distortion value. 0.0 to 1.0. Default = 0.5.</para>
		/// </summary>
		public extern float distortionLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
