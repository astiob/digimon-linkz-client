using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Filters the classes represented in an array of <see cref="T:System.Type" /> objects.</summary>
	/// <returns>true to include the <see cref="T:System.Type" /> in the filtered list; otherwise false.</returns>
	/// <param name="m">The Type object to which the filter is applied. </param>
	/// <param name="filterCriteria">An arbitrary object used to filter the list. </param>
	[ComVisible(true)]
	[Serializable]
	public delegate bool TypeFilter(Type m, object filterCriteria);
}
