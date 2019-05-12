using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	/// <summary>Specifies the type of Windows account used.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum WindowsAccountType
	{
		/// <summary>A normal user account.</summary>
		Normal,
		/// <summary>A Windows guest account.</summary>
		Guest,
		/// <summary>A Windows system account.</summary>
		System,
		/// <summary>An anonymous account.</summary>
		Anonymous
	}
}
