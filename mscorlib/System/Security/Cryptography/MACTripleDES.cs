using Mono.Security.Cryptography;
using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes a Message Authentication Code (MAC) using <see cref="T:System.Security.Cryptography.TripleDES" /> for the input data <see cref="T:System.Security.Cryptography.CryptoStream" />.</summary>
	[ComVisible(true)]
	public class MACTripleDES : KeyedHashAlgorithm
	{
		private TripleDES tdes;

		private MACAlgorithm mac;

		private bool m_disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.MACTripleDES" /> class.</summary>
		public MACTripleDES()
		{
			this.Setup("TripleDES", null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.MACTripleDES" /> class with the specified key data.</summary>
		/// <param name="rgbKey">The secret key for <see cref="T:System.Security.Cryptography.MACTripleDES" /> encryption. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="rgbKey" /> parameter is null. </exception>
		public MACTripleDES(byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			this.Setup("TripleDES", rgbKey);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.MACTripleDES" /> class with the specified key data using the specified implementation of <see cref="T:System.Security.Cryptography.TripleDES" />.</summary>
		/// <param name="strTripleDES">The name of the <see cref="T:System.Security.Cryptography.TripleDES" /> implementation to use. </param>
		/// <param name="rgbKey">The secret key for <see cref="T:System.Security.Cryptography.MACTripleDES" /> encryption. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="rgbKey" /> parameter is null. </exception>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The <paramref name="strTripleDES" /> parameter is not a valid name of a <see cref="T:System.Security.Cryptography.TripleDES" /> implementation. </exception>
		public MACTripleDES(string strTripleDES, byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			if (strTripleDES == null)
			{
				this.Setup("TripleDES", rgbKey);
			}
			else
			{
				this.Setup(strTripleDES, rgbKey);
			}
		}

		private void Setup(string strTripleDES, byte[] rgbKey)
		{
			this.tdes = TripleDES.Create(strTripleDES);
			this.tdes.Padding = PaddingMode.Zeros;
			if (rgbKey != null)
			{
				this.tdes.Key = rgbKey;
			}
			this.HashSizeValue = this.tdes.BlockSize;
			this.Key = this.tdes.Key;
			this.mac = new MACAlgorithm(this.tdes);
			this.m_disposed = false;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.MACTripleDES" />.</summary>
		~MACTripleDES()
		{
			this.Dispose(false);
		}

		/// <summary>Gets or sets the padding mode used in the hashing algorithm.</summary>
		/// <returns>The padding mode used in the hashing algorithm.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The property cannot be set because the padding mode is invalid.</exception>
		[ComVisible(false)]
		public PaddingMode Padding
		{
			get
			{
				return this.tdes.Padding;
			}
			set
			{
				this.tdes.Padding = value;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.MACTripleDES" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected override void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
				}
				if (this.tdes != null)
				{
					this.tdes.Clear();
				}
				if (disposing)
				{
					this.KeyValue = null;
					this.tdes = null;
				}
				base.Dispose(disposing);
				this.m_disposed = true;
			}
		}

		/// <summary>Initializes an instance of <see cref="T:System.Security.Cryptography.MACTripleDES" />.</summary>
		public override void Initialize()
		{
			if (this.m_disposed)
			{
				throw new ObjectDisposedException("MACTripleDES");
			}
			this.State = 0;
			this.mac.Initialize(this.KeyValue);
		}

		/// <summary>Routes data written to the object into the <see cref="T:System.Security.Cryptography.TripleDES" /> encryptor for computing the Message Authentication Code (MAC).</summary>
		/// <param name="rgbData">The input data. </param>
		/// <param name="ibStart">The offset into the byte array from which to begin using data. </param>
		/// <param name="cbSize">The number of bytes in the array to use as data. </param>
		protected override void HashCore(byte[] rgbData, int ibStart, int cbSize)
		{
			if (this.m_disposed)
			{
				throw new ObjectDisposedException("MACTripleDES");
			}
			if (this.State == 0)
			{
				this.Initialize();
				this.State = 1;
			}
			this.mac.Core(rgbData, ibStart, cbSize);
		}

		/// <summary>Returns the computed Message Authentication Code (MAC) after all data is written to the object.</summary>
		/// <returns>The computed MAC.</returns>
		protected override byte[] HashFinal()
		{
			if (this.m_disposed)
			{
				throw new ObjectDisposedException("MACTripleDES");
			}
			this.State = 0;
			return this.mac.Final();
		}
	}
}
