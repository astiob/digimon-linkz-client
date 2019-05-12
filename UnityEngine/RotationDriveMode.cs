using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Control ConfigurableJoint's rotation with either X &amp; YZ or Slerp Drive.</para>
	/// </summary>
	public enum RotationDriveMode
	{
		/// <summary>
		///   <para>Use XY &amp; Z Drive.</para>
		/// </summary>
		XYAndZ,
		/// <summary>
		///   <para>Use Slerp drive.</para>
		/// </summary>
		Slerp
	}
}
