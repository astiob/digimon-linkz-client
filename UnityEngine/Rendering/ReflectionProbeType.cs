using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Reflection probe type: cube or card.</para>
	/// </summary>
	public enum ReflectionProbeType
	{
		/// <summary>
		///   <para>Surrounding of the reflection probe is rendered into cubemap.</para>
		/// </summary>
		Cube,
		/// <summary>
		///   <para>Surrounding of the reflection probe is rendered onto a quad.</para>
		/// </summary>
		Card
	}
}
