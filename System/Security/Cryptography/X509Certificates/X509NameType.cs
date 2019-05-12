using System;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Specifies the type of name the X509 certificate contains.</summary>
	public enum X509NameType
	{
		/// <summary>The simple name of a subject or issuer of an X509 certificate.</summary>
		SimpleName,
		/// <summary>The email address of the subject or issuer associated of an X509 certificate.</summary>
		EmailName,
		/// <summary>The UPN name of the subject or issuer of an X509 certificate.</summary>
		UpnName,
		/// <summary>The DNS name associated with the alternative name of either the subject or issuer of an X509 certificate.</summary>
		DnsName,
		/// <summary>The DNS name associated with the alternative name of either the subject or the issuer of an X.509 certificate.  This value is equivalent to the <see cref="F:System.Security.Cryptography.X509Certificates.X509NameType.DnsName" /> value.</summary>
		DnsFromAlternativeName,
		/// <summary>The URL address associated with the alternative name of either the subject or issuer of an X509 certificate.</summary>
		UrlName
	}
}
