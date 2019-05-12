using System;

namespace System.Xml
{
	/// <summary>Specifies how to treat the time value when converting between string and <see cref="T:System.DateTime" />.</summary>
	public enum XmlDateTimeSerializationMode
	{
		/// <summary>Treat as local time. If the <see cref="T:System.DateTime" /> object represents a Coordinated Universal Time (UTC), it is converted to the local time.</summary>
		Local,
		/// <summary>Treat as a UTC. If the <see cref="T:System.DateTime" /> object represents a local time, it is converted to a UTC.</summary>
		Utc,
		/// <summary>Treat as a local time if a <see cref="T:System.DateTime" /> is being converted to a string.</summary>
		Unspecified,
		/// <summary>Time zone information should be preserved when converting.</summary>
		RoundtripKind
	}
}
