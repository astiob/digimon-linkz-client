using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies characteristics of the X.500 distinguished name.</summary>
	[Flags]
	public enum X500DistinguishedNameFlags
	{
		/// <summary>The distinguished name has no special characteristics.</summary>
		None = 0,
		/// <summary>The distinguished name is reversed.</summary>
		Reversed = 1,
		/// <summary>The distinguished name uses semicolons.</summary>
		UseSemicolons = 16,
		/// <summary>The distinguished name does not use the plus sign.</summary>
		DoNotUsePlusSign = 32,
		/// <summary>The distinguished name does not use quotation marks.</summary>
		DoNotUseQuotes = 64,
		/// <summary>The distinguished name uses commas.</summary>
		UseCommas = 128,
		/// <summary>The distinguished name uses the new line character.</summary>
		UseNewLines = 256,
		/// <summary>The distinguished name uses UTF8 encoding.</summary>
		UseUTF8Encoding = 4096,
		/// <summary>The distinguished name uses T61 encoding.</summary>
		UseT61Encoding = 8192,
		/// <summary>The distinguished name uses UTF8 encoding.</summary>
		ForceUTF8Encoding = 16384
	}
}
