using System;

namespace System
{
	/// <summary>Controls how URI information is escaped.</summary>
	/// <filterpriority>1</filterpriority>
	public enum UriFormat
	{
		/// <summary>Escaping is performed according to the rules in RFC 2396.</summary>
		UriEscaped = 1,
		/// <summary>No escaping is performed.</summary>
		Unescaped,
		/// <summary>Characters that have a reserved meaning in the requested URI components remain escaped. All others are not escaped. See Remarks.</summary>
		SafeUnescaped
	}
}
