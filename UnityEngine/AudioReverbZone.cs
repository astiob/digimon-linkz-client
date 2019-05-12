using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Reverb Zones are used when you want to create location based ambient effects in the scene.</para>
	/// </summary>
	public sealed class AudioReverbZone : Behaviour
	{
		/// <summary>
		///   <para>The distance from the centerpoint that the reverb will have full effect at. Default = 10.0.</para>
		/// </summary>
		public extern float minDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The distance from the centerpoint that the reverb will not have any effect. Default = 15.0.</para>
		/// </summary>
		public extern float maxDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set/Get reverb preset properties.</para>
		/// </summary>
		public extern AudioReverbPreset reverbPreset { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Room effect level (at mid frequencies).</para>
		/// </summary>
		public extern int room { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Relative room effect level at high frequencies.</para>
		/// </summary>
		public extern int roomHF { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Relative room effect level at low frequencies.</para>
		/// </summary>
		public extern int roomLF { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reverberation decay time at mid frequencies.</para>
		/// </summary>
		public extern float decayTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>High-frequency to mid-frequency decay time ratio.</para>
		/// </summary>
		public extern float decayHFRatio { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Early reflections level relative to room effect.</para>
		/// </summary>
		public extern int reflections { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Initial reflection delay time.</para>
		/// </summary>
		public extern float reflectionsDelay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Late reverberation level relative to room effect.</para>
		/// </summary>
		public extern int reverb { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Late reverberation delay time relative to initial reflection.</para>
		/// </summary>
		public extern float reverbDelay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reference high frequency (hz).</para>
		/// </summary>
		public extern float HFReference { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reference low frequency (hz).</para>
		/// </summary>
		public extern float LFReference { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Like rolloffscale in global settings, but for reverb room size effect.</para>
		/// </summary>
		public extern float roomRolloffFactor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Value that controls the echo density in the late reverberation decay.</para>
		/// </summary>
		public extern float diffusion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Value that controls the modal density in the late reverberation decay.</para>
		/// </summary>
		public extern float density { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
