using System;

namespace System.ComponentModel
{
	/// <summary>Specifies how the collection is changed.</summary>
	public enum CollectionChangeAction
	{
		/// <summary>Specifies that an element was added to the collection.</summary>
		Add = 1,
		/// <summary>Specifies that an element was removed from the collection.</summary>
		Remove,
		/// <summary>Specifies that the entire collection has changed. This is caused by using methods that manipulate the entire collection, such as <see cref="M:System.Collections.CollectionBase.Clear" />.</summary>
		Refresh
	}
}
