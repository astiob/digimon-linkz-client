using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Reflection probe's update mode.</para>
	/// </summary>
	public enum ReflectionProbeMode
	{
		/// <summary>
		///   <para>Reflection probe is baked in the Editor.</para>
		/// </summary>
		Baked,
		/// <summary>
		///   <para>Reflection probe is updating in realtime.</para>
		/// </summary>
		Realtime,
		/// <summary>
		///   <para>Reflection probe uses a custom texture specified by the user.</para>
		/// </summary>
		Custom
	}
}
