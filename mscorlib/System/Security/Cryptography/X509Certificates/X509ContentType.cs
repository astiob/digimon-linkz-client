using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies the format of an X.509 certificate. </summary>
	[ComVisible(true)]
	public enum X509ContentType
	{
		/// <summary>An unknown X.509 certificate.  </summary>
		Unknown,
		/// <summary>A single X.509 certificate.</summary>
		Cert,
		/// <summary>A single serialized X.509 certificate. </summary>
		SerializedCert,
		/// <summary>A PFX-formatted certificate. The Pfx value is identical to the Pkcs12 value.</summary>
		Pfx,
		/// <summary>A serialized store.</summary>
		SerializedStore,
		/// <summary>A PKCS #7–formatted certificate.</summary>
		Pkcs7,
		/// <summary>An Authenticode X.509 certificate. </summary>
		Authenticode,
		/// <summary>A PKCS #12–formatted certificate. The Pkcs12 value is identical to the Pfx value.</summary>
		Pkcs12 = 3
	}
}
