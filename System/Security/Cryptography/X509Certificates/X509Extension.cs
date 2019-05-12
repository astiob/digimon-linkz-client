using System;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Represents an X509 extension.</summary>
	public class X509Extension : AsnEncodedData
	{
		private bool _critical;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Extension" /> class.</summary>
		protected X509Extension()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Extension" /> class.</summary>
		/// <param name="encodedExtension">The encoded data to be used to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise false.</param>
		public X509Extension(AsnEncodedData encodedExtension, bool critical)
		{
			if (encodedExtension.Oid == null)
			{
				throw new ArgumentNullException("encodedExtension.Oid");
			}
			base.Oid = encodedExtension.Oid;
			base.RawData = encodedExtension.RawData;
			this._critical = critical;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Extension" /> class.</summary>
		/// <param name="oid">The object identifier used to identify the extension.</param>
		/// <param name="rawData">The encoded data used to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="oid" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="oid" /> is an empty string ("").</exception>
		public X509Extension(Oid oid, byte[] rawData, bool critical)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			base.Oid = oid;
			base.RawData = rawData;
			this._critical = critical;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Extension" /> class.</summary>
		/// <param name="oid">A string representing the object identifier.</param>
		/// <param name="rawData">The encoded data used to create the extension.</param>
		/// <param name="critical">true if the extension is critical; otherwise false.</param>
		public X509Extension(string oid, byte[] rawData, bool critical) : base(oid, rawData)
		{
			this._critical = critical;
		}

		/// <summary>Gets a Boolean value indicating whether the extension is critical.</summary>
		/// <returns>true if the extension is critical; otherwise, false.</returns>
		public bool Critical
		{
			get
			{
				return this._critical;
			}
			set
			{
				this._critical = value;
			}
		}

		/// <summary>Copies the extension properties of the specified <see cref="T:System.Security.Cryptography.AsnEncodedData" /> object.</summary>
		/// <param name="asnEncodedData">The <see cref="T:System.Security.Cryptography.AsnEncodedData" /> to be copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asnEncodedData" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asnEncodedData" /> does not have a valid X.509 extension.</exception>
		public override void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("encodedData");
			}
			X509Extension x509Extension = asnEncodedData as X509Extension;
			if (x509Extension == null)
			{
				throw new ArgumentException(Locale.GetText("Expected a X509Extension instance."));
			}
			base.CopyFrom(asnEncodedData);
			this._critical = x509Extension.Critical;
		}

		internal string FormatUnkownData(byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				stringBuilder.Append(data[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}
	}
}
