using System;

namespace Mono.Security.X509.Extensions
{
	public class SubjectAltNameExtension : X509Extension
	{
		private GeneralNames _names;

		public SubjectAltNameExtension()
		{
			this.extnOid = "2.5.29.17";
			this._names = new GeneralNames();
		}

		public SubjectAltNameExtension(ASN1 asn1) : base(asn1)
		{
		}

		public SubjectAltNameExtension(X509Extension extension) : base(extension)
		{
		}

		public SubjectAltNameExtension(string[] rfc822, string[] dnsNames, string[] ipAddresses, string[] uris)
		{
			this._names = new GeneralNames(rfc822, dnsNames, ipAddresses, uris);
			this.extnValue = new ASN1(4, this._names.GetBytes());
			this.extnOid = "2.5.29.17";
		}

		protected override void Decode()
		{
			ASN1 asn = new ASN1(this.extnValue.Value);
			if (asn.Tag != 48)
			{
				throw new ArgumentException("Invalid SubjectAltName extension");
			}
			this._names = new GeneralNames(asn);
		}

		public override string Name
		{
			get
			{
				return "Subject Alternative Name";
			}
		}

		public string[] RFC822
		{
			get
			{
				return this._names.RFC822;
			}
		}

		public string[] DNSNames
		{
			get
			{
				return this._names.DNSNames;
			}
		}

		public string[] IPAddresses
		{
			get
			{
				return this._names.IPAddresses;
			}
		}

		public string[] UniformResourceIdentifiers
		{
			get
			{
				return this._names.UniformResourceIdentifiers;
			}
		}

		public override string ToString()
		{
			return this._names.ToString();
		}
	}
}
