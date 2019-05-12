using System;
using System.IO;

namespace Mono.Security.X509
{
	public class X509Stores
	{
		private string _storePath;

		private X509Store _personal;

		private X509Store _other;

		private X509Store _intermediate;

		private X509Store _trusted;

		private X509Store _untrusted;

		internal X509Stores(string path)
		{
			this._storePath = path;
		}

		public X509Store Personal
		{
			get
			{
				if (this._personal == null)
				{
					string path = Path.Combine(this._storePath, "My");
					this._personal = new X509Store(path, false);
				}
				return this._personal;
			}
		}

		public X509Store OtherPeople
		{
			get
			{
				if (this._other == null)
				{
					string path = Path.Combine(this._storePath, "AddressBook");
					this._other = new X509Store(path, false);
				}
				return this._other;
			}
		}

		public X509Store IntermediateCA
		{
			get
			{
				if (this._intermediate == null)
				{
					string path = Path.Combine(this._storePath, "CA");
					this._intermediate = new X509Store(path, true);
				}
				return this._intermediate;
			}
		}

		public X509Store TrustedRoot
		{
			get
			{
				if (this._trusted == null)
				{
					string path = Path.Combine(this._storePath, "Trust");
					this._trusted = new X509Store(path, true);
				}
				return this._trusted;
			}
		}

		public X509Store Untrusted
		{
			get
			{
				if (this._untrusted == null)
				{
					string path = Path.Combine(this._storePath, "Disallowed");
					this._untrusted = new X509Store(path, false);
				}
				return this._untrusted;
			}
		}

		public void Clear()
		{
			if (this._personal != null)
			{
				this._personal.Clear();
			}
			this._personal = null;
			if (this._other != null)
			{
				this._other.Clear();
			}
			this._other = null;
			if (this._intermediate != null)
			{
				this._intermediate.Clear();
			}
			this._intermediate = null;
			if (this._trusted != null)
			{
				this._trusted.Clear();
			}
			this._trusted = null;
			if (this._untrusted != null)
			{
				this._untrusted.Clear();
			}
			this._untrusted = null;
		}

		public X509Store Open(string storeName, bool create)
		{
			if (storeName == null)
			{
				throw new ArgumentNullException("storeName");
			}
			string path = Path.Combine(this._storePath, storeName);
			if (!create && !Directory.Exists(path))
			{
				return null;
			}
			return new X509Store(path, true);
		}

		public class Names
		{
			public const string Personal = "My";

			public const string OtherPeople = "AddressBook";

			public const string IntermediateCA = "CA";

			public const string TrustedRoot = "Trust";

			public const string Untrusted = "Disallowed";
		}
	}
}
