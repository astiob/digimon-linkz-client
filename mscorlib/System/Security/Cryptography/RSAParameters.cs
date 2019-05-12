using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the standard parameters for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
	[ComVisible(true)]
	[Serializable]
	public struct RSAParameters
	{
		/// <summary>Represents the P parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] P;

		/// <summary>Represents the Q parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] Q;

		/// <summary>Represents the D parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] D;

		/// <summary>Represents the DP parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] DP;

		/// <summary>Represents the DQ parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] DQ;

		/// <summary>Represents the InverseQ parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] InverseQ;

		/// <summary>Represents the Modulus parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		public byte[] Modulus;

		/// <summary>Represents the Exponent parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.</summary>
		public byte[] Exponent;
	}
}
