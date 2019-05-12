using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Describes the type of a COM member.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum ComMemberType
	{
		/// <summary>The member is a normal method.</summary>
		Method,
		/// <summary>The member gets properties.</summary>
		PropGet,
		/// <summary>The member sets properties.</summary>
		PropSet
	}
}
