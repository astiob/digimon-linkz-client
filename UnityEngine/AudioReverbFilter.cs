using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Audio Reverb Filter takes an Audio Clip and distortionates it in a.</para>
	/// </summary>
	public sealed class AudioReverbFilter : Behaviour
	{
		/// <summary>
		///   <para>Set/Get reverb preset properties.</para>
		/// </summary>
		public extern AudioReverbPreset reverbPreset { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Mix level of dry signal in output in mB. Ranges from -10000.0 to 0.0. Default is 0.</para>
		/// </summary>
		public extern float dryLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Room effect level at low frequencies in mB. Ranges from -10000.0 to 0.0. Default is 0.0.</para>
		/// </summary>
		public extern float room { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Room effect high-frequency level re. low frequency level in mB. Ranges from -10000.0 to 0.0. Default is 0.0.</para>
		/// </summary>
		public extern float roomHF { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Rolloff factor for room effect. Ranges from 0.0 to 10.0. Default is 10.0.</para>
		/// </summary>
		public extern float roomRolloff { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reverberation decay time at low-frequencies in seconds. Ranges from 0.1 to 20.0. Default is 1.0.</para>
		/// </summary>
		public extern float decayTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Decay HF Ratio : High-frequency to low-frequency decay time ratio. Ranges from 0.1 to 2.0. Default is 0.5.</para>
		/// </summary>
		public extern float decayHFRatio { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Early reflections level relative to room effect in mB. Ranges from -10000.0 to 1000.0. Default is -10000.0.</para>
		/// </summary>
		public extern float reflectionsLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Late reverberation level relative to room effect in mB. Ranges from -10000.0 to 2000.0. Default is 0.0.</para>
		/// </summary>
		public extern float reflectionsDelay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Late reverberation level relative to room effect in mB. Ranges from -10000.0 to 2000.0. Default is 0.0.</para>
		/// </summary>
		public extern float reverbLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Late reverberation delay time relative to first reflection in seconds. Ranges from 0.0 to 0.1. Default is 0.04.</para>
		/// </summary>
		public extern float reverbDelay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reverberation diffusion (echo density) in percent. Ranges from 0.0 to 100.0. Default is 100.0.</para>
		/// </summary>
		public extern float diffusion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reverberation density (modal density) in percent. Ranges from 0.0 to 100.0. Default is 100.0.</para>
		/// </summary>
		public extern float density { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reference high frequency in Hz. Ranges from 20.0 to 20000.0. Default is 5000.0.</para>
		/// </summary>
		public extern float hfReference { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Room effect low-frequency level in mB. Ranges from -10000.0 to 0.0. Default is 0.0.</para>
		/// </summary>
		public extern float roomLF { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reference low-frequency in Hz. Ranges from 20.0 to 1000.0. Default is 250.0.</para>
		/// </summary>
		public extern float lfReference { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
