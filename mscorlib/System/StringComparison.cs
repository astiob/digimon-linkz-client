using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Specifies the culture, case, and sort rules to be used by certain overloads of the <see cref="M:System.String.Compare(System.String,System.String)" /> and <see cref="M:System.String.Equals(System.Object)" /> methods.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public enum StringComparison
	{
		/// <summary>Compare strings using culture-sensitive sort rules and the current culture.</summary>
		CurrentCulture,
		/// <summary>Compare strings using culture-sensitive sort rules, the current culture, and ignoring the case of the strings being compared.</summary>
		CurrentCultureIgnoreCase,
		/// <summary>Compare strings using culture-sensitive sort rules and the invariant culture.</summary>
		InvariantCulture,
		/// <summary>Compare strings using culture-sensitive sort rules, the invariant culture, and ignoring the case of the strings being compared.</summary>
		InvariantCultureIgnoreCase,
		/// <summary>Compare strings using ordinal sort rules.</summary>
		Ordinal,
		/// <summary>Compare strings using ordinal sort rules and ignoring the case of the strings being compared.</summary>
		OrdinalIgnoreCase
	}
}
