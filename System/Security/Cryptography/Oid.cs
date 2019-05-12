using System;

namespace System.Security.Cryptography
{
	/// <summary>Represents a cryptographic object identifier. This class cannot be inherited.</summary>
	public sealed class Oid
	{
		internal const string oidRSA = "1.2.840.113549.1.1.1";

		internal const string nameRSA = "RSA";

		internal const string oidPkcs7Data = "1.2.840.113549.1.7.1";

		internal const string namePkcs7Data = "PKCS 7 Data";

		internal const string oidPkcs9ContentType = "1.2.840.113549.1.9.3";

		internal const string namePkcs9ContentType = "Content Type";

		internal const string oidPkcs9MessageDigest = "1.2.840.113549.1.9.4";

		internal const string namePkcs9MessageDigest = "Message Digest";

		internal const string oidPkcs9SigningTime = "1.2.840.113549.1.9.5";

		internal const string namePkcs9SigningTime = "Signing Time";

		internal const string oidMd5 = "1.2.840.113549.2.5";

		internal const string nameMd5 = "md5";

		internal const string oid3Des = "1.2.840.113549.3.7";

		internal const string name3Des = "3des";

		internal const string oidSha1 = "1.3.14.3.2.26";

		internal const string nameSha1 = "sha1";

		internal const string oidSubjectAltName = "2.5.29.17";

		internal const string nameSubjectAltName = "Subject Alternative Name";

		internal const string oidNetscapeCertType = "2.16.840.1.113730.1.1";

		internal const string nameNetscapeCertType = "Netscape Cert Type";

		private string _value;

		private string _name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Oid" /> class.</summary>
		public Oid()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Oid" /> class using a string value of an <see cref="T:System.Security.Cryptography.Oid" /> object.</summary>
		/// <param name="oid">An object identifier.</param>
		public Oid(string oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			this._value = oid;
			this._name = this.GetName(oid);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Oid" /> class using the specified value and friendly name.</summary>
		/// <param name="value">The dotted number of the identifier.</param>
		/// <param name="friendlyName">The friendly name of the identifier.</param>
		public Oid(string value, string friendlyName)
		{
			this._value = value;
			this._name = friendlyName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.Oid" /> class using the specified <see cref="T:System.Security.Cryptography.Oid" /> object.</summary>
		/// <param name="oid">The object identifier information to use to create the new object identifier.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="oid " />is null.</exception>
		public Oid(Oid oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			this._value = oid.Value;
			this._name = oid.FriendlyName;
		}

		/// <summary>Gets or sets the friendly name of the identifier.</summary>
		/// <returns>The friendly name of the identifier.</returns>
		public string FriendlyName
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				this._value = this.GetValue(this._name);
			}
		}

		/// <summary>Gets or sets the dotted number of the identifier.</summary>
		/// <returns>The dotted number of the identifier.</returns>
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this._name = this.GetName(this._value);
			}
		}

		private string GetName(string oid)
		{
			switch (oid)
			{
			case "1.2.840.113549.1.1.1":
				return "RSA";
			case "1.2.840.113549.1.7.1":
				return "PKCS 7 Data";
			case "1.2.840.113549.1.9.3":
				return "Content Type";
			case "1.2.840.113549.1.9.4":
				return "Message Digest";
			case "1.2.840.113549.1.9.5":
				return "Signing Time";
			case "1.2.840.113549.3.7":
				return "3des";
			case "2.5.29.19":
				return "Basic Constraints";
			case "2.5.29.15":
				return "Key Usage";
			case "2.5.29.37":
				return "Enhanced Key Usage";
			case "2.5.29.14":
				return "Subject Key Identifier";
			case "2.5.29.17":
				return "Subject Alternative Name";
			case "2.16.840.1.113730.1.1":
				return "Netscape Cert Type";
			case "1.2.840.113549.2.5":
				return "md5";
			case "1.3.14.3.2.26":
				return "sha1";
			}
			return this._name;
		}

		private string GetValue(string name)
		{
			switch (name)
			{
			case "RSA":
				return "1.2.840.113549.1.1.1";
			case "PKCS 7 Data":
				return "1.2.840.113549.1.7.1";
			case "Content Type":
				return "1.2.840.113549.1.9.3";
			case "Message Digest":
				return "1.2.840.113549.1.9.4";
			case "Signing Time":
				return "1.2.840.113549.1.9.5";
			case "3des":
				return "1.2.840.113549.3.7";
			case "Basic Constraints":
				return "2.5.29.19";
			case "Key Usage":
				return "2.5.29.15";
			case "Enhanced Key Usage":
				return "2.5.29.37";
			case "Subject Key Identifier":
				return "2.5.29.14";
			case "Subject Alternative Name":
				return "2.5.29.17";
			case "Netscape Cert Type":
				return "2.16.840.1.113730.1.1";
			case "md5":
				return "1.2.840.113549.2.5";
			case "sha1":
				return "1.3.14.3.2.26";
			}
			return this._value;
		}
	}
}
