using System;
using System.Globalization;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Represents a collection of <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects. This class cannot be inherited.</summary>
	public class X509Certificate2Collection : X509CertificateCollection
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> class without any <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> information.</summary>
		public X509Certificate2Collection()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> class using the specified certificate collection.</summary>
		/// <param name="certificates">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object. </param>
		public X509Certificate2Collection(X509Certificate2Collection certificates)
		{
			this.AddRange(certificates);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> class using an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object.</summary>
		/// <param name="certificate">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object to start the collection from.</param>
		public X509Certificate2Collection(X509Certificate2 certificate)
		{
			this.Add(certificate);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> class using an array of <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects.</summary>
		/// <param name="certificates">An array of <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects. </param>
		public X509Certificate2Collection(X509Certificate2[] certificates)
		{
			this.AddRange(certificates);
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is equal to or greater than the <see cref="P:System.Collections.CollectionBase.Count" /> property. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="index" /> is null. </exception>
		public new X509Certificate2 this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("negative index");
				}
				if (index >= base.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index >= Count");
				}
				return (X509Certificate2)base.InnerList[index];
			}
			set
			{
				base.InnerList[index] = value;
			}
		}

		/// <summary>Adds an object to the end of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" />.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> index at which the <paramref name="certificate" /> has been added.</returns>
		/// <param name="certificate">An X.509 certificate represented as an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificate" /> is null. </exception>
		public int Add(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			return base.InnerList.Add(certificate);
		}

		/// <summary>Adds multiple <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects in an array to the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="certificates">An array of <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificates" /> is null. </exception>
		[MonoTODO("Method isn't transactional (like documented)")]
		public void AddRange(X509Certificate2[] certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			for (int i = 0; i < certificates.Length; i++)
			{
				base.InnerList.Add(certificates[i]);
			}
		}

		/// <summary>Adds multiple <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects in an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object to another <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="certificates">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificates" /> is null. </exception>
		[MonoTODO("Method isn't transactional (like documented)")]
		public void AddRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			base.InnerList.AddRange(certificates);
		}

		/// <summary>Determines whether the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object contains a specific certificate.</summary>
		/// <returns>true if the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> contains the specified <paramref name="certificate" />; otherwise, false.</returns>
		/// <param name="certificate">The <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object to locate in the collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificate" /> is null. </exception>
		public bool Contains(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			foreach (object obj in base.InnerList)
			{
				X509Certificate2 x509Certificate = (X509Certificate2)obj;
				if (x509Certificate.Equals(certificate))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Exports X.509 certificate information into a byte array.</summary>
		/// <returns>X.509 certificate information in a byte array.</returns>
		/// <param name="contentType">A supported <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> object. </param>
		[MonoTODO("only support X509ContentType.Cert")]
		public byte[] Export(X509ContentType contentType)
		{
			return this.Export(contentType, null);
		}

		/// <summary>Exports X.509 certificate information into a byte array using a password.</summary>
		/// <returns>X.509 certificate information in a byte array.</returns>
		/// <param name="contentType">A supported <see cref="T:System.Security.Cryptography.X509Certificates.X509ContentType" /> object. </param>
		/// <param name="password">A string used to protect the byte array. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate is unreadable, the content is invalid or, in the case of a certificate requiring a password, the private key could not be exported because the password provided was incorrect. </exception>
		[MonoTODO("only support X509ContentType.Cert")]
		public byte[] Export(X509ContentType contentType, string password)
		{
			switch (contentType)
			{
			case X509ContentType.Cert:
			case X509ContentType.SerializedCert:
			case X509ContentType.Pfx:
				if (this.Count > 0)
				{
					return this[this.Count - 1].Export(contentType, password);
				}
				break;
			case X509ContentType.SerializedStore:
				break;
			case X509ContentType.Pkcs7:
				break;
			default:
			{
				string text = Locale.GetText("Cannot export certificate(s) to the '{0}' format", new object[]
				{
					contentType
				});
				throw new CryptographicException(text);
			}
			}
			return null;
		}

		/// <summary>Searches an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object using the search criteria specified by the <see cref="T:System.Security.Cryptography.X509Certificates.X509FindType" /> enumeration and the <paramref name="findValue" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</returns>
		/// <param name="findType">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509FindType" />  values. </param>
		/// <param name="findValue">The search criteria as an object. </param>
		/// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">
		///   <paramref name="findType" /> is invalid. </exception>
		[MonoTODO("Does not support X509FindType.FindByTemplateName, FindByApplicationPolicy and FindByCertificatePolicy")]
		public X509Certificate2Collection Find(X509FindType findType, object findValue, bool validOnly)
		{
			if (findValue == null)
			{
				throw new ArgumentNullException("findValue");
			}
			string text = string.Empty;
			string text2 = string.Empty;
			X509KeyUsageFlags x509KeyUsageFlags = X509KeyUsageFlags.None;
			DateTime t = DateTime.MinValue;
			switch (findType)
			{
			case X509FindType.FindByThumbprint:
			case X509FindType.FindBySubjectName:
			case X509FindType.FindBySubjectDistinguishedName:
			case X509FindType.FindByIssuerName:
			case X509FindType.FindByIssuerDistinguishedName:
			case X509FindType.FindBySerialNumber:
			case X509FindType.FindByTemplateName:
			case X509FindType.FindBySubjectKeyIdentifier:
				try
				{
					text = (string)findValue;
				}
				catch (Exception inner)
				{
					string text3 = Locale.GetText("Invalid find value type '{0}', expected '{1}'.", new object[]
					{
						findValue.GetType(),
						"string"
					});
					throw new CryptographicException(text3, inner);
				}
				break;
			case X509FindType.FindByTimeValid:
			case X509FindType.FindByTimeNotYetValid:
			case X509FindType.FindByTimeExpired:
				try
				{
					t = (DateTime)findValue;
				}
				catch (Exception inner2)
				{
					string text4 = Locale.GetText("Invalid find value type '{0}', expected '{1}'.", new object[]
					{
						findValue.GetType(),
						"X509DateTime"
					});
					throw new CryptographicException(text4, inner2);
				}
				break;
			case X509FindType.FindByApplicationPolicy:
			case X509FindType.FindByCertificatePolicy:
			case X509FindType.FindByExtension:
				try
				{
					text2 = (string)findValue;
				}
				catch (Exception inner3)
				{
					string text5 = Locale.GetText("Invalid find value type '{0}', expected '{1}'.", new object[]
					{
						findValue.GetType(),
						"X509KeyUsageFlags"
					});
					throw new CryptographicException(text5, inner3);
				}
				try
				{
					CryptoConfig.EncodeOID(text2);
				}
				catch (CryptographicUnexpectedOperationException)
				{
					string text6 = Locale.GetText("Invalid OID value '{0}'.", new object[]
					{
						text2
					});
					throw new ArgumentException("findValue", text6);
				}
				break;
			case X509FindType.FindByKeyUsage:
				try
				{
					x509KeyUsageFlags = (X509KeyUsageFlags)((int)findValue);
				}
				catch (Exception inner4)
				{
					string text7 = Locale.GetText("Invalid find value type '{0}', expected '{1}'.", new object[]
					{
						findValue.GetType(),
						"X509KeyUsageFlags"
					});
					throw new CryptographicException(text7, inner4);
				}
				break;
			default:
			{
				string text8 = Locale.GetText("Invalid find type '{0}'.", new object[]
				{
					findType
				});
				throw new CryptographicException(text8);
			}
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			foreach (object obj in base.InnerList)
			{
				X509Certificate2 x509Certificate = (X509Certificate2)obj;
				bool flag = false;
				switch (findType)
				{
				case X509FindType.FindByThumbprint:
					flag = (string.Compare(text, x509Certificate.Thumbprint, true, invariantCulture) == 0 || string.Compare(text, x509Certificate.GetCertHashString(), true, invariantCulture) == 0);
					break;
				case X509FindType.FindBySubjectName:
				{
					string nameInfo = x509Certificate.GetNameInfo(X509NameType.SimpleName, false);
					flag = (nameInfo.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0);
					break;
				}
				case X509FindType.FindBySubjectDistinguishedName:
					flag = (string.Compare(text, x509Certificate.Subject, true, invariantCulture) == 0);
					break;
				case X509FindType.FindByIssuerName:
				{
					string nameInfo2 = x509Certificate.GetNameInfo(X509NameType.SimpleName, true);
					flag = (nameInfo2.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0);
					break;
				}
				case X509FindType.FindByIssuerDistinguishedName:
					flag = (string.Compare(text, x509Certificate.Issuer, true, invariantCulture) == 0);
					break;
				case X509FindType.FindBySerialNumber:
					flag = (string.Compare(text, x509Certificate.SerialNumber, true, invariantCulture) == 0);
					break;
				case X509FindType.FindByTimeValid:
					flag = (t >= x509Certificate.NotBefore && t <= x509Certificate.NotAfter);
					break;
				case X509FindType.FindByTimeNotYetValid:
					flag = (t < x509Certificate.NotBefore);
					break;
				case X509FindType.FindByTimeExpired:
					flag = (t > x509Certificate.NotAfter);
					break;
				case X509FindType.FindByApplicationPolicy:
					flag = (x509Certificate.Extensions.Count == 0);
					break;
				case X509FindType.FindByExtension:
					flag = (x509Certificate.Extensions[text2] != null);
					break;
				case X509FindType.FindByKeyUsage:
				{
					X509KeyUsageExtension x509KeyUsageExtension = x509Certificate.Extensions["2.5.29.15"] as X509KeyUsageExtension;
					flag = (x509KeyUsageExtension == null || (x509KeyUsageExtension.KeyUsages & x509KeyUsageFlags) == x509KeyUsageFlags);
					break;
				}
				case X509FindType.FindBySubjectKeyIdentifier:
				{
					X509SubjectKeyIdentifierExtension x509SubjectKeyIdentifierExtension = x509Certificate.Extensions["2.5.29.14"] as X509SubjectKeyIdentifierExtension;
					if (x509SubjectKeyIdentifierExtension != null)
					{
						flag = (string.Compare(text, x509SubjectKeyIdentifierExtension.SubjectKeyIdentifier, true, invariantCulture) == 0);
					}
					break;
				}
				}
				if (flag)
				{
					if (validOnly)
					{
						try
						{
							if (x509Certificate.Verify())
							{
								x509Certificate2Collection.Add(x509Certificate);
							}
						}
						catch
						{
						}
					}
					else
					{
						x509Certificate2Collection.Add(x509Certificate);
					}
				}
			}
			return x509Certificate2Collection;
		}

		/// <summary>Returns an enumerator that can iterate through a <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Enumerator" /> object that can iterate through the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</returns>
		public new X509Certificate2Enumerator GetEnumerator()
		{
			return new X509Certificate2Enumerator(this);
		}

		/// <summary>Imports a certificate in the form of a byte array into a <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="rawData">A byte array containing data from an X.509 certificate. </param>
		[MonoTODO("same limitations as X509Certificate2.Import")]
		public void Import(byte[] rawData)
		{
			X509Certificate2 x509Certificate = new X509Certificate2();
			x509Certificate.Import(rawData);
			this.Add(x509Certificate);
		}

		/// <summary>Imports a certificate, in the form of a byte array that requires a password to access the certificate, into a <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="rawData">A byte array containing data from an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object. </param>
		/// <param name="password">The password required to access the certificate information. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls how and where the private key is imported. </param>
		[MonoTODO("same limitations as X509Certificate2.Import")]
		public void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			X509Certificate2 x509Certificate = new X509Certificate2();
			x509Certificate.Import(rawData, password, keyStorageFlags);
			this.Add(x509Certificate);
		}

		/// <summary>Imports a certificate file into a <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="fileName">The name of the file containing the certificate information. </param>
		[MonoTODO("same limitations as X509Certificate2.Import")]
		public void Import(string fileName)
		{
			X509Certificate2 x509Certificate = new X509Certificate2();
			x509Certificate.Import(fileName);
			this.Add(x509Certificate);
		}

		/// <summary>Imports a certificate file that requires a password into a <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="fileName">The name of the file containing the certificate information. </param>
		/// <param name="password">The password required to access the certificate information. </param>
		/// <param name="keyStorageFlags">One of the <see cref="T:System.Security.Cryptography.X509Certificates.X509KeyStorageFlags" /> values that controls how and where the private key is imported. </param>
		[MonoTODO("same limitations as X509Certificate2.Import")]
		public void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			X509Certificate2 x509Certificate = new X509Certificate2();
			x509Certificate.Import(fileName, password, keyStorageFlags);
			this.Add(x509Certificate);
		}

		/// <summary>Inserts an object into the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object at the specified index.</summary>
		/// <param name="index">The zero-based index at which to insert <paramref name="certificate" />. </param>
		/// <param name="certificate">The <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object to insert. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than the <see cref="P:System.Collections.CollectionBase.Count" /> property. </exception>
		/// <exception cref="T:System.NotSupportedException">The collection is read-only.-or- The collection has a fixed size. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificate" /> is null. </exception>
		public void Insert(int index, X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("negative index");
			}
			if (index >= base.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index >= Count");
			}
			base.InnerList.Insert(index, certificate);
		}

		/// <summary>Removes the first occurrence of a certificate from the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="certificate">The <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object to be removed from the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificate" /> is null. </exception>
		public void Remove(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				X509Certificate x509Certificate = (X509Certificate)base.InnerList[i];
				if (x509Certificate.Equals(certificate))
				{
					base.InnerList.RemoveAt(i);
					return;
				}
			}
		}

		/// <summary>Removes multiple <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects in an array from an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="certificates">An array of <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificates" /> is null. </exception>
		[MonoTODO("Method isn't transactional (like documented)")]
		public void RemoveRange(X509Certificate2[] certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificate");
			}
			foreach (X509Certificate2 certificate in certificates)
			{
				this.Remove(certificate);
			}
		}

		/// <summary>Removes multiple <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> objects in an <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object from another <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</summary>
		/// <param name="certificates">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificates" /> is null. </exception>
		[MonoTODO("Method isn't transactional (like documented)")]
		public void RemoveRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificate");
			}
			foreach (X509Certificate2 certificate in certificates)
			{
				this.Remove(certificate);
			}
		}
	}
}
