using System;

namespace System
{
	/// <summary>Defines the parts of a URI for the <see cref="M:System.Uri.GetLeftPart(System.UriPartial)" /> method.</summary>
	/// <filterpriority>2</filterpriority>
	public enum UriPartial
	{
		/// <summary>The scheme segment of the URI.</summary>
		Scheme,
		/// <summary>The scheme and authority segments of the URI.</summary>
		Authority,
		/// <summary>The scheme, authority, and path segments of the URI.</summary>
		Path,
		/// <summary>The scheme, authority, path, and query segments of the URI.</summary>
		Query
	}
}
