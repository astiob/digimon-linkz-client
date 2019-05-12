using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Specifies whether white space should be ignored in the base 64 transformation.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum FromBase64TransformMode
	{
		/// <summary>White space should be ignored.</summary>
		IgnoreWhiteSpaces,
		/// <summary>White space should not be ignored.</summary>
		DoNotIgnoreWhiteSpaces
	}
}
