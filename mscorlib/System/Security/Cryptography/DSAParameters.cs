using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Contains the typical parameters for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
	[ComVisible(true)]
	[Serializable]
	public struct DSAParameters
	{
		/// <summary>Specifies the counter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public int Counter;

		/// <summary>Specifies the G parameter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public byte[] G;

		/// <summary>Specifies the J parameter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public byte[] J;

		/// <summary>Specifies the P parameter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public byte[] P;

		/// <summary>Specifies the Q parameter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public byte[] Q;

		/// <summary>Specifies the seed for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public byte[] Seed;

		/// <summary>Specifies the X parameter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		[NonSerialized]
		public byte[] X;

		/// <summary>Specifies the Y parameter for the <see cref="T:System.Security.Cryptography.DSA" /> algorithm.</summary>
		public byte[] Y;
	}
}
