using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies the way to open the X.509 certificate store.</summary>
	[Flags]
	public enum OpenFlags
	{
		/// <summary>Open the X.509 certificate store for reading only.</summary>
		ReadOnly = 0,
		/// <summary>Open the X.509 certificate store for both reading and writing.</summary>
		ReadWrite = 1,
		/// <summary>Open the X.509 certificate store for the highest access allowed.</summary>
		MaxAllowed = 2,
		/// <summary>Opens only existing stores; if no store exists, the <see cref="M:System.Security.Cryptography.X509Certificates.X509Store.Open(System.Security.Cryptography.X509Certificates.OpenFlags)" /> method will not create a new store.</summary>
		OpenExistingOnly = 4,
		/// <summary>Open the X.509 certificate store and include archived certificates.</summary>
		IncludeArchived = 8
	}
}
