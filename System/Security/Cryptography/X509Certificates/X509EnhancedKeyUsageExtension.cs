using Mono.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Defines the collection of object identifiers (OIDs) that indicates the applications that use the key. This class cannot be inherited.</summary>
	public sealed class X509EnhancedKeyUsageExtension : X509Extension
	{
		internal const string oid = "2.5.29.37";

		internal const string friendlyName = "Enhanced Key Usage";

		private OidCollection _enhKeyUsage;

		private AsnDecodeStatus _status;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension" /> class.</summary>
		public X509EnhancedKeyUsageExtension()
		{
			this._oid = new Oid("2.5.29.37", "Enhanced Key Usage");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension" /> class using an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object and a value that identifies whether the extension is critical.</summary>
		/// <param name="encodedEnhancedKeyUsages">The encoded data to use to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		public X509EnhancedKeyUsageExtension(AsnEncodedData encodedEnhancedKeyUsages, bool critical)
		{
			this._oid = new Oid("2.5.29.37", "Enhanced Key Usage");
			this._raw = encodedEnhancedKeyUsages.RawData;
			base.Critical = critical;
			this._status = this.Decode(base.RawData);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension" /> class using an <see cref="T:System.Security.Cryptography.OidCollection" /> and a value that identifies whether the extension is critical. </summary>
		/// <param name="enhancedKeyUsages">An <see cref="T:System.Security.Cryptography.OidCollection" /> collection. </param>
		/// <param name="critical">true if the extension is critical; otherwise, false.</param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The specified <see cref="T:System.Security.Cryptography.OidCollection" />  contains one or more corrupt values.</exception>
		public X509EnhancedKeyUsageExtension(OidCollection enhancedKeyUsages, bool critical)
		{
			if (enhancedKeyUsages == null)
			{
				throw new ArgumentNullException("enhancedKeyUsages");
			}
			this._oid = new Oid("2.5.29.37", "Enhanced Key Usage");
			base.Critical = critical;
			this._enhKeyUsage = enhancedKeyUsages.ReadOnlyCopy();
			base.RawData = this.Encode();
		}

		/// <summary>Gets the collection of object identifiers (OIDs) that indicate the applications that use the key.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.OidCollection" /> object indicating the applications that use the key.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public OidCollection EnhancedKeyUsages
		{
			get
			{
				AsnDecodeStatus status = this._status;
				if (status != AsnDecodeStatus.Ok && status != AsnDecodeStatus.InformationNotAvailable)
				{
					throw new CryptographicException("Badly encoded extension.");
				}
				if (this._enhKeyUsage == null)
				{
					this._enhKeyUsage = new OidCollection();
				}
				this._enhKeyUsage.ReadOnly = true;
				return this._enhKeyUsage;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension" /> class using an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <param name="asnEncodedData">The encoded data to use to create the extension.</param>
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("encodedData");
			}
			X509Extension x509Extension = asnEncodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(Locale.GetText("Wrong type."), "asnEncodedData");
			}
			if (x509Extension._oid == null)
			{
				this._oid = new Oid("2.5.29.37", "Enhanced Key Usage");
			}
			else
			{
				this._oid = new Oid(x509Extension._oid);
			}
			base.RawData = x509Extension.RawData;
			base.Critical = x509Extension.Critical;
			this._status = this.Decode(base.RawData);
		}

		internal AsnDecodeStatus Decode(byte[] extension)
		{
			if (extension == null || extension.Length == 0)
			{
				return AsnDecodeStatus.BadAsn;
			}
			if (extension[0] != 48)
			{
				return AsnDecodeStatus.BadTag;
			}
			if (this._enhKeyUsage == null)
			{
				this._enhKeyUsage = new OidCollection();
			}
			try
			{
				ASN1 asn = new ASN1(extension);
				if (asn.Tag != 48)
				{
					throw new CryptographicException(Locale.GetText("Invalid ASN.1 Tag"));
				}
				for (int i = 0; i < asn.Count; i++)
				{
					this._enhKeyUsage.Add(new Oid(ASN1Convert.ToOid(asn[i])));
				}
			}
			catch
			{
				return AsnDecodeStatus.BadAsn;
			}
			return AsnDecodeStatus.Ok;
		}

		internal byte[] Encode()
		{
			ASN1 asn = new ASN1(48);
			foreach (Oid oid in this._enhKeyUsage)
			{
				asn.Add(ASN1Convert.FromOid(oid.Value));
			}
			return asn.GetBytes();
		}

		internal override string ToString(bool multiLine)
		{
			switch (this._status)
			{
			case AsnDecodeStatus.BadAsn:
				return string.Empty;
			case AsnDecodeStatus.BadTag:
			case AsnDecodeStatus.BadLength:
				return base.FormatUnkownData(this._raw);
			case AsnDecodeStatus.InformationNotAvailable:
				return "Information Not Available";
			default:
			{
				if (this._oid.Value != "2.5.29.37")
				{
					return string.Format("Unknown Key Usage ({0})", this._oid.Value);
				}
				if (this._enhKeyUsage.Count == 0)
				{
					return "Information Not Available";
				}
				StringBuilder stringBuilder = new StringBuilder();
				int i = 0;
				while (i < this._enhKeyUsage.Count)
				{
					Oid oid = this._enhKeyUsage[i];
					string value = oid.Value;
					if (value == null)
					{
						goto IL_102;
					}
					if (X509EnhancedKeyUsageExtension.<>f__switch$mapE == null)
					{
						X509EnhancedKeyUsageExtension.<>f__switch$mapE = new Dictionary<string, int>(1)
						{
							{
								"1.3.6.1.5.5.7.3.1",
								0
							}
						};
					}
					int num;
					if (!X509EnhancedKeyUsageExtension.<>f__switch$mapE.TryGetValue(value, out num))
					{
						goto IL_102;
					}
					if (num != 0)
					{
						goto IL_102;
					}
					stringBuilder.Append("Server Authentication (");
					IL_113:
					stringBuilder.Append(oid.Value);
					stringBuilder.Append(")");
					if (multiLine)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					else if (i != this._enhKeyUsage.Count - 1)
					{
						stringBuilder.Append(", ");
					}
					i++;
					continue;
					IL_102:
					stringBuilder.Append("Unknown Key Usage (");
					goto IL_113;
				}
				return stringBuilder.ToString();
			}
			}
		}
	}
}
