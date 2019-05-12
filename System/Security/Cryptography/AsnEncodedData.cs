using Mono.Security;
using Mono.Security.Cryptography;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.Security.Cryptography
{
	/// <summary>Represents Abstract Syntax Notation One (ASN.1)-encoded data.</summary>
	public class AsnEncodedData
	{
		internal Oid _oid;

		internal byte[] _raw;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class.</summary>
		protected AsnEncodedData()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using a byte array.</summary>
		/// <param name="oid">A string that represents <see cref="T:System.Security.Cryptography.Oid" /> information.</param>
		/// <param name="rawData">A byte array that contains Abstract Syntax Notation One (ASN.1)-encoded data.</param>
		public AsnEncodedData(string oid, byte[] rawData)
		{
			this._oid = new Oid(oid);
			this.RawData = rawData;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using an <see cref="T:System.Security.Cryptography.Oid" /> object and a byte array.</summary>
		/// <param name="oid">An <see cref="T:System.Security.Cryptography.Oid" /> object.</param>
		/// <param name="rawData">A byte array that contains Abstract Syntax Notation One (ASN.1)-encoded data.</param>
		public AsnEncodedData(Oid oid, byte[] rawData)
		{
			this.Oid = oid;
			this.RawData = rawData;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using an instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class.</summary>
		/// <param name="asnEncodedData">An instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData" /> is null.</exception>
		public AsnEncodedData(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (asnEncodedData._oid != null)
			{
				this.Oid = new Oid(asnEncodedData._oid);
			}
			this.RawData = asnEncodedData._raw;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.AsnEncodedData" /> class using a byte array.</summary>
		/// <param name="rawData">A byte array that contains Abstract Syntax Notation One (ASN.1)-encoded data.</param>
		public AsnEncodedData(byte[] rawData)
		{
			this.RawData = rawData;
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.Cryptography.Oid" /> value for an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.Oid" /> object.</returns>
		public Oid Oid
		{
			get
			{
				return this._oid;
			}
			set
			{
				if (value == null)
				{
					this._oid = null;
				}
				else
				{
					this._oid = new Oid(value);
				}
			}
		}

		/// <summary>Gets or sets the Abstract Syntax Notation One (ASN.1)-encoded data represented in a byte array.</summary>
		/// <returns>A byte array that represents the Abstract Syntax Notation One (ASN.1)-encoded data.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value is null.</exception>
		public byte[] RawData
		{
			get
			{
				return this._raw;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("RawData");
				}
				this._raw = (byte[])value.Clone();
			}
		}

		/// <summary>Copies information from an <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object to base the new object on.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData " />is null.</exception>
		public virtual void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (asnEncodedData._oid == null)
			{
				this.Oid = null;
			}
			else
			{
				this.Oid = new Oid(asnEncodedData._oid);
			}
			this.RawData = asnEncodedData._raw;
		}

		/// <summary>Returns a formatted version of the Abstract Syntax Notation One (ASN.1)-encoded data as a string.</summary>
		/// <returns>A formatted string that represents the Abstract Syntax Notation One (ASN.1)-encoded data.</returns>
		/// <param name="multiLine">true if the return string should contain carriage returns; otherwise, false.</param>
		public virtual string Format(bool multiLine)
		{
			if (this._raw == null)
			{
				return string.Empty;
			}
			if (this._oid == null)
			{
				return this.Default(multiLine);
			}
			return this.ToString(multiLine);
		}

		internal virtual string ToString(bool multiLine)
		{
			string value = this._oid.Value;
			switch (value)
			{
			case "2.5.29.19":
				return this.BasicConstraintsExtension(multiLine);
			case "2.5.29.37":
				return this.EnhancedKeyUsageExtension(multiLine);
			case "2.5.29.15":
				return this.KeyUsageExtension(multiLine);
			case "2.5.29.14":
				return this.SubjectKeyIdentifierExtension(multiLine);
			case "2.5.29.17":
				return this.SubjectAltName(multiLine);
			case "2.16.840.1.113730.1.1":
				return this.NetscapeCertType(multiLine);
			}
			return this.Default(multiLine);
		}

		internal string Default(bool multiLine)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this._raw.Length; i++)
			{
				stringBuilder.Append(this._raw[i].ToString("x2"));
				if (i != this._raw.Length - 1)
				{
					stringBuilder.Append(" ");
				}
			}
			return stringBuilder.ToString();
		}

		internal string BasicConstraintsExtension(bool multiLine)
		{
			string result;
			try
			{
				System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension x509BasicConstraintsExtension = new System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension(this, false);
				result = x509BasicConstraintsExtension.ToString(multiLine);
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		internal string EnhancedKeyUsageExtension(bool multiLine)
		{
			string result;
			try
			{
				System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = new System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension(this, false);
				result = x509EnhancedKeyUsageExtension.ToString(multiLine);
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		internal string KeyUsageExtension(bool multiLine)
		{
			string result;
			try
			{
				System.Security.Cryptography.X509Certificates.X509KeyUsageExtension x509KeyUsageExtension = new System.Security.Cryptography.X509Certificates.X509KeyUsageExtension(this, false);
				result = x509KeyUsageExtension.ToString(multiLine);
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		internal string SubjectKeyIdentifierExtension(bool multiLine)
		{
			string result;
			try
			{
				System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension x509SubjectKeyIdentifierExtension = new System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension(this, false);
				result = x509SubjectKeyIdentifierExtension.ToString(multiLine);
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		internal string SubjectAltName(bool multiLine)
		{
			if (this._raw.Length < 5)
			{
				return "Information Not Available";
			}
			string result;
			try
			{
				ASN1 asn = new ASN1(this._raw);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < asn.Count; i++)
				{
					ASN1 asn2 = asn[i];
					byte tag = asn2.Tag;
					string value;
					string value2;
					if (tag != 129)
					{
						if (tag != 130)
						{
							value = string.Format("Unknown ({0})=", asn2.Tag);
							value2 = CryptoConvert.ToHex(asn2.Value);
						}
						else
						{
							value = "DNS Name=";
							value2 = Encoding.ASCII.GetString(asn2.Value);
						}
					}
					else
					{
						value = "RFC822 Name=";
						value2 = Encoding.ASCII.GetString(asn2.Value);
					}
					stringBuilder.Append(value);
					stringBuilder.Append(value2);
					if (multiLine)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					else if (i < asn.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				result = stringBuilder.ToString();
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		internal string NetscapeCertType(bool multiLine)
		{
			if (this._raw.Length < 4 || this._raw[0] != 3 || this._raw[1] != 2)
			{
				return "Information Not Available";
			}
			int num = this._raw[3] >> (int)this._raw[2] << (int)this._raw[2];
			StringBuilder stringBuilder = new StringBuilder();
			if ((num & 128) == 128)
			{
				stringBuilder.Append("SSL Client Authentication");
			}
			if ((num & 64) == 64)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SSL Server Authentication");
			}
			if ((num & 32) == 32)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SMIME");
			}
			if ((num & 16) == 16)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Signature");
			}
			if ((num & 8) == 8)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Unknown cert type");
			}
			if ((num & 4) == 4)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SSL CA");
			}
			if ((num & 2) == 2)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("SMIME CA");
			}
			if ((num & 1) == 1)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("Signature CA");
			}
			stringBuilder.AppendFormat(" ({0})", num.ToString("x2"));
			return stringBuilder.ToString();
		}
	}
}
