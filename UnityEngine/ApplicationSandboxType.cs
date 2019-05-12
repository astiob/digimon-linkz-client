using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Application sandbox type.</para>
	/// </summary>
	public enum ApplicationSandboxType
	{
		/// <summary>
		///   <para>Application sandbox type is unknown.</para>
		/// </summary>
		Unknown,
		/// <summary>
		///   <para>Application not running in a sandbox.</para>
		/// </summary>
		NotSandboxed,
		/// <summary>
		///   <para>Application is running in a sandbox.</para>
		/// </summary>
		Sandboxed,
		/// <summary>
		///   <para>Application is running in broken sandbox.</para>
		/// </summary>
		SandboxBroken
	}
}
