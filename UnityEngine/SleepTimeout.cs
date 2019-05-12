using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Constants for special values of Screen.sleepTimeout.</para>
	/// </summary>
	public sealed class SleepTimeout
	{
		/// <summary>
		///   <para>Prevent screen dimming.</para>
		/// </summary>
		public const int NeverSleep = -1;

		/// <summary>
		///   <para>Set the sleep timeout to whatever the user has specified in the system settings.</para>
		/// </summary>
		public const int SystemSetting = -2;
	}
}
