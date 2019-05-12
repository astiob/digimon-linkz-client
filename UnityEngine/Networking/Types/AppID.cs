using System;
using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
	/// <summary>
	///   <para>The AppID identifies the application on the Unity Cloud or UNET servers.</para>
	/// </summary>
	[DefaultValue(18446744073709551615UL)]
	public enum AppID : ulong
	{
		/// <summary>
		///   <para>Invalid AppID.</para>
		/// </summary>
		Invalid = 18446744073709551615UL
	}
}
