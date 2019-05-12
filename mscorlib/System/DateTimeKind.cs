using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Specifies whether a <see cref="T:System.DateTime" /> object represents a local time, a Coordinated Universal Time (UTC), or is not specified as either local time or UTC.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public enum DateTimeKind
	{
		/// <summary>The time represented is not specified as either local time or Coordinated Universal Time (UTC).</summary>
		Unspecified,
		/// <summary>The time represented is UTC.</summary>
		Utc,
		/// <summary>The time represented is local time.</summary>
		Local
	}
}
