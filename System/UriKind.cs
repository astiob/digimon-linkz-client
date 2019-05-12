using System;

namespace System
{
	/// <summary>Defines the kinds of <see cref="T:System.Uri" />s for the <see cref="M:System.Uri.IsWellFormedUriString(System.String,System.UriKind)" /> and several <see cref="Overload:System.Uri.#ctor" /> methods.</summary>
	public enum UriKind
	{
		/// <summary>The kind of the Uri is indeterminate.</summary>
		RelativeOrAbsolute,
		/// <summary>The Uri is an absolute Uri.</summary>
		Absolute,
		/// <summary>The Uri is a relative Uri.</summary>
		Relative
	}
}
