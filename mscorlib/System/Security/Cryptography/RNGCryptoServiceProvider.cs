using System;
using System.Runtime.CompilerServices;

namespace System.Security.Cryptography
{
	/// <summary>Implements a cryptographic Random Number Generator (RNG) using the implementation provided by the cryptographic service provider (CSP). This class cannot be inherited.</summary>
	public sealed class RNGCryptoServiceProvider : RandomNumberGenerator
	{
		private static object _lock;

		private IntPtr _handle;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RNGCryptoServiceProvider" /> class.</summary>
		public RNGCryptoServiceProvider()
		{
			this._handle = RNGCryptoServiceProvider.RngInitialize(null);
			this.Check();
		}

		static RNGCryptoServiceProvider()
		{
			if (RNGCryptoServiceProvider.RngOpen())
			{
				RNGCryptoServiceProvider._lock = new object();
			}
		}

		private void Check()
		{
			if (this._handle == IntPtr.Zero)
			{
				throw new CryptographicException(Locale.GetText("Couldn't access random source."));
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RngOpen();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr RngInitialize(byte[] seed);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr RngGetBytes(IntPtr handle, byte[] data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RngClose(IntPtr handle);

		/// <summary>Fills an array of bytes with a cryptographically strong sequence of random values.</summary>
		/// <param name="data">The array to fill with a cryptographically strong sequence of random values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		public override void GetBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (RNGCryptoServiceProvider._lock == null)
			{
				this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data);
			}
			else
			{
				object @lock = RNGCryptoServiceProvider._lock;
				lock (@lock)
				{
					this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, data);
				}
			}
			this.Check();
		}

		/// <summary>Fills an array of bytes with a cryptographically strong sequence of random nonzero values.</summary>
		/// <param name="data">The array to fill with a cryptographically strong sequence of random nonzero values. </param>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The cryptographic service provider (CSP) cannot be acquired. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		public override void GetNonZeroBytes(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] array = new byte[data.Length * 2];
			int i = 0;
			while (i < data.Length)
			{
				this._handle = RNGCryptoServiceProvider.RngGetBytes(this._handle, array);
				this.Check();
				for (int j = 0; j < array.Length; j++)
				{
					if (i == data.Length)
					{
						break;
					}
					if (array[j] != 0)
					{
						data[i++] = array[j];
					}
				}
			}
		}

		~RNGCryptoServiceProvider()
		{
			if (this._handle != IntPtr.Zero)
			{
				RNGCryptoServiceProvider.RngClose(this._handle);
				this._handle = IntPtr.Zero;
			}
		}
	}
}
