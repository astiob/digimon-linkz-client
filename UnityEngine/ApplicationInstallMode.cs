using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Application installation mode (Read Only).</para>
	/// </summary>
	public enum ApplicationInstallMode
	{
		/// <summary>
		///   <para>Application install mode unknown.</para>
		/// </summary>
		Unknown,
		/// <summary>
		///   <para>Application installed via online store.</para>
		/// </summary>
		Store,
		/// <summary>
		///   <para>Application installed via developer build.</para>
		/// </summary>
		DeveloperBuild,
		/// <summary>
		///   <para>Application installed via ad hoc distribution.</para>
		/// </summary>
		Adhoc,
		/// <summary>
		///   <para>Application installed via enterprise distribution.</para>
		/// </summary>
		Enterprise,
		/// <summary>
		///   <para>Application running in editor.</para>
		/// </summary>
		Editor
	}
}
