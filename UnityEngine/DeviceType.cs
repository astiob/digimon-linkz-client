using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Enumeration for SystemInfo.deviceType, denotes a coarse grouping of kinds of devices.</para>
	/// </summary>
	public enum DeviceType
	{
		/// <summary>
		///   <para>Device type is unknown. You should never see this in practice.</para>
		/// </summary>
		Unknown,
		/// <summary>
		///   <para>A handheld device like mobile phone or a tablet.</para>
		/// </summary>
		Handheld,
		/// <summary>
		///   <para>A stationary gaming console.</para>
		/// </summary>
		Console,
		/// <summary>
		///   <para>Desktop or laptop computer.</para>
		/// </summary>
		Desktop
	}
}
