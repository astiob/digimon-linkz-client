using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Rolloff modes that a 3D sound can have in an audio source.</para>
	/// </summary>
	public enum AudioRolloffMode
	{
		/// <summary>
		///   <para>Use this mode when you want a real-world rolloff.</para>
		/// </summary>
		Logarithmic,
		/// <summary>
		///   <para>Use this mode when you want to lower the volume of your sound over the distance.</para>
		/// </summary>
		Linear,
		/// <summary>
		///   <para>Use this when you want to use a custom rolloff.</para>
		/// </summary>
		Custom
	}
}
