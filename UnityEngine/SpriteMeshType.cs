using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Defines the type of mesh generated for a sprite.</para>
	/// </summary>
	public enum SpriteMeshType
	{
		/// <summary>
		///   <para>Rectangle mesh equal to the user specified sprite size.</para>
		/// </summary>
		FullRect,
		/// <summary>
		///   <para>Tight mesh based on pixel alpha values. As many excess pixels are cropped as possible.</para>
		/// </summary>
		Tight
	}
}
