using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes location service status.</para>
	/// </summary>
	public enum LocationServiceStatus
	{
		/// <summary>
		///   <para>Location service is stopped.</para>
		/// </summary>
		Stopped,
		/// <summary>
		///   <para>Location service is initializing, some time later it will switch to.</para>
		/// </summary>
		Initializing,
		/// <summary>
		///   <para>Location service is running and locations could be queried.</para>
		/// </summary>
		Running,
		/// <summary>
		///   <para>Location service failed (user denied access to location service).</para>
		/// </summary>
		Failed
	}
}
