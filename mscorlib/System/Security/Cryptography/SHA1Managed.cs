using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes the <see cref="T:System.Security.Cryptography.SHA1" /> hash for the input data using the managed library. </summary>
	[ComVisible(true)]
	public class SHA1Managed : SHA1
	{
		private SHA1Internal sha;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.SHA1Managed" /> class.</summary>
		/// <exception cref="T:System.InvalidOperationException">This class is not compliant with the FIPS algorithm.</exception>
		public SHA1Managed()
		{
			this.sha = new SHA1Internal();
		}

		/// <summary>Routes data written to the object into the <see cref="T:System.Security.Cryptography.SHA1Managed" /> hash algorithm for computing the hash.</summary>
		/// <param name="rgb">The input data. </param>
		/// <param name="ibStart">The offset into the byte array from which to begin using data. </param>
		/// <param name="cbSize">The number of bytes in the array to use as data. </param>
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			this.State = 1;
			this.sha.HashCore(rgb, ibStart, cbSize);
		}

		/// <summary>Returns the computed <see cref="T:System.Security.Cryptography.SHA1" /> hash value after all data has been written to the object.</summary>
		/// <returns>The computed hash code.</returns>
		protected override byte[] HashFinal()
		{
			this.State = 0;
			return this.sha.HashFinal();
		}

		/// <summary>Initializes an instance of <see cref="T:System.Security.Cryptography.SHA1Managed" />.</summary>
		public override void Initialize()
		{
			this.sha.Initialize();
		}
	}
}
