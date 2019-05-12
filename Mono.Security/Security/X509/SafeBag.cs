using System;

namespace Mono.Security.X509
{
	internal class SafeBag
	{
		private string _bagOID;

		private ASN1 _asn1;

		public SafeBag(string bagOID, ASN1 asn1)
		{
			this._bagOID = bagOID;
			this._asn1 = asn1;
		}

		public string BagOID
		{
			get
			{
				return this._bagOID;
			}
		}

		public ASN1 ASN1
		{
			get
			{
				return this._asn1;
			}
		}
	}
}
