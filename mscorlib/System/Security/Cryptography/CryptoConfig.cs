using Mono.Xml;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Accesses the cryptography configuration information.</summary>
	[ComVisible(true)]
	public class CryptoConfig
	{
		private const string defaultNamespace = "System.Security.Cryptography.";

		private const string defaultSHA1 = "System.Security.Cryptography.SHA1CryptoServiceProvider";

		private const string defaultMD5 = "System.Security.Cryptography.MD5CryptoServiceProvider";

		private const string defaultSHA256 = "System.Security.Cryptography.SHA256Managed";

		private const string defaultSHA384 = "System.Security.Cryptography.SHA384Managed";

		private const string defaultSHA512 = "System.Security.Cryptography.SHA512Managed";

		private const string defaultRSA = "System.Security.Cryptography.RSACryptoServiceProvider";

		private const string defaultDSA = "System.Security.Cryptography.DSACryptoServiceProvider";

		private const string defaultDES = "System.Security.Cryptography.DESCryptoServiceProvider";

		private const string default3DES = "System.Security.Cryptography.TripleDESCryptoServiceProvider";

		private const string defaultRC2 = "System.Security.Cryptography.RC2CryptoServiceProvider";

		private const string defaultAES = "System.Security.Cryptography.RijndaelManaged";

		private const string defaultRNG = "System.Security.Cryptography.RNGCryptoServiceProvider";

		private const string defaultHMAC = "System.Security.Cryptography.HMACSHA1";

		private const string defaultMAC3DES = "System.Security.Cryptography.MACTripleDES";

		private const string defaultDSASigDesc = "System.Security.Cryptography.DSASignatureDescription";

		private const string defaultRSASigDesc = "System.Security.Cryptography.RSAPKCS1SHA1SignatureDescription";

		private const string defaultRIPEMD160 = "System.Security.Cryptography.RIPEMD160Managed";

		private const string defaultHMACMD5 = "System.Security.Cryptography.HMACMD5";

		private const string defaultHMACRIPEMD160 = "System.Security.Cryptography.HMACRIPEMD160";

		private const string defaultHMACSHA256 = "System.Security.Cryptography.HMACSHA256";

		private const string defaultHMACSHA384 = "System.Security.Cryptography.HMACSHA384";

		private const string defaultHMACSHA512 = "System.Security.Cryptography.HMACSHA512";

		private const string defaultC14N = "System.Security.Cryptography.Xml.XmlDsigC14NTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultC14NWithComments = "System.Security.Cryptography.Xml.XmlDsigC14NWithCommentsTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultBase64 = "System.Security.Cryptography.Xml.XmlDsigBase64Transform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultXPath = "System.Security.Cryptography.Xml.XmlDsigXPathTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultXslt = "System.Security.Cryptography.Xml.XmlDsigXsltTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultEnveloped = "System.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultXmlDecryption = "System.Security.Cryptography.Xml.XmlDecryptionTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultExcC14N = "System.Security.Cryptography.Xml.XmlDsigExcC14NTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultExcC14NWithComments = "System.Security.Cryptography.Xml.XmlDsigExcC14NWithCommentsTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultX509Data = "System.Security.Cryptography.Xml.KeyInfoX509Data, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultKeyName = "System.Security.Cryptography.Xml.KeyInfoName, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultKeyValueDSA = "System.Security.Cryptography.Xml.DSAKeyValue, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultKeyValueRSA = "System.Security.Cryptography.Xml.RSAKeyValue, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string defaultRetrievalMethod = "System.Security.Cryptography.Xml.KeyInfoRetrievalMethod, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		private const string managedSHA1 = "System.Security.Cryptography.SHA1Managed";

		private const string oidSHA1 = "1.3.14.3.2.26";

		private const string oidMD5 = "1.2.840.113549.2.5";

		private const string oidSHA256 = "2.16.840.1.101.3.4.2.1";

		private const string oidSHA384 = "2.16.840.1.101.3.4.2.2";

		private const string oidSHA512 = "2.16.840.1.101.3.4.2.3";

		private const string oidDSA = "1.2.840.10040.4.1";

		private const string oidDES = "1.3.14.3.2.7";

		private const string oid3DES = "1.2.840.113549.3.7";

		private const string oidRC2 = "1.2.840.113549.3.2";

		private const string oid3DESKeyWrap = "1.2.840.113549.1.9.16.3.6";

		private const string nameSHA1a = "SHA";

		private const string nameSHA1b = "SHA1";

		private const string nameSHA1c = "System.Security.Cryptography.SHA1";

		private const string nameSHA1d = "System.Security.Cryptography.HashAlgorithm";

		private const string nameMD5a = "MD5";

		private const string nameMD5b = "System.Security.Cryptography.MD5";

		private const string nameSHA256a = "SHA256";

		private const string nameSHA256b = "SHA-256";

		private const string nameSHA256c = "System.Security.Cryptography.SHA256";

		private const string nameSHA384a = "SHA384";

		private const string nameSHA384b = "SHA-384";

		private const string nameSHA384c = "System.Security.Cryptography.SHA384";

		private const string nameSHA512a = "SHA512";

		private const string nameSHA512b = "SHA-512";

		private const string nameSHA512c = "System.Security.Cryptography.SHA512";

		private const string nameRSAa = "RSA";

		private const string nameRSAb = "System.Security.Cryptography.RSA";

		private const string nameRSAc = "System.Security.Cryptography.AsymmetricAlgorithm";

		private const string nameDSAa = "DSA";

		private const string nameDSAb = "System.Security.Cryptography.DSA";

		private const string nameDESa = "DES";

		private const string nameDESb = "System.Security.Cryptography.DES";

		private const string name3DESa = "3DES";

		private const string name3DESb = "TripleDES";

		private const string name3DESc = "Triple DES";

		private const string name3DESd = "System.Security.Cryptography.TripleDES";

		private const string nameRC2a = "RC2";

		private const string nameRC2b = "System.Security.Cryptography.RC2";

		private const string nameAESa = "Rijndael";

		private const string nameAESb = "System.Security.Cryptography.Rijndael";

		private const string nameAESc = "System.Security.Cryptography.SymmetricAlgorithm";

		private const string nameRNGa = "RandomNumberGenerator";

		private const string nameRNGb = "System.Security.Cryptography.RandomNumberGenerator";

		private const string nameKeyHasha = "System.Security.Cryptography.KeyedHashAlgorithm";

		private const string nameHMACSHA1a = "HMACSHA1";

		private const string nameHMACSHA1b = "System.Security.Cryptography.HMACSHA1";

		private const string nameMAC3DESa = "MACTripleDES";

		private const string nameMAC3DESb = "System.Security.Cryptography.MACTripleDES";

		private const string name3DESKeyWrap = "TripleDESKeyWrap";

		private const string nameRIPEMD160a = "RIPEMD160";

		private const string nameRIPEMD160b = "RIPEMD-160";

		private const string nameRIPEMD160c = "System.Security.Cryptography.RIPEMD160";

		private const string nameHMACa = "HMAC";

		private const string nameHMACb = "System.Security.Cryptography.HMAC";

		private const string nameHMACMD5a = "HMACMD5";

		private const string nameHMACMD5b = "System.Security.Cryptography.HMACMD5";

		private const string nameHMACRIPEMD160a = "HMACRIPEMD160";

		private const string nameHMACRIPEMD160b = "System.Security.Cryptography.HMACRIPEMD160";

		private const string nameHMACSHA256a = "HMACSHA256";

		private const string nameHMACSHA256b = "System.Security.Cryptography.HMACSHA256";

		private const string nameHMACSHA384a = "HMACSHA384";

		private const string nameHMACSHA384b = "System.Security.Cryptography.HMACSHA384";

		private const string nameHMACSHA512a = "HMACSHA512";

		private const string nameHMACSHA512b = "System.Security.Cryptography.HMACSHA512";

		private const string urlXmlDsig = "http://www.w3.org/2000/09/xmldsig#";

		private const string urlDSASHA1 = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

		private const string urlRSASHA1 = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

		private const string urlSHA1 = "http://www.w3.org/2000/09/xmldsig#sha1";

		private const string urlC14N = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";

		private const string urlC14NWithComments = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";

		private const string urlBase64 = "http://www.w3.org/2000/09/xmldsig#base64";

		private const string urlXPath = "http://www.w3.org/TR/1999/REC-xpath-19991116";

		private const string urlXslt = "http://www.w3.org/TR/1999/REC-xslt-19991116";

		private const string urlEnveloped = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

		private const string urlXmlDecryption = "http://www.w3.org/2002/07/decrypt#XML";

		private const string urlExcC14NWithComments = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";

		private const string urlExcC14N = "http://www.w3.org/2001/10/xml-exc-c14n#";

		private const string urlSHA256 = "http://www.w3.org/2001/04/xmlenc#sha256";

		private const string urlSHA512 = "http://www.w3.org/2001/04/xmlenc#sha512";

		private const string urlHMACSHA256 = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

		private const string urlHMACSHA384 = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384";

		private const string urlHMACSHA512 = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512";

		private const string urlHMACRIPEMD160 = "http://www.w3.org/2001/04/xmldsig-more#hmac-ripemd160";

		private const string urlX509Data = "http://www.w3.org/2000/09/xmldsig# X509Data";

		private const string urlKeyName = "http://www.w3.org/2000/09/xmldsig# KeyName";

		private const string urlKeyValueDSA = "http://www.w3.org/2000/09/xmldsig# KeyValue/DSAKeyValue";

		private const string urlKeyValueRSA = "http://www.w3.org/2000/09/xmldsig# KeyValue/RSAKeyValue";

		private const string urlRetrievalMethod = "http://www.w3.org/2000/09/xmldsig# RetrievalMethod";

		private const string oidX509SubjectKeyIdentifier = "2.5.29.14";

		private const string oidX509KeyUsage = "2.5.29.15";

		private const string oidX509BasicConstraints = "2.5.29.19";

		private const string oidX509EnhancedKeyUsage = "2.5.29.37";

		private const string nameX509SubjectKeyIdentifier = "System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string nameX509KeyUsage = "System.Security.Cryptography.X509Certificates.X509KeyUsageExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string nameX509BasicConstraints = "System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string nameX509EnhancedKeyUsage = "System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private const string nameX509Chain = "X509Chain";

		private const string defaultX509Chain = "System.Security.Cryptography.X509Certificates.X509Chain, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		private static object lockObject = new object();

		private static Hashtable algorithms;

		private static Hashtable oid;

		private static void Initialize()
		{
			Hashtable hashtable = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
			hashtable.Add("SHA", "System.Security.Cryptography.SHA1CryptoServiceProvider");
			hashtable.Add("SHA1", "System.Security.Cryptography.SHA1CryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.SHA1", "System.Security.Cryptography.SHA1CryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.HashAlgorithm", "System.Security.Cryptography.SHA1CryptoServiceProvider");
			hashtable.Add("MD5", "System.Security.Cryptography.MD5CryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.MD5", "System.Security.Cryptography.MD5CryptoServiceProvider");
			hashtable.Add("SHA256", "System.Security.Cryptography.SHA256Managed");
			hashtable.Add("SHA-256", "System.Security.Cryptography.SHA256Managed");
			hashtable.Add("System.Security.Cryptography.SHA256", "System.Security.Cryptography.SHA256Managed");
			hashtable.Add("SHA384", "System.Security.Cryptography.SHA384Managed");
			hashtable.Add("SHA-384", "System.Security.Cryptography.SHA384Managed");
			hashtable.Add("System.Security.Cryptography.SHA384", "System.Security.Cryptography.SHA384Managed");
			hashtable.Add("SHA512", "System.Security.Cryptography.SHA512Managed");
			hashtable.Add("SHA-512", "System.Security.Cryptography.SHA512Managed");
			hashtable.Add("System.Security.Cryptography.SHA512", "System.Security.Cryptography.SHA512Managed");
			hashtable.Add("RSA", "System.Security.Cryptography.RSACryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.RSA", "System.Security.Cryptography.RSACryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.AsymmetricAlgorithm", "System.Security.Cryptography.RSACryptoServiceProvider");
			hashtable.Add("DSA", "System.Security.Cryptography.DSACryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.DSA", "System.Security.Cryptography.DSACryptoServiceProvider");
			hashtable.Add("DES", "System.Security.Cryptography.DESCryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.DES", "System.Security.Cryptography.DESCryptoServiceProvider");
			hashtable.Add("3DES", "System.Security.Cryptography.TripleDESCryptoServiceProvider");
			hashtable.Add("TripleDES", "System.Security.Cryptography.TripleDESCryptoServiceProvider");
			hashtable.Add("Triple DES", "System.Security.Cryptography.TripleDESCryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.TripleDES", "System.Security.Cryptography.TripleDESCryptoServiceProvider");
			hashtable.Add("RC2", "System.Security.Cryptography.RC2CryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.RC2", "System.Security.Cryptography.RC2CryptoServiceProvider");
			hashtable.Add("Rijndael", "System.Security.Cryptography.RijndaelManaged");
			hashtable.Add("System.Security.Cryptography.Rijndael", "System.Security.Cryptography.RijndaelManaged");
			hashtable.Add("System.Security.Cryptography.SymmetricAlgorithm", "System.Security.Cryptography.RijndaelManaged");
			hashtable.Add("RandomNumberGenerator", "System.Security.Cryptography.RNGCryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.RandomNumberGenerator", "System.Security.Cryptography.RNGCryptoServiceProvider");
			hashtable.Add("System.Security.Cryptography.KeyedHashAlgorithm", "System.Security.Cryptography.HMACSHA1");
			hashtable.Add("HMACSHA1", "System.Security.Cryptography.HMACSHA1");
			hashtable.Add("System.Security.Cryptography.HMACSHA1", "System.Security.Cryptography.HMACSHA1");
			hashtable.Add("MACTripleDES", "System.Security.Cryptography.MACTripleDES");
			hashtable.Add("System.Security.Cryptography.MACTripleDES", "System.Security.Cryptography.MACTripleDES");
			hashtable.Add("RIPEMD160", "System.Security.Cryptography.RIPEMD160Managed");
			hashtable.Add("RIPEMD-160", "System.Security.Cryptography.RIPEMD160Managed");
			hashtable.Add("System.Security.Cryptography.RIPEMD160", "System.Security.Cryptography.RIPEMD160Managed");
			hashtable.Add("System.Security.Cryptography.HMAC", "System.Security.Cryptography.HMACSHA1");
			hashtable.Add("HMACMD5", "System.Security.Cryptography.HMACMD5");
			hashtable.Add("System.Security.Cryptography.HMACMD5", "System.Security.Cryptography.HMACMD5");
			hashtable.Add("HMACRIPEMD160", "System.Security.Cryptography.HMACRIPEMD160");
			hashtable.Add("System.Security.Cryptography.HMACRIPEMD160", "System.Security.Cryptography.HMACRIPEMD160");
			hashtable.Add("HMACSHA256", "System.Security.Cryptography.HMACSHA256");
			hashtable.Add("System.Security.Cryptography.HMACSHA256", "System.Security.Cryptography.HMACSHA256");
			hashtable.Add("HMACSHA384", "System.Security.Cryptography.HMACSHA384");
			hashtable.Add("System.Security.Cryptography.HMACSHA384", "System.Security.Cryptography.HMACSHA384");
			hashtable.Add("HMACSHA512", "System.Security.Cryptography.HMACSHA512");
			hashtable.Add("System.Security.Cryptography.HMACSHA512", "System.Security.Cryptography.HMACSHA512");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig#dsa-sha1", "System.Security.Cryptography.DSASignatureDescription");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig#rsa-sha1", "System.Security.Cryptography.RSAPKCS1SHA1SignatureDescription");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig#sha1", "System.Security.Cryptography.SHA1CryptoServiceProvider");
			hashtable.Add("http://www.w3.org/TR/2001/REC-xml-c14n-20010315", "System.Security.Cryptography.Xml.XmlDsigC14NTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments", "System.Security.Cryptography.Xml.XmlDsigC14NWithCommentsTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig#base64", "System.Security.Cryptography.Xml.XmlDsigBase64Transform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/TR/1999/REC-xpath-19991116", "System.Security.Cryptography.Xml.XmlDsigXPathTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/TR/1999/REC-xslt-19991116", "System.Security.Cryptography.Xml.XmlDsigXsltTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig#enveloped-signature", "System.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2001/10/xml-exc-c14n#", "System.Security.Cryptography.Xml.XmlDsigExcC14NTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2001/10/xml-exc-c14n#WithComments", "System.Security.Cryptography.Xml.XmlDsigExcC14NWithCommentsTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2002/07/decrypt#XML", "System.Security.Cryptography.Xml.XmlDecryptionTransform, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2001/04/xmlenc#sha256", "System.Security.Cryptography.SHA256Managed");
			hashtable.Add("http://www.w3.org/2001/04/xmlenc#sha512", "System.Security.Cryptography.SHA512Managed");
			hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "System.Security.Cryptography.HMACSHA256");
			hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-sha384", "System.Security.Cryptography.HMACSHA384");
			hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-sha512", "System.Security.Cryptography.HMACSHA512");
			hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-ripemd160", "System.Security.Cryptography.HMACRIPEMD160");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig# X509Data", "System.Security.Cryptography.Xml.KeyInfoX509Data, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig# KeyName", "System.Security.Cryptography.Xml.KeyInfoName, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig# KeyValue/DSAKeyValue", "System.Security.Cryptography.Xml.DSAKeyValue, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig# KeyValue/RSAKeyValue", "System.Security.Cryptography.Xml.RSAKeyValue, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("http://www.w3.org/2000/09/xmldsig# RetrievalMethod", "System.Security.Cryptography.Xml.KeyInfoRetrievalMethod, System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			hashtable.Add("2.5.29.14", "System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			hashtable.Add("2.5.29.15", "System.Security.Cryptography.X509Certificates.X509KeyUsageExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			hashtable.Add("2.5.29.19", "System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			hashtable.Add("2.5.29.37", "System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			hashtable.Add("X509Chain", "System.Security.Cryptography.X509Certificates.X509Chain, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Hashtable hashtable2 = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
			hashtable2.Add("System.Security.Cryptography.SHA1CryptoServiceProvider", "1.3.14.3.2.26");
			hashtable2.Add("System.Security.Cryptography.SHA1Managed", "1.3.14.3.2.26");
			hashtable2.Add("SHA1", "1.3.14.3.2.26");
			hashtable2.Add("System.Security.Cryptography.SHA1", "1.3.14.3.2.26");
			hashtable2.Add("System.Security.Cryptography.MD5CryptoServiceProvider", "1.2.840.113549.2.5");
			hashtable2.Add("MD5", "1.2.840.113549.2.5");
			hashtable2.Add("System.Security.Cryptography.MD5", "1.2.840.113549.2.5");
			hashtable2.Add("System.Security.Cryptography.SHA256Managed", "2.16.840.1.101.3.4.2.1");
			hashtable2.Add("SHA256", "2.16.840.1.101.3.4.2.1");
			hashtable2.Add("System.Security.Cryptography.SHA256", "2.16.840.1.101.3.4.2.1");
			hashtable2.Add("System.Security.Cryptography.SHA384Managed", "2.16.840.1.101.3.4.2.2");
			hashtable2.Add("SHA384", "2.16.840.1.101.3.4.2.2");
			hashtable2.Add("System.Security.Cryptography.SHA384", "2.16.840.1.101.3.4.2.2");
			hashtable2.Add("System.Security.Cryptography.SHA512Managed", "2.16.840.1.101.3.4.2.3");
			hashtable2.Add("SHA512", "2.16.840.1.101.3.4.2.3");
			hashtable2.Add("System.Security.Cryptography.SHA512", "2.16.840.1.101.3.4.2.3");
			hashtable2.Add("TripleDESKeyWrap", "1.2.840.113549.1.9.16.3.6");
			hashtable2.Add("DES", "1.3.14.3.2.7");
			hashtable2.Add("TripleDES", "1.2.840.113549.3.7");
			hashtable2.Add("RC2", "1.2.840.113549.3.2");
			CryptoConfig.algorithms = hashtable;
			CryptoConfig.oid = hashtable2;
		}

		private static void LoadConfig(string filename, Hashtable algorithms, Hashtable oid)
		{
			if (!File.Exists(filename))
			{
				return;
			}
			try
			{
				using (TextReader textReader = new StreamReader(filename))
				{
					CryptoConfig.CryptoHandler handler = new CryptoConfig.CryptoHandler(algorithms, oid);
					SmallXmlParser smallXmlParser = new SmallXmlParser();
					smallXmlParser.Parse(textReader, handler);
				}
			}
			catch
			{
			}
		}

		/// <summary>Creates a new instance of the specified cryptographic object.</summary>
		/// <returns>A new instance of the specified cryptographic object.</returns>
		/// <param name="name">The simple name of the cryptographic object of which to create an instance. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm described by the <paramref name="name" /> parameter was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		public static object CreateFromName(string name)
		{
			return CryptoConfig.CreateFromName(name, null);
		}

		/// <summary>Creates a new instance of the specified cryptographic object with the specified arguments.</summary>
		/// <returns>A new instance of the specified cryptographic object.</returns>
		/// <param name="name">The simple name of the cryptographic object of which to create an instance. </param>
		/// <param name="args">The arguments used to create the specified cryptographic object. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm described by the <paramref name="name" /> parameter was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		public static object CreateFromName(string name, params object[] args)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = CryptoConfig.lockObject;
			lock (obj)
			{
				if (CryptoConfig.algorithms == null)
				{
					CryptoConfig.Initialize();
				}
			}
			object result;
			try
			{
				string text = (string)CryptoConfig.algorithms[name];
				if (text == null)
				{
					text = name;
				}
				Type type = Type.GetType(text);
				result = Activator.CreateInstance(type, args);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>Gets the object identifier (OID) of the algorithm corresponding to the specified simple name.</summary>
		/// <returns>The OID of the specified algorithm.</returns>
		/// <param name="name">The simple name of the algorithm for which to get the OID. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		public static string MapNameToOID(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = CryptoConfig.lockObject;
			lock (obj)
			{
				if (CryptoConfig.oid == null)
				{
					CryptoConfig.Initialize();
				}
			}
			return (string)CryptoConfig.oid[name];
		}

		/// <summary>Encodes the specified object identifier (OID).</summary>
		/// <returns>A byte array containing the encoded OID.</returns>
		/// <param name="str">The OID to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="str" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">An error occurred while encoding the OID. </exception>
		public static byte[] EncodeOID(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			char[] separator = new char[]
			{
				'.'
			};
			string[] array = str.Split(separator);
			if (array.Length < 2)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("OID must have at least two parts"));
			}
			byte[] array2 = new byte[str.Length];
			try
			{
				byte b = Convert.ToByte(array[0]);
				byte b2 = Convert.ToByte(array[1]);
				array2[2] = Convert.ToByte((int)(b * 40 + b2));
			}
			catch
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("Invalid OID"));
			}
			int num = 3;
			for (int i = 2; i < array.Length; i++)
			{
				long num2 = Convert.ToInt64(array[i]);
				if (num2 > 127L)
				{
					byte[] array3 = CryptoConfig.EncodeLongNumber(num2);
					Buffer.BlockCopy(array3, 0, array2, num, array3.Length);
					num += array3.Length;
				}
				else
				{
					array2[num++] = Convert.ToByte(num2);
				}
			}
			int num3 = 2;
			byte[] array4 = new byte[num];
			array4[0] = 6;
			if (num > 127)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("OID > 127 bytes"));
			}
			array4[1] = Convert.ToByte(num - 2);
			Buffer.BlockCopy(array2, num3, array4, num3, num - num3);
			return array4;
		}

		private static byte[] EncodeLongNumber(long x)
		{
			if (x > 2147483647L || x < -2147483648L)
			{
				throw new OverflowException(Locale.GetText("Part of OID doesn't fit in Int32"));
			}
			long num = x;
			int num2 = 1;
			while (num > 127L)
			{
				num >>= 7;
				num2++;
			}
			byte[] array = new byte[num2];
			for (int i = 0; i < num2; i++)
			{
				num = x >> 7 * i;
				num &= 127L;
				if (i != 0)
				{
					num += 128L;
				}
				array[num2 - i - 1] = Convert.ToByte(num);
			}
			return array;
		}

		private class CryptoHandler : SmallXmlParser.IContentHandler
		{
			private Hashtable algorithms;

			private Hashtable oid;

			private Hashtable names;

			private Hashtable classnames;

			private int level;

			public CryptoHandler(Hashtable algorithms, Hashtable oid)
			{
				this.algorithms = algorithms;
				this.oid = oid;
				this.names = new Hashtable();
				this.classnames = new Hashtable();
			}

			public void OnStartParsing(SmallXmlParser parser)
			{
			}

			public void OnEndParsing(SmallXmlParser parser)
			{
				foreach (object obj in this.names)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					try
					{
						this.algorithms.Add(dictionaryEntry.Key, this.classnames[dictionaryEntry.Value]);
					}
					catch
					{
					}
				}
				this.names.Clear();
				this.classnames.Clear();
			}

			private string Get(SmallXmlParser.IAttrList attrs, string name)
			{
				for (int i = 0; i < attrs.Names.Length; i++)
				{
					if (attrs.Names[i] == name)
					{
						return attrs.Values[i];
					}
				}
				return string.Empty;
			}

			public void OnStartElement(string name, SmallXmlParser.IAttrList attrs)
			{
				switch (this.level)
				{
				case 0:
					if (name == "configuration")
					{
						this.level++;
					}
					break;
				case 1:
					if (name == "mscorlib")
					{
						this.level++;
					}
					break;
				case 2:
					if (name == "cryptographySettings")
					{
						this.level++;
					}
					break;
				case 3:
					if (name == "oidMap")
					{
						this.level++;
					}
					else if (name == "cryptoNameMapping")
					{
						this.level++;
					}
					break;
				case 4:
					if (name == "oidEntry")
					{
						this.oid.Add(this.Get(attrs, "name"), this.Get(attrs, "OID"));
					}
					else if (name == "nameEntry")
					{
						this.names.Add(this.Get(attrs, "name"), this.Get(attrs, "class"));
					}
					else if (name == "cryptoClasses")
					{
						this.level++;
					}
					break;
				case 5:
					if (name == "cryptoClass")
					{
						this.classnames.Add(attrs.Names[0], attrs.Values[0]);
					}
					break;
				}
			}

			public void OnEndElement(string name)
			{
				switch (this.level)
				{
				case 1:
					if (name == "configuration")
					{
						this.level--;
					}
					break;
				case 2:
					if (name == "mscorlib")
					{
						this.level--;
					}
					break;
				case 3:
					if (name == "cryptographySettings")
					{
						this.level--;
					}
					break;
				case 4:
					if (name == "oidMap" || name == "cryptoNameMapping")
					{
						this.level--;
					}
					break;
				case 5:
					if (name == "cryptoClasses")
					{
						this.level--;
					}
					break;
				}
			}

			public void OnProcessingInstruction(string name, string text)
			{
			}

			public void OnChars(string text)
			{
			}

			public void OnIgnorableWhitespace(string text)
			{
			}
		}
	}
}
