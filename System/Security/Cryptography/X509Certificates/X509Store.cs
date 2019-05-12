using Mono.Security.X509;
using System;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates
{
	/// <summary>Represents an X.509 store, which is a physical store where certificates are persisted and managed. This class cannot be inherited.</summary>
	public sealed class X509Store
	{
		private string _name;

		private StoreLocation _location;

		private X509Certificate2Collection list;

		private OpenFlags _flags;

		private X509Store store;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using the personal certificates of the current user store.</summary>
		public X509Store() : this("MY", StoreLocation.CurrentUser)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using the specified store name.</summary>
		/// <param name="storeName">A string value representing the store name. See <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" />  for more information. </param>
		public X509Store(string storeName) : this(storeName, StoreLocation.CurrentUser)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using the specified <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> value.</summary>
		/// <param name="storeName">One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> values. </param>
		public X509Store(StoreName storeName) : this(storeName, StoreLocation.CurrentUser)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using the specified <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> value.</summary>
		/// <param name="storeLocation">One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> values. </param>
		public X509Store(StoreLocation storeLocation) : this("MY", storeLocation)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using the specified <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> and <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> values.</summary>
		/// <param name="storeName">One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> values. </param>
		/// <param name="storeLocation">One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> values. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="storeLocation" /> is not a valid location or <paramref name="storeName" /> is not a valid name. </exception>
		public X509Store(StoreName storeName, StoreLocation storeLocation)
		{
			if (storeName < StoreName.AddressBook || storeName > StoreName.TrustedPublisher)
			{
				throw new ArgumentException("storeName");
			}
			if (storeLocation < StoreLocation.CurrentUser || storeLocation > StoreLocation.LocalMachine)
			{
				throw new ArgumentException("storeLocation");
			}
			if (storeName != StoreName.CertificateAuthority)
			{
				this._name = storeName.ToString();
			}
			else
			{
				this._name = "CA";
			}
			this._location = storeLocation;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using an Intptr handle to an HCERTSTORE store.</summary>
		/// <param name="storeHandle">An <see cref="T:System.IntPtr" /> handle to an HCERTSTORE store.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="storeHandle" /> parameter is null.</exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The <paramref name="storeHandle" /> parameter points to an invalid context.</exception>
		[MonoTODO("Mono's stores are fully managed. All handles are invalid.")]
		public X509Store(IntPtr storeHandle)
		{
			if (storeHandle == IntPtr.Zero)
			{
				throw new ArgumentNullException("storeHandle");
			}
			throw new CryptographicException("Invalid handle.");
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Store" /> class using a string representing a value from the <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> enumeration and a value from the <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> enumeration.</summary>
		/// <param name="storeName">A string representing a value from the <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> enumeration. </param>
		/// <param name="storeLocation">One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> values. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="storeLocation" /> contains invalid values. </exception>
		public X509Store(string storeName, StoreLocation storeLocation)
		{
			if (storeLocation < StoreLocation.CurrentUser || storeLocation > StoreLocation.LocalMachine)
			{
				throw new ArgumentException("storeLocation");
			}
			this._name = storeName;
			this._location = storeLocation;
		}

		/// <summary>Returns a collection of certificates located in an X.509 certificate store.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object.</returns>
		public X509Certificate2Collection Certificates
		{
			get
			{
				if (this.list == null)
				{
					this.list = new X509Certificate2Collection();
				}
				else if (this.store == null)
				{
					this.list.Clear();
				}
				return this.list;
			}
		}

		/// <summary>Gets the location of the X.509 certificate store.</summary>
		/// <returns>One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreLocation" /> values.</returns>
		public StoreLocation Location
		{
			get
			{
				return this._location;
			}
		}

		/// <summary>Gets the name of the X.509 certificate store.</summary>
		/// <returns>One of the <see cref="T:System.Security.Cryptography.X509Certificates.StoreName" /> values.</returns>
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		private X509Stores Factory
		{
			get
			{
				if (this._location == StoreLocation.CurrentUser)
				{
					return X509StoreManager.CurrentUser;
				}
				return X509StoreManager.LocalMachine;
			}
		}

		private bool IsOpen
		{
			get
			{
				return this.store != null;
			}
		}

		private bool IsReadOnly
		{
			get
			{
				return Environment.UnityWebSecurityEnabled || (this._flags & OpenFlags.ReadWrite) == OpenFlags.ReadOnly;
			}
		}

		internal X509Store Store
		{
			get
			{
				return this.store;
			}
		}

		/// <summary>Gets an <see cref="T:System.IntPtr" /> handle to an HCERTSTORE store.  </summary>
		/// <returns>An <see cref="T:System.IntPtr" /> handle to an HCERTSTORE store.</returns>
		[MonoTODO("Mono's stores are fully managed. Always returns IntPtr.Zero.")]
		public IntPtr StoreHandle
		{
			get
			{
				return IntPtr.Zero;
			}
		}

		/// <summary>Adds a certificate to an X.509 certificate store.</summary>
		/// <param name="certificate">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificate" /> is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The certificate could not be added to the store.</exception>
		public void Add(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (!this.IsOpen)
			{
				throw new CryptographicException(Locale.GetText("Store isn't opened."));
			}
			if (this.IsReadOnly)
			{
				throw new CryptographicException(Locale.GetText("Store is read-only."));
			}
			if (!this.Exists(certificate))
			{
				try
				{
					this.store.Import(new X509Certificate(certificate.RawData));
				}
				finally
				{
					this.Certificates.Add(certificate);
				}
			}
		}

		/// <summary>Adds a collection of certificates to an X.509 certificate store.</summary>
		/// <param name="certificates">An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2Collection" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificates" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		[MonoTODO("Method isn't transactional (like documented)")]
		public void AddRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			if (certificates.Count == 0)
			{
				return;
			}
			if (!this.IsOpen)
			{
				throw new CryptographicException(Locale.GetText("Store isn't opened."));
			}
			if (this.IsReadOnly)
			{
				throw new CryptographicException(Locale.GetText("Store is read-only."));
			}
			foreach (X509Certificate2 x509Certificate in certificates)
			{
				if (!this.Exists(x509Certificate))
				{
					try
					{
						this.store.Import(new X509Certificate(x509Certificate.RawData));
					}
					finally
					{
						this.Certificates.Add(x509Certificate);
					}
				}
			}
		}

		/// <summary>Closes an X.509 certificate store.</summary>
		public void Close()
		{
			this.store = null;
			if (this.list != null)
			{
				this.list.Clear();
			}
		}

		/// <summary>Opens an X.509 certificate store or creates a new store, depending on <see cref="T:System.Security.Cryptography.X509Certificates.OpenFlags" /> flag settings.</summary>
		/// <param name="flags">One the <see cref="T:System.Security.Cryptography.X509Certificates.OpenFlags" /> values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The store is unreadable. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.ArgumentException">The store contains invalid values.</exception>
		public void Open(OpenFlags flags)
		{
			if (string.IsNullOrEmpty(this._name))
			{
				throw new CryptographicException(Locale.GetText("Invalid store name (null or empty)."));
			}
			string name = this._name;
			string storeName;
			if (name != null)
			{
				if (X509Store.<>f__switch$mapF == null)
				{
					X509Store.<>f__switch$mapF = new Dictionary<string, int>(1)
					{
						{
							"Root",
							0
						}
					};
				}
				int num;
				if (X509Store.<>f__switch$mapF.TryGetValue(name, out num))
				{
					if (num == 0)
					{
						storeName = "Trust";
						goto IL_8B;
					}
				}
			}
			storeName = this._name;
			IL_8B:
			bool create = (flags & OpenFlags.OpenExistingOnly) != OpenFlags.OpenExistingOnly;
			this.store = this.Factory.Open(storeName, create);
			if (this.store == null)
			{
				throw new CryptographicException(Locale.GetText("Store {0} doesn't exists.", new object[]
				{
					this._name
				}));
			}
			this._flags = flags;
			foreach (X509Certificate x509Certificate in this.store.Certificates)
			{
				this.Certificates.Add(new X509Certificate2(x509Certificate.RawData));
			}
		}

		/// <summary>Removes a certificate from an X.509 certificate store.</summary>
		/// <param name="certificate">The certificate to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificate" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void Remove(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (!this.IsOpen)
			{
				throw new CryptographicException(Locale.GetText("Store isn't opened."));
			}
			if (!this.Exists(certificate))
			{
				return;
			}
			if (this.IsReadOnly)
			{
				throw new CryptographicException(Locale.GetText("Store is read-only."));
			}
			try
			{
				this.store.Remove(new X509Certificate(certificate.RawData));
			}
			finally
			{
				this.Certificates.Remove(certificate);
			}
		}

		/// <summary>Removes a range of certificates from an X.509 certificate store.</summary>
		/// <param name="certificates">A range of certificates to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="certificates" /> is null. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		[MonoTODO("Method isn't transactional (like documented)")]
		public void RemoveRange(X509Certificate2Collection certificates)
		{
			if (certificates == null)
			{
				throw new ArgumentNullException("certificates");
			}
			if (certificates.Count == 0)
			{
				return;
			}
			if (!this.IsOpen)
			{
				throw new CryptographicException(Locale.GetText("Store isn't opened."));
			}
			bool flag = false;
			foreach (X509Certificate2 certificate in certificates)
			{
				if (this.Exists(certificate))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			if (this.IsReadOnly)
			{
				throw new CryptographicException(Locale.GetText("Store is read-only."));
			}
			try
			{
				foreach (X509Certificate2 x509Certificate in certificates)
				{
					this.store.Remove(new X509Certificate(x509Certificate.RawData));
				}
			}
			finally
			{
				this.Certificates.RemoveRange(certificates);
			}
		}

		private bool Exists(X509Certificate2 certificate)
		{
			if (this.store == null || this.list == null || certificate == null)
			{
				return false;
			}
			foreach (X509Certificate2 other in this.list)
			{
				if (certificate.Equals(other))
				{
					return true;
				}
			}
			return false;
		}
	}
}
