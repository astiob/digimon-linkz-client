using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Provides the Authenticode X.509v3 digital signature of a code assembly as evidence for policy evaluation. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class Publisher : IBuiltInEvidence, IIdentityPermissionFactory
	{
		private X509Certificate m_cert;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.Publisher" /> class with the Authenticode X.509v3 certificate containing the publisher's public key.</summary>
		/// <param name="cert">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> that contains the software publisher's public key. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="cert" /> parameter is null. </exception>
		public Publisher(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			if (cert.GetHashCode() == 0)
			{
				throw new ArgumentException("cert");
			}
			this.m_cert = cert;
		}

		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return ((!verbose) ? 1 : 3) + this.m_cert.GetRawCertData().Length;
		}

		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return 0;
		}

		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			return 0;
		}

		/// <summary>Gets the publisher's Authenticode X.509v3 certificate.</summary>
		/// <returns>The publisher's <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" />.</returns>
		public X509Certificate Certificate
		{
			get
			{
				if (this.m_cert.GetHashCode() == 0)
				{
					throw new ArgumentException("m_cert");
				}
				return this.m_cert;
			}
		}

		/// <summary>Creates an equivalent copy of the <see cref="T:System.Security.Policy.Publisher" />.</summary>
		/// <returns>A new, identical copy of the <see cref="T:System.Security.Policy.Publisher" />.</returns>
		public object Copy()
		{
			return new Publisher(this.m_cert);
		}

		/// <summary>Creates an identity permission that corresponds to the current instance of the <see cref="T:System.Security.Policy.Publisher" /> class.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.PublisherIdentityPermission" /> for the specified <see cref="T:System.Security.Policy.Publisher" />.</returns>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> from which to construct the identity permission. </param>
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new PublisherIdentityPermission(this.m_cert);
		}

		/// <summary>Compares the current <see cref="T:System.Security.Policy.Publisher" /> to the specified object for equivalence.</summary>
		/// <returns>true if the two instances of the <see cref="T:System.Security.Policy.Publisher" /> class are equal; otherwise, false.</returns>
		/// <param name="o">The <see cref="T:System.Security.Policy.Publisher" /> to test for equivalence with the current object. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="o" /> parameter is not a <see cref="T:System.Security.Policy.Publisher" /> object. </exception>
		public override bool Equals(object o)
		{
			Publisher publisher = o as Publisher;
			if (publisher == null)
			{
				throw new ArgumentException("o", Locale.GetText("not a Publisher instance."));
			}
			return this.m_cert.Equals(publisher.Certificate);
		}

		/// <summary>Gets the hash code of the current <see cref="P:System.Security.Policy.Publisher.Certificate" />.</summary>
		/// <returns>The hash code of the current <see cref="P:System.Security.Policy.Publisher.Certificate" />.</returns>
		public override int GetHashCode()
		{
			return this.m_cert.GetHashCode();
		}

		/// <summary>Returns a string representation of the current <see cref="T:System.Security.Policy.Publisher" />.</summary>
		/// <returns>A representation of the current <see cref="T:System.Security.Policy.Publisher" />.</returns>
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Publisher");
			securityElement.AddAttribute("version", "1");
			SecurityElement securityElement2 = new SecurityElement("X509v3Certificate");
			string rawCertDataString = this.m_cert.GetRawCertDataString();
			if (rawCertDataString != null)
			{
				securityElement2.Text = rawCertDataString;
			}
			securityElement.AddChild(securityElement2);
			return securityElement.ToString();
		}
	}
}
